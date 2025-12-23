////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Reports.mmm;

/// <summary>
/// OrdersMMMYearSelectorComponent
/// </summary>
public partial class MMMYearSelectorComponent
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    PeriodBaseModel? aboutPeriod;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        aboutPeriod = await RetailRepo.AboutPeriodAsync();
        if (aboutPeriod is not null)
        {
            if (aboutPeriod.Start is null || aboutPeriod.Start > DateTime.Now)
                aboutPeriod.Start = DateTime.Now;

            if (aboutPeriod.End is null || aboutPeriod.End < DateTime.Now)
                aboutPeriod.End = DateTime.Now;
        }
        await SetBusyAsync(false);
    }
}