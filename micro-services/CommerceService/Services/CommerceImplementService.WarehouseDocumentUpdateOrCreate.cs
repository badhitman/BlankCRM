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
    public async Task<TResponseModel<int>> WarehouseDocumentUpdateOrCreateAsync(TAuthRequestStandardModel<WarehouseDocumentModelDB> req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }
        ValidateReportModel ck = GlobalTools.ValidateObject(req.Payload);
        if (!ck.IsValid)
        {
            res.Messages.InjectException(ck.ValidationResults);
            return res;
        }

        TResponseModel<bool?> warehouseNegativeBalanceAllowed = await StorageTransmissionRepo
              .ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.WarehouseNegativeBalanceAllowed, token);

        string msg;
        if (req.Payload.WarehouseId == req.Payload.WritingOffWarehouseId)
        {
            msg = $"Склад списания и склад поступления не может быть одни и тем же";
            loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError($"{msg}. Измените или удалите склад списания");
            return res;
        }
        loggerRepo.LogInformation($"{nameof(req)}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
        req.Payload.DeliveryDate = req.Payload.DeliveryDate.SetKindUtc();
        if (req.Payload.Name is not null)
            req.Payload.Name = req.Payload.Name.Trim();
        req.Payload.NormalizedUpperName = req.Payload.Name?.ToUpper() ?? "";

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        DateTime dtu = DateTime.UtcNow;
        if (req.Payload.Id < 1)
        {
            req.Payload.Rows?.Clear();
            req.Payload.Id = 0;
            req.Payload.Version = Guid.NewGuid();
            req.Payload.CreatedAtUTC = dtu;
            req.Payload.LastUpdatedAtUTC = dtu;
            req.Payload.IsDisabled = true;
            await context.AddAsync(req, token);
            await context.SaveChangesAsync(token);
            res.Response = req.Payload.Id;
            res.AddSuccess("Документ создан");
            return res;
        }

        WarehouseDocumentModelDB warehouseDocumentDb = await context
            .WarehouseDocuments
            .Include(x => x.Rows)
            .FirstAsync(x => x.Id == req.Payload.Id, cancellationToken: token);

        if (warehouseDocumentDb.Version != req.Payload.Version)
        {
            msg = $"Документ #{warehouseDocumentDb.Id} был кем-то изменён (version concurrent)";
            loggerRepo.LogWarning($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError($"{msg}. Перед редактированием обновите страницу (F5), что бы загрузить актуальную версию документа");
            return res;
        }
        loggerRepo.LogInformation($"{nameof(warehouseDocumentDb)}:{JsonConvert.SerializeObject(warehouseDocumentDb, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
        if (warehouseDocumentDb.Rows is null || warehouseDocumentDb.Rows.Count == 0)
        {
            res.Response = await context.WarehouseDocuments
                .Where(x => x.Id == req.Payload.Id)
                .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Payload.Name)
                .SetProperty(p => p.Description, req.Payload.Description)
                .SetProperty(p => p.DeliveryDate, req.Payload.DeliveryDate)
                .SetProperty(p => p.IsDisabled, req.Payload.IsDisabled)
                .SetProperty(p => p.WarehouseId, req.Payload.WarehouseId)
                .SetProperty(p => p.WritingOffWarehouseId, req.Payload.WritingOffWarehouseId)
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.LastUpdatedAtUTC, dtu), cancellationToken: token);

            res.AddSuccess("Складской документ обновлён");
            return res;
        }

        List<LockTransactionModelDB> offersLocked = [];
        foreach (RowOfWarehouseDocumentModelDB rowDoc in warehouseDocumentDb.Rows)
        {
            offersLocked.Add(new LockTransactionModelDB()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerAreaId = req.Payload.WarehouseId,
                LockerId = rowDoc.OfferId,
                Marker = nameof(WarehouseDocumentUpdateOrCreateAsync),
            });
            if (req.Payload.WritingOffWarehouseId > 0)
                offersLocked.Add(new LockTransactionModelDB()
                {
                    LockerName = nameof(OfferAvailabilityModelDB),
                    LockerAreaId = req.Payload.WritingOffWarehouseId,
                    LockerId = rowDoc.OfferId,
                    Marker = nameof(WarehouseDocumentUpdateOrCreateAsync),
                });

            if (warehouseDocumentDb.WritingOffWarehouseId != req.Payload.WritingOffWarehouseId && warehouseDocumentDb.WritingOffWarehouseId > 0)
                offersLocked.Add(new LockTransactionModelDB()
                {
                    LockerName = nameof(OfferAvailabilityModelDB),
                    LockerAreaId = warehouseDocumentDb.WritingOffWarehouseId,
                    LockerId = rowDoc.OfferId,
                    Marker = nameof(WarehouseDocumentUpdateOrCreateAsync),
                });

            if (warehouseDocumentDb.WarehouseId != req.Payload.WarehouseId)
                offersLocked.Add(new LockTransactionModelDB()
                {
                    LockerName = nameof(OfferAvailabilityModelDB),
                    LockerAreaId = warehouseDocumentDb.WarehouseId,
                    LockerId = rowDoc.OfferId,
                    Marker = nameof(WarehouseDocumentUpdateOrCreateAsync),
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
                msg = $"Не удалось выполнить команду блокировки регистров остатков {nameof(WarehouseDocumentUpdateOrCreateAsync)}: ";
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

        if (warehouseDocumentDb.IsDisabled != req.Payload.IsDisabled)
        {
            if (req.Payload.IsDisabled)
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
                        registerOffer = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == req.Payload.WarehouseId),
                        registerOfferWriteOff = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == req.Payload.WritingOffWarehouseId);

                    if (registerOfferWriteOff is not null)
                    {
                        if (registerOfferWriteOff.Quantity < rowOfDocument.Quantity && warehouseNegativeBalanceAllowed.Response != true)
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
                    else if (req.Payload.WritingOffWarehouseId > 0)
                    {
                        if (warehouseNegativeBalanceAllowed.Response != true)
                        {
                            msg = $"Количество [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.GetName()}'] не может быть списано (остаток отсутствует)";
                            loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                            res.AddError($"{msg}. Баланс не может быть отрицательным");
                            break;
                        }
                        else
                        {
                            registerOfferWriteOff = new()
                            {
                                WarehouseId = req.Payload.WritingOffWarehouseId,
                                OfferId = rowOfDocument.OfferId,
                                NomenclatureId = rowOfDocument.NomenclatureId,
                                Quantity = -rowOfDocument.Quantity,
                            };
                            await context.OffersAvailability.AddAsync(registerOfferWriteOff, token);
                            await context.SaveChangesAsync(token);
                            registersOffersDb.Add(registerOfferWriteOff);
                        }
                    }

                    if (registerOffer is not null)
                    {
                        await context.OffersAvailability
                             .Where(y => y.Id == registerOffer.Id)
                             .ExecuteUpdateAsync(set => set
                                .SetProperty(p => p.Quantity, p => p.Quantity - rowOfDocument.Quantity), cancellationToken: token);

                        registerOffer.Quantity += rowOfDocument.Quantity;
                    }
                    else
                    {
                        registerOffer = new OfferAvailabilityModelDB()
                        {
                            WarehouseId = req.Payload.WarehouseId,
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
            if (warehouseDocumentDb.WritingOffWarehouseId != req.Payload.WritingOffWarehouseId)
            {
                foreach (RowOfWarehouseDocumentModelDB rowOfDocument in warehouseDocumentDb.Rows)
                {
                    OfferAvailabilityModelDB?
                                            registerOffer = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == req.Payload.WritingOffWarehouseId),
                                            registerOfferWriteOff = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == warehouseDocumentDb.WritingOffWarehouseId);

                    if (registerOfferWriteOff is not null)
                    {
                        if (registerOfferWriteOff.Quantity < rowOfDocument.Quantity && warehouseNegativeBalanceAllowed.Response != true)
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
                    else if (req.Payload.WritingOffWarehouseId > 0)
                    {
                        if (warehouseNegativeBalanceAllowed.Response != true)
                        {
                            msg = $"Количество [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.GetName()}'] не может быть списано (остаток отсутствует)";
                            loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                            res.AddError($"{msg}. Баланс не может быть отрицательным");
                            break;
                        }
                        else
                        {
                            await context.OffersAvailability.AddAsync(new()
                            {
                                WarehouseId = req.Payload.WritingOffWarehouseId,
                                NomenclatureId = rowOfDocument.NomenclatureId,
                                OfferId = rowOfDocument.OfferId,
                                Quantity = rowOfDocument.Quantity,
                            }, token);

                            await context.SaveChangesAsync(token);
                        }
                    }

                    if (registerOffer is not null)
                    {
                        await context.OffersAvailability
                            .Where(y => y.Id == registerOffer.Id)
                            .ExecuteUpdateAsync(set =>
                                set.SetProperty(p => p.Quantity, registerOffer.Quantity + rowOfDocument.Quantity), cancellationToken: token);

                        registerOffer.Quantity += rowOfDocument.Quantity;
                    }
                    else if (req.Payload.WritingOffWarehouseId > 0)
                        await context.OffersAvailability.AddAsync(new()
                        {
                            WarehouseId = req.Payload.WritingOffWarehouseId,
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

            if (warehouseDocumentDb.WarehouseId != req.Payload.WarehouseId)
            {
                foreach (RowOfWarehouseDocumentModelDB rowOfDocument in warehouseDocumentDb.Rows)
                {
                    OfferAvailabilityModelDB?
                                           registerOffer = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == req.Payload.WarehouseId),
                                           registerOfferWriteOff = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == warehouseDocumentDb.WarehouseId);

                    if (registerOfferWriteOff is not null)
                    {
                        if (registerOfferWriteOff.Quantity < rowOfDocument.Quantity && warehouseNegativeBalanceAllowed.Response != true)
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
                    else if (req.Payload.WritingOffWarehouseId > 0)
                    {
                        if (warehouseNegativeBalanceAllowed.Response != true)
                        {
                            msg = $"Количество [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.GetName()}'] не может быть списано (остаток отсутствует)";
                            loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                            res.AddError($"{msg}. Баланс не может быть отрицательным");
                            break;
                        }
                        else
                        {
                            registerOfferWriteOff = new()
                            {
                                WarehouseId = warehouseDocumentDb.WarehouseId,
                                NomenclatureId = rowOfDocument.NomenclatureId,
                                OfferId = rowOfDocument.OfferId,
                                Quantity = -rowOfDocument.Quantity,
                            };
                            await context.OffersAvailability.AddAsync(registerOfferWriteOff, token);
                            await context.SaveChangesAsync(token);
                        }
                    }

                    if (registerOffer is not null)
                    {
                        await context.OffersAvailability
                            .Where(y => y.Id == registerOffer.Id)
                            .ExecuteUpdateAsync(set =>
                                set.SetProperty(p => p.Quantity, registerOffer.Quantity + rowOfDocument.Quantity), cancellationToken: token);

                        registerOffer.Quantity += rowOfDocument.Quantity;
                    }
                    else if (req.Payload.WritingOffWarehouseId > 0)
                        await context.OffersAvailability.AddAsync(new()
                        {
                            WarehouseId = req.Payload.WritingOffWarehouseId,
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

        if (offersLocked.Count != 0)
            context.RemoveRange(offersLocked);

        await context.SaveChangesAsync(token);

        res.Response = await context.WarehouseDocuments
                .Where(x => x.Id == req.Payload.Id)
                .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Payload.Name)
                .SetProperty(p => p.Description, req.Payload.Description)
                .SetProperty(p => p.DeliveryDate, req.Payload.DeliveryDate)
                .SetProperty(p => p.IsDisabled, req.Payload.IsDisabled)
                .SetProperty(p => p.WarehouseId, req.Payload.WarehouseId)
                .SetProperty(p => p.WritingOffWarehouseId, req.Payload.WritingOffWarehouseId)
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.LastUpdatedAtUTC, dtu), cancellationToken: token);

        res.AddSuccess("Складской документ обновлён");
        await transaction.CommitAsync(token);
        return res;
    }
}