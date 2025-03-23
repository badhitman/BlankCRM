////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;
using MudBlazor;

namespace BlazorWebLib.Components.Commerce.Organizations;

/// <summary>
/// BanksListDetailsOrganizationComponent
/// </summary>
public partial class BanksListDetailsOrganizationComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IDialogService DialogService { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required OrganizationModelDB CurrentOrganization { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public bool ReadOnly { get; set; }


    /// <inheritdoc/>
    public MudExpansionPanels? PanelsRef { get; private set; }

    /// <inheritdoc/>
    public bool IsExpanded { get; private set; }

    /// <inheritdoc/>
    void OnExpandedChanged(bool newVal)
    {
        if (ReadOnly)
        { 
            IsExpanded = false; 
            return; 
        }

        IsExpanded = newVal;
    }

    private Task<IDialogReference> CreateNewBankDetails()
    {
        DialogParameters<BankDetailsEditComponent> parameters = new()
        {
            { x => x.BankDetails, BankDetailsModelDB.BuildEmpty(CurrentOrganization) },
            { x => x.StateHasChangedHandler, StateHasChangedCall }
        };
        DialogOptions options = new() { CloseOnEscapeKey = true };

        return DialogService.ShowAsync<BankDetailsEditComponent>("Банковские реквизиты", parameters, options);
    }
}