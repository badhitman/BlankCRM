﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using System.Runtime.Versioning;
using System.IO.Compression;
using SharedLib;

namespace BlazorLib.Components.ToolsApp;

/// <summary>
/// SyncManageComponent
/// </summary>
[UnsupportedOSPlatform("browser")]
public partial class SyncManageComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IToolsAppManager AppManagerRepo { get; set; } = default!;

    [Inject]
    IClientHTTPRestService RestClientRepo { get; set; } = default!;

    [Inject]
    IServerToolsService ServicedRepo { get; set; } = default!;

    [Inject]
    ILogger<SyncManageComponent> LoggerRepo { get; set; } = default!;


    /// <summary>
    /// SyncDirectory
    /// </summary>
    [Parameter, EditorRequired]
    public required SyncDirectoryModelDB SyncDirectory { get; set; }

    /// <summary>
    /// CloseCommand
    /// </summary>
    [Parameter, EditorRequired]
    public required Action CloseCommand { get; set; }


    private string searchStringQuery = "";

    string? InfoAbout;

    TResponseModel<List<ToolsFilesResponseModel>>? localScan;
    bool localScanBusy;

    TResponseModel<List<ToolsFilesResponseModel>>? remoteScan;
    bool remoteScanBusy;

    bool IndeterminateProgress;
    long forUpdateOrAddSum;

    ToolsFilesResponseModel[]? forDelete = null;
    ToolsFilesResponseModel[]? forUpdateOrAdd = null;

    DirectoryInfo? LocalDirectoryInfo;
    ResponseBaseModel? RemoteDirectoryInfo;


    /// <inheritdoc/>
    public double ValueProgress { get; set; }
    private bool FilterFunc1(ToolsFilesResponseModel element) => FilterFunc(element, searchStringQuery);

    private static bool FilterFunc(ToolsFilesResponseModel element, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.FullName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Hash?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true)
            return true;

        return false;
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await Test();
    }

    async Task Test()
    {
        await SetBusy();

        LocalDirectoryInfo = string.IsNullOrWhiteSpace(SyncDirectory.LocalDirectory)
            ? null
            : new(SyncDirectory.LocalDirectory);

        if (LocalDirectoryInfo is null)
            SnackbarRepo.Error("Не указана локальная папка");
        else if (!LocalDirectoryInfo.Exists)
            SnackbarRepo.Error("Локальной папки не существует");
        else
            SnackbarRepo.Success("Локальная папка существует");

        RemoteDirectoryInfo = string.IsNullOrWhiteSpace(SyncDirectory.RemoteDirectory)
                ? null
                : await RestClientRepo.DirectoryExist(SyncDirectory.RemoteDirectory);

        if (RemoteDirectoryInfo is not null)
            SnackbarRepo.ShowMessagesResponse(RemoteDirectoryInfo.Messages);
        else
            SnackbarRepo.Error("Не указана директория на удалённом сервере");


        await SetBusy(false);
    }


    async Task SyncRun()
    {
        if (string.IsNullOrWhiteSpace(SyncDirectory.RemoteDirectory) || string.IsNullOrWhiteSpace(SyncDirectory.LocalDirectory))
        {
            SnackbarRepo.Error("RemoteDirectory or LocalDirectory: is empty");
            return;
        }

        IndeterminateProgress = true;
        //await ParentPage.HoldPageUpdate(true);
        await SetBusy();

        forDelete = null;
        forUpdateOrAdd = null;

        await Task.WhenAll([ReadLocalData(), ReadRemoteData()]);
        //await ParentPage.HoldPageUpdate(false);
        if (localScan?.Response is null || remoteScan?.Response is null)
        {
            SnackbarRepo.Error("localScan is null || remoteScan is null");
            await SetBusy(false);
            return;
        }

        await SetBusy(false);

        forDelete = [.. remoteScan.Response.Where(x => !localScan.Response.Any(y => x.SafeScopeName == y.SafeScopeName))];

        forUpdateOrAdd = [.. localScan.Response
           .Where(x => !remoteScan.Response.Any(y => x.SafeScopeName == y.SafeScopeName))
           .Union(remoteScan.Response.Where(x => localScan.Response.Any(y => x.SafeScopeName == y.SafeScopeName && !x.Equals(y))))
           .OrderByDescending(x => x.Size)];

        forUpdateOrAddSum = forUpdateOrAdd.Sum(x => x.Size);
    }

    async Task Send()
    {
        if (forDelete is null || forUpdateOrAdd is null)
        {
            SnackbarRepo.Error("forDelete is null || forUpdateOrAdd is null");
            return;
        }

        if (string.IsNullOrWhiteSpace(SyncDirectory.RemoteDirectory))
        {
            SnackbarRepo.Error("RemoteDirectory is empty");
            return;
        }

        if (string.IsNullOrWhiteSpace(SyncDirectory.LocalDirectory))
        {
            SnackbarRepo.Error("LocalDirectory is empty");
            return;
        }

        if (forDelete.Length == 0 && forUpdateOrAdd.Length == 0)
            return;

        IndeterminateProgress = true;
        ValueProgress = 0;
        //await ParentPage.HoldPageUpdate(true);
        await SetBusy();

        MemoryStream ms;

        if (forDelete.Length != 0)
        {
            InfoAbout = "Удаление файлов...";
            await SetBusy();
            foreach (ToolsFilesResponseModel tFile in forDelete)
                await RestClientRepo.DeleteFile(new DeleteRemoteFileRequestModel()
                {
                    RemoteDirectory = SyncDirectory.RemoteDirectory,
                    SafeScopeName = tFile.SafeScopeName,
                });
            InfoAbout = $"Удалено файлов: {forDelete.Length} шт.";
            await SetBusy();
        }

        using MD5 md5 = MD5.Create();
        string _hash;
        long totalTransferData = 0, totalReadData = 0;
        IndeterminateProgress = false;

        if (forUpdateOrAdd.Length != 0)
        {
            InfoAbout = "Отправка файлов...";
            int _cntFiles = 0;
            foreach (ToolsFilesResponseModel tFile in forUpdateOrAdd)//.OrderBy(x => x.Size)
            {
                _cntFiles++;
                totalReadData += tFile.Size;
                try
                {
                    string archive = Path.GetTempFileName();
                    using ZipArchive zip = ZipFile.Open(archive, ZipArchiveMode.Update);

                    string _fnT = Path.Combine(SyncDirectory.LocalDirectory, tFile.SafeScopeName);

                    ZipArchiveEntry entry = zip.CreateEntryFromFile(_fnT, tFile.SafeScopeName);
                    zip.Dispose();
                    ms = new MemoryStream(File.ReadAllBytes(archive));

                    TResponseModel<PartUploadSessionModel> sessionPartUpload = await RestClientRepo.FilePartUploadSessionStart(new PartUploadSessionStartRequestModel()
                    {
                        RemoteDirectory = SyncDirectory.RemoteDirectory,
                        FileSize = ms.Length,
                        FileName = tFile.SafeScopeName,
                    });

                    if (sessionPartUpload.Response is null || !sessionPartUpload.Success())
                    {
                        SnackbarRepo.Error($"Ошибка открытия сессии отправки файла: {sessionPartUpload.Message()}");
                        return;
                    }

                    totalTransferData += ms.Length;
                    ValueProgress = totalReadData / (forUpdateOrAddSum / 100);
                    InfoAbout = $"Отправлено файлов: {_cntFiles} шт. (~{GlobalTools.SizeDataAsString(totalReadData)} zip:{GlobalTools.SizeDataAsString(totalTransferData)})";

                    if (sessionPartUpload.Response.FilePartsMetadata.Count == 1)
                    {
                        TResponseModel<string> resUpd = await RestClientRepo.UpdateFile(tFile.SafeScopeName, SyncDirectory.RemoteDirectory, ms.ToArray());

                        if (resUpd.Messages.Any(x => x.TypeMessage == ResultTypesEnum.Error || x.TypeMessage >= ResultTypesEnum.Info))
                            SnackbarRepo.ShowMessagesResponse(resUpd.Messages);

                        using FileStream stream = File.OpenRead(_fnT);
                        _hash = Convert.ToBase64String(md5.ComputeHash(stream));

                        if (_hash != resUpd.Response)
                            SnackbarRepo.Error($"Hash file conflict `{tFile.FullName}`: L[{_hash}]{_fnT} R[{Path.Combine(tFile.SafeScopeName, SyncDirectory.RemoteDirectory)}]");
                    }
                    else
                    {
                        foreach (FilePartMetadataModel fileMd in sessionPartUpload.Response.FilePartsMetadata)
                        {
                            ms.Position = fileMd.PartFilePositionStart;
                            byte[] _buff = new byte[fileMd.PartFileSize];
                            ms.Read(_buff, 0, _buff.Length);
                            ResponseBaseModel _subRest = await RestClientRepo.FilePartUpload(new SessionFileRequestModel(sessionPartUpload.Response.SessionId, fileMd.PartFileId, _buff, Path.GetFileName(tFile.FullName), fileMd.PartFileIndex));
                            if (!_subRest.Success())
                                SnackbarRepo.ShowMessagesResponse(_subRest.Messages);

                            StateHasChanged();
                        }
                    }

                    File.Delete(archive);
                }
                catch (Exception ex)
                {
                    SnackbarRepo.Add(ex.Message, MudBlazor.Severity.Error, c => c.DuplicatesBehavior = MudBlazor.SnackbarDuplicatesBehavior.Allow);
                    LoggerRepo.LogError(ex, $"Ошибка отправки порции данных: {tFile}");
                }
            }
        }

        await SyncRun();
        //await ParentPage.HoldPageUpdate(false);

        if (totalTransferData != 0)
            SnackbarRepo.Add($"Отправлено: {GlobalTools.SizeDataAsString(totalTransferData)}", MudBlazor.Severity.Info, c => c.DuplicatesBehavior = MudBlazor.SnackbarDuplicatesBehavior.Allow);
        await SetBusy(false);
    }

    async Task ReadLocalData()
    {
        localScan = null;
        if (string.IsNullOrWhiteSpace(SyncDirectory.LocalDirectory))
        {
            SnackbarRepo.Error("Не указана текущая локальная папка клиента");
            return;
        }

        localScanBusy = true;
        await Task.Delay(1);
        StateHasChanged();
        localScan = await ServicedRepo.GetDirectoryData(new ToolsFilesRequestModel
        {
            RemoteDirectory = SyncDirectory.LocalDirectory,
            CalculationHash = true,
        });
        localScanBusy = false;
    }

    async Task ReadRemoteData()
    {
        remoteScan = null;
        if (string.IsNullOrWhiteSpace(SyncDirectory.RemoteDirectory))
        {
            SnackbarRepo.Error("Не указана папка удалённого сервера");
            return;
        }

        remoteScanBusy = true;
        await Task.Delay(1);
        StateHasChanged();
        remoteScan = await RestClientRepo.GetDirectoryData(new ToolsFilesRequestModel
        {
            RemoteDirectory = SyncDirectory.RemoteDirectory,
            CalculationHash = true,
        });
        remoteScanBusy = false;
        await Task.Delay(1);
        StateHasChanged();
    }
}