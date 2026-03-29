////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml;
using System.Data;
using SharedLib;
using DbcLib;

namespace CommerceService;

/// <summary>
/// Commerce
/// </summary>
public partial class CommerceImplementService : ICommerceService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UploadOffersAsync(List<NomenclatureScopeModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        if (req.Count == 0)
            return ResponseBaseModel.CreateWarning("List<NomenclatureScopeModel> Count == 0");

        string? _nameUpper;
        List<ResultMessage> Messages = [];
        foreach (NomenclatureScopeModel _n in req)
        {
            if (!Enum.TryParse(_n.BaseUnit, true, out UnitsOfMeasurementEnum _bu))
            {
                Messages.Add(new() { TypeMessage = MessagesTypesEnum.Error, Text = $"Единица измерения `{_n.BaseUnit}` не корректная (не удалось конвертировать)" });
                continue;
            }

            _nameUpper = _n.Name?.ToUpper().Trim();
            NomenclatureModelDB? readNomenclature = await context.Nomenclatures
                .FirstOrDefaultAsync(x => x.NormalizedNameUpper == _nameUpper, cancellationToken: token);

            if (readNomenclature is null)
            {
                Messages.Add(new() { TypeMessage = MessagesTypesEnum.Info, Text = $"Создана номенклатура: {_n.Name}" });
                readNomenclature = new()
                {
                    IsDisabled = _n.IsDisabled,
                    NormalizedNameUpper = _nameUpper,
                    BaseUnit = _bu,
                    Description = _n.Description,
                    Name = _n.Name,
                    CreatedAtUTC = DateTime.UtcNow,
                };
                await context.Nomenclatures.AddAsync(readNomenclature, token);
                await context.SaveChangesAsync(cancellationToken: token);
            }

            foreach (OfferScopeModel _off in _n.Offers)
            {
                if (!Enum.TryParse(_off.OfferUnit, true, out _bu))
                {
                    Messages.Add(new() { TypeMessage = MessagesTypesEnum.Error, Text = $"Единица измерения `{_off.OfferUnit}` не корректная (не удалось конвертировать)" });
                    continue;
                }

                OfferModelDB? readOffer = await context.Offers
                    .FirstOrDefaultAsync(x => x.NomenclatureId == readNomenclature.Id && x.Name == _off.Name, cancellationToken: token);

                if (readOffer is null)
                {
                    Messages.Add(new() { TypeMessage = MessagesTypesEnum.Info, Text = $"Создан оффер: {_off.Name}" });
                    readOffer = new()
                    {
                        IsDisabled = _off.IsDisabled,
                        Description = _off.Description,
                        Name = _off.Name,
                        Multiplicity = _off.Multiplicity,
                        NomenclatureId = readNomenclature.Id,
                        Price = _off.Price,
                        Weight = _off.Weight,
                        ShortName = _off.ShortName,
                        QuantitiesTemplate = _off.QuantitiesTemplate,
                        OfferUnit = _bu,
                        CreatedAtUTC = DateTime.UtcNow,
                    };

                    await context.Offers.AddAsync(readOffer, token);
                    await context.SaveChangesAsync(cancellationToken: token);
                }
            }
        }

        if (Messages.Count != 0)
            return ResponseBaseModel.Create(Messages);

        return ResponseBaseModel.CreateSuccess("Ok");
    }
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OfferUpdateOrCreateAsync(TAuthRequestStandardModel<OfferModelDB> req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();

        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }

        if (!string.IsNullOrWhiteSpace(req.Payload.QuantitiesTemplate))
        {
            System.Collections.Immutable.ImmutableList<decimal> idss = req.Payload.QuantitiesTemplate.SplitToDecimalList();

            if (idss.Count == 0)
            {
                res.AddError("Формат доступных значений не корректный");
                return res;
            }
            req.Payload.QuantitiesTemplate = string.Join(" ", idss.Order());
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        DateTime dtu = DateTime.UtcNow;
        if (req.Payload.Id < 1)
        {
            req.Payload = new()
            {
                Name = req.Payload.Name,
                QuantitiesTemplate = req.Payload.QuantitiesTemplate,
                CreatedAtUTC = dtu,
                Description = req.Payload.Description,
                ShortName = req.Payload.ShortName,
                IsDisabled = req.Payload.IsDisabled,
                Multiplicity = req.Payload.Multiplicity,
                NomenclatureId = req.Payload.NomenclatureId,
                OfferUnit = req.Payload.OfferUnit,
                Price = req.Payload.Price,
                LastUpdatedAtUTC = dtu,
                Weight = req.Payload.Weight,
            };

            await context.Offers.AddAsync(req.Payload, token);
            await context.SaveChangesAsync(token);
            res.AddSuccess("Предложение добавлено");
            res.Response = req.Payload.Id;
            return res;
        }

        res.Response = await context.Offers
            .Where(x => x.Id == req.Payload.Id)
            .ExecuteUpdateAsync(set => set
            .SetProperty(p => p.Name, req.Payload.Name)
            .SetProperty(p => p.Description, req.Payload.Description)
            .SetProperty(p => p.QuantitiesTemplate, req.Payload.QuantitiesTemplate)
            .SetProperty(p => p.ShortName, req.Payload.ShortName)
            .SetProperty(p => p.IsDisabled, req.Payload.IsDisabled)
            .SetProperty(p => p.Multiplicity, req.Payload.Multiplicity)
            .SetProperty(p => p.NomenclatureId, req.Payload.NomenclatureId)
            .SetProperty(p => p.OfferUnit, req.Payload.OfferUnit)
            .SetProperty(p => p.Price, req.Payload.Price)
            .SetProperty(p => p.Weight, req.Payload.Weight)
            .SetProperty(p => p.LastUpdatedAtUTC, dtu), cancellationToken: token);

        res.AddSuccess($"Обновление `{GetType().Name}` выполнено");
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseStandardModel<OfferModelDB>>> OffersSelectAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<OffersSelectRequestModel>> req, CancellationToken token = default)
    {
        if (req.Payload?.Payload is null)
        {
            return new()
            {
                Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "req.Payload?.Payload is null" }]
            };
        }

        if (req.Payload.PageSize < 10)
            req.Payload.PageSize = 10;

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<OfferModelDB> q = from o in context.Offers
                                     join n in context.Nomenclatures.Where(x => x.ContextName == req.Payload.Payload.ContextName || ((req.Payload.Payload.ContextName == null || req.Payload.Payload.ContextName == "") && (x.ContextName == null || x.ContextName == ""))) on o.NomenclatureId equals n.Id
                                     select o;

        if (req.Payload.Payload.NomenclatureFilter is not null && req.Payload.Payload.NomenclatureFilter.Length != 0)
            q = q.Where(x => req.Payload.Payload.NomenclatureFilter.Any(y => y == x.NomenclatureId));

        if (req.Payload.Payload.AfterDateUpdate is not null)
            q = q.Where(x => x.LastUpdatedAtUTC >= req.Payload.Payload.AfterDateUpdate);

        IOrderedQueryable<OfferModelDB> oq = req.Payload.SortingDirection == DirectionsEnum.Up
          ? q.OrderBy(x => x.CreatedAtUTC)
          : q.OrderByDescending(x => x.CreatedAtUTC);

        return new()
        {
            Response = new()
            {
                PageNum = req.Payload.PageNum,
                PageSize = req.Payload.PageSize,
                SortingDirection = req.Payload.SortingDirection,
                SortBy = req.Payload.SortBy,
                TotalRowsCount = await q.CountAsync(cancellationToken: token),
                Response = [.. await oq.Skip(req.Payload.PageNum * req.Payload.PageSize).Take(req.Payload.PageSize).Include(x => x.Nomenclature).ToArrayAsync(cancellationToken: token)]
            }
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<OfferModelDB[]>> OffersReadAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default)
    {
        TResponseModel<OfferModelDB[]> res = new();
        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }

        if (!req.Payload.Any(x => x > 0))
        {
            res.AddError($"Пустой запрос > {nameof(OffersReadAsync)}");
            return res;
        }
        req.Payload = [.. req.Payload.Where(x => x > 0)];
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        res.Response = await context
            .Offers
            .Where(x => req.Payload.Any(y => x.Id == y))
            .Include(x => x.Nomenclature)
            .ToArrayAsync(cancellationToken: token);

        return res;
    }
}