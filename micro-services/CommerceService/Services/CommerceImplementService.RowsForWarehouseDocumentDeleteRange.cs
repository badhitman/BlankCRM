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
    public async Task<TResponseModel<bool>> RowsForWarehouseDocumentDeleteAsync(int[] req, CancellationToken token = default)
    {
        string msg;
        TResponseModel<bool> res = new() { Response = req.Any(x => x > 0) };
        if (!res.Response)
        {
            res.AddError($"Пустой запрос > {nameof(RowsForWarehouseDocumentDeleteAsync)}");
            return res;
        }

        TResponseModel<bool?> warehouseNegativeBalanceAllowed = await StorageTransmissionRepo
              .ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.WarehouseNegativeBalanceAllowed, token);

        req = [.. req.Distinct()];
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<RowOfWarehouseDocumentModelDB> mainQuery = context.RowsWarehouses.Where(x => req.Any(y => y == x.Id));
        var q = from r in mainQuery
                join d in context.WarehouseDocuments on r.WarehouseDocumentId equals d.Id
                select new
                {
                    DocumentId = d.Id,
                    d.IsDisabled,
                    d.WarehouseId,
                    d.WritingOffWarehouseId,
                    r.OfferId,
                    r.NomenclatureId,
                    r.Quantity
                };

        var _allRowsOfDocuments = await q
           .ToArrayAsync(cancellationToken: token);

        if (_allRowsOfDocuments.Length == 0)
        {
            res.AddWarning($"Данные не найдены. Метод удаления [{nameof(RowsForWarehouseDocumentDeleteAsync)}] не может выполнить команду.");
            return res;
        }
        List<LockTransactionModelDB> offersLocked = [];
        foreach (var x in _allRowsOfDocuments)
        {
            if (!offersLocked.Any(y => y.LockerId == x.OfferId && y.LockerAreaId == x.WarehouseId))
                offersLocked.Add(new LockTransactionModelDB()
                {
                    LockerName = nameof(OfferAvailabilityModelDB),
                    LockerId = x.OfferId,
                    LockerAreaId = x.WarehouseId,
                    Marker = nameof(RowsForWarehouseDocumentDeleteAsync),
                });

            if (!offersLocked.Any(y => y.LockerId == x.OfferId && y.LockerAreaId == x.WritingOffWarehouseId))
                offersLocked.Add(new LockTransactionModelDB()
                {
                    LockerName = nameof(OfferAvailabilityModelDB),
                    LockerId = x.OfferId,
                    LockerAreaId = x.WritingOffWarehouseId,
                    Marker = nameof(RowsForWarehouseDocumentDeleteAsync),
                });
        }

        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken: token);
        try
        {
            await context.AddRangeAsync(offersLocked, token);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            msg = $"Не удалось выполнить команду блокировки БД {nameof(WarehouseDocumentUpdateAsync)}: ";
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError($"{msg}{ex.Message}");
            return res;
        }

        int[] _offersIds = [.. _allRowsOfDocuments.Select(x => x.OfferId).Distinct()];
        List<OfferAvailabilityModelDB> registersOffersDb = await context.OffersAvailability
           .Where(x => _offersIds.Any(y => y == x.OfferId))
           .ToListAsync(cancellationToken: token);

        foreach (int doc_id in _allRowsOfDocuments.Select(x => x.DocumentId).Distinct())
            await context.WarehouseDocuments.Where(x => x.Id == doc_id).ExecuteUpdateAsync(set => set
            .SetProperty(p => p.Version, Guid.NewGuid())
            .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        foreach (var rowOfDocumentElement in _allRowsOfDocuments.Where(x => !x.IsDisabled))
        {
            loggerRepo.LogInformation($"{nameof(rowOfDocumentElement)}: {JsonConvert.SerializeObject(rowOfDocumentElement)}");
            OfferAvailabilityModelDB?
                offerRegister = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocumentElement.OfferId && x.WarehouseId == rowOfDocumentElement.WarehouseId),
                offerRegisterWritingOff = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocumentElement.OfferId && x.WarehouseId == rowOfDocumentElement.WritingOffWarehouseId);

            if (offerRegisterWritingOff is not null)
            {
                await context.OffersAvailability.Where(y => y.Id == offerRegisterWritingOff.Id)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Quantity, p => p.Quantity + rowOfDocumentElement.Quantity), cancellationToken: token);
            }
            else if (rowOfDocumentElement.WritingOffWarehouseId > 0)
            {
                await context.OffersAvailability.AddAsync(new OfferAvailabilityModelDB()
                {
                    WarehouseId = rowOfDocumentElement.WritingOffWarehouseId,
                    Quantity = rowOfDocumentElement.Quantity,
                    NomenclatureId = rowOfDocumentElement.NomenclatureId,
                    OfferId = rowOfDocumentElement.OfferId,
                }, token);
            }

            if (offerRegister is null)
            {
                if (warehouseNegativeBalanceAllowed.Response != true)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Остаток оффера #{rowOfDocumentElement.OfferId} на складе #{rowOfDocumentElement.WarehouseId} не достаточный";
                    loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError(msg);
                    return res;
                }
                else
                {
                    offerRegister = new()
                    {
                        NomenclatureId = rowOfDocumentElement.NomenclatureId,
                        WarehouseId = rowOfDocumentElement.WarehouseId,
                        Quantity = -rowOfDocumentElement.Quantity,
                        OfferId = rowOfDocumentElement.OfferId,
                    };

                    registersOffersDb.Add(offerRegister);
                    await context.OffersAvailability.AddAsync(offerRegister, token);
                    await context.SaveChangesAsync(token);
                }
            }
            else if (offerRegister.Quantity < rowOfDocumentElement.Quantity && warehouseNegativeBalanceAllowed.Response != true)
            {
                await transaction.RollbackAsync(token);
                msg = $"Остаток оффера #{rowOfDocumentElement.OfferId} на складе #{rowOfDocumentElement.WarehouseId} не достаточный";
                loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError(msg);
                return res;
            }
            else
            {
                await context.OffersAvailability.Where(y => y.Id == offerRegister.Id)
                    .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Quantity, p => p.Quantity - rowOfDocumentElement.Quantity), cancellationToken: token);
            }
        }

        if (offersLocked.Count != 0)
            context.RemoveRange(offersLocked);

        res.Response = await context.RowsWarehouses.Where(x => req.Any(y => y == x.Id)).ExecuteDeleteAsync(cancellationToken: token) != 0;
        await context.SaveChangesAsync(token);
        await transaction.CommitAsync(token);

        res.AddSuccess("Команда удаления выполнена");
        return res;
    }
}