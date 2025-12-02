////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;

namespace BlazorLib.Components.Commerce.Pages;

/// <summary>
/// OrganizationEditPage
/// </summary>
public partial class OrganizationEditPage : BlazorBusyComponentBaseModel
{
    /// <summary>
    /// OrganizationId
    /// </summary>
    [Parameter]
    public int? OrganizationId { get; set; }
}