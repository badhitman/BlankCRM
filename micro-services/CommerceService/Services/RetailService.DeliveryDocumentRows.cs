////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;
using Newtonsoft.Json;

namespace CommerceService;

/// <summary>
/// Розница
/// </summary>
public partial class RetailService : IRetailService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        DeliveryDocumentRetailModelDB docDb = await context.DeliveryDocumentsRetail.FirstAsync(x => x.Id == req.DocumentId, cancellationToken: token);

        TResponseModel<int> res = new();
        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        string msg;

        if (!offDeliveriesStatuses.Contains(docDb.DeliveryStatus))
        {
            LockTransactionModelDB locker = new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = req.OfferId,
                LockerAreaId = docDb.WarehouseId,
                Marker = nameof(CreateRowOfDeliveryDocumentAsync),
            };
            try
            {
                await context.LockTransactions.AddAsync(locker, token);
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
               .Where(x => x.OfferId == req.OfferId)
               .ToListAsync(cancellationToken: token);

            OfferAvailabilityModelDB? regOfferAv = offerAvailabilityDB
                .Where(x => x.OfferId == req.OfferId && x.WarehouseId == docDb.WarehouseId)
                .FirstOrDefault();

            if (regOfferAv is null)
            {
                msg = $"Количество [offer: #{req.OfferId} '{req.Offer?.GetName()}'] не может быть списано (остаток отсутствует)";
                loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError($"{msg}. Баланс не может быть отрицательным");
                return res;
            }
            else if (regOfferAv.Quantity < req.Quantity)
            {
                msg = $"Количество [offer: #{req.OfferId} '{req.Offer?.GetName()}'] не может быть списано (остаток {regOfferAv.Quantity})";
                loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError($"{msg}. Баланс не может быть отрицательным");
                return res;
            }

            await context.OffersAvailability
                   .Where(x => x.Id == regOfferAv.Id)
                       .ExecuteUpdateAsync(set => set
                           .SetProperty(p => p.Quantity, p => p.Quantity - req.Quantity), cancellationToken: token);

            await context.LockTransactions
               .Where(x => x.Id == locker.Id)
               .ExecuteDeleteAsync(cancellationToken: token);
        }

        req.Offer = null;
        req.Nomenclature = null;
        req.Offer = null;
        req.Document = null;
        req.Comment = req.Comment?.Trim();
        req.Version = Guid.NewGuid();

        await context.RowsDeliveryDocumentsRetail.AddAsync(req, token);
        await context.SaveChangesAsync(token);

        await context.DeliveryDocumentsRetail
            .Where(x => x.Id == docDb.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

        await transaction.CommitAsync(token);
        return new() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default)
    {
        if (req.Quantity <= 0)
            return ResponseBaseModel.CreateError("Количество должно быть больше нуля");

        req.Offer = null;
        req.Nomenclature = null;
        req.Offer = null;
        req.Document = null;
        req.Comment = req.Comment?.Trim();
        loggerRepo.LogInformation($"{nameof(req)}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        DeliveryDocumentRetailModelDB deliveryDocumentDb = await context.DeliveryDocumentsRetail.FirstAsync(x => x.Id == req.DocumentId, cancellationToken: token);
        loggerRepo.LogInformation($"{nameof(deliveryDocumentDb)}: {JsonConvert.SerializeObject(deliveryDocumentDb, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
        RowOfDeliveryRetailDocumentModelDB rowOfDeliveryRetailDocument = await context.RowsDeliveryDocumentsRetail
            .Include(x => x.Offer!)
            .ThenInclude(x => x.Nomenclature)
            .FirstAsync(x => x.Id == req.Id, cancellationToken: token);
        loggerRepo.LogInformation($"{nameof(rowOfDeliveryRetailDocument)}: {JsonConvert.SerializeObject(rowOfDeliveryRetailDocument, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
        if (rowOfDeliveryRetailDocument.Version != req.Version)
            return ResponseBaseModel.CreateError($"Строку документа уже кто-то изменил. Обновите документ и попробуйте изменить его снова");

        if (offDeliveriesStatuses.Contains(deliveryDocumentDb.DeliveryStatus))
        {
            await context.RowsDeliveryDocumentsRetail
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.OfferId, req.OfferId)
                .SetProperty(p => p.Quantity, req.Quantity)
                .SetProperty(p => p.Amount, req.Amount)
                .SetProperty(p => p.Comment, req.Comment)
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.NomenclatureId, req.NomenclatureId), cancellationToken: token);

            await context.DeliveryDocumentsRetail
             .Where(x => x.Id == req.DocumentId)
             .ExecuteUpdateAsync(set => set
                 .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

            return ResponseBaseModel.CreateSuccess("Ok");
        }

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        List<LockTransactionModelDB> lockers = [new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = req.OfferId,
                LockerAreaId = deliveryDocumentDb.WarehouseId,
                Marker = nameof(UpdateRowOfDeliveryDocumentAsync),
            }];
        if (rowOfDeliveryRetailDocument.OfferId != req.OfferId)
            lockers.Add(new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = rowOfDeliveryRetailDocument.OfferId,
                LockerAreaId = deliveryDocumentDb.WarehouseId,
                Marker = nameof(UpdateRowOfDeliveryDocumentAsync),
            });

        string msg;
        try
        {
            await context.AddRangeAsync(lockers, token);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            msg = $"Не удалось выполнить команду: ";
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            return ResponseBaseModel.CreateError($"{msg}{ex.Message}"); ;
        }

        List<OfferAvailabilityModelDB> offerAvailabilityDB = await context
            .OffersAvailability
            .Where(x => x.OfferId == req.OfferId || x.OfferId == rowOfDeliveryRetailDocument.OfferId)
            .ToListAsync(cancellationToken: token);

        OfferAvailabilityModelDB? regOfferAv;
        if (rowOfDeliveryRetailDocument.OfferId != req.OfferId)
        {
            regOfferAv = offerAvailabilityDB
                .FirstOrDefault(x => x.OfferId == rowOfDeliveryRetailDocument.OfferId && x.WarehouseId == deliveryDocumentDb.WarehouseId);

            if (regOfferAv is null)
            {
                await context.OffersAvailability.AddAsync(new()
                {
                    OfferId = rowOfDeliveryRetailDocument.OfferId,
                    NomenclatureId = rowOfDeliveryRetailDocument.NomenclatureId,
                    WarehouseId = deliveryDocumentDb.WarehouseId,
                    Quantity = rowOfDeliveryRetailDocument.Quantity,
                }, token);
            }
            else
            {
                await context.OffersAvailability
                    .Where(x => x.Id == regOfferAv.Id)
                    .ExecuteUpdateAsync(set => set
                       .SetProperty(p => p.Quantity, p => p.Quantity + rowOfDeliveryRetailDocument.Quantity), cancellationToken: token);
            }
        }

        regOfferAv = offerAvailabilityDB
            .Where(x => x.OfferId == req.OfferId && x.WarehouseId == deliveryDocumentDb.WarehouseId)
            .FirstOrDefault();

        decimal _quantity = rowOfDeliveryRetailDocument.OfferId != req.OfferId
            ? req.Quantity
            : req.Quantity - rowOfDeliveryRetailDocument.Quantity;

        if (_quantity < 0)
        {
            if (regOfferAv is not null)
            {
                await context.OffersAvailability
                    .Where(x => x.Id == regOfferAv.Id)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Quantity, p => p.Quantity - _quantity), cancellationToken: token);
            }
            else
            {
                regOfferAv = new()
                {
                    NomenclatureId = rowOfDeliveryRetailDocument.NomenclatureId,
                    WarehouseId = deliveryDocumentDb.WarehouseId,
                    OfferId = rowOfDeliveryRetailDocument.OfferId,
                    Quantity = -_quantity,
                };
                await context.OffersAvailability.AddAsync(regOfferAv, token);
            }
        }
        else
        {
            if (regOfferAv is null)
            {
                await transaction.RollbackAsync(token);
                msg = $"На складе #{deliveryDocumentDb.WarehouseId} отсутствует офер #{req.OfferId}";
                loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

                return ResponseBaseModel.CreateError(msg);
            }
            else if (regOfferAv.Quantity < _quantity)
            {
                await transaction.RollbackAsync(token);
                msg = $"На складе #{deliveryDocumentDb.WarehouseId} отсутствует офер #{regOfferAv.OfferId}";
                loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

                return ResponseBaseModel.CreateError(msg);
            }

            await context.OffersAvailability
                .Where(x => x.Id == regOfferAv.Id)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Quantity, p => p.Quantity - _quantity), cancellationToken: token);
        }

        if (lockers.Count != 0)
            context.RemoveRange(lockers);

        await context.SaveChangesAsync(token);
        await context.DeliveryDocumentsRetail
            .Where(x => x.Id == req.DocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

        await context.RowsDeliveryDocumentsRetail
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.OfferId, req.OfferId)
                .SetProperty(p => p.Quantity, req.Quantity)
                .SetProperty(p => p.Amount, req.Amount)
                .SetProperty(p => p.Comment, req.Comment)
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.NomenclatureId, req.NomenclatureId), cancellationToken: token);

        await transaction.CommitAsync(token);
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteRowOfDeliveryDocumentAsync(int rowId, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        RowOfDeliveryRetailDocumentModelDB rowDb = await context.RowsDeliveryDocumentsRetail
           .Include(x => x.Document)
           .FirstAsync(x => x.Id == rowId, cancellationToken: token);

        DeliveryDocumentRetailModelDB deliveryDocumentRetailDb = rowDb.Document!;
        //loggerRepo.LogInformation($"{nameof(deliveryDocumentRetailDb)}: {JsonConvert.SerializeObject(deliveryDocumentRetailDb, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);
        if (offDeliveriesStatuses.Contains(deliveryDocumentRetailDb.DeliveryStatus))
        {
            await context.RowsDeliveryDocumentsRetail
                .Where(x => x.Id == rowId)
                .ExecuteDeleteAsync(cancellationToken: token);

            await context.DeliveryDocumentsRetail
             .Where(x => x.Id == rowDb.DocumentId)
             .ExecuteUpdateAsync(set => set
                 .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

            await transaction.CommitAsync(token);
            return ResponseBaseModel.CreateSuccess("Элемент удалён");
        }

        List<LockTransactionModelDB> lockers = [new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = rowDb.OfferId,
                LockerAreaId = deliveryDocumentRetailDb.WarehouseId,
                Marker = nameof(DeleteRowOfDeliveryDocumentAsync),
            }];

        string msg;
        try
        {
            await context.AddRangeAsync(lockers, token);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            msg = $"Не удалось выполнить команду: ";
            loggerRepo.LogError(ex, $"{msg} #{rowId}");
            return ResponseBaseModel.CreateError($"{msg}{ex.Message}"); ;
        }

        OfferAvailabilityModelDB? regOfferAv = await context
            .OffersAvailability
            .Where(x => x.OfferId == rowDb.OfferId)
            .FirstOrDefaultAsync(x => x.OfferId == rowDb.OfferId && x.WarehouseId == deliveryDocumentRetailDb.WarehouseId, cancellationToken: token);

        if (regOfferAv is null)
        {
            regOfferAv = new()
            {
                WarehouseId = deliveryDocumentRetailDb.WarehouseId,
                Quantity = rowDb.Quantity,
                OfferId = rowDb.OfferId,
                NomenclatureId = rowDb.NomenclatureId,
            };
            await context.OffersAvailability.AddAsync(regOfferAv, token);
        }
        else
        {
            await context.OffersAvailability
                .Where(x => x.Id == regOfferAv.Id)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Quantity, p => p.Quantity + rowDb.Quantity), cancellationToken: token);
        }

        if (lockers.Count != 0)
            context.RemoveRange(lockers);

        await context.SaveChangesAsync(token);

        await context.RowsDeliveryDocumentsRetail
            .Where(x => x.Id == rowId)
            .ExecuteDeleteAsync(cancellationToken: token);

        await context.DeliveryDocumentsRetail
         .Where(x => x.Id == rowDb.DocumentId)
         .ExecuteUpdateAsync(set => set
             .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

        await transaction.CommitAsync(token);
        return ResponseBaseModel.CreateSuccess("Элемент удалён");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<RowOfDeliveryRetailDocumentModelDB>> SelectRowsOfDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectRowsOfDeliveriesRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new() { Status = new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Ошибка запроса: Payload is null" }] } };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<RowOfDeliveryRetailDocumentModelDB> q = context.RowsDeliveryDocumentsRetail.Where(x => x.DocumentId == req.Payload.DeliveryDocumentId).AsQueryable();

        IQueryable<RowOfDeliveryRetailDocumentModelDB> pq = q
            .OrderBy(x => x.Id)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        List<RowOfDeliveryRetailDocumentModelDB> res = await pq
            .Include(x => x.Document)
            .Include(x => x.Offer!)
            .ThenInclude(x => x.Nomenclature)
            .ToListAsync(cancellationToken: token);

        foreach (RowOfDeliveryRetailDocumentModelDB row in res.Where(x => x.Amount <= 0 || x.WeightOffer <= 0))
        {
            if (row.Amount <= 0)
                row.Amount = row.Quantity * row.Offer!.Price;

            if (row.WeightOffer <= 0)
                row.WeightOffer = row.Quantity * row.Offer!.Weight;

            context.Update(row);
            await context.SaveChangesAsync(token);
        }

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = res
        };
    }
}