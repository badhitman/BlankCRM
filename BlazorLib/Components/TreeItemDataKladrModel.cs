﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using MudBlazor;
using SharedLib;

namespace BlazorLib;

/// <summary>
/// Tree Item Data Kladr
/// </summary>
public class TreeItemDataKladrModel : TreeItemData<ObjectKLADRModelDB?>
{
    /// <summary>
    /// Tree Item Data Kladr
    /// </summary>
    public TreeItemDataKladrModel(ObjectKLADRModelDB entry, string icon) : base(entry)
    {
        Text = entry.NAME;
        Icon = icon;
        Expandable = entry.Id > 0;
    }

    /// <inheritdoc/>
    public TreeItemDataKladrModel(TreeItemData<ObjectKLADRModelDB> x)
    {
        TreeItemDataKladrModel _sender = (TreeItemDataKladrModel)x!;
        Value = _sender.Value;
        Children = _sender.Children;
        Icon = _sender.Icon;
        Expanded = _sender.Expanded;
        Expandable = _sender.Expandable;
        Visible = _sender.Visible;
        Text = _sender.Text;
        Selected = _sender.Selected;
    }

    /// <inheritdoc/>
    public static bool operator ==(TreeItemDataKladrModel? e1, TreeItemDataKladrModel? e2)
        => (e1 is null && e2 is null) || e1?.Value == e2?.Value;

    /// <inheritdoc/>
    public static bool operator !=(TreeItemDataKladrModel? e1, TreeItemDataKladrModel? e2)
        => e1?.Value != e2?.Value;


    /// <inheritdoc/>
    public static bool operator ==(TreeItemDataKladrModel? e1, TreeItemData<ObjectKLADRModelDB?> e2)
        => (e1 is null && e2 is null) || e1?.Value == e2?.Value;

    /// <inheritdoc/>
    public static bool operator !=(TreeItemDataKladrModel? e1, TreeItemData<ObjectKLADRModelDB?> e2)
        => e1?.Value != e2?.Value;


    /// <inheritdoc/>
    public static bool operator ==(TreeItemData<ObjectKLADRModelDB?> e1, TreeItemDataKladrModel? e2)
        => (e1 is null && e2 is null) || e1?.Value == e2?.Value;

    /// <inheritdoc/>
    public static bool operator !=(TreeItemData<ObjectKLADRModelDB?> e2, TreeItemDataKladrModel? e1)
        => e1?.Value != e2?.Value;


    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is TreeItemDataKladrModel _e)
            return Value == _e.Value;
        else if (obj is TreeItemData<ObjectKLADRModelDB?> _v)
            return Value == _v.Value;

        return base.Equals(obj);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
        => Value!.GetHashCode();
}