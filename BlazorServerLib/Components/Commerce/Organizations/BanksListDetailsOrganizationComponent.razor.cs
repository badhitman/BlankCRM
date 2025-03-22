////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.Commerce.Organizations;

/// <summary>
/// BanksListDetailsOrganizationComponent
/// </summary>
public partial class BanksListDetailsOrganizationComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required OrganizationModelDB CurrentOrganization { get; set; }


    bool IsExpanded;

    /// <inheritdoc/>
    private async Task OnExpandedChanged(bool newVal)
    {
        IsExpanded=newVal;
        if (newVal)
        {


        }
        else
        {

        }
    }
}