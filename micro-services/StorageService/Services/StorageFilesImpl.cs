////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using DbcLib;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using SharedLib;
using System.Text;
using System.Text.RegularExpressions;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace StorageService;

/// <summary>
/// Хранилище файлов приложений IMongoDatabase mongoFs
/// </summary>
public class StorageFilesImpl(
    IDbContextFactory<StorageContext> cloudParametersDbFactory,
    IOptions<MongoConfigModel> mongoConf,
    IIdentityTransmission identityRepo,
    ICommerceTransmission commRepo,
    IHelpDeskTransmission HelpDeskRepo,
    WebConfigModel webConfig,
    ILogger<ParametersStorage> loggerRepo) : IFilesStorage
{

#if DEBUG
    static readonly TimeSpan _ts = TimeSpan.FromSeconds(2);
#else
    static readonly TimeSpan _ts = TimeSpan.FromSeconds(10);
#endif

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
    public async Task<TPaginationResponseStandardModel<StorageFileModelDB>> FilesSelectAsync(TPaginationRequestStandardModel<SelectMetadataRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
        {
            loggerRepo.LogError("req.Payload is null");
            return new();
        }

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
        TPaginationResponseStandardModel<StorageFileModelDB> res = new()
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
    public Task<TResponseModel<DirectoryReadResponseModel>> GetDirectoryInfoAsync(DirectoryReadRequestModel req, CancellationToken token = default)
    {
        TResponseModel<DirectoryReadResponseModel> res = new();
        if (!Directory.Exists(req.FolderPath))
        {
            res.AddError($"Папка [{req.FolderPath}] не существует");
            return Task.FromResult(res);
        }

        FileInfo _root = new(req.FolderPath);
        res.Response = new()
        {
            FullPath = _root.FullName,
            LastWriteTimeUtc = _root.LastWriteTimeUtc,
            IsDirectory = (_root.Attributes & FileAttributes.Directory) == FileAttributes.Directory,
            DirectoryItems = []
        };

        if (!res.Response.IsDirectory)
            res.Response.FileSizeBytes = _root.Length;

        string[] allFiles = Directory.GetFiles(req.FolderPath), allDirectories = Directory.GetDirectories(req.FolderPath);
        foreach (string _f in allDirectories.Union(allFiles))
        {
            FileAttributes attr = File.GetAttributes(_f);
            bool _isDir = (attr & FileAttributes.Directory) == FileAttributes.Directory;
            FileInfo _fi = new(_f);
            res.Response.DirectoryItems.Add(new()
            {
                FullPath = _fi.FullName,
                IsDirectory = _isDir,
                FileSizeBytes = _isDir ? default : _fi.Length,
                LastWriteTimeUtc = _isDir ? default : _fi.LastWriteTimeUtc,
            });
        }
        return Task.FromResult(res);
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<FileContentModel>> ReadFileAsync(TAuthRequestStandardModel<RequestFileReadModel> req, CancellationToken token = default)
    {
        TResponseModel<FileContentModel> res = new();

        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }

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

        if (!allowed) // проверка токена прямого доступа к файлу
            allowed = !string.IsNullOrWhiteSpace(req.Payload.TokenAccess) && file_db.AccessRules?.Any(x => x.AccessRuleType == FileAccessRulesTypesEnum.Token && x.Option == req.Payload.TokenAccess) == true;

        UserInfoModel? currentUser = null;
        if (!allowed && !string.IsNullOrWhiteSpace(req.SenderActionUserId))
        {
            TResponseModel<UserInfoModel[]> findUserRes = await identityRepo.GetUsersOfIdentityAsync([req.SenderActionUserId], token);
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
                    TAuthRequestStandardModel<IssuesReadRequestModel> reqIssues = new()
                    {
                        SenderActionUserId = req.SenderActionUserId,
                        Payload = new()
                        {
                            IssuesIds = [.. issues_ids],
                            IncludeSubscribersOnly = false,
                        }
                    };
                    TResponseModel<IssueHelpDeskModelDB[]> findIssues = await HelpDeskRepo.IssuesReadAsync(reqIssues, token);
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
        IMongoDatabase mongoFs = new MongoClient(mongoConf.Value.ToString()).GetDatabase($"{mongoConf.Value.FilesSystemName}{GlobalStaticConstantsTransmission.GetModePrefix}");
        GridFSBucket gridFS = new(mongoFs);

        try
        {
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
        }
        catch (Exception ex)
        {
            res.Response = new()
            {
                ApplicationName = file_db.ApplicationName,
                AuthorIdentityId = file_db.AuthorIdentityId,
                FileName = "file_not_found-grid_fs",
                PropertyName = file_db.PropertyName,
                CreatedAt = file_db.CreatedAt,
                OwnerPrimaryKey = file_db.OwnerPrimaryKey,
                PointId = file_db.PointId,
                PrefixPropertyName = file_db.PrefixPropertyName,
                Payload = Encoding.UTF8.GetBytes(ex.Message),
                ContentType = "text/plain;charset=UTF-8",
            };
        }
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<StorageFileModelDB>> SaveFileAsync(TAuthRequestStandardModel<StorageFileMetadataModel> req, CancellationToken token = default)
    {
        TResponseModel<StorageFileModelDB> res = new();

        if (req.Payload?.Payload is null)
        {
            res.AddError("req.Payload?.Payload is null");
            return res;
        }
        if (string.IsNullOrWhiteSpace(req.SenderActionUserId))
        {
            res.AddError("string.IsNullOrWhiteSpace(req.Payload.AuthorUserIdentity)");
            return res;
        }

        req.Payload.FileName ??= "";
        IMongoDatabase mongoFs = new MongoClient(mongoConf.Value.ToString()).GetDatabase($"{mongoConf.Value.FilesSystemName}{GlobalStaticConstantsTransmission.GetModePrefix}");
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
            AuthorIdentityId = req.SenderActionUserId,
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

        if (GlobalToolsStandard.IsImageFile(_file_name))
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
                NormalizedTagNameUpper = nameof(GlobalToolsStandard.IsImageFile).ToUpper(),
                TagName = nameof(GlobalToolsStandard.IsImageFile),
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
                        if (orderDb.HelpDeskId.HasValue && orderDb.HelpDeskId.Value > 0)
                        {
                            msg = $"В <a href=\"{webConfig.ClearBaseUri}/issue-card/{orderDb.HelpDeskId.Value}\">заказ #{orderDb.Id}</a> добавлен файл '<u>{_file_name}</u>' {GlobalToolsStandard.SizeDataAsString(req.Payload.Payload.Length)}";
                            loggerRepo.LogInformation($"{msg} [{nameof(res.Response.PointId)}:{_uf}]");
                            reqPulse = new()
                            {
                                Payload = new()
                                {
                                    Payload = new()
                                    {
                                        Description = msg,
                                        IssueId = orderDb.HelpDeskId.Value,
                                        PulseType = PulseIssuesTypesEnum.Files,
                                        Tag = Routes.ADD_ACTION_NAME
                                    },
                                    SenderActionUserId = GlobalStaticConstantsRoles.Roles.System,
                                }
                            };

                            await HelpDeskRepo.PulsePushAsync(reqPulse, false, token);
                        }
                    }
                    break;
                case Routes.ISSUE_CONTROLLER_NAME:
                    msg = $"В <a href=\"{webConfig.ClearBaseUri}/issue-card/{req.Payload.OwnerPrimaryKey.Value}\">заявку #{req.Payload.OwnerPrimaryKey.Value}</a> добавлен файл '<u>{_file_name}</u>' {GlobalToolsStandard.SizeDataAsString(req.Payload.Payload.Length)}";
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
                    await HelpDeskRepo.PulsePushAsync(reqPulse, false, token);
                    break;
            }
        }

        return res;
    }
}