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
    public async Task<TResponseModel<int>> WarehouseDocumentUpdateAsync(WarehouseDocumentModelDB req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        ValidateReportModel ck = GlobalTools.ValidateObject(req);
        if (!ck.IsValid)
        {
            res.Messages.InjectException(ck.ValidationResults);
            return res;
        }

        req.DeliveryDate = req.DeliveryDate.SetKindUtc();
        if (req.Name is not null)
            req.Name = req.Name.Trim();
        req.NormalizedUpperName = req.Name?.ToUpper() ?? "";

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        string msg;
        DateTime dtu = DateTime.UtcNow;
        if (req.Id < 1)
        {
            req.Rows?.Clear();
            req.Id = 0;
            req.Version = Guid.NewGuid();
            req.CreatedAtUTC = dtu;
            req.LastUpdatedAtUTC = dtu;
            req.IsDisabled = true;
            await context.AddAsync(req, token);
            await context.SaveChangesAsync(token);
            res.Response = req.Id;
            res.AddSuccess("Документ создан");
            return res;
        }

        WarehouseDocumentModelDB warehouseDocumentDb = await context
            .WarehouseDocuments
            .Include(x => x.Rows)
            .FirstAsync(x => x.Id == req.Id, cancellationToken: token);

        if (warehouseDocumentDb.Version != req.Version)
        {
            msg = $"Документ #{warehouseDocumentDb.Id} был кем-то изменён (version concurrent)";
            loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError($"{msg}. Перед редактированием обновите страницу (F5), что бы загрузить актуальную версию объекта");
            return res;
        }

        List<LockTransactionModelDB> offersLocked = [];
        if (warehouseDocumentDb.Rows!.Count != 0)
            warehouseDocumentDb.Rows.ForEach(x =>
            {
                offersLocked.Add(new LockTransactionModelDB() { LockerName = nameof(OfferAvailabilityModelDB), LockerId = x.OfferId, LockerAreaId = req.WarehouseId });
                if (req.WritingOffWarehouseId > 0 && req.WritingOffWarehouseId != req.WarehouseId)
                    offersLocked.Add(new LockTransactionModelDB() { LockerName = nameof(OfferAvailabilityModelDB), LockerId = x.OfferId, LockerAreaId = req.WritingOffWarehouseId });
            });

        offersLocked = [.. offersLocked.DistinctBy(x => x.LockerAreaId)];

        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken: token);
        if (offersLocked.Count != 0)
        {
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
        }

        int[] _offersIds = [.. warehouseDocumentDb.Rows.Select(x => x.OfferId).Distinct()];
        List<OfferAvailabilityModelDB> registersOffersDb = await context.OffersAvailability
            .Where(x => _offersIds.Any(y => y == x.OfferId))
            .ToListAsync(cancellationToken: token);

        if (warehouseDocumentDb.IsDisabled != req.IsDisabled)
        {
            if (req.IsDisabled)
            {
                warehouseDocumentDb.Rows.ForEach(async rowOfDocument =>
                {
                    OfferAvailabilityModelDB? registerOffer = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == warehouseDocumentDb.WarehouseId);
                    if (registerOffer is not null)
                    {
                        if (registerOffer.Quantity < rowOfDocument.Quantity)
                        {
                            msg = $"Количество сторно [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.Name}'] больше баланса в БД: ";
                            loggerRepo.LogError($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                            res.AddError($"{msg}. Баланс не может быть отрицательным");
                            return;
                        }

                        await context.OffersAvailability.Where(y => y.Id == registerOffer.Id)
                         .ExecuteUpdateAsync(set => set
                         .SetProperty(p => p.Quantity, registerOffer.Quantity - rowOfDocument.Quantity));
                    }
                    else
                    {
                        msg = $"Количество сторно [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.Name}'] не может быть списано (остаток 0): ";
                        loggerRepo.LogError($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                        res.AddError($"{msg}. Баланс не может быть отрицательным");
                        return;
                    }
                });
                if (!res.Success())
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Не удалось выполнить обновить складской документ: ";
                    loggerRepo.LogError($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError(msg);
                    return res;
                }
            }
            else
            {
                List<OfferAvailabilityModelDB> offAvAdding = [];
                warehouseDocumentDb.Rows.ForEach(async rowOfDocument =>
                {
                    OfferAvailabilityModelDB? registerOffer = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == req.WarehouseId);

                    if (registerOffer is not null)
                        await context.OffersAvailability
                             .Where(y => y.Id == registerOffer.Id)
                             .ExecuteUpdateAsync(set => set.SetProperty(p => p.Quantity, registerOffer.Quantity + rowOfDocument.Quantity));
                    else
                        offAvAdding.Add(new OfferAvailabilityModelDB()
                        {
                            WarehouseId = warehouseDocumentDb.WarehouseId,
                            NomenclatureId = rowOfDocument.NomenclatureId,
                            OfferId = rowOfDocument.OfferId,
                            Quantity = rowOfDocument.Quantity,
                        });

                    if (warehouseDocumentDb.WarehouseId != req.WarehouseId)
                    {
                        registerOffer = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == warehouseDocumentDb.WarehouseId);

                        if (registerOffer is not null)
                            await context.OffersAvailability.Where(y => y.Id == registerOffer.Id).ExecuteUpdateAsync(set => set.SetProperty(p => p.Quantity, registerOffer.Quantity - rowOfDocument.Quantity));
                        else
                            offAvAdding.Add(new()
                            {
                                WarehouseId = warehouseDocumentDb.WarehouseId,
                                NomenclatureId = rowOfDocument.NomenclatureId,
                                OfferId = rowOfDocument.OfferId,
                                Quantity = -rowOfDocument.Quantity,
                            });
                    }
                });
                if (offAvAdding.Count != 0)
                    await context.AddRangeAsync(offAvAdding, token);
            }
        }

        res.Response = await context.WarehouseDocuments
                .Where(x => x.Id == req.Id)
                .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name)
                .SetProperty(p => p.Description, req.Description)
                .SetProperty(p => p.DeliveryDate, req.DeliveryDate)
                .SetProperty(p => p.IsDisabled, req.IsDisabled)
                .SetProperty(p => p.WarehouseId, req.WarehouseId)
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.LastUpdatedAtUTC, dtu), cancellationToken: token);

        if (offersLocked.Count != 0)
            context.RemoveRange(offersLocked);

        res.AddSuccess("Складской документ обновлён");
        await context.SaveChangesAsync(token);
        await transaction.CommitAsync(token);
        return res;
    }
}