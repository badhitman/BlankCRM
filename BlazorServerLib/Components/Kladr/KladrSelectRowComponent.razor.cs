////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorWebLib.Components.Kladr;

/// <inheritdoc/>
public partial class KladrSelectRowComponent
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required KladrResponseModel RowElement { get; set; }
}