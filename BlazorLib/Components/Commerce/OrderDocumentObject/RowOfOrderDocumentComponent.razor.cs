////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorLib.Components.Commerce.OrderDocumentObject;

/// <summary>
/// RowOfOrderDocumentComponent
/// </summary>
public partial class RowOfOrderDocumentComponent : BlazorBusyComponentBaseModel
{
    /// <summary>
    /// Строка заказа (документа)
    /// </summary>
    [Parameter, EditorRequired]
    public required RowOfOrderDocumentModelDB Row { get; set; }

    /// <summary>
    /// Issue
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required IssueHelpDeskModelDB Issue { get; set; }
}