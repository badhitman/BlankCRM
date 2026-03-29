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
    public async Task<TResponseModel<List<PriceRuleForOfferModelDB>>> PricesRulesGetForOffersAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default)
    {
        TResponseModel<List<PriceRuleForOfferModelDB>> res = new();

        if (req.Payload is null)
        {
            res.AddError("Offer not set fo request");
            return res;
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        return new()
        {
            Response = await context
            .PricesRules.Where(x => req.Payload.Any(y => x.OfferId == y))
            .Include(x => x.Offer)
            .ToListAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> PriceRuleUpdateOrCreateAsync(TAuthRequestStandardModel<PriceRuleForOfferModelDB> req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        if (req.Payload is null)
        {
            res.AddError("price update body request: is null");
            return res;
        }

        if (req.Payload.Name is not null)
            req.Payload.Name = req.Payload.Name.Trim();

        if (req.Payload.QuantityRule <= 1)
        {
            res.AddError("Количество должно быть больше одного");
            return res;
        }
        if (await context.PricesRules.AnyAsync(x => x.Id != req.Payload.Id && x.OfferId == req.Payload.OfferId && x.QuantityRule == req.Payload.QuantityRule, cancellationToken: token))
        {
            res.AddError("Правило с таким количеством уже существует");
            return res;
        }

        if (req.Payload.Id < 1)
        {
            req.Payload.CreatedAtUTC = DateTime.UtcNow;
            req.Payload.LastUpdatedAtUTC = DateTime.UtcNow;
            await context.PricesRules.AddAsync(req.Payload, token);
            await context.SaveChangesAsync(token);
            res.AddSuccess("Создано новое правило ценообразования");
        }
        else
        {
            await context
                .PricesRules
                .Where(x => x.Id == req.Payload.Id)
                .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.IsDisabled, req.Payload.IsDisabled)
                .SetProperty(p => p.Name, req.Payload.Name)
                .SetProperty(p => p.PriceRule, req.Payload.PriceRule)
                .SetProperty(p => p.QuantityRule, req.Payload.QuantityRule)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

            res.AddSuccess("Правило ценообразования обновлено");
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<PriceRuleForOfferModelDB>> PriceRuleDeleteAsync(TAuthRequestStandardModel<PriceRuleDeleteRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new()
            {
                Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "req.Payload is null" }]
            };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<PriceRuleForOfferModelDB> q = context.PricesRules.Where(x => x.Id == req.Payload.RuleId);
        TResponseModel<PriceRuleForOfferModelDB> res = new()
        {
            Response = await q.Include(x => x.Offer).FirstAsync(cancellationToken: token)
        };

        if (await q.ExecuteDeleteAsync(cancellationToken: token) > 0)
            res.AddSuccess("Правило ценообразования успешно удалено");
        else
            res.AddInfo("Правило отсутствует");

        return res;
    }
}