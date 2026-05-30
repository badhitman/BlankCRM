////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorOutersLib.Components.Daichi;

/// <summary>
/// ProductRowParamsComponent
/// </summary>
public partial class ProductRowParamsComponent
{
    /// <summary>
    /// Params
    /// </summary>
    [Parameter, EditorRequired]
    public ParamsProductDaichiModelDB? Params { get; set; }
}