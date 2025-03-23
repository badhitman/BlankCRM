////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.Commerce.Organizations;

/// <summary>
/// BankDetailsEditComponent
/// </summary>
public partial class BankDetailsEditComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required BankDetailsModelDB BankDetails { get; set; }

    BankDetailsModelDB? bankDetailsEdit;

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        bankDetailsEdit = GlobalTools.CreateDeepCopy(BankDetails);
    }
}