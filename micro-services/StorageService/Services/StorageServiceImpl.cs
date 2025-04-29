////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using MongoDB.Driver.GridFS;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;
using ImageMagick;
using SharedLib;
using DbcLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace StorageService;

/// <summary>
/// Хранилище параметров приложений
/// </summary>
/// <remarks>
/// Значения/данные сериализуются в JSON строку при сохранении и десерализируются при чтении
/// </remarks>
public class StorageServiceImpl(
    IDbContextFactory<StorageContext> cloudParametersDbFactory,
    
    IMemoryCache cache,
    IMongoDatabase mongoFs,
    IIdentityTransmission identityRepo,
    ICommerceTransmission commRepo,
    IHelpdeskTransmission HelpdeskRepo,
    WebConfigModel webConfig,
    ILogger<StorageServiceImpl> loggerRepo) : ISerializeStorage
{
#if DEBUG
    static readonly TimeSpan _ts = TimeSpan.FromSeconds(2);
#else
    static readonly TimeSpan _ts = TimeSpan.FromSeconds(10);
#endif

    

    #region tags
    /// <inheritdoc/>
    public async Task<TResponseModel<FilesAreaMetadataModel[]>> FilesAreaGetMetadataAsync(FilesAreaMetadataRequestModel req, CancellationToken token = default)
    {
        using StorageContext context = await cloudParametersDbFactory.CreateDbContextAsync(token);
        IQueryable<StorageFileModelDB> q = context
            .CloudFiles
            .AsQueryable();

        if (req.ApplicationsNamesFilter is not null && req.ApplicationsNamesFilter.Length != 0)
            q = q.Where(x => req.ApplicationsNamesFilter.Contains(x.ApplicationName));

        var res = await q
            .GroupBy(x => x.ApplicationName)
            .Select(x => new
            {
                AppName = x.Key,
                CountFiles = x.Count(),
                SummSize = x.Sum(y => y.FileLength)
            })
            .ToArrayAsync(cancellationToken: token);

        return new()
        {
            Response =
            [.. res
            .Select(x => new FilesAreaMetadataModel()
            {
                ApplicationName = x.AppName,
                CountFiles = x.CountFiles,
                SizeFilesSum = x.SummSize
            })]
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<StorageFileModelDB>> FilesSelectAsync(TPaginationRequestModel<SelectMetadataRequestModel> req, CancellationToken token = default)
    {
        using StorageContext context = await cloudParametersDbFactory.CreateDbContextAsync(token);

        if (req.PageSize < 5)
            req.PageSize = 5;

        IQueryable<StorageFileModelDB> q = context
            .CloudFiles
            .AsQueryable();

        if (req.Payload.ApplicationsNames is not null && req.Payload.ApplicationsNames.Length != 0)
            q = q.Where(x => req.Payload.ApplicationsNames.Any(y => y == x.ApplicationName));

        if (!string.IsNullOrWhiteSpace(req.Payload.PropertyName))
            q = q.Where(x => x.PropertyName == req.Payload.PropertyName);

        if (!string.IsNullOrWhiteSpace(req.Payload.PrefixPropertyName))
            q = q.Where(x => x.PrefixPropertyName == req.Payload.PrefixPropertyName);

        if (req.Payload.OwnerPrimaryKey.HasValue && req.Payload.OwnerPrimaryKey.Value > 0)
            q = q.Where(x => x.OwnerPrimaryKey == req.Payload.OwnerPrimaryKey.Value);

        if (!string.IsNullOrWhiteSpace(req.Payload.SearchQuery))
            q = q.Where(x => x.NormalizedFileNameUpper!.Contains(req.Payload.SearchQuery.ToUpper()));

        IQueryable<StorageFileModelDB> oq = req.SortingDirection == DirectionsEnum.Up
          ? q.OrderBy(x => x.CreatedAt).Skip(req.PageNum * req.PageSize).Take(req.PageSize)
          : q.OrderByDescending(x => x.CreatedAt).Skip(req.PageNum * req.PageSize).Take(req.PageSize);

        int trc = await q.CountAsync(cancellationToken: token);
        TPaginationResponseModel<StorageFileModelDB> res = new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = trc,
            Response = await oq.ToListAsync(cancellationToken: token),
        };
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<FileContentModel>> ReadFileAsync(TAuthRequestModel<RequestFileReadModel> req, CancellationToken token = default)
    {
        TResponseModel<FileContentModel> res = new();
        using StorageContext context = await cloudParametersDbFactory.CreateDbContextAsync(token);
        StorageFileModelDB? file_db = await context
            .CloudFiles
            .Include(x => x.AccessRules)
            .FirstOrDefaultAsync(x => x.Id == req.Payload.FileId, cancellationToken: token);

        if (file_db is null)
        {
            res.AddError($"Файл #{req.Payload} не найден");
            return res;
        }

        // если правил для файла не установлено или вызывающий является владельцем (тот кто его загрузил) файла
        bool allowed = file_db.AccessRules is null || file_db.AccessRules.Count == 0 || (!string.IsNullOrEmpty(req.SenderActionUserId) && file_db.AuthorIdentityId == req.SenderActionUserId);

        string[] abs_rules = ["*", "all", "any"];
        // правило: доступ любому авторизованному пользователю
        allowed = allowed ||
            (!string.IsNullOrWhiteSpace(req.SenderActionUserId) && file_db.AccessRules?.Any(x => x.AccessRuleType == FileAccessRulesTypesEnum.User && (x.Option == req.SenderActionUserId || abs_rules.Contains(x.Option.Trim().ToLower()))) == true);

        // проверка токена прямого доступа к файлу
        allowed = allowed || (!string.IsNullOrWhiteSpace(req.Payload.TokenAccess) && file_db.AccessRules?.Any(x => x.AccessRuleType == FileAccessRulesTypesEnum.Token && x.Option == req.SenderActionUserId) == true);

        UserInfoModel? currentUser = null;
        if (!allowed && !string.IsNullOrWhiteSpace(req.SenderActionUserId))
        {
            TResponseModel<UserInfoModel[]> findUserRes = await identityRepo.GetUsersIdentityAsync([req.SenderActionUserId], token);
            currentUser = findUserRes.Response?.Single();
            if (currentUser is null)
            {
                res.AddError($"Пользователь #{req.SenderActionUserId} не найден");
                return res;
            }
            allowed = currentUser.IsAdmin;
        }

        if (!allowed)
        {
            List<string>? issues_rules = file_db
                        .AccessRules?
                        .Where(x => x.AccessRuleType == FileAccessRulesTypesEnum.Issue)
                        .Select(x => x.Option)
                        .ToList();

            if (issues_rules is not null && issues_rules.Count != 0)
            {
                List<int> issues_ids = [];
                issues_rules.ForEach(x => { if (int.TryParse(x, out int issue_id)) { issues_ids.Add(issue_id); } });
                if (issues_ids.Count > 0)
                {
                    TAuthRequestModel<IssuesReadRequestModel> reqIssues = new()
                    {
                        SenderActionUserId = req.SenderActionUserId,
                        Payload = new()
                        {
                            IssuesIds = [.. issues_ids],
                            IncludeSubscribersOnly = false,
                        }
                    };
                    TResponseModel<IssueHelpdeskModelDB[]> findIssues = await HelpdeskRepo.IssuesReadAsync(reqIssues, token);
                    allowed = findIssues.Success() &&
                        findIssues.Response?.Any(x => x.AuthorIdentityUserId == req.SenderActionUserId || x.ExecutorIdentityUserId == req.SenderActionUserId || x.Subscribers?.Any(y => y.UserId == req.SenderActionUserId) == true) == true;
                }
            }
        }

        if (!allowed)
        {
            res.AddError($"Файл #{req.Payload} не прочитан");
            return res;
        }

        using MemoryStream stream = new();
        GridFSBucket gridFS = new(mongoFs);
        await gridFS.DownloadToStreamAsync(new ObjectId(file_db.PointId), stream, cancellationToken: token);

        res.Response = new()
        {
            ApplicationName = file_db.ApplicationName,
            AuthorIdentityId = file_db.AuthorIdentityId,
            FileName = file_db.FileName,
            PropertyName = file_db.PropertyName,
            CreatedAt = file_db.CreatedAt,
            OwnerPrimaryKey = file_db.OwnerPrimaryKey,
            PointId = file_db.PointId,
            PrefixPropertyName = file_db.PrefixPropertyName,
            Payload = stream.ToArray(),
            Id = file_db.Id,
            ContentType = file_db.ContentType,
        };

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<StorageFileModelDB>> SaveFileAsync(TAuthRequestModel<StorageImageMetadataModel> req, CancellationToken token = default)
    {
        TResponseModel<StorageFileModelDB> res = new();
        GridFSBucket gridFS = new(mongoFs);
        Regex rx = new(@"\s+", RegexOptions.Compiled);
        string _file_name = rx.Replace(req.Payload.FileName.Trim(), " ");
        if (string.IsNullOrWhiteSpace(_file_name))
            _file_name = $"без имени: {DateTime.UtcNow}";

        using MemoryStream stream = new(req.Payload.Payload);
        ObjectId _uf = await gridFS.UploadFromStreamAsync(_file_name, stream, cancellationToken: token);
        using StorageContext context = await cloudParametersDbFactory.CreateDbContextAsync(token);
        res.Response = new StorageFileModelDB()
        {
            ApplicationName = req.Payload.ApplicationName,
            AuthorIdentityId = req.Payload.AuthorUserIdentity,
            FileName = _file_name,
            NormalizedFileNameUpper = _file_name.ToUpper(),
            ContentType = req.Payload.ContentType,
            PropertyName = req.Payload.PropertyName,
            PointId = _uf.ToString(),
            CreatedAt = DateTime.UtcNow,
            OwnerPrimaryKey = req.Payload.OwnerPrimaryKey,
            PrefixPropertyName = req.Payload.PrefixPropertyName,
            ReferrerMain = req.Payload.Referrer,
            FileLength = req.Payload.Payload.Length,
        };

        await context.AddAsync(res.Response, token);
        await context.SaveChangesAsync(token);

        if (GlobalTools.IsImageFile(_file_name))
        {
            using MagickImage image = new(req.Payload.Payload);
            //
            string _h = $"Height:{image.Height}", _w = $"Width:{image.Width}";
            await context.AddAsync(new TagModelDB()
            {
                ApplicationName = Routes.FILE_CONTROLLER_NAME,
                PropertyName = Routes.METADATA_CONTROLLER_NAME,
                CreatedAt = DateTime.UtcNow,
                NormalizedTagNameUpper = _h.ToUpper(),
                TagName = _h,
                OwnerPrimaryKey = res.Response.Id,
                PrefixPropertyName = Routes.DEFAULT_CONTROLLER_NAME,
            }, token);
            await context.AddAsync(new TagModelDB()
            {
                ApplicationName = Routes.FILE_CONTROLLER_NAME,
                PropertyName = Routes.METADATA_CONTROLLER_NAME,
                CreatedAt = DateTime.UtcNow,
                NormalizedTagNameUpper = _w.ToUpper(),
                TagName = _w,
                OwnerPrimaryKey = res.Response.Id,
                PrefixPropertyName = Routes.DEFAULT_CONTROLLER_NAME,
            }, token);
            await context.AddAsync(new TagModelDB()
            {
                ApplicationName = Routes.FILE_CONTROLLER_NAME,
                PropertyName = Routes.METADATA_CONTROLLER_NAME,
                CreatedAt = DateTime.UtcNow,
                NormalizedTagNameUpper = nameof(GlobalTools.IsImageFile).ToUpper(),
                TagName = nameof(GlobalTools.IsImageFile),
                OwnerPrimaryKey = res.Response.Id,
                PrefixPropertyName = Routes.DEFAULT_CONTROLLER_NAME,
            }, token);
        }

        if (req.Payload.OwnerPrimaryKey.HasValue && req.Payload.OwnerPrimaryKey.Value > 0)
        {
            PulseRequestModel reqPulse;
            string msg;
            switch (req.Payload.ApplicationName)
            {
                case Routes.ORDER_CONTROLLER_NAME:
                    TResponseModel<OrderDocumentModelDB[]> get_order = await commRepo.OrdersReadAsync(new() { Payload = [req.Payload.OwnerPrimaryKey.Value], SenderActionUserId = req.SenderActionUserId }, token);
                    if (!get_order.Success() || get_order.Response is null)
                        res.AddRangeMessages(get_order.Messages);
                    else
                    {
                        OrderDocumentModelDB orderDb = get_order.Response.Single();
                        if (orderDb.HelpdeskId.HasValue && orderDb.HelpdeskId.Value > 0)
                        {
                            msg = $"В <a href=\"{webConfig.ClearBaseUri}/issue-card/{orderDb.HelpdeskId.Value}\">заказ #{orderDb.Id}</a> добавлен файл '<u>{_file_name}</u>' {GlobalTools.SizeDataAsString(req.Payload.Payload.Length)}";
                            loggerRepo.LogInformation($"{msg} [{nameof(res.Response.PointId)}:{_uf}]");
                            reqPulse = new()
                            {
                                Payload = new()
                                {
                                    Payload = new()
                                    {
                                        Description = msg,
                                        IssueId = orderDb.HelpdeskId.Value,
                                        PulseType = PulseIssuesTypesEnum.Files,
                                        Tag = Routes.ADD_ACTION_NAME
                                    },
                                    SenderActionUserId = GlobalStaticConstantsRoles.Roles.System,
                                }
                            };

                            await HelpdeskRepo.PulsePushAsync(reqPulse, false, token);
                        }
                    }
                    break;
                case Routes.ISSUE_CONTROLLER_NAME:
                    msg = $"В <a href=\"{webConfig.ClearBaseUri}/issue-card/{req.Payload.OwnerPrimaryKey.Value}\">заявку #{req.Payload.OwnerPrimaryKey.Value}</a> добавлен файл '<u>{_file_name}</u>' {GlobalTools.SizeDataAsString(req.Payload.Payload.Length)}";
                    loggerRepo.LogInformation($"{msg} [{nameof(res.Response.PointId)}:{_uf}]");
                    reqPulse = new()
                    {
                        Payload = new()
                        {
                            Payload = new()
                            {
                                Description = msg,
                                IssueId = req.Payload.OwnerPrimaryKey.Value,
                                PulseType = PulseIssuesTypesEnum.Files,
                                Tag = Routes.ADD_ACTION_NAME
                            },
                            SenderActionUserId = GlobalStaticConstantsRoles.Roles.System,
                        }
                    };
                    await HelpdeskRepo.PulsePushAsync(reqPulse, false, token);
                    break;
            }
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TagSetAsync(TagSetModel req, CancellationToken token = default)
    {
        using StorageContext context = await cloudParametersDbFactory.CreateDbContextAsync(token);
        ResponseBaseModel res = new();

        IQueryable<TagModelDB> q = context
            .CloudTags
            .Where(x =>
            x.OwnerPrimaryKey == req.Id &&
            x.ApplicationName == req.ApplicationName &&
            x.NormalizedTagNameUpper == req.Name.ToUpper() &&
            x.PropertyName == req.PropertyName &&
            x.PrefixPropertyName == req.PrefixPropertyName);

        if (req.Set)
        {
            if (await q.AnyAsync(cancellationToken: token))
                res.AddInfo("Тег уже установлен");
            else
            {
                await context.AddAsync(new TagModelDB()
                {
                    ApplicationName = req.ApplicationName,
                    TagName = req.Name,
                    PropertyName = req.PropertyName,
                    CreatedAt = DateTime.UtcNow,
                    NormalizedTagNameUpper = req.Name.ToUpper(),
                    PrefixPropertyName = req.PrefixPropertyName,
                    OwnerPrimaryKey = req.Id,
                }, token);
                await context.SaveChangesAsync(token);
            }
        }
        else
        {
            if (q.Any())
                res.AddSuccess("Тег успешно установлен");
            else
                res.AddInfo("Тег отсутствует");
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<TagModelDB>> TagsSelectAsync(TPaginationRequestModel<SelectMetadataRequestModel> req, CancellationToken token = default)
    {
        if (req.PageSize < 5)
            req.PageSize = 5;
        using StorageContext context = await cloudParametersDbFactory.CreateDbContextAsync(token);

        IQueryable<TagModelDB> q = context
            .CloudTags
            .AsQueryable();

        if (req.Payload.ApplicationsNames is not null && req.Payload.ApplicationsNames.Length != 0)
            q = q.Where(x => req.Payload.ApplicationsNames.Any(y => y == x.ApplicationName));

        if (!string.IsNullOrWhiteSpace(req.Payload.PropertyName))
            q = q.Where(x => x.PropertyName == req.Payload.PropertyName);

        if (!string.IsNullOrWhiteSpace(req.Payload.PrefixPropertyName))
            q = q.Where(x => x.PrefixPropertyName == req.Payload.PrefixPropertyName);

        if (req.Payload.OwnerPrimaryKey.HasValue && req.Payload.OwnerPrimaryKey.Value > 0)
            q = q.Where(x => x.OwnerPrimaryKey == req.Payload.OwnerPrimaryKey.Value);

        if (!string.IsNullOrWhiteSpace(req.Payload.SearchQuery))
            q = q.Where(x => x.NormalizedTagNameUpper!.Contains(req.Payload.SearchQuery.ToUpper()));

        IQueryable<TagModelDB> oq = req.SortingDirection == DirectionsEnum.Up
          ? q.OrderBy(x => x.TagName).Skip(req.PageNum * req.PageSize).Take(req.PageSize)
          : q.OrderByDescending(x => x.TagName).Skip(req.PageNum * req.PageSize).Take(req.PageSize);

        int trc = await q.CountAsync(cancellationToken: token);
        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = trc,
            Response = await oq.ToListAsync(cancellationToken: token),
        };
    }
    #endregion

    #region storage parameters
    /// <inheritdoc/>
    public async Task<T?[]> FindAsync<T>(RequestStorageBaseModel req, CancellationToken token = default)
    {
        req.Normalize();
        using StorageContext context = await cloudParametersDbFactory.CreateDbContextAsync(token);
        string _tn = typeof(T).FullName ?? throw new Exception();
        StorageCloudParameterModelDB[] _dbd = await context
            .CloudProperties
            .Where(x => x.TypeName == _tn && x.ApplicationName == req.ApplicationName && x.PropertyName == req.PropertyName)
            .ToArrayAsync(cancellationToken: token);

        return [.. _dbd.Select(x => JsonConvert.DeserializeObject<T>(x.SerializedDataJson))];
    }

    /// <inheritdoc/>
    public async Task<T?> ReadAsync<T>(StorageMetadataModel req, CancellationToken token = default)
    {
        req.Normalize();
        string mem_key = $"{req.PropertyName}/{req.OwnerPrimaryKey}/{req.PrefixPropertyName}/{req.ApplicationName}".Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
        if (cache.TryGetValue(mem_key, out T? sd))
            return sd;

        using StorageContext context = await cloudParametersDbFactory.CreateDbContextAsync(token);
        string _tn = typeof(T).FullName ?? throw new Exception();

        StorageCloudParameterModelDB? pdb = await context
            .CloudProperties
            .Where(x => x.TypeName == _tn && x.OwnerPrimaryKey == req.OwnerPrimaryKey && x.PrefixPropertyName == req.PrefixPropertyName && x.ApplicationName == req.ApplicationName)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(x => x.PropertyName == req.PropertyName, cancellationToken: token);

        if (pdb is null)
            return default;

        try
        {
            T? rawData = JsonConvert.DeserializeObject<T>(pdb.SerializedDataJson);
            cache.Set(mem_key, rawData, new MemoryCacheEntryOptions().SetAbsoluteExpiration(_ts));
            return rawData;
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, $"Ошибка де-сериализации [{typeof(T).FullName}] из: {pdb.SerializedDataJson}");
            return default;
        }
    }

    /// <inheritdoc/>
    public async Task SaveAsync<T>(T obj, StorageMetadataModel set, bool trimHistory = false, CancellationToken token = default)
    {
        if (obj is null)
            throw new ArgumentNullException(nameof(obj));
        set.Normalize();
        StorageCloudParameterModelDB _set = new()
        {
            ApplicationName = set.ApplicationName,
            PropertyName = set.PropertyName,
            TypeName = typeof(T).FullName ?? throw new Exception(),
            SerializedDataJson = JsonConvert.SerializeObject(obj),
            OwnerPrimaryKey = set.OwnerPrimaryKey,
            PrefixPropertyName = set.PrefixPropertyName,
        };
        ResponseBaseModel res = await FlushParameterAsync(_set, trimHistory, token);
        if (res.Success())
        {
            string mem_key = $"{set.PropertyName}/{set.OwnerPrimaryKey}/{set.PrefixPropertyName}/{set.ApplicationName}".Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
            cache.Set(mem_key, obj, new MemoryCacheEntryOptions().SetAbsoluteExpiration(_ts));
        }
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int?>> FlushParameterAsync(StorageCloudParameterModelDB _set, bool trimHistory = false, CancellationToken token = default)
    {
        using StorageContext context = await cloudParametersDbFactory.CreateDbContextAsync(token);
        TResponseModel<int?> res = new();
        _set.Id = 0;
        await context.AddAsync(_set, token);
        bool success;
        _set.Normalize();
        Random rnd = new();
        for (int i = 0; i < 5; i++)
        {
            success = false;
            try
            {
                await context.SaveChangesAsync(token);
                string mem_key = $"{_set.PropertyName}/{_set.OwnerPrimaryKey}/{_set.PrefixPropertyName}/{_set.ApplicationName}".Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
                cache.Remove(mem_key);
                success = true;
                res.AddSuccess($"Данные успешно сохранены{(i > 0 ? $" (на попытке [{i}])" : "")}: {_set.ApplicationName}/{_set.PropertyName}".Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar));
                res.Response = _set.Id;
            }
            catch (Exception ex)
            {
                res.AddInfo($"Попытка записи [{i}]: {ex.Message}");
                _set.CreatedAt = DateTime.UtcNow;
                await Task.Delay(TimeSpan.FromMilliseconds(rnd.Next(100, 300)), token);
            }

            if (success)
                break;
        }

        IQueryable<StorageCloudParameterModelDB> qf = context
                 .CloudProperties
                 .Where(x => x.TypeName == _set.TypeName && x.ApplicationName == _set.ApplicationName && x.PropertyName == _set.PropertyName && x.OwnerPrimaryKey == _set.OwnerPrimaryKey && x.PrefixPropertyName == _set.PrefixPropertyName)
                 .AsQueryable();

        if (trimHistory)
        {
            await qf
                .Where(x => x.Id != _set.Id)
                .ExecuteDeleteAsync(cancellationToken: token);
        }
        else if (await qf.CountAsync(cancellationToken: token) > 50)
        {
            for (int i = 0; i < 5; i++)
            {
                success = false;
                try
                {
                    await qf
                        .OrderBy(x => x.CreatedAt)
                        .Take(50)
                        .ExecuteDeleteAsync(cancellationToken: token);
                    res.AddSuccess($"Ротация успешно выполнена на попытке [{i}]");
                    success = true;
                }
                catch (Exception ex)
                {
                    res.AddInfo($"Попытка записи [{i}]: {ex.Message}");
                    await Task.Delay(TimeSpan.FromMilliseconds(rnd.Next(100, 300)), token);
                }

                if (success)
                    break;
            }
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<StorageCloudParameterPayloadModel>> ReadParameterAsync(StorageMetadataModel req, CancellationToken token = default)
    {
        req.Normalize();
        string mem_key = $"{req.PropertyName}/{req.OwnerPrimaryKey}/{req.PrefixPropertyName}/{req.ApplicationName}".Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
        TResponseModel<StorageCloudParameterPayloadModel> res = new();
        if (cache.TryGetValue(mem_key, out StorageCloudParameterPayloadModel? sd))
        {
            res.Response = sd;
            return res;
        }
        string msg;
        using StorageContext context = await cloudParametersDbFactory.CreateDbContextAsync(token);
        StorageCloudParameterModelDB? parameter_db = await context
            .CloudProperties
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(x =>
            x.OwnerPrimaryKey == req.OwnerPrimaryKey &&
            x.PropertyName == req.PropertyName &&
            x.ApplicationName == req.ApplicationName &&
            x.PrefixPropertyName == req.PrefixPropertyName, cancellationToken: token);

        if (parameter_db is not null)
        {
            res.Response = new StorageCloudParameterPayloadModel()
            {
                ApplicationName = parameter_db.ApplicationName,
                PropertyName = parameter_db.PropertyName,
                OwnerPrimaryKey = parameter_db.OwnerPrimaryKey,
                PrefixPropertyName = parameter_db.PrefixPropertyName,
                TypeName = parameter_db.TypeName,
                SerializedDataJson = parameter_db.SerializedDataJson,
            };
            msg = $"Параметр `{req}` прочитан";
            res.AddInfo(msg);
        }
        else
        {
            msg = $"Параметр не найден: `{req}`";
            res.AddWarning(msg);
        }

        cache.Set(mem_key, res.Response, new MemoryCacheEntryOptions().SetAbsoluteExpiration(_ts));

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<StorageCloudParameterPayloadModel>>> ReadParametersAsync(StorageMetadataModel[] req, CancellationToken token = default)
    {
        BlockingCollection<StorageCloudParameterPayloadModel> res = [];
        BlockingCollection<ResultMessage> _messages = [];
        await Task.WhenAll(req.Select(x => Task.Run(async () =>
        {
            x.Normalize();
            TResponseModel<StorageCloudParameterPayloadModel> _subResult = await ReadParameterAsync(x);
            if (_subResult.Success() && _subResult.Response is not null)
                res.Add(_subResult.Response);
            if (_subResult.Messages.Count != 0)
                _subResult.Messages.ForEach(m => _messages.Add(m));
        })));

        return new TResponseModel<List<StorageCloudParameterPayloadModel>>()
        {
            Response = [.. res],
            Messages = [.. _messages],
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<FoundParameterModel[]?>> FindAsync(RequestStorageBaseModel req, CancellationToken token = default)
    {
        req.Normalize();
        TResponseModel<FoundParameterModel[]?> res = new();
        using StorageContext context = await cloudParametersDbFactory.CreateDbContextAsync(token);
        StorageCloudParameterModelDB[] prop_db = await context
            .CloudProperties
            .Where(x => req.PropertyName == x.PropertyName && req.ApplicationName == x.ApplicationName)
            .ToArrayAsync(cancellationToken: token);

        res.Response = [.. prop_db
            .Select(x => new FoundParameterModel()
            {
                SerializedDataJson = JsonConvert.SerializeObject(x),
                CreatedAt = DateTime.UtcNow,
                OwnerPrimaryKey = x.OwnerPrimaryKey,
                PrefixPropertyName = x.PrefixPropertyName,
            })];

        return res;
    }
    #endregion
}