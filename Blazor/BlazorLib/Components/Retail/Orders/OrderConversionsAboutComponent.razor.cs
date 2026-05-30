////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Orders;

/// <summary>
/// OrderConversionsAboutComponent
/// </summary>
public partial class OrderConversionsAboutComponent
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required List<ConversionOrderRetailLinkModelDB> ConversionsDocuments { get; set; }


    ConversionOrderRetailLinkModelDB[]? ActualConversionsDocuments;

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        ActualConversionsDocuments = [..ConversionsDocuments.Where(x => x.ConversionDocument?.IsDisabled != true)];
    }
}