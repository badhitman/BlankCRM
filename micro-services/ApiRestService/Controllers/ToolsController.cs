﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstants;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using SharedLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace ApiRestService.Controllers;

/// <summary>
/// Tools
/// </summary>
[TypeFilter(typeof(RolesAuthorizationFilter), Arguments = [$"{nameof(ExpressApiRolesEnum.SystemRoot)}"])]
[Route("api/[controller]/[action]"), ApiController, ServiceFilter(typeof(UnhandledExceptionAttribute))]
public class ToolsController(
    IServerToolsService toolsRepo,
    IManualCustomCacheService memCache,
    IStorageTransmission storeRepo,
    IOptions<PartUploadSessionConfigModel> сonfigPartUploadSession) : ControllerBase
{
    static readonly MemCachePrefixModel
        PartUploadCacheSessionsPrefix = new($"{Routes.PART_CONTROLLER_NAME}-{Routes.UPLOAD_ACTION_NAME}", Routes.SESSIONS_CONTROLLER_NAME),
        PartUploadCacheFilesMarkersPrefix = new($"{Routes.PART_CONTROLLER_NAME}-{Routes.UPLOAD_ACTION_NAME}", Routes.MARK_ACTION_NAME),
        PartUploadCacheFilesDumpsPrefix = new($"{Routes.PART_CONTROLLER_NAME}-{Routes.UPLOAD_ACTION_NAME}", Routes.DUMP_ACTION_NAME);

    /// <summary>
    /// Перейти к странице логов с искомой строкой
    /// </summary>
    [HttpPost($"/{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.LOGS_ACTION_NAME}-{Routes.PAGE_ACTION_NAME}/{Routes.GOTO_ACTION_NAME}-for-{Routes.RECORD_CONTROLLER_NAME}"), LoggerNolog]
    public async Task<TPaginationResponseModel<NLogRecordModelDB>> GoToPageForRow(TPaginationRequestModel<int> req)
    {
        return await storeRepo.GoToPageForRowAsync(req);
    }

    /// <summary>
    /// Чтение логов
    /// </summary>
    [HttpPost($"/{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.LOGS_ACTION_NAME}-{Routes.SELECT_ACTION_NAME}"), LoggerNolog]
    public async Task<TPaginationResponseModel<NLogRecordModelDB>> LogsSelect(TPaginationRequestModel<LogsSelectRequestModel> req)
    {
        return await storeRepo.LogsSelectAsync(req);
    }

    /// <summary>
    /// Чтение логов
    /// </summary>
    [HttpPost($"/{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.LOGS_ACTION_NAME}-{Routes.METADATA_CONTROLLER_NAME}"), LoggerNolog]
    public async Task<TResponseModel<LogsMetadataResponseModel>> MetadataLogs(PeriodDatesTimesModel req)
    {
        return await storeRepo.MetadataLogsAsync(req);
    }

    /// <summary>
    /// Загрузка порции файла
    /// </summary>
    [HttpPost($"/{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.PART_CONTROLLER_NAME}-{Routes.UPLOAD_ACTION_NAME}")]
    public async Task<ResponseBaseModel> PartUpload(IFormFile uploadedFile, [FromQuery(Name = $"{Routes.SESSION_CONTROLLER_NAME}_{Routes.TOKEN_CONTROLLER_NAME}")] string sessionToken, [FromQuery(Name = $"{Routes.FILE_CONTROLLER_NAME}_{Routes.TOKEN_CONTROLLER_NAME}")] string fileToken)
    {
        if (uploadedFile is null || uploadedFile.Length == 0)
            return ResponseBaseModel.CreateError($"Данные файла отсутствуют - {nameof(PartUpload)}");

        if (uploadedFile.Length > сonfigPartUploadSession.Value.PartUploadSize)
            return ResponseBaseModel.CreateError($"Пакет данных слишком велик");

        sessionToken = Encoding.UTF8.GetString(Convert.FromBase64String(sessionToken));
        fileToken = Encoding.UTF8.GetString(Convert.FromBase64String(fileToken));

        PartUploadSessionModel? sessionUploadPart = await memCache.GetObjectAsync<PartUploadSessionModel>(new MemCacheComplexKeyModel(sessionToken, PartUploadCacheSessionsPrefix));

        if (sessionUploadPart is null)
            return ResponseBaseModel.CreateError("Сессия не найдена");

        string _file_name = Path.Combine(sessionUploadPart.RemoteDirectory, uploadedFile.FileName.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar).Trim());
        using MemoryStream ms = new();
        uploadedFile.OpenReadStream().CopyTo(ms);
        FilePartMetadataModel currentPartMetadata = sessionUploadPart.FilePartsMetadata.First(x => x.PartFileId == fileToken);

        if (currentPartMetadata.PartFileSize != uploadedFile.Length)
            return ResponseBaseModel.CreateError($"Размер пакета данных ожидался {GlobalToolsStandard.SizeDataAsString(currentPartMetadata.PartFileSize)}, а в запросе получено {GlobalToolsStandard.SizeDataAsString(uploadedFile.Length)}");

        if (sessionUploadPart.FilePartsMetadata.Count == 1)
        {
            await Task.WhenAll([
                Task.Run(async () => { await memCache.RemoveAsync(new MemCacheComplexKeyModel(fileToken, PartUploadCacheFilesMarkersPrefix)); }),
                Task.Run(async () => { await memCache.RemoveAsync(new MemCacheComplexKeyModel(fileToken, PartUploadCacheFilesDumpsPrefix)); }),
                Task.Run(async () => { await memCache.RemoveAsync(new MemCacheComplexKeyModel(sessionToken, PartUploadCacheSessionsPrefix)); })]);

            return await toolsRepo.UpdateFileAsync(_file_name, sessionUploadPart.RemoteDirectory, ms.ToArray());
        }
        else
        {
            FilePartMetadataModel[] partsFiles = [.. sessionUploadPart.FilePartsMetadata.Where(x => !x.PartFileId.Equals(fileToken))];

            int _countMarkers = 0;
            await Task.WhenAll(partsFiles.Select(x => Task.Run(async () =>
            {
                string? marker = await memCache.GetStringValueAsync(new MemCacheComplexKeyModel(x.PartFileId, PartUploadCacheFilesMarkersPrefix));
                if (string.IsNullOrWhiteSpace(marker))
                    Interlocked.Increment(ref _countMarkers);
            })));

            if (_countMarkers != 0)
            {
                await Task.WhenAll([
                    Task.Run(async () => { await memCache.SetStringAsync(PartUploadCacheFilesMarkersPrefix, fileToken,DateTime.Now.ToString(), TimeSpan.FromSeconds(сonfigPartUploadSession.Value.PartUploadSessionTimeoutSeconds)); }),
                    Task.Run(async () => { await memCache.WriteBytesAsync(new MemCacheComplexKeyModel(fileToken,PartUploadCacheFilesDumpsPrefix), ms.ToArray(), TimeSpan.FromSeconds(сonfigPartUploadSession.Value.PartUploadSessionTimeoutSeconds)); })]);
            }
            else
            {
                ResponseBaseModel response = new();
                ConcurrentDictionary<string, byte[]> filesDumps = [];

                await Task.WhenAll(partsFiles.Select(x => Task.Run(async () =>
                {
                    byte[]? rawBytes = await memCache.GetBytesAsync(new MemCacheComplexKeyModel(x.PartFileId, PartUploadCacheFilesDumpsPrefix));
                    if (rawBytes is null)
                    {
                        lock (response)
                        {
                            response.AddError($"Ошибка склеивания порций файлов: в кеше отсутствует порция {x.PartFileId}");
                        }
                        return;
                    }
                    else if (!filesDumps.TryAdd(x.PartFileId, rawBytes))
                    {
                        lock (response)
                        {
                            response.AddError($"Ошибка склеивания порций файлов: не удалось разместить порцию данных {x.PartFileId} в ConcurrentDictionary<string, byte[]>");
                        }
                        return;
                    }

                })));
                if (!response.Success())
                    return response;

                filesDumps.TryAdd(fileToken, ms.ToArray());
                ms.Position = 0;
                foreach (FilePartMetadataModel _fileDump in sessionUploadPart.FilePartsMetadata.OrderBy(x => x.PartFileIndex))
                {
                    if (!filesDumps.TryGetValue(_fileDump.PartFileId, out byte[]? partBytes) || partBytes is null)
                        response.AddError($"Ошибка извлечения дампа порции данных: {_fileDump.PartFileId}");

                    if (!response.Success())
                        break;

                    await ms.WriteAsync(partBytes);
                }
                if (!response.Success())
                    return response;
                // 
                await Task.WhenAll(partsFiles.Select(x => Task.Run(async () => { await memCache.RemoveAsync(new MemCacheComplexKeyModel(x.PartFileId, PartUploadCacheFilesMarkersPrefix)); }))
                    .Union([Task.Run(async () => { await memCache.RemoveAsync(new MemCacheComplexKeyModel(sessionToken, PartUploadCacheSessionsPrefix)); })])
                    .Union(partsFiles.Select(x => Task.Run(async () => { await memCache.RemoveAsync(new MemCacheComplexKeyModel(x.PartFileId, PartUploadCacheFilesDumpsPrefix)); }))));

                return await toolsRepo.UpdateFileAsync(_file_name, sessionUploadPart.RemoteDirectory, ms.ToArray());
            }
        }

        return ResponseBaseModel.CreateSuccess("Done!");
    }

    /// <summary>
    /// Создать сессию порционной (частями) загрузки файлов
    /// </summary>
    [HttpPost($"/{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.SESSION_CONTROLLER_NAME}-{Routes.PART_CONTROLLER_NAME}-{Routes.UPLOAD_ACTION_NAME}-{Routes.START_ACTION_NAME}")]
    public async Task<TResponseModel<PartUploadSessionModel>> PartUploadSessionStart(PartUploadSessionStartRequestModel req)
    {
        TResponseModel<PartUploadSessionModel> res = new();
        DirectoryInfo _di = new(req.RemoteDirectory);
        if (!_di.Exists)
        {
            res.AddError($"Удалённая папка `{_di.FullName}` не существует.");
            return res;
        }

        res.Response = new()
        {
            FilePartsMetadata = [],
            SessionId = Guid.NewGuid().ToString(),

            RemoteDirectory = _di.FullName,
            FileName = req.FileName
        };

        long scaleFileSize = req.FileSize;

        int partsCount = (int)Math.Ceiling(req.FileSize / (double)сonfigPartUploadSession.Value.PartUploadSize);
        for (uint i = 0; i < partsCount; i++)
        {
            if (scaleFileSize == 0)
                throw new Exception("Ошибка нарезки файла");

            FilePartMetadataModel _rq = new()
            {
                PartFileId = Guid.NewGuid().ToString(),
                PartFilePositionStart = req.FileSize - scaleFileSize, //i * сonfigPartUploadSession.Value.PartUploadSize,
                PartFileSize = Math.Min(scaleFileSize, сonfigPartUploadSession.Value.PartUploadSize),
                PartFileIndex = i,
            };
            res.Response.FilePartsMetadata.Add(_rq);
            scaleFileSize -= _rq.PartFileSize;
        }

        await memCache.SetObjectAsync(new MemCacheComplexKeyModel(res.Response.SessionId, PartUploadCacheSessionsPrefix), res.Response, TimeSpan.FromSeconds(сonfigPartUploadSession.Value.PartUploadSessionTimeoutSeconds));
        return res;
    }

    /// <summary>
    /// Обновить файл (или создать если его не существует)
    /// </summary>
    /// <remarks>
    /// Роль: <see cref="ExpressApiRolesEnum.SystemRoot"/>
    /// </remarks>
    [HttpPost($"/{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.FILE_CONTROLLER_NAME}-{Routes.UPDATE_ACTION_NAME}")]
    public async Task<TResponseModel<string>> FileUpdateOrCreate(IFormFile uploadedFile, [FromQuery(Name = $"{Routes.REMOTE_CONTROLLER_NAME}_{Routes.DIRECTORY_CONTROLLER_NAME}")] string remoteDirectory)
    {
        TResponseModel<string> response = new();
        remoteDirectory = Encoding.UTF8.GetString(Convert.FromBase64String(remoteDirectory));
        remoteDirectory = remoteDirectory.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);

        if (uploadedFile is null || uploadedFile.Length == 0)
        {
            response.AddError($"Данные файла отсутствуют - {nameof(FileUpdateOrCreate)}");
            return response;
        }

        string _file_name = Path.Combine(remoteDirectory, uploadedFile.FileName.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar).Trim());
        using MemoryStream ms = new();
        uploadedFile.OpenReadStream().CopyTo(ms);

        return await toolsRepo.UpdateFileAsync(_file_name, remoteDirectory, ms.ToArray());
    }

    /// <summary>
    /// Выполнить команду shell/cmd
    /// </summary>
    /// <remarks>
    /// Роль: <see cref="ExpressApiRolesEnum.SystemRoot"/>
    /// </remarks>
    [HttpPost($"/{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.CMD_CONTROLLER_NAME}/{Routes.EXE_ACTION_NAME}")]
    public TResponseModel<string> ExeCommand(ExeCommandModelDB req)
    {
        TResponseModel<string> res = new();

        try
        {
            res.Response = GlobalTools.RunCommandWithBash(req.Arguments, req.FileName);
        }
        catch (Exception ex)
        {
            res.Messages.InjectException(ex);
        }

        return res;
    }

    /// <summary>
    /// Получить список файлов из директории
    /// </summary>
    /// <remarks>
    /// Роль: <see cref="ExpressApiRolesEnum.SystemRoot"/>
    /// </remarks>
    [HttpPost($"/{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.DIRECTORY_CONTROLLER_NAME}/{Routes.GET_ACTION_NAME}-{Routes.DATA_ACTION_NAME}"), LoggerNolog]
    public Task<TResponseModel<List<ToolsFilesResponseModel>>> GetDirectoryData(ToolsFilesRequestModel req)
        => toolsRepo.GetDirectoryDataAsync(req);

    /// <summary>
    /// Существование директории
    /// </summary>
    /// <remarks>
    /// Роль: <see cref="ExpressApiRolesEnum.SystemRoot"/>
    /// </remarks>
    [HttpPost($"/{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.DIRECTORY_CONTROLLER_NAME}/{Routes.CHECK_ACTION_NAME}"), LoggerNolog]
    public Task<ResponseBaseModel> DirectoryExist([FromBody] string directoryPath)
        => toolsRepo.DirectoryExistAsync(directoryPath);

    /// <summary>
    /// Удалить файл
    /// </summary>
    /// <remarks>
    /// Роль: <see cref="ExpressApiRolesEnum.SystemRoot"/>
    /// </remarks>
    [HttpPost($"/{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.FILE_CONTROLLER_NAME}-{Routes.DELETE_ACTION_NAME}")]
    public async Task<TResponseModel<bool>> FileDelete(DeleteRemoteFileRequestModel req)
    {
        return await toolsRepo.DeleteFileAsync(req);
    }
}