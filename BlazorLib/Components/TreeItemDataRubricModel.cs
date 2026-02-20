////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using MudBlazor;
using SharedLib;

namespace BlazorLib;

/// <summary>
/// Tree Item Data Rubric
/// </summary>
public class TreeItemDataRubricModel : TreeItemData<RubricNestedModel?>
{
    /// <summary>
    /// Tree Item Data Rubric
    /// </summary>
    public TreeItemDataRubricModel(RubricNestedModel entry, string icon) : base(entry)
    {
        Text = entry.Name;
        Icon = icon;
        Expandable = entry.Id > 0;
    }

    /// <inheritdoc/>
    public TreeItemDataRubricModel(TreeItemData<RubricNestedModel> x)
    {
        TreeItemDataRubricModel _sender = (TreeItemDataRubricModel)x!;
        Value = _sender.Value;
        Children = _sender.Children;
        Icon = _sender.Icon;
        MoveRowState = _sender.MoveRowState;
        Expanded = _sender.Expanded;
        Expandable = _sender.Expandable;
        Visible = _sender.Visible;
        Text = _sender.Text;
        Selected = _sender.Selected;
    }

    /// <summary>
    /// Состояние элемента касательно возможности его сдвинуть (выше/ниже)
    /// </summary>
    /// <remarks>
    /// Для организации перемещения/сдвига строк в таблицах/списках
    /// </remarks>
    public MoveRowStatesEnum MoveRowState { get; set; }

    /// <inheritdoc/>
    public static bool operator ==(TreeItemDataRubricModel? e1, TreeItemDataRubricModel? e2)
        => (e1 is null && e2 is null) || (e1 is not null && e2 is not null && e1.Equals(e2));

    /// <inheritdoc/>
    public static bool operator !=(TreeItemDataRubricModel? e1, TreeItemDataRubricModel? e2)
    {
        if (e2?.Value is null && e1?.Value is null)
            return false;
        if (e2?.Value is null || e1?.Value is null)
            return true;

        return e1.Value.Equals(e2.Value);
    }


    /// <inheritdoc/>
    public static bool operator ==(TreeItemDataRubricModel? e1, TreeItemData<RubricNestedModel?> e2)
    {
        if (e2.Value is null && e1?.Value is null)
            return true;
        if (e2.Value is null || e1?.Value is null)
            return false;

        return  e1.Value.Equals(e2.Value);
    }

    /// <inheritdoc/>
    public static bool operator !=(TreeItemDataRubricModel? e1, TreeItemData<RubricNestedModel?> e2)
    {
        if (e2.Value is null && e1?.Value is null)
            return false;
        if (e2.Value is null || e1?.Value is null)
            return true;

        return !e1.Value.Equals(e2.Value);
    }


    /// <inheritdoc/>
    public static bool operator ==(TreeItemData<RubricNestedModel?> e1, TreeItemDataRubricModel? e2)
    {
        if (e1.Value is null && e2?.Value is null)
            return true;
        if (e1.Value is null || e2?.Value is null)
            return false;

        return e1.Value.Equals(e2.Value);
    }

    /// <inheritdoc/>
    public static bool operator !=(TreeItemData<RubricNestedModel?> e2, TreeItemDataRubricModel? e1)
    {
        if(e2.Value is null && e1?.Value is null)
            return true;
        if (e2.Value is null || e1?.Value is null)
            return false;


        return e1.Value != e2.Value; ;
    }


    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is TreeItemDataRubricModel _e)
        {
            if (_e.Value is null && Value is null)
                return true;
            if (_e.Value is null || Value is null)
                return false;

            return Value.Equals(_e.Value);
        }
        else if (obj is TreeItemData<RubricNestedModel?> _v)
        {
            if (_v.Value is null && Value is null)
                return true;
            if (_v.Value is null || Value is null)
                return false;

            return Value.Equals(_v.Value);
        }

        return base.Equals(obj);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
        => Value!.GetHashCode();
}