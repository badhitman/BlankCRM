////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;
using MudBlazor;

namespace BlazorLib.Components.Commerce;

/// <summary>
/// PriceRuleElementComponent
/// </summary>
public partial class PriceRuleElementComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <summary>
    /// PriceRule
    /// </summary>
    [Parameter, EditorRequired]
    public required PriceRuleForOfferModelDB PriceRule { get; set; }

    /// <summary>
    /// OwnerComponent
    /// </summary>
    [Parameter, EditorRequired]
    public required PricesRulesForOfferComponent OwnerComponent { get; set; }


    bool InitDelete;

    /// <inheritdoc/>
    public bool IsActive { get; set; }


    /// <inheritdoc/>
    public async Task SaveRule()
    {
        if (CurrentUserSession is null)
            return;

        await SetBusyAsync();
        TResponseModel<int> res = await CommerceRepo.PriceRuleUpdateOrCreateAsync(new() { Payload = PriceRule, SenderActionUserId = CurrentUserSession.UserId });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await OwnerComponent.ReloadRules();
    }

    /// <inheritdoc/>
    public async Task DeleteRule()
    {
        if (CurrentUserSession is null)
            return;

        await SetBusyAsync();
        ResponseBaseModel res = await CommerceRepo.PriceRuleDeleteAsync(new()
        {
            Payload = new() { RuleId = PriceRule.Id },
            SenderActionUserId = CurrentUserSession.UserId
        });

        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await OwnerComponent.ReloadRules();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        OwnerComponent.RulesViewsComponents.Add(this);
    }
}