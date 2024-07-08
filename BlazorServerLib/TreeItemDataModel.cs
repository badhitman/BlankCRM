﻿using MudBlazor;
using SharedLib;

namespace BlazorWebLib;

/// <inheritdoc/>
public class TreeItemDataModel : TreeItemData<EntryTagModel>
{
    /// <summary>
    /// Всплывающая подсказка
    /// </summary>
    public string? Tooltip { get; set; }

    /// <summary>
    /// Признак того что элемент не может быть выгружен в генератор кода.
    /// </summary>
    /// <remarks>
    /// Например поле типа [генератор]
    /// </remarks>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Информация
    /// </summary>
    public string? Information { get; set; }

    /// <inheritdoc/>
    public TreeItemDataModel(EntryTagModel entry, string icon) : base(entry)
    {
        Text = entry.Name;
        Icon = icon;
    }
}
