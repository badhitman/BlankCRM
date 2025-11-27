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

        RowOfWarehouseDocumentModelDB? rowDb = req.Id > 0
            ? await context.RowsWarehouses.FirstAsync(x => x.Id == req.Id, cancellationToken: token)
            : null;

        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken: token);

        await queryDocumentDb.ExecuteUpdateAsync(set => set
             .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow)
             .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

        List<LockTransactionModelDB> lockers = [new()
        {
            LockerName = nameof(OfferAvailabilityModelDB),
            LockerId = req.OfferId,
            LockerAreaId = warehouseDocDB.WarehouseId,
        }];

        if (warehouseDocDB.WritingOffWarehouseId > 0)
        {
            lockers.Add(new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = req.OfferId,
                LockerAreaId = warehouseDocDB.WritingOffWarehouseId,
            });
        }

        if (rowDb is not null && rowDb.OfferId != req.OfferId)
        {
            lockers.Add(new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = rowDb.OfferId,
                LockerAreaId = warehouseDocDB.WarehouseId,
            });

            if (warehouseDocDB.WritingOffWarehouseId > 0)
                lockers.Add(new()
                {
                    LockerName = nameof(OfferAvailabilityModelDB),
                    LockerId = rowDb.OfferId,
                    LockerAreaId = warehouseDocDB.WritingOffWarehouseId,
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

        OfferAvailabilityModelDB?
            regOfferAv = offerAvailabilityDB.FirstOrDefault(x => x.OfferId == req.OfferId && x.WarehouseId == warehouseDocDB.WarehouseId),
            regOfferAvWritingOff = offerAvailabilityDB.FirstOrDefault(x => x.OfferId == req.OfferId && x.WarehouseId == warehouseDocDB.WritingOffWarehouseId);

        if (!warehouseDocDB.IsDisabled)
        {
            if (rowDb is not null && rowDb.OfferId != req.OfferId)
            {
                regOfferAv = offerAvailabilityDB.FirstOrDefault(x => x.OfferId == rowDb.OfferId && x.WarehouseId == warehouseDocDB.WarehouseId);
                regOfferAvWritingOff = offerAvailabilityDB.FirstOrDefault(x => x.OfferId == rowDb.OfferId && x.WarehouseId == warehouseDocDB.WritingOffWarehouseId);

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

                if (regOfferAv is null || regOfferAv.Quantity < rowDb.Quantity)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Остаток офера #{rowDb.OfferId} на складе #{warehouseDocDB.WarehouseId} не достаточный";
                    loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
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

            decimal _quantity;
            if (warehouseDocDB.WritingOffWarehouseId > 0)
            {
                _quantity = rowDb is null
                   ? -req.Quantity
                   : rowDb.Quantity - req.Quantity;

                if (regOfferAvWritingOff is null || regOfferAvWritingOff.Quantity < -_quantity)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Остаток офера #{req.OfferId} на складе #{warehouseDocDB.WritingOffWarehouseId} списания не достаточный";
                    loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError(msg);
                    return res;
                }

                await context.OffersAvailability.Where(y => y.Id == regOfferAvWritingOff.Id)
                        .ExecuteUpdateAsync(set => set
                           .SetProperty(p => p.Quantity, p => p.Quantity + _quantity), cancellationToken: token);

                regOfferAvWritingOff.Quantity += _quantity;
            }

            _quantity = rowDb is null
                   ? req.Quantity
                   : req.Quantity - rowDb.Quantity;

            if (regOfferAv is null)
            {
                if (_quantity < 0)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"На складе #{warehouseDocDB.WarehouseId} отсутствует офер #{req.OfferId}";
                    loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError(msg);
                    return res;
                }

                if (_quantity > 0)
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
                if (regOfferAv.Quantity < -_quantity)
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
            if (rowDb.Version != req.Version)
            {
                await transaction.RollbackAsync(token);
                msg = "Строка документа была уже кем-то изменена";
                loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError($"{msg}. Обновите документ (F5), что бы получить актуальные данные");
                return res;
            }

            res.Response = await context.RowsWarehouses
                   .Where(x => x.Id == req.Id)
                   .ExecuteUpdateAsync(set => set
                      .SetProperty(p => p.OfferId, req.OfferId)
                      .SetProperty(p => p.NomenclatureId, req.NomenclatureId)
                      .SetProperty(p => p.Quantity, req.Quantity)
                      .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);
        }

        context.RemoveRange(lockers);
        await context.SaveChangesAsync(token);
        await transaction.CommitAsync(token);

        res.AddSuccess($"Обновление `строки складского документа` выполнено");
        return res;
    }
}