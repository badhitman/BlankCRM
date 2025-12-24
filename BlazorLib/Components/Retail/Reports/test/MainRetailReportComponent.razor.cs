////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Retail.Reports.mmm;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Reports.test;

/// <summary>
/// MainRetailReportComponent
/// </summary>
public partial class MainRetailReportComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    [Inject]
    IParametersStorageTransmission StorageTransmissionRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter]
    public MMMWrapperComponent? Owner { get; set; }


    StorageMetadataModel StorageMetadata = new()
    {
        ApplicationName = "MMM",
        PropertyName = "office",
        PrefixPropertyName = "bonus-amount",
    };

    decimal _bonusAmount = 0;
    decimal BonusAmount
    {
        get => _bonusAmount;
        set
        {
            _bonusAmount = value;
            InvokeAsync(SaveParameter);
        }
    }

    MainReportResponseModel? ReportData;

    DateRange? _dateRange;
    DateRange? DateRangeProp
    {
        get => _dateRange;
        set
        {
            _dateRange = value;
            InvokeAsync(ReloadServerData);
        }
    }

    async void SaveParameter()
    {
        _ = await StorageTransmissionRepo.SaveParameterAsync<decimal?>(BonusAmount, StorageMetadata, true);
        StateHasChanged();
    }

    /// <inheritdoc/>
    public async Task ReloadServerData()
    {
        if (Owner?.SelectedWeek is not null)
            _dateRange = new()
            {
                Start = Owner.SelectedWeek.Value.Start,
                End = Owner.SelectedWeek.Value.End,
            };

        MainReportRequestModel req = new();

        if (Owner is not null && Owner.SelectedWeek.HasValue)
        {
            req.NumWeekOfYear = Owner.SelectedWeek.Value.NumWeekOfYear;
        }
        else if (DateRangeProp is not null)
        {
            req.Start = DateRangeProp.Start;
            req.End = DateRangeProp.End;
        }

        await SetBusyAsync();
        ReportData = await RetailRepo.GetMainReportAsync(req);
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await SetBusyAsync();
        TResponseModel<decimal> showCreatingIssue = await StorageTransmissionRepo.ReadParameterAsync<decimal>(StorageMetadata);
        _bonusAmount = showCreatingIssue.Response;
        if (Owner?.SelectedWeek is not null)
            _dateRange = new()
            {
                Start = Owner.SelectedWeek.Value.Start,
                End = Owner.SelectedWeek.Value.End,
            };

        await ReloadServerData();
        await SetBusyAsync(false);
    }
}