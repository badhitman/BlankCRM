﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorWebLib.Components.Kladr.main;

/// <summary>
/// KladrRowStreetComponent
/// </summary>
public partial class KladrRowStreetComponent
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required StreetKLADRModelDB ObjectKLADR { get; set; }
}