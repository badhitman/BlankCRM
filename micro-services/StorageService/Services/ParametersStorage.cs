////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using MongoDB.Driver;
using SharedLib;
using DbcLib;

namespace StorageService;

/// <summary>
/// Хранилище параметров приложений
/// </summary>
/// <remarks>
/// Значения/данные сериализуются в JSON строку при сохранении и десерализируются при чтении
/// </remarks>
public class ParametersStorage(
    IDbContextFactory<StorageContext> cloudParametersDbFactory,
    IMemoryCache cache,
    ILogger<ParametersStorage> loggerRepo) : IParametersStorage
{
#if DEBUG
    static readonly TimeSpan _ts = TimeSpan.FromSeconds(2);
#else
    static readonly TimeSpan _ts = TimeSpan.FromSeconds(10);
#endif

    #region tags
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
            if (!q.Any())
                res.AddSuccess("Тег отсутствует");
            else
                res.AddInfo($"Тег удалён: {q.ExecuteDeleteAsync(cancellationToken: token)}");
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<TagViewModel>> TagsSelectAsync(TPaginationRequestStandardModel<SelectMetadataRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
        {
            loggerRepo.LogError("req.Payload is null");
            return new();
        }

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
        List<TagModelDB> _resDb = await oq.ToListAsync(cancellationToken: token);
        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = trc,
            Response = [.. _resDb.Select(x => (TagViewModel)x)],
        };
    }
    #endregion

    #region storage parameters
    /// <inheritdoc/>
    public async Task<FundedParametersModel<T>[]> FindAsync<T>(FindStorageBaseModel req, CancellationToken token = default)
    {
        req.Normalize();
        using StorageContext context = await cloudParametersDbFactory.CreateDbContextAsync(token);
        string _tn = typeof(T).FullName ?? throw new Exception();

        IQueryable<StorageCloudParameterModelDB> q = context
            .CloudProperties.Where(x => x.TypeName == _tn);

        if (!string.IsNullOrWhiteSpace(req.ApplicationName))
            q = q.Where(x => x.ApplicationName == req.ApplicationName);

        if (!string.IsNullOrWhiteSpace(req.PropertyName))
            q = q.Where(x => x.PropertyName == req.PropertyName);

        StorageCloudParameterModelDB[] _dbd = await q
            .ToArrayAsync(cancellationToken: token);

        return [.. _dbd.Select(x => new FundedParametersModel<T>()
        {
             ApplicationName = x.ApplicationName,
             PropertyName = x.PropertyName,
             CreatedAt = x.CreatedAt,
             Id = x.Id,
             OwnerPrimaryKey = x.OwnerPrimaryKey,
             PrefixPropertyName = x.PrefixPropertyName,
             Payload = string.IsNullOrWhiteSpace(x.SerializedDataJson) ? default : JsonConvert.DeserializeObject<T>(x.SerializedDataJson)
        })];
    }

    /// <inheritdoc/>
    public async Task<T?> ReadAsync<T>(StorageMetadataModel req, T? defaultValue = default, CancellationToken token = default)
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
            return defaultValue;

        try
        {
            T? rawData = string.IsNullOrWhiteSpace(pdb.SerializedDataJson) ? default : JsonConvert.DeserializeObject<T>(pdb.SerializedDataJson);
            cache.Set(mem_key, rawData, new MemoryCacheEntryOptions().SetAbsoluteExpiration(_ts));
            return rawData;
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, $"Ошибка де-сериализации [{typeof(T).FullName}] из: {pdb.SerializedDataJson}");
            return defaultValue;
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
    public async Task<TResponseModel<int?>> FlushParameterAsync(StorageCloudParameterViewModel set, bool trimHistory = false, CancellationToken token = default)
    {
        using StorageContext context = await cloudParametersDbFactory.CreateDbContextAsync(token);
        TResponseModel<int?> res = new();
        StorageCloudParameterModelDB _set = (StorageCloudParameterModelDB)set;
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
    public async Task<TResponseModel<FoundParameterModel[]>> FindRawAsync(FindStorageBaseModel req, CancellationToken token = default)
    {
        req.Normalize();
        TResponseModel<FoundParameterModel[]> res = new();
        using StorageContext context = await cloudParametersDbFactory.CreateDbContextAsync(token);
        IQueryable<StorageCloudParameterModelDB> q = context
            .CloudProperties.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.ApplicationName))
            q = q.Where(x => x.ApplicationName == req.ApplicationName);

        if (!string.IsNullOrWhiteSpace(req.PropertyName))
            q = q.Where(x => x.PropertyName == req.PropertyName);

        StorageCloudParameterModelDB[] prop_db = await q
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

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteParameter(StorageMetadataModel req, CancellationToken token = default)
    {
        req.Normalize();
        using StorageContext context = await cloudParametersDbFactory.CreateDbContextAsync(token);

        string mem_key = $"{req.PropertyName}/{req.OwnerPrimaryKey}/{req.PrefixPropertyName}/{req.ApplicationName}".Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);

        cache.Remove(mem_key);
        IQueryable<StorageCloudParameterModelDB> parameter_q = context
            .CloudProperties
            .OrderByDescending(x => x.CreatedAt)
            .Where(x =>
            x.OwnerPrimaryKey == req.OwnerPrimaryKey &&
            x.PropertyName == req.PropertyName &&
            x.ApplicationName == req.ApplicationName &&
            x.PrefixPropertyName == req.PrefixPropertyName);

        context.RemoveRange(parameter_q);

        return await context.SaveChangesAsync(token) == 0
            ? ResponseBaseModel.CreateWarning("Parameters not exist`s")
            : ResponseBaseModel.CreateSuccess("Deleted");
    }
    #endregion
}