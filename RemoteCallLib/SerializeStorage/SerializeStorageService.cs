﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using DbcLib;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLib;

namespace RemoteCallLib;

/// <inheritdoc/>
public class SerializeStorageService(IDbContextFactory<CloudParametersContext> cloudParametersDbFactory, ILogger<SerializeStorageService> loggerRepo) : ISerializeStorage
{
    /// <inheritdoc/>
    public async Task<T?[]> Find<T>(RequestStorageCloudParameterModel req)
    {
        CloudParametersContext context = await cloudParametersDbFactory.CreateDbContextAsync();
        string _tn = typeof(T).FullName ?? throw new Exception();
        StorageCloudParameterModelDB[] _dbd = await context
            .CloudProperties
            .Where(x => x.TypeName == _tn && x.ApplicationName == req.ApplicationName && x.Name == req.Name)
            .ToArrayAsync();

        return _dbd.Select(x => JsonConvert.DeserializeObject<T>(x.SerializedDataJson)).ToArray();
    }

    /// <inheritdoc/>
    public async Task<T?> Read<T>(StorageCloudParameterModel req)
    {
        CloudParametersContext context = await cloudParametersDbFactory.CreateDbContextAsync();
        string _tn = typeof(T).FullName ?? throw new Exception();

        StorageCloudParameterModelDB? pdb = await context
            .CloudProperties
            .Where(x => x.TypeName == _tn && x.OwnerPrimaryKey == req.OwnerPrimaryKey && x.PrefixPropertyName == req.PrefixPropertyName && x.ApplicationName == req.ApplicationName)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(x => x.Name == req.Name);

        if (pdb is null)
            return default;

        try
        {
            return JsonConvert.DeserializeObject<T>(pdb.SerializedDataJson);
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, $"Ошибка де-сериализации [{typeof(T).FullName}] из: {pdb.SerializedDataJson}");
            return default;
        }
    }

    /// <inheritdoc/>
    public async Task Save<T>(T obj, StorageCloudParameterModel set)
    {
        if (obj is null)
            throw new ArgumentNullException(nameof(obj));

        StorageCloudParameterModelDB _set = new()
        {
            ApplicationName = set.ApplicationName,
            Name = set.Name,
            TypeName = typeof(T).FullName ?? throw new Exception(),
            SerializedDataJson = JsonConvert.SerializeObject(obj),
            OwnerPrimaryKey = set.OwnerPrimaryKey,
            PrefixPropertyName = set.PrefixPropertyName,
        };
        ResponseBaseModel res = await FlushParameter(_set);
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int?>> FlushParameter(StorageCloudParameterModelDB _set)
    {
        CloudParametersContext context = await cloudParametersDbFactory.CreateDbContextAsync();
        TResponseModel<int?> res = new();
        await context.AddAsync(_set);
        bool success;
        Random rnd = new();
        for (int i = 0; i < 5; i++)
        {
            success = false;
            try
            {
                await context.SaveChangesAsync();
                success = true;
                res.AddSuccess($"Данные успешно сохранены на попытке [{i}]");
                res.Response = _set.Id;
            }
            catch (Exception ex)
            {
                res.AddInfo($"Попытка записи [{i}]: {ex.Message}");
                _set.CreatedAt = DateTime.UtcNow;
                await Task.Delay(TimeSpan.FromMilliseconds(rnd.Next(400, 700)));
            }

            if (success)
                break;
        }

        IQueryable<StorageCloudParameterModelDB> qf = context
                 .CloudProperties
                 .Where(x => x.TypeName == _set.TypeName && x.ApplicationName == _set.ApplicationName && x.Name == _set.Name && x.OwnerPrimaryKey == _set.OwnerPrimaryKey && x.PrefixPropertyName == _set.PrefixPropertyName)
                 .AsQueryable();

        int history_count = await qf.CountAsync();
        if (history_count > 150)
        {
            for (int i = 0; i < 5; i++)
            {
                success = false;
                try
                {
                    await qf
                        .OrderBy(x => x.CreatedAt)
                        .Take(50)
                        .ExecuteDeleteAsync();
                    res.AddSuccess($"Ротация успешно выполнена на попытке [{i}]");
                    success = true;
                }
                catch (Exception ex)
                {
                    res.AddInfo($"Попытка записи [{i}]: {ex.Message}");
                    await Task.Delay(TimeSpan.FromMilliseconds(rnd.Next(400, 700)));
                }

                if (success)
                    break;
            }
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<StorageCloudParameterPayloadModel?>> ReadParameter(StorageCloudParameterReadModel req)
    {
        CloudParametersContext context = await cloudParametersDbFactory.CreateDbContextAsync();
        TResponseModel<StorageCloudParameterPayloadModel?> res = new();
        StorageCloudParameterModelDB? parameter_db = await context
            .CloudProperties
            .FirstOrDefaultAsync(x =>
            x.OwnerPrimaryKey == req.OwnerPrimaryKey &&
            x.Name == req.Name &&
            x.ApplicationName == req.ApplicationName &&
            x.PrefixPropertyName == req.PrefixPropertyName &&
            x.TypeName == req.TypeName);

        res.Response = new StorageCloudParameterPayloadModel()
        {
            ApplicationName = req.ApplicationName,
            Name = req.Name,
            OwnerPrimaryKey = req.OwnerPrimaryKey,
            PrefixPropertyName = req.PrefixPropertyName,
            TypeName = req.TypeName,
            SerializedDataJson = parameter_db is null ? string.Empty : JsonConvert.SerializeObject(parameter_db) ?? string.Empty,
        };

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<FoundParameterModel[]?>> Find(RequestStorageCloudParameterModel req)
    {
        TResponseModel<FoundParameterModel[]?> res = new();
        CloudParametersContext context = await cloudParametersDbFactory.CreateDbContextAsync();
        StorageCloudParameterModelDB[] prop_db = await context
            .CloudProperties
            .Where(x => req.Name == x.Name && req.ApplicationName == x.ApplicationName)
            .ToArrayAsync();

        res.Response = prop_db
            .Select(x => new FoundParameterModel()
            {
                SerializedDataJson = JsonConvert.SerializeObject(x),
                CreatedAt = DateTime.UtcNow,
                OwnerPrimaryKey = x.OwnerPrimaryKey,
                PrefixPropertyName = x.PrefixPropertyName,
            })
            .ToArray();

        return res;
    }
}