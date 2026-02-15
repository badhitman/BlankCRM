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
    public async Task<TResponseModel<Dictionary<int, DeliveryDocumentMetadataModel>>> RowsDeleteFromWarehouseDocumentAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default)
    {
        string msg;
        TResponseModel<Dictionary<int, DeliveryDocumentMetadataModel>> res = new();

        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }

        req.Payload = [.. req.Payload.Where(x => x > 0)];
        if (!req.Payload.Any(x => x > 0))
        {
            res.AddError($"Пустой запрос > {nameof(RowsDeleteFromWarehouseDocumentAsync)}");
            return res;
        }

        TResponseModel<bool?> warehouseNegativeBalanceAllowed = await StorageTransmissionRepo
              .ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.WarehouseNegativeBalanceAllowed, token);

        req.Payload = [.. req.Payload.Distinct()];
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        List<RowOfWarehouseDocumentModelDB> rowsDB = await context
            .RowsWarehouses
            .Where(x => req.Payload.Any(y => y == x.Id))
            .Include(x => x.Offer)
            .Include(x => x.Nomenclature)
            .ToListAsync(cancellationToken: token);

        if (rowsDB.Count == 0)
        {
            res.AddWarning($"Данные не найдены. Метод удаления [{nameof(RowsDeleteFromWarehouseDocumentAsync)}] не может выполнить команду.");
            return res;
        }
        List<LockTransactionModelDB> offersLocked = [];
        foreach (var x in rowsDB)
        {
            if (!offersLocked.Any(y => y.LockerId == x.OfferId && y.LockerAreaId == x.WarehouseDocument!.WarehouseId))
                offersLocked.Add(new LockTransactionModelDB()
                {
                    LockerName = nameof(OfferAvailabilityModelDB),
                    LockerId = x.OfferId,
                    LockerAreaId = x.WarehouseDocument!.WarehouseId,
                    Marker = nameof(RowsDeleteFromWarehouseDocumentAsync),
                });

            if (!offersLocked.Any(y => y.LockerId == x.OfferId && y.LockerAreaId == x.WarehouseDocument!.WritingOffWarehouseId))
                offersLocked.Add(new LockTransactionModelDB()
                {
                    LockerName = nameof(OfferAvailabilityModelDB),
                    LockerId = x.OfferId,
                    LockerAreaId = x.WarehouseDocument!.WritingOffWarehouseId,
                    Marker = nameof(RowsDeleteFromWarehouseDocumentAsync),
                });
        }

        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken: token);
        try
        {
            await context.LockTransactions.AddRangeAsync(offersLocked, token);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            msg = $"Не удалось выполнить команду блокировки БД {nameof(WarehouseDocumentUpdateOrCreateAsync)}: ";
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError($"{msg}{ex.Message}");
            return res;
        }

        int[] _offersIds = [.. rowsDB.Select(x => x.OfferId).Distinct()];
        List<OfferAvailabilityModelDB> registersOffersDb = await context.OffersAvailability
           .Where(x => _offersIds.Any(y => y == x.OfferId))
           .ToListAsync(cancellationToken: token);

        foreach (int doc_id in rowsDB.Select(x => x.WarehouseDocument!.Id).Distinct())
            await context.WarehouseDocuments.Where(x => x.Id == doc_id).ExecuteUpdateAsync(set => set
            .SetProperty(p => p.Version, Guid.NewGuid())
            .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        foreach (var rowOfDocumentElement in rowsDB.Where(x => !x.WarehouseDocument!.IsDisabled))
        {
            loggerRepo.LogInformation($"{nameof(rowOfDocumentElement)}: {JsonConvert.SerializeObject(rowOfDocumentElement, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            OfferAvailabilityModelDB?
                offerRegister = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocumentElement.OfferId && x.WarehouseId == rowOfDocumentElement.WarehouseDocument!.WarehouseId),
                offerRegisterWritingOff = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocumentElement.OfferId && x.WarehouseId == rowOfDocumentElement.WarehouseDocument!.WritingOffWarehouseId);

            if (offerRegisterWritingOff is not null)
            {
                await context.OffersAvailability.Where(y => y.Id == offerRegisterWritingOff.Id)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Quantity, p => p.Quantity + rowOfDocumentElement.Quantity), cancellationToken: token);
            }
            else if (rowOfDocumentElement.WarehouseDocument!.WritingOffWarehouseId > 0)
            {
                await context.OffersAvailability.AddAsync(new OfferAvailabilityModelDB()
                {
                    WarehouseId = rowOfDocumentElement.WarehouseDocument!.WritingOffWarehouseId,
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
                    msg = $"Остаток оффера #{rowOfDocumentElement.OfferId} на складе #{rowOfDocumentElement.WarehouseDocument!.WarehouseId} не достаточный";
                    loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError(msg);
                    return res;
                }
                else
                {
                    offerRegister = new()
                    {
                        NomenclatureId = rowOfDocumentElement.NomenclatureId,
                        WarehouseId = rowOfDocumentElement.WarehouseDocument!.WarehouseId,
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
                msg = $"Остаток оффера #{rowOfDocumentElement.OfferId} на складе #{rowOfDocumentElement.WarehouseDocument!.WarehouseId} не достаточный";
                loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
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
            context.LockTransactions.RemoveRange(offersLocked);

        res.Response = [];
        foreach (var v in rowsDB.GroupBy(x => x.WarehouseDocumentId))
        {
            Guid docVer = Guid.NewGuid();
            await context.WarehouseDocuments
                .Where(x => v.Key == x.Id)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Version, docVer), cancellationToken: token);

            res.Response.Add(v.Key, new() { Rows = [.. v], VersionDocument = docVer });
        }

        await context.RowsWarehouses
           .Where(x => req.Payload.Any(y => y == x.Id))
           .ExecuteDeleteAsync(cancellationToken: token);

        await transaction.CommitAsync(token);

        res.AddSuccess("Команда удаления выполнена");
        return res;
    }
}