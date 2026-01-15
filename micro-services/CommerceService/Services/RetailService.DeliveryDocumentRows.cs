////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLib;
using DbcLib;

namespace CommerceService;

/// <summary>
/// Розница
/// </summary>
public partial class RetailService : IRetailService
{
    /// <inheritdoc/>
    public async Task<DocumentNewVersionResponseModel> CreateRowOfDeliveryDocumentAsync(TAuthRequestStandardModel<RowOfDeliveryRetailDocumentModelDB> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "req.Payload is null",
                }]
            };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        DeliveryDocumentRetailModelDB docDb = await context.DeliveryDocumentsRetail
            .FirstAsync(x => x.Id == req.Payload.DocumentId, cancellationToken: token);

        DocumentNewVersionResponseModel res = new();
        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        string msg;

        if (!offDeliveriesStatuses.Contains(docDb.DeliveryStatus))
        {
            LockTransactionModelDB locker = new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = req.Payload.OfferId,
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
                loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                return res;
            }

            List<OfferAvailabilityModelDB> offerAvailabilityDB = await context
               .OffersAvailability
               .Where(x => x.OfferId == req.Payload.OfferId)
               .ToListAsync(cancellationToken: token);

            OfferAvailabilityModelDB? regOfferAv = offerAvailabilityDB
                .Where(x => x.OfferId == req.Payload.OfferId && x.WarehouseId == docDb.WarehouseId)
                .FirstOrDefault();

            if (regOfferAv is null)
            {
                msg = $"Количество [offer: #{req.Payload.OfferId} '{req.Payload.Offer?.GetName()}'] не может быть списано (остаток отсутствует)";
                loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError($"{msg}. Баланс не может быть отрицательным");
                return res;
            }
            else if (regOfferAv.Quantity < req.Payload.Quantity)
            {
                msg = $"Количество [offer: #{req.Payload.OfferId} '{req.Payload.Offer?.GetName()}'] не может быть списано (остаток {regOfferAv.Quantity})";
                loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError($"{msg}. Баланс не может быть отрицательным");
                return res;
            }

            await context.OffersAvailability
                   .Where(x => x.Id == regOfferAv.Id)
                       .ExecuteUpdateAsync(set => set
                           .SetProperty(p => p.Quantity, p => p.Quantity - req.Payload.Quantity), cancellationToken: token);

            await context.LockTransactions
               .Where(x => x.Id == locker.Id)
               .ExecuteDeleteAsync(cancellationToken: token);
        }

        req.Payload.Offer = null;
        req.Payload.Nomenclature = null;
        req.Payload.Offer = null;
        req.Payload.Document = null;
        req.Payload.Comment = req.Payload.Comment?.Trim();
        req.Payload.Version = Guid.NewGuid();

        await context.RowsDeliveryDocumentsRetail.AddAsync(req.Payload, token);
        await context.SaveChangesAsync(token);

        res.DocumentNewVersion = Guid.NewGuid();
        res.Response = req.Payload.Id;

        await context.DeliveryDocumentsRetail
            .Where(x => x.Id == docDb.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Version, res.DocumentNewVersion), cancellationToken: token);

        await transaction.CommitAsync(token);
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<Guid?>> UpdateRowOfDeliveryDocumentAsync(TAuthRequestStandardModel<RowOfDeliveryRetailDocumentModelDB> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "req.Payload is null" }] };

        if (req.Payload.Quantity <= 0)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Количество должно быть больше нуля" }] };

        req.Payload.Offer = null;
        req.Payload.Nomenclature = null;
        req.Payload.Offer = null;
        req.Payload.Document = null;
        req.Payload.Comment = req.Payload.Comment?.Trim();

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        TResponseModel<Guid?> res = new() { Response = Guid.NewGuid() };

        RowOfDeliveryRetailDocumentModelDB rowOfDeliveryRetailDocument = await context.RowsDeliveryDocumentsRetail
            .Include(x => x.Document)
            .Include(x => x.Offer!)
            .ThenInclude(x => x.Nomenclature)
            .FirstAsync(x => x.Id == req.Payload.Id, cancellationToken: token);

        DeliveryDocumentRetailModelDB docDb = rowOfDeliveryRetailDocument.Document!;

        if (rowOfDeliveryRetailDocument.Version != req.Payload.Version)
        {
            res.AddError($"Строку документа уже кто-то изменил. Обновите документ и попробуйте изменить его снова");
            return res;
        }

        if (offDeliveriesStatuses.Contains(docDb.DeliveryStatus))
        {
            await context.RowsDeliveryDocumentsRetail
            .Where(x => x.Id == req.Payload.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.OfferId, req.Payload.OfferId)
                .SetProperty(p => p.Quantity, req.Payload.Quantity)
                .SetProperty(p => p.Amount, req.Payload.Amount)
                .SetProperty(p => p.Comment, req.Payload.Comment)
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.NomenclatureId, req.Payload.NomenclatureId), cancellationToken: token);

            await context.DeliveryDocumentsRetail
             .Where(x => x.Id == req.Payload.DocumentId)
             .ExecuteUpdateAsync(set => set
                 .SetProperty(p => p.Version, res.Response), cancellationToken: token);

            res.AddSuccess("Ok");
            return res;
        }

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        List<LockTransactionModelDB> lockers = [new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = req.Payload.OfferId,
                LockerAreaId = docDb.WarehouseId,
                Marker = nameof(UpdateRowOfDeliveryDocumentAsync),
            }];
        if (rowOfDeliveryRetailDocument.OfferId != req.Payload.OfferId)
            lockers.Add(new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = rowOfDeliveryRetailDocument.OfferId,
                LockerAreaId = docDb.WarehouseId,
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
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError($"{msg}{ex.Message}");
            return res;
        }

        List<OfferAvailabilityModelDB> offerAvailabilityDB = await context
            .OffersAvailability
            .Where(x => x.OfferId == req.Payload.OfferId || x.OfferId == rowOfDeliveryRetailDocument.OfferId)
            .ToListAsync(cancellationToken: token);

        OfferAvailabilityModelDB? regOfferAv;
        if (rowOfDeliveryRetailDocument.OfferId != req.Payload.OfferId)
        {
            regOfferAv = offerAvailabilityDB
                .FirstOrDefault(x => x.OfferId == rowOfDeliveryRetailDocument.OfferId && x.WarehouseId == docDb.WarehouseId);

            if (regOfferAv is null)
            {
                await context.OffersAvailability.AddAsync(new()
                {
                    OfferId = rowOfDeliveryRetailDocument.OfferId,
                    NomenclatureId = rowOfDeliveryRetailDocument.NomenclatureId,
                    WarehouseId = docDb.WarehouseId,
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
            .Where(x => x.OfferId == req.Payload.OfferId && x.WarehouseId == docDb.WarehouseId)
            .FirstOrDefault();

        decimal _quantity = rowOfDeliveryRetailDocument.OfferId != req.Payload.OfferId
            ? req.Payload.Quantity
            : req.Payload.Quantity - rowOfDeliveryRetailDocument.Quantity;

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
                    WarehouseId = docDb.WarehouseId,
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
                msg = $"На складе #{docDb.WarehouseId} отсутствует офер #{req.Payload.OfferId}";
                loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError(msg);
                return res;
            }
            else if (regOfferAv.Quantity < _quantity)
            {
                await transaction.RollbackAsync(token);
                msg = $"На складе #{docDb.WarehouseId} отсутствует офер #{regOfferAv.OfferId}";
                loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError(msg);
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
            .Where(x => x.Id == req.Payload.DocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

        res.Response = Guid.NewGuid();
        await context.RowsDeliveryDocumentsRetail
            .Where(x => x.Id == req.Payload.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.OfferId, req.Payload.OfferId)
                .SetProperty(p => p.Quantity, req.Payload.Quantity)
                .SetProperty(p => p.Amount, req.Payload.Amount)
                .SetProperty(p => p.Comment, req.Payload.Comment)
                .SetProperty(p => p.Version, res.Response)
                .SetProperty(p => p.NomenclatureId, req.Payload.NomenclatureId), cancellationToken: token);

        await transaction.CommitAsync(token);
        res.AddSuccess("Ok");
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<RowOfDeliveryRetailDocumentModelDB>> DeleteRowOfDeliveryDocumentAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
    {
        int rowId = req.Payload;
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        TResponseModel<RowOfDeliveryRetailDocumentModelDB> res = new()
        {
            Response = await context.RowsDeliveryDocumentsRetail
           .Include(x => x.Document)
           .FirstAsync(x => x.Id == rowId, cancellationToken: token)
        };
        DeliveryDocumentRetailModelDB deliveryDocumentRetailDb = res.Response.Document!;

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);
        if (offDeliveriesStatuses.Contains(deliveryDocumentRetailDb.DeliveryStatus))
        {
            await context.RowsDeliveryDocumentsRetail
                .Where(x => x.Id == rowId)
                .ExecuteDeleteAsync(cancellationToken: token);
            res.Response.Document!.Version = Guid.NewGuid();
            await context.DeliveryDocumentsRetail
             .Where(x => x.Id == res.Response.DocumentId)
             .ExecuteUpdateAsync(set => set
                 .SetProperty(p => p.Version, res.Response.Document.Version), cancellationToken: token);

            await transaction.CommitAsync(token);
            res.AddSuccess("Элемент удалён");
            return res;
        }

        List<LockTransactionModelDB> lockers = [new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = res.Response.OfferId,
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
            msg = $"Не удалось выполнить {nameof(DeleteRowOfDeliveryDocumentAsync)}: ";
            loggerRepo.LogError(ex, $"{msg} #{rowId}");
            res.Messages.InjectException(ex);
            res.AddError(msg);
            return res;
        }

        OfferAvailabilityModelDB? regOfferAv = await context
            .OffersAvailability
            .Where(x => x.OfferId == res.Response.OfferId)
            .FirstOrDefaultAsync(x => x.OfferId == res.Response.OfferId && x.WarehouseId == deliveryDocumentRetailDb.WarehouseId, cancellationToken: token);

        if (regOfferAv is null)
        {
            regOfferAv = new()
            {
                WarehouseId = deliveryDocumentRetailDb.WarehouseId,
                Quantity = res.Response.Quantity,
                OfferId = res.Response.OfferId,
                NomenclatureId = res.Response.NomenclatureId,
            };
            await context.OffersAvailability.AddAsync(regOfferAv, token);
        }
        else
        {
            await context.OffersAvailability
                .Where(x => x.Id == regOfferAv.Id)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Quantity, p => p.Quantity + res.Response.Quantity), cancellationToken: token);
        }

        await context.RowsDeliveryDocumentsRetail
            .Where(x => x.Id == rowId)
            .ExecuteDeleteAsync(cancellationToken: token);

        res.Response.Document!.Version = Guid.NewGuid();
        await context.DeliveryDocumentsRetail
         .Where(x => x.Id == res.Response.DocumentId)
         .ExecuteUpdateAsync(set => set
             .SetProperty(p => p.Version, res.Response.Document.Version), cancellationToken: token);

        if (lockers.Count != 0)
            context.RemoveRange(lockers);

        await context.SaveChangesAsync(token);

        await transaction.CommitAsync(token);
        res.AddSuccess("Элемент удалён");
        return res;
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