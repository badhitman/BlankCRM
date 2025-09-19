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
        if (req.Quantity == 0)
        {
            res.AddError($"Количество не может быть нулевым");
            return res;
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<WarehouseDocumentModelDB> queryDocumentDb = context.WarehouseDocuments.Where(x => x.Id == req.WarehouseDocumentId);

        WarehouseDocumentRecord whDoc = await queryDocumentDb
            .Select(x => new WarehouseDocumentRecord(x.WarehouseId, x.IsDisabled))
            .FirstAsync(cancellationToken: token);

        // List<Task> tasks = [];
        if (whDoc.WarehouseId == 0)
        {
            msg = "В документе не указан склад";
            loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError(msg);
            return res;
        }

        if (await context.RowsWarehouses.AnyAsync(x => x.Id != req.Id && x.OfferId == req.OfferId && x.WarehouseDocumentId == req.WarehouseDocumentId, cancellationToken: token))
        {
            msg = "В документе уже существует этот офер. Установите ему требуемое количество";
            loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError(msg);
            return res;
        }

        if (!res.Success())
            return res;

        RowOfWarehouseDocumentModelDB? rowDb = req.Id > 0
            ? await context.RowsWarehouses.FirstAsync(x => x.Id == req.Id, cancellationToken: token)
            : null;

        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken: token);
        List<LockTransactionModelDB> lockers = [new()
        {
            LockerName = nameof(OfferAvailabilityModelDB),
            LockerId = req.OfferId,
            RubricId = whDoc.WarehouseId,
        }];

        if (rowDb is not null && rowDb.OfferId != req.OfferId)
        {
            lockers.Add(new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = rowDb.OfferId,
                RubricId = whDoc.WarehouseId,
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

        OfferAvailabilityModelDB? regOfferAv = await context
            .OffersAvailability
            .FirstOrDefaultAsync(x => x.OfferId == req.OfferId && x.WarehouseId == whDoc.WarehouseId, cancellationToken: token);

        if (regOfferAv is null && !whDoc.IsDisabled)
        {
            regOfferAv = new()
            {
                OfferId = req.OfferId,
                NomenclatureId = req.NomenclatureId,
                WarehouseId = whDoc.WarehouseId,
            };

            await context.AddAsync(regOfferAv, token);
        }
        OfferAvailabilityModelDB? regOfferAvStorno = null;
        if (rowDb is not null && rowDb.OfferId != req.OfferId)
        {
            regOfferAvStorno = new()
            {
                OfferId = rowDb.OfferId,
                NomenclatureId = rowDb.NomenclatureId,
                WarehouseId = whDoc.WarehouseId,
            };
        }

        await queryDocumentDb.ExecuteUpdateAsync(set => set
             .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow)
             .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

        if (req.Id < 1)
        {
            if (regOfferAv is not null && !whDoc.IsDisabled)
                regOfferAv.Quantity += req.Quantity;

            req.Version = Guid.NewGuid();
            await context.AddAsync(req, token);
            await context.SaveChangesAsync(token);

            res.AddSuccess("Товар добавлен к документу");
            res.Response = req.Id;
        }
        else
        {
            if (rowDb!.Version != req.Version)
            {
                await transaction.RollbackAsync(token);
                msg = "Строка документа была уже кем-то изменена";
                loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError($"{msg}. Обновите документ (F5), что бы получить актуальные данные");
                return res;
            }

            decimal _delta = req.Quantity - rowDb.Quantity;
            if (_delta == 0)
                res.AddInfo("Количество не изменилось");
            else if (regOfferAv is not null && !whDoc.IsDisabled)
            {
                regOfferAv.Quantity += _delta;
                if (regOfferAv.Id > 0)
                    context.Update(regOfferAv);

                if (regOfferAvStorno is not null)
                {
                    regOfferAvStorno.Quantity -= _delta;
                    context.Update(regOfferAvStorno);
                }
            }

            res.Response = await context.RowsWarehouses
                       .Where(x => x.Id == req.Id)
                       .ExecuteUpdateAsync(set => set
                       .SetProperty(p => p.Quantity, req.Quantity).SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);
        }
        context.RemoveRange(lockers);
        await context.SaveChangesAsync(token);
        await transaction.CommitAsync(token);

        res.AddSuccess($"Обновление `строки складского документа` выполнено");
        return res;
    }
}