////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;

namespace BlazorLib;

/// <summary>
/// MetaPropertyBaseComponent
/// </summary>
public class MetaPropertyBaseComponent : BlazorBusyComponentBaseAuthModel
{
    /// <summary>
    /// Приложения
    /// </summary>
    [Parameter]
    public string[]? ApplicationsNames { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    [Parameter]
    public string? PropertyName { get; set; }

    /// <summary>
    /// Префикс
    /// </summary>
    [Parameter]
    public string? PrefixPropertyName { get; set; }

    /// <summary>
    /// Идентификатор [PK] владельца объекта
    /// </summary>
    [Parameter]
    public int? OwnerPrimaryKey { get; set; }

    /// <summary>
    /// ManageMode
    /// </summary>
    [Parameter]
    public bool ManageMode { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    [Parameter]
    public string? Title { get; set; }
}