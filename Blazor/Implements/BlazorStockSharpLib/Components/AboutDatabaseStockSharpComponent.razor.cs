////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using BlazorLib;
using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorStockSharpLib.Components;

/// <summary>
/// AboutDatabaseStockSharpComponent
/// </summary>
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