////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Newtonsoft.Json;
using SharedLib;
using DbcLib;

namespace CommerceService;

/// <summary>
/// Commerce
/// </summary>
public partial class CommerceImplementService : ICommerceService
{
    /// <inheritdoc/>
    public async Task<DocumentNewVersionResponseModel> RowForWarehouseDocumentUpdateOrCreateAsync(TAuthRequestStandardModel<RowOfWarehouseDocumentModelDB> req, CancellationToken token = default)
    {
        #region prepare
        string msg;
        DocumentNewVersionResponseModel res = new();

        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }

        if (req.Payload.Quantity <= 0)
        {
            res.AddError($"Количество должно быть больше нуля");
            return res;
        }

        TResponseModel<bool?> warehouseNegativeBalanceAllowed = await StorageTransmissionRepo
              .ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.WarehouseNegativeBalanceAllowed, token);

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<WarehouseDocumentModelDB> queryDocumentDb = context
            .WarehouseDocuments
            .Where(x => x.Id == req.Payload.WarehouseDocumentId);

        WarehouseDocumentRecord warehouseDocDB = await queryDocumentDb
            .Select(x => new WarehouseDocumentRecord(x.WarehouseId, x.WritingOffWarehouseId, x.IsDisabled))
            .FirstAsync(cancellationToken: token);

        if (warehouseDocDB.WarehouseId == 0)
        {
            msg = "В документе не указан склад";
            loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError(msg);
            return res;
        }

        if (await context.RowsWarehouses.AnyAsync(x => x.Id != req.Payload.Id && x.OfferId == req.Payload.OfferId && x.WarehouseDocumentId == req.Payload.WarehouseDocumentId, cancellationToken: token))
        {
            msg = "В документе уже существует такая позиция. Установите ему требуемое количество";
            loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError(msg);
            return res;
        }

        if (req.Payload.NomenclatureId < 1)
            req.Payload.NomenclatureId = await context.Offers
                .Where(x => x.Id == req.Payload.OfferId)
                .Select(x => x.NomenclatureId)
                .FirstAsync(cancellationToken: token);

        loggerRepo.LogInformation($"{nameof(warehouseDocDB)}: {JsonConvert.SerializeObject(warehouseDocDB, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
        RowOfWarehouseDocumentModelDB? rowDb = req.Payload.Id > 0
            ? await context.RowsWarehouses.FirstAsync(x => x.Id == req.Payload.Id, cancellationToken: token)
            : null;

        if (rowDb is not null && rowDb.Version != req.Payload.Version)
        {
            msg = "Строка документа была уже кем-то изменена";
            loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError($"{msg}. Обновите документ (F5), что бы получить актуальные данные");
            return res;
        }

        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken: token);
        res.DocumentNewVersion = Guid.NewGuid();
        await queryDocumentDb.ExecuteUpdateAsync(set => set
             .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow)
             .SetProperty(p => p.Version, res.DocumentNewVersion), cancellationToken: token);

        List<LockTransactionModelDB> lockers = [new()
        {
            LockerName = nameof(OfferAvailabilityModelDB),
            LockerId = req.Payload.OfferId,
            LockerAreaId = warehouseDocDB.WarehouseId,
            Marker = nameof(RowForWarehouseDocumentUpdateOrCreateAsync),
        }];

        if (warehouseDocDB.WritingOffWarehouseId > 0)
        {
            lockers.Add(new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = req.Payload.OfferId,
                LockerAreaId = warehouseDocDB.WritingOffWarehouseId,
                Marker = nameof(RowForWarehouseDocumentUpdateOrCreateAsync),
            });
        }

        if (rowDb is not null && rowDb.OfferId != req.Payload.OfferId)
        {
            lockers.Add(new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = rowDb.OfferId,
                LockerAreaId = warehouseDocDB.WarehouseId,
                Marker = nameof(RowForWarehouseDocumentUpdateOrCreateAsync),
            });

            if (warehouseDocDB.WritingOffWarehouseId > 0)
                lockers.Add(new()
                {
                    LockerName = nameof(OfferAvailabilityModelDB),
                    LockerId = rowDb.OfferId,
                    LockerAreaId = warehouseDocDB.WritingOffWarehouseId,
                    Marker = nameof(RowForWarehouseDocumentUpdateOrCreateAsync),
                });
        }

        try
        {
            await context.LockTransactions.AddRangeAsync(lockers, token);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            msg = $"Не удалось выполнить команду: ";
            res.AddError($"{msg}{ex.Message}");
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            return res;
        }
        #endregion

        IQueryable<OfferAvailabilityModelDB> offerAvailabilityQuery = context
            .OffersAvailability
            .Where(x => x.OfferId == req.Payload.OfferId || (rowDb != null && x.OfferId == rowDb.OfferId));

        List<OfferAvailabilityModelDB> offerAvailabilityDB = await offerAvailabilityQuery.Include(x => x.Offer)
            .Include(x => x.Nomenclature)
            .ToListAsync(cancellationToken: token);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(MethodBase.GetCurrentMethod()!.Name, req.Payload.WarehouseDocumentId.ToString(), GlobalTools.CreateDeepCopy(offerAvailabilityDB));
        if (!warehouseDocDB.IsDisabled)
        {
            OfferAvailabilityModelDB? regOfferAv, regOfferAvWritingOff;
            if (rowDb is not null && rowDb.OfferId != req.Payload.OfferId)
            {
                regOfferAv = offerAvailabilityDB.FirstOrDefault(x => x.OfferId == rowDb.OfferId && x.WarehouseId == warehouseDocDB.WritingOffWarehouseId);
                regOfferAvWritingOff = offerAvailabilityDB.FirstOrDefault(x => x.OfferId == rowDb.OfferId && x.WarehouseId == warehouseDocDB.WarehouseId);

                if (warehouseDocDB.WritingOffWarehouseId > 0)
                {
                    if (regOfferAvWritingOff is null)
                    {
                        regOfferAvWritingOff = new()
                        {
                            WarehouseId = warehouseDocDB.WritingOffWarehouseId,
                            Quantity = rowDb.Quantity,
                            NomenclatureId = rowDb.NomenclatureId,
                            OfferId = rowDb.OfferId,
                        };

                        await context.OffersAvailability.AddAsync(regOfferAvWritingOff, token);
                        await context.SaveChangesAsync(token);
                        offerAvailabilityDB.Add(regOfferAvWritingOff);
                    }
                    else
                    {
                        await context.OffersAvailability.Where(y => y.Id == regOfferAvWritingOff.Id)
                            .ExecuteUpdateAsync(set => set
                                .SetProperty(p => p.Quantity, p => p.Quantity + rowDb.Quantity), cancellationToken: token);

                        regOfferAvWritingOff.Quantity += rowDb.Quantity;
                    }
                }

                if (regOfferAv is null)
                {
                    if (warehouseNegativeBalanceAllowed.Response != true)
                    {
                        await transaction.RollbackAsync(token);
                        msg = $"Остаток офера #{rowDb.OfferId} на складе #{warehouseDocDB.WarehouseId} не достаточный";
                        loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                        res.AddError(msg);
                        return res;
                    }

                    regOfferAv = new()
                    {
                        WarehouseId = warehouseDocDB.WarehouseId,
                        Quantity = -rowDb.Quantity,
                        NomenclatureId = rowDb.NomenclatureId,
                        OfferId = rowDb.OfferId,
                    };
                    await context.OffersAvailability.AddAsync(regOfferAv, token);
                    await context.SaveChangesAsync(token);
                    offerAvailabilityDB.Add(regOfferAv);
                }
                else if (regOfferAv.Quantity < rowDb.Quantity && warehouseNegativeBalanceAllowed.Response != true)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Остаток офера #{rowDb.OfferId} на складе #{warehouseDocDB.WarehouseId} не достаточный";
                    loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError(msg);
                    return res;
                }
                else
                {
                    await context.OffersAvailability.Where(y => y.Id == regOfferAv.Id)
                        .ExecuteUpdateAsync(set => set
                            .SetProperty(p => p.Quantity, p => p.Quantity - rowDb.Quantity), cancellationToken: token);

                    regOfferAv.Quantity -= rowDb.Quantity;
                }
            }

            regOfferAv = offerAvailabilityDB.FirstOrDefault(x => x.OfferId == req.Payload.OfferId && x.WarehouseId == warehouseDocDB.WarehouseId);
            regOfferAvWritingOff = offerAvailabilityDB.FirstOrDefault(x => x.OfferId == req.Payload.OfferId && x.WarehouseId == warehouseDocDB.WritingOffWarehouseId);
            //
            decimal _quantity = rowDb is null
                   ? req.Payload.Quantity
                   : rowDb.Quantity - req.Payload.Quantity;

            if (warehouseDocDB.WritingOffWarehouseId > 0)
            {
                if (regOfferAvWritingOff is null)
                {
                    if (warehouseNegativeBalanceAllowed.Response != true)
                    {
                        await transaction.RollbackAsync(token);
                        msg = $"Остаток офера #{req.Payload.OfferId} на складе #{warehouseDocDB.WritingOffWarehouseId} списания не достаточный";
                        loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                        res.AddError(msg);
                        return res;
                    }

                    regOfferAvWritingOff = new()
                    {
                        WarehouseId = warehouseDocDB.WritingOffWarehouseId,
                        OfferId = req.Payload.OfferId,
                        NomenclatureId = req.Payload.NomenclatureId,
                        Quantity = -_quantity,
                    };
                    await context.OffersAvailability.AddAsync(regOfferAvWritingOff, token);
                    await context.SaveChangesAsync(token);
                    offerAvailabilityDB.Add(regOfferAvWritingOff);
                }
                else if (regOfferAvWritingOff.Quantity - _quantity < 0 && warehouseNegativeBalanceAllowed.Response != true)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Остаток офера #{req.Payload.OfferId} на складе #{warehouseDocDB.WritingOffWarehouseId} списания не достаточный";
                    loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError(msg);
                    return res;
                }
                else
                {
                    await context.OffersAvailability.Where(y => y.Id == regOfferAvWritingOff.Id)
                        .ExecuteUpdateAsync(set => set
                            .SetProperty(p => p.Quantity, p => p.Quantity - _quantity), cancellationToken: token);

                    regOfferAvWritingOff.Quantity -= _quantity;
                }
            }

            if (regOfferAv is null)
            {
                if (_quantity < 0)
                {
                    if (warehouseNegativeBalanceAllowed.Response != true)
                    {
                        await transaction.RollbackAsync(token);
                        msg = $"На складе #{warehouseDocDB.WarehouseId} отсутствует офер #{req.Payload.OfferId}";
                        loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                        res.AddError(msg);
                        return res;
                    }
                    else
                    {
                        regOfferAv = new()
                        {
                            WarehouseId = warehouseDocDB.WarehouseId,
                            OfferId = req.Payload.OfferId,
                            NomenclatureId = req.Payload.NomenclatureId,
                            Quantity = -_quantity,
                        };
                    }
                }
                else if (_quantity > 0)
                    await context.OffersAvailability.AddAsync(new()
                    {
                        OfferId = req.Payload.OfferId,
                        NomenclatureId = req.Payload.NomenclatureId,
                        WarehouseId = warehouseDocDB.WarehouseId,
                        Quantity = _quantity,
                    }, token);
            }
            else
            {
                if (regOfferAv.Quantity + _quantity < 0 && warehouseNegativeBalanceAllowed.Response != true)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"На складе #{warehouseDocDB.WarehouseId} отсутствует офер #{regOfferAv.OfferId}";
                    loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError(msg);
                    return res;
                }

                await context.OffersAvailability
                    .Where(x => x.Id == regOfferAv.Id)
                        .ExecuteUpdateAsync(set => set
                            .SetProperty(p => p.Quantity, p => p.Quantity + _quantity), cancellationToken: token);

                regOfferAv.Quantity += _quantity;
            }
        }

        if (rowDb is null)
        {
            req.Payload.Version = Guid.NewGuid();
            await context.RowsWarehouses.AddAsync(req.Payload, token);
            await context.SaveChangesAsync(token);

            res.AddSuccess("Строка добавлена к документу");
            res.Response = req.Payload.Id;
        }
        else
        {
            res.Response = await context.RowsWarehouses
                   .Where(x => x.Id == req.Payload.Id)
                   .ExecuteUpdateAsync(set => set
                      .SetProperty(p => p.OfferId, req.Payload.OfferId)
                      .SetProperty(p => p.NomenclatureId, req.Payload.NomenclatureId)
                      .SetProperty(p => p.Quantity, req.Payload.Quantity)
                      .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

            res.AddSuccess("Строка документа обновлена");
        }

        if (lockers.Count != 0)
            context.LockTransactions.RemoveRange(lockers);

        await context.SaveChangesAsync(token);
        await transaction.CommitAsync(token);
        await indexingRepo.SaveHistoryForReceiverAsync(trace.SetResponse(await offerAvailabilityQuery.ToListAsync(cancellationToken: token)), token);
        res.AddSuccess($"Обновление `строки складского документа` выполнено");
        return res;
    }
}