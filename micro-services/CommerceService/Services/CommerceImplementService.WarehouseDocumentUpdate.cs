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

        string msg;
        if (req.WarehouseId == req.WritingOffWarehouseId)
        {
            msg = $"Склад списания и склад поступления не может быть одни и тем же";
            loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError($"{msg}. Измените или удалите склад списания");
            return res;
        }

        req.DeliveryDate = req.DeliveryDate.SetKindUtc();
        if (req.Name is not null)
            req.Name = req.Name.Trim();
        req.NormalizedUpperName = req.Name?.ToUpper() ?? "";

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
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
            loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError($"{msg}. Перед редактированием обновите страницу (F5), что бы загрузить актуальную версию документа");
            return res;
        }

        List<LockTransactionModelDB> offersLocked = [];
        if (warehouseDocumentDb.Rows!.Count != 0)
            foreach (RowOfWarehouseDocumentModelDB rowDoc in warehouseDocumentDb.Rows)
            {
                offersLocked.Add(new LockTransactionModelDB()
                {
                    LockerName = nameof(OfferAvailabilityModelDB),
                    LockerAreaId = req.WarehouseId,
                    LockerId = rowDoc.OfferId,
                });
                if (req.WritingOffWarehouseId > 0)
                    offersLocked.Add(new LockTransactionModelDB()
                    {
                        LockerName = nameof(OfferAvailabilityModelDB),
                        LockerAreaId = req.WritingOffWarehouseId,
                        LockerId = rowDoc.OfferId,
                    });

                if (warehouseDocumentDb.WritingOffWarehouseId != req.WritingOffWarehouseId && warehouseDocumentDb.WritingOffWarehouseId > 0)
                    offersLocked.Add(new LockTransactionModelDB()
                    {
                        LockerName = nameof(OfferAvailabilityModelDB),
                        LockerAreaId = warehouseDocumentDb.WritingOffWarehouseId,
                        LockerId = rowDoc.OfferId,
                    });

                if (warehouseDocumentDb.WarehouseId != req.WarehouseId)
                    offersLocked.Add(new LockTransactionModelDB()
                    {
                        LockerName = nameof(OfferAvailabilityModelDB),
                        LockerAreaId = warehouseDocumentDb.WarehouseId,
                        LockerId = rowDoc.OfferId,
                    });
            }

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
                msg = $"Не удалось выполнить команду блокировки регистров остатков {nameof(WarehouseDocumentUpdateAsync)}: ";
                loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError($"{msg}{ex.Message}");
                return res;
            }
        }

        int[] _offersIds = [.. warehouseDocumentDb.Rows.Select(x => x.OfferId).Distinct()];
        List<OfferAvailabilityModelDB> registersOffersDb = await context.OffersAvailability
            .Where(x => _offersIds.Any(y => y == x.OfferId))
            .Include(x => x.Offer)
            .Include(x => x.Nomenclature)
            .ToListAsync(cancellationToken: token);

        if (warehouseDocumentDb.IsDisabled != req.IsDisabled)
        {
            if (req.IsDisabled)
            {
                foreach (RowOfWarehouseDocumentModelDB rowOfDocument in warehouseDocumentDb.Rows)
                {
                    OfferAvailabilityModelDB?
                                            registerOffer = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == warehouseDocumentDb.WarehouseId),
                                            registerOfferWriteOff = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == warehouseDocumentDb.WritingOffWarehouseId);

                    if (registerOfferWriteOff is not null)
                    {
                        await context.OffersAvailability.Where(y => y.Id == registerOfferWriteOff.Id)
                         .ExecuteUpdateAsync(set => set
                            .SetProperty(p => p.Quantity, p => p.Quantity + rowOfDocument.Quantity), cancellationToken: token);

                        registerOfferWriteOff.Quantity += rowOfDocument.Quantity;
                    }
                    else if (warehouseDocumentDb.WritingOffWarehouseId > 0)
                    {
                        registerOfferWriteOff = new OfferAvailabilityModelDB()
                        {
                            WarehouseId = warehouseDocumentDb.WritingOffWarehouseId,
                            Quantity = rowOfDocument.Quantity,
                            NomenclatureId = rowOfDocument.NomenclatureId,
                            OfferId = rowOfDocument.OfferId,
                        };
                        await context.OffersAvailability.AddAsync(registerOfferWriteOff, token);
                    }

                    if (registerOffer is not null)
                    {
                        if (registerOffer.Quantity < rowOfDocument.Quantity)
                        {
                            msg = $"Количество сторно [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.Name}'] больше баланса в БД";
                            loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                            res.AddError($"{msg}. Баланс не может быть отрицательным");
                            break;
                        }

                        await context.OffersAvailability.Where(y => y.Id == registerOffer.Id)
                         .ExecuteUpdateAsync(set => set
                            .SetProperty(p => p.Quantity, p => p.Quantity - rowOfDocument.Quantity), cancellationToken: token);

                        registerOffer.Quantity -= rowOfDocument.Quantity;
                    }
                    else
                    {
                        msg = $"Количество сторно [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.GetName()}'] не может быть списано (остаток отсутствует)";
                        loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                        res.AddError($"{msg}. Баланс не может быть отрицательным");
                        break;
                    }
                }

                if (!res.Success())
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Не удалось обновить складской документ: ";
                    loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError(msg);
                    return res;
                }
            }
            else
            {
                foreach (RowOfWarehouseDocumentModelDB rowOfDocument in warehouseDocumentDb.Rows)
                {
                    OfferAvailabilityModelDB?
                        registerOffer = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == req.WarehouseId),
                        registerOfferWriteOff = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == req.WritingOffWarehouseId);

                    if (registerOfferWriteOff is not null)
                    {
                        if (registerOfferWriteOff.Quantity < rowOfDocument.Quantity)
                        {
                            msg = $"Количество [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.GetName()}'] не может быть списано (остаток {registerOfferWriteOff.Quantity})";
                            loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                            res.AddError($"{msg}. Баланс не может быть отрицательным");
                            break;
                        }

                        await context.OffersAvailability
                                                     .Where(y => y.Id == registerOfferWriteOff.Id)
                                                     .ExecuteUpdateAsync(set =>
                                                        set.SetProperty(p => p.Quantity, p => p.Quantity - rowOfDocument.Quantity), cancellationToken: token);

                        registerOfferWriteOff.Quantity -= rowOfDocument.Quantity;
                    }
                    else if (req.WritingOffWarehouseId > 0)
                    {
                        msg = $"Количество [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.GetName()}'] не может быть списано (остаток отсутствует)";
                        loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                        res.AddError($"{msg}. Баланс не может быть отрицательным");
                        break;
                    }

                    if (registerOffer is not null)
                    {
                        await context.OffersAvailability
                             .Where(y => y.Id == registerOffer.Id)
                             .ExecuteUpdateAsync(set => set
                                .SetProperty(p => p.Quantity, p => p.Quantity + rowOfDocument.Quantity), cancellationToken: token);

                        registerOffer.Quantity += rowOfDocument.Quantity;
                    }
                    else
                    {
                        registerOffer = new OfferAvailabilityModelDB()
                        {
                            WarehouseId = req.WarehouseId,
                            NomenclatureId = rowOfDocument.NomenclatureId,
                            OfferId = rowOfDocument.OfferId,
                            Quantity = rowOfDocument.Quantity,
                        };
                        await context.OffersAvailability.AddAsync(registerOffer, cancellationToken: token);
                    }
                }

                if (!res.Success())
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Не удалось обновить складской документ: ";
                    loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError(msg);
                    return res;
                }
            }
        }
        else if (!warehouseDocumentDb.IsDisabled)
        {
            if (warehouseDocumentDb.WritingOffWarehouseId != req.WritingOffWarehouseId)
            {
                foreach (RowOfWarehouseDocumentModelDB rowOfDocument in warehouseDocumentDb.Rows)
                {
                    OfferAvailabilityModelDB?
                                            registerOffer = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == req.WritingOffWarehouseId),
                                            registerOfferWriteOff = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == warehouseDocumentDb.WritingOffWarehouseId);

                    if (registerOfferWriteOff is not null)
                    {
                        if (registerOfferWriteOff.Quantity < rowOfDocument.Quantity)
                        {
                            msg = $"Количество [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.GetName()}'] не может быть списано (остаток {registerOfferWriteOff.Quantity})";
                            loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                            res.AddError($"{msg}. Баланс не может быть отрицательным");
                            break;
                        }

                        await context.OffersAvailability
                             .Where(y => y.Id == registerOfferWriteOff.Id)
                             .ExecuteUpdateAsync(set => set
                                .SetProperty(p => p.Quantity, p => p.Quantity - rowOfDocument.Quantity), cancellationToken: token);

                        registerOfferWriteOff.Quantity -= rowOfDocument.Quantity;
                    }
                    else if (req.WritingOffWarehouseId > 0)
                    {
                        msg = $"Количество [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.GetName()}'] не может быть списано (остаток отсутствует)";
                        loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                        res.AddError($"{msg}. Баланс не может быть отрицательным");
                        break;
                    }

                    if (registerOffer is not null)
                    {
                        await context.OffersAvailability
                            .Where(y => y.Id == registerOffer.Id)
                            .ExecuteUpdateAsync(set =>
                                set.SetProperty(p => p.Quantity, registerOffer.Quantity + rowOfDocument.Quantity), cancellationToken: token);

                        registerOffer.Quantity += rowOfDocument.Quantity;
                    }
                    else
                        await context.OffersAvailability.AddAsync(new()
                        {
                            WarehouseId = req.WritingOffWarehouseId,
                            NomenclatureId = rowOfDocument.NomenclatureId,
                            OfferId = rowOfDocument.OfferId,
                            Quantity = rowOfDocument.Quantity,
                        }, token);
                }

                if (!res.Success())
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Не удалось обновить складской документ: ";
                    loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError(msg);
                    return res;
                }
            }

            if (warehouseDocumentDb.WarehouseId != req.WarehouseId)
            {
                foreach (RowOfWarehouseDocumentModelDB rowOfDocument in warehouseDocumentDb.Rows)
                {
                    OfferAvailabilityModelDB?
                                           registerOffer = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == req.WarehouseId),
                                           registerOfferWriteOff = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == warehouseDocumentDb.WarehouseId);

                    if (registerOfferWriteOff is not null)
                    {
                        if (registerOfferWriteOff.Quantity < rowOfDocument.Quantity)
                        {
                            msg = $"Количество [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.GetName()}'] не может быть списано (остаток {registerOfferWriteOff.Quantity})";
                            loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                            res.AddError($"{msg}. Баланс не может быть отрицательным");
                            break;
                        }

                        await context.OffersAvailability
                             .Where(y => y.Id == registerOfferWriteOff.Id)
                             .ExecuteUpdateAsync(set => set
                                .SetProperty(p => p.Quantity, p => p.Quantity - rowOfDocument.Quantity), cancellationToken: token);

                        registerOfferWriteOff.Quantity -= rowOfDocument.Quantity;
                    }
                    else if (req.WritingOffWarehouseId > 0)
                    {
                        msg = $"Количество [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.GetName()}'] не может быть списано (остаток отсутствует)";
                        loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                        res.AddError($"{msg}. Баланс не может быть отрицательным");
                        break;
                    }

                    if (registerOffer is not null)
                    {
                        await context.OffersAvailability
                            .Where(y => y.Id == registerOffer.Id)
                            .ExecuteUpdateAsync(set =>
                                set.SetProperty(p => p.Quantity, registerOffer.Quantity + rowOfDocument.Quantity), cancellationToken: token);

                        registerOffer.Quantity += rowOfDocument.Quantity;
                    }
                    else
                        await context.OffersAvailability.AddAsync(new()
                        {
                            WarehouseId = req.WritingOffWarehouseId,
                            NomenclatureId = rowOfDocument.NomenclatureId,
                            OfferId = rowOfDocument.OfferId,
                            Quantity = rowOfDocument.Quantity,
                        }, token);
                }

                if (!res.Success())
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Не удалось обновить складской документ: ";
                    loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError(msg);
                    return res;
                }
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
                .SetProperty(p => p.WritingOffWarehouseId, req.WritingOffWarehouseId)
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