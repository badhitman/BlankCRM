////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

public partial class AboutDatabaseStockSharpComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IManageStockSharpService manRepo { get; set; } = default!;

    AboutDatabasesResponseModel? About;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        About = await manRepo.AboutDatabases();
        await SetBusyAsync(false);
    }
}