﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;

namespace BlazorLib.Components;

/// <inheritdoc/>
public partial class LastUpdateViewComponent : ComponentBase
{
    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    [Parameter, EditorRequired]
    public required DateTime? LastUpdateDate { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public bool Reverse { get; set; }

    /// <inheritdoc/>
    protected TimeSpan? Delta => Reverse ? LastUpdateDate - DateTime.Now : DateTime.Now - LastUpdateDate;
}