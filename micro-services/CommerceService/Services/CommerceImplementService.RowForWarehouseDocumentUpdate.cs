////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
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
    public async Task<TResponseModel<int>> RowForWarehouseDocumentUpdateAsync(RowOfWarehouseDocumentModelDB req, CancellationToken token = default)
    {
        string msg;
        TResponseModel<int> res = new();
        if (req.Quantity <= 0)
        {
            res.AddError($"Количество должно быть больше нуля");
            return res;
        }

        TResponseModel<bool?> warehouseNegativeBalanceAllowed = await StorageTransmissionRepo
              .ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.WarehouseNegativeBalanceAllowed, token);

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<WarehouseDocumentModelDB> queryDocumentDb = context
            .WarehouseDocuments
            .Where(x => x.Id == req.WarehouseDocumentId);

        WarehouseDocumentRecord warehouseDocDB = await queryDocumentDb
            .Select(x => new WarehouseDocumentRecord(x.WarehouseId, x.WritingOffWarehouseId, x.IsDisabled))
            .FirstAsync(cancellationToken: token);

        if (warehouseDocDB.WarehouseId == 0)
        {
            msg = "В документе не указан склад";
            loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError(msg);
            return res;
        }

        if (await context.RowsWarehouses.AnyAsync(x => x.Id != req.Id && x.OfferId == req.OfferId && x.WarehouseDocumentId == req.WarehouseDocumentId, cancellationToken: token))
        {
            msg = "В документе уже существует такая позиция. Установите ему требуемое количество";
            loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError(msg);
            return res;
        }

        if (req.NomenclatureId < 1)
            req.NomenclatureId = await context.Offers.Where(x => x.Id == req.OfferId).Select(x => x.NomenclatureId).FirstAsync(cancellationToken: token);
        loggerRepo.LogInformation($"{nameof(warehouseDocDB)}: {JsonConvert.SerializeObject(warehouseDocDB)}");
        RowOfWarehouseDocumentModelDB? rowDb = req.Id > 0
            ? await context.RowsWarehouses.FirstAsync(x => x.Id == req.Id, cancellationToken: token)
            : null;

        if (rowDb is not null && rowDb.Version != req.Version)
        {
            msg = "Строка документа была уже кем-то изменена";
            loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError($"{msg}. Обновите документ (F5), что бы получить актуальные данные");
            return res;
        }

        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken: token);

        await queryDocumentDb.ExecuteUpdateAsync(set => set
             .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow)
             .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

        List<LockTransactionModelDB> lockers = [new()
        {
            LockerName = nameof(OfferAvailabilityModelDB),
            LockerId = req.OfferId,
            LockerAreaId = warehouseDocDB.WarehouseId,
            Marker = nameof(RowForWarehouseDocumentUpdateAsync),
        }];

        if (warehouseDocDB.WritingOffWarehouseId > 0)
        {
            lockers.Add(new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = req.OfferId,
                LockerAreaId = warehouseDocDB.WritingOffWarehouseId,
                Marker = nameof(RowForWarehouseDocumentUpdateAsync),
            });
        }

        if (rowDb is not null && rowDb.OfferId != req.OfferId)
        {
            lockers.Add(new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = rowDb.OfferId,
                LockerAreaId = warehouseDocDB.WarehouseId,
                Marker = nameof(RowForWarehouseDocumentUpdateAsync),
            });

            if (warehouseDocDB.WritingOffWarehouseId > 0)
                lockers.Add(new()
                {
                    LockerName = nameof(OfferAvailabilityModelDB),
                    LockerId = rowDb.OfferId,
                    LockerAreaId = warehouseDocDB.WritingOffWarehouseId,
                    Marker = nameof(RowForWarehouseDocumentUpdateAsync),
                });
        }

        try
        {
            await context.AddRangeAsync(lockers, token);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            msg = $"Не удалось выполнить команду: ";
            res.AddError($"{msg}{ex.Message}");
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            return res;
        }

        List<OfferAvailabilityModelDB> offerAvailabilityDB = await context
            .OffersAvailability
            .Where(x => x.OfferId == req.OfferId || (rowDb != null && x.OfferId == rowDb.OfferId))
            .ToListAsync(cancellationToken: token);

        if (!warehouseDocDB.IsDisabled)
        {
            OfferAvailabilityModelDB? regOfferAv, regOfferAvWritingOff;
            if (rowDb is not null && rowDb.OfferId != req.OfferId)
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
                //
                if (regOfferAvWritingOff is null)
                {
                    if (warehouseNegativeBalanceAllowed.Response == true)
                    {
                        regOfferAvWritingOff = new()
                        {
                            WarehouseId = warehouseDocDB.WarehouseId,
                            Quantity = -rowDb.Quantity,
                            NomenclatureId = rowDb.NomenclatureId,
                            OfferId = rowDb.OfferId,
                        };
                        await context.OffersAvailability.AddAsync(regOfferAvWritingOff, token);
                        await context.SaveChangesAsync(token);
                        offerAvailabilityDB.Add(regOfferAvWritingOff);
                    }
                    else
                    {
                        await transaction.RollbackAsync(token);
                        msg = $"Остаток офера #{rowDb.OfferId} на складе #{warehouseDocDB.WarehouseId} не достаточный";
                        loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                        res.AddError(msg);
                        return res;
                    }

                }
                else if (regOfferAvWritingOff.Quantity < rowDb.Quantity && warehouseNegativeBalanceAllowed.Response != true)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Остаток офера #{rowDb.OfferId} на складе #{warehouseDocDB.WarehouseId} не достаточный";
                    loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError(msg);
                    return res;
                }
                else
                {
                    await context.OffersAvailability.Where(y => y.Id == regOfferAvWritingOff.Id)
                                               .ExecuteUpdateAsync(set => set
                                                  .SetProperty(p => p.Quantity, p => p.Quantity - rowDb.Quantity), cancellationToken: token);

                    regOfferAvWritingOff.Quantity -= rowDb.Quantity;
                }
            }

            regOfferAv = offerAvailabilityDB.FirstOrDefault(x => x.OfferId == req.OfferId && x.WarehouseId == warehouseDocDB.WarehouseId);
            regOfferAvWritingOff = offerAvailabilityDB.FirstOrDefault(x => x.OfferId == req.OfferId && x.WarehouseId == warehouseDocDB.WritingOffWarehouseId);
            // 

            decimal _quantity = rowDb is null
                   ? -req.Quantity
                   : -(rowDb.Quantity - req.Quantity);

            if (warehouseDocDB.WritingOffWarehouseId > 0)
            {
                if (regOfferAvWritingOff is null)
                {
                    if (warehouseNegativeBalanceAllowed.Response != true)
                    {
                        await transaction.RollbackAsync(token);
                        msg = $"Остаток офера #{req.OfferId} на складе #{warehouseDocDB.WritingOffWarehouseId} списания не достаточный";
                        loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                        res.AddError(msg);
                        return res;
                    }
                    else
                    {
                        regOfferAvWritingOff = new()
                        {
                            WarehouseId = warehouseDocDB.WritingOffWarehouseId,
                            OfferId = req.OfferId,
                            NomenclatureId = req.NomenclatureId,
                            Quantity = _quantity,
                        };
                        await context.OffersAvailability.AddAsync(regOfferAvWritingOff, token);
                        await context.SaveChangesAsync(token);
                        offerAvailabilityDB.Add(regOfferAvWritingOff);
                    }
                }
                else if (regOfferAvWritingOff.Quantity + _quantity < 0 && warehouseNegativeBalanceAllowed.Response != true)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Остаток офера #{req.OfferId} на складе #{warehouseDocDB.WritingOffWarehouseId} списания не достаточный";
                    loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError(msg);
                    return res;
                }
                else
                {
                    await context.OffersAvailability.Where(y => y.Id == regOfferAvWritingOff.Id)
                            .ExecuteUpdateAsync(set => set
                               .SetProperty(p => p.Quantity, p => p.Quantity + _quantity), cancellationToken: token);

                    regOfferAvWritingOff.Quantity += _quantity;
                }
            }

            _quantity = -_quantity;

            if (regOfferAv is null)
            {
                if (_quantity < 0)
                {
                    if (warehouseNegativeBalanceAllowed.Response != true)
                    {
                        await transaction.RollbackAsync(token);
                        msg = $"На складе #{warehouseDocDB.WarehouseId} отсутствует офер #{req.OfferId}";
                        loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                        res.AddError(msg);
                        return res;
                    }
                    else
                    {
                        regOfferAv = new()
                        {
                            WarehouseId = warehouseDocDB.WarehouseId,
                            OfferId = req.OfferId,
                            NomenclatureId = req.NomenclatureId,
                            Quantity = _quantity,
                        };
                    }
                }
                else if (_quantity > 0)
                    await context.OffersAvailability.AddAsync(new()
                    {
                        OfferId = req.OfferId,
                        NomenclatureId = req.NomenclatureId,
                        WarehouseId = warehouseDocDB.WarehouseId,
                        Quantity = _quantity,
                    }, token);
            }
            else
            {
                if (regOfferAv.Quantity < -_quantity && warehouseNegativeBalanceAllowed.Response != true)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"На складе #{warehouseDocDB.WarehouseId} отсутствует офер #{regOfferAv.OfferId}";
                    loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
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
            req.Version = Guid.NewGuid();
            await context.AddAsync(req, token);
            await context.SaveChangesAsync(token);

            res.AddSuccess("Товар добавлен к документу");
            res.Response = req.Id;
        }
        else
        {

            res.Response = await context.RowsWarehouses
                   .Where(x => x.Id == req.Id)
                   .ExecuteUpdateAsync(set => set
                      .SetProperty(p => p.OfferId, req.OfferId)
                      .SetProperty(p => p.NomenclatureId, req.NomenclatureId)
                      .SetProperty(p => p.Quantity, req.Quantity)
                      .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);
        }

        if (lockers.Count != 0)
            context.RemoveRange(lockers);

        await context.SaveChangesAsync(token);

        await transaction.CommitAsync(token);

        res.AddSuccess($"Обновление `строки складского документа` выполнено");
        return res;
    }
}