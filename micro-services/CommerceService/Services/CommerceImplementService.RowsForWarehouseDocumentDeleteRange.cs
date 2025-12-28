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

        TResponseModel<bool?> res_WarehouseNegativeBalanceAllowed = await StorageTransmissionRepo
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

        var _allOffersOfDocument = await q
           .ToArrayAsync(cancellationToken: token);

        if (_allOffersOfDocument.Length == 0)
        {
            res.AddWarning($"Данные не найдены. Метод удаления [{nameof(RowsForWarehouseDocumentDeleteAsync)}] не может выполнить команду.");
            return res;
        }
        List<LockTransactionModelDB> offersLocked = [];
        foreach (var x in _allOffersOfDocument)
        {
            if (!offersLocked.Any(y => y.LockerId == x.OfferId && y.LockerAreaId == x.WarehouseId))
                offersLocked.Add(new LockTransactionModelDB()
                {
                    LockerName = nameof(OfferAvailabilityModelDB),
                    LockerId = x.OfferId,
                    LockerAreaId = x.WarehouseId,
                });

            if (!offersLocked.Any(y => y.LockerId == x.OfferId && y.LockerAreaId == x.WritingOffWarehouseId))
                offersLocked.Add(new LockTransactionModelDB()
                {
                    LockerName = nameof(OfferAvailabilityModelDB),
                    LockerId = x.OfferId,
                    LockerAreaId = x.WritingOffWarehouseId,
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

        int[] _offersIds = [.. _allOffersOfDocument.Select(x => x.OfferId).Distinct()];
        List<OfferAvailabilityModelDB> registersOffersDb = await context.OffersAvailability
           .Where(x => _offersIds.Any(y => y == x.OfferId))
           .ToListAsync(cancellationToken: token);

        foreach (int doc_id in _allOffersOfDocument.Select(x => x.DocumentId).Distinct())
            await context.WarehouseDocuments.Where(x => x.Id == doc_id).ExecuteUpdateAsync(set => set
            .SetProperty(p => p.Version, Guid.NewGuid())
            .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        foreach (var rowEl in _allOffersOfDocument.Where(x => !x.IsDisabled))
        {
            OfferAvailabilityModelDB?
                offerRegister = registersOffersDb.FirstOrDefault(x => x.OfferId == rowEl.OfferId && x.WarehouseId == rowEl.WarehouseId),
                offerRegisterWritingOff = registersOffersDb.FirstOrDefault(x => x.OfferId == rowEl.OfferId && x.WarehouseId == rowEl.WritingOffWarehouseId);

            if (offerRegisterWritingOff is null)
            {
                await context.OffersAvailability.AddAsync(new OfferAvailabilityModelDB()
                {
                    WarehouseId = rowEl.WritingOffWarehouseId,
                    Quantity = rowEl.Quantity,
                    NomenclatureId = rowEl.NomenclatureId,
                    OfferId = rowEl.OfferId,
                }, token);
            }
            else
            {
                await context.OffersAvailability.Where(y => y.Id == offerRegisterWritingOff.Id)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Quantity, p => p.Quantity + rowEl.Quantity), cancellationToken: token);
            }

            if (offerRegister is null || offerRegister.Quantity < rowEl.Quantity)
            {
                await transaction.RollbackAsync(token);
                msg = $"Остаток офера #{rowEl.OfferId} на складе #{rowEl.WarehouseId} не достаточный";
                loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError(msg);
                return res;
            }
            else
            {
                await context.OffersAvailability.Where(y => y.Id == offerRegister.Id)
                    .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Quantity, p => p.Quantity - rowEl.Quantity), cancellationToken: token);
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