////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using BlazorLib.Components.Retail.Reports.test;
using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Reports.mmm;

/// <summary>
/// MMMReportsComponent
/// </summary>
public partial class MMMReportsComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter]
    public MMMWrapperComponent? Owner { get; set; }


    //PaymentsRetailReportComponent? _finReport;
    OffersOfOrdersRetailReportComponent? _retailReport;
    OffersOfDeliveriesRetailReportComponent? _delReport;
    MainRetailReportComponent? _mainReport;

    /// <inheritdoc/>
    public async Task Reload()
    {
        //if (_finReport is not null)
        //    await _finReport.Reload();

        if (_retailReport is not null)
            await _retailReport.Reload();

        if (_delReport is not null)
            await _delReport.Reload();

        if (_mainReport is not null)
            await _mainReport.ReloadServerData();

        base.StateHasChangedCall();
    }
}