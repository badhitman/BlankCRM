////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorWebLib.Components.Kladr;

/// <inheritdoc/>
public partial class KladrMainNavComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IKladrNavigationService KladrNavRepo { get; set; } = default!;


    /// <summary>
    /// Имя контекста
    /// </summary>
    [Parameter]
    public string? ContextName { get; set; }

    /// <summary>
    /// ValueChanged
    /// </summary>
    [Parameter]
    public KladrNavigationTreeViewOptionsModel? SelectedValuesChanged { get; set; }

    /// <summary>
    /// Без вложенных узлов
    /// </summary>
    [Parameter]
    public bool SingleLevelMode { get; set; }


    List<TreeItemDataKladrModel> InitialTreeItems { get; set; } = [];
    void SelectedValuesChangeHandler(IReadOnlyCollection<ObjectKLADRModelDB?> SelectedValues)
    {
        SelectedValuesChanged?.SelectedValuesChangedHandler(SelectedValues);
    }

    List<TreeItemData<ObjectKLADRModelDB>> ConvertRubrics(IEnumerable<ObjectKLADRModelDB> rubrics)
    {


        return [.. rubrics.Select(x => {


            TreeItemDataKladrModel _ri = new (x, x.Id == 0 ? Icons.Material.Filled.PlaylistAdd : SelectedValuesChanged is null ? Icons.Material.Filled.CropFree : Icons.Custom.Uncategorized.Folder )
                {
                    Selected = SelectedValuesChanged?.SelectedNodes.Contains(x.Id) == true
                };

            if(SingleLevelMode)
                _ri.Expandable = false;

            return _ri;
        })];
    }

    void ItemUpdAction(ObjectKLADRModelDB sender)
    {
        TreeItemDataKladrModel findNode = FindNode(sender.CODE, InitialTreeItems) ?? throw new Exception();
        findNode.Text = sender.NAME;
        findNode.Value?.Update(sender);
    }

    async void ReloadNodeAction(string parent_id)
    {
        List<ObjectKLADRModelDB> rubrics = await RequestRubrics(parent_id);
        if (!string.IsNullOrWhiteSpace(parent_id))
        {
            TreeItemDataKladrModel findNode = FindNode(parent_id, InitialTreeItems) ?? throw new Exception();
            findNode.Children = ConvertRubrics(rubrics)!;
        }
        else
        {
            InitialTreeItems = [.. ConvertRubrics(rubrics).Select(x => new TreeItemDataKladrModel(x))]; //.Cast<TreeItemDataKladrModel>()];
        }
        await SetBusy(false);
    }

    static TreeItemDataKladrModel? FindNode(string parent_id, IEnumerable<TreeItemDataKladrModel> treeItems)
    {
        TreeItemDataKladrModel? res = treeItems.FirstOrDefault(x => x.Value?.CODE == parent_id);
        if (res is not null)
            return res;

        TreeItemDataKladrModel? FindChildNode(List<TreeItemData<ObjectKLADRModelDB?>> children)
        {
            TreeItemData<ObjectKLADRModelDB?>? res_child = children.FirstOrDefault(x => x.Value?.CODE == parent_id);
            if (res_child is not null)
                return (TreeItemDataKladrModel?)res_child;

            foreach (TreeItemData<ObjectKLADRModelDB?> c in children)
            {
                if (c.Children is not null)
                {
                    res_child = FindChildNode(c.Children);
                    if (res_child is not null)
                        return (TreeItemDataKladrModel?)res_child;
                }
            }

            return null;
        }

        foreach (TreeItemDataKladrModel _tin in treeItems)
        {
            if (_tin.Children is not null)
            {
                res = FindChildNode(_tin.Children);
                if (res is not null)
                    return res;
            }
        }

        return null;
    }

    /// <inheritdoc/>
    protected override async void OnInitialized()
    {
        List<ObjectKLADRModelDB> rubrics = await RequestRubrics();
        InitialTreeItems = [.. ConvertRubrics(rubrics).Select(x => new TreeItemDataKladrModel(x))];
        await SetBusy(false);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<TreeItemData<ObjectKLADRModelDB?>>> LoadServerData(ObjectKLADRModelDB? parentValue)
    {
        ArgumentNullException.ThrowIfNull(parentValue);

        List<ObjectKLADRModelDB> rubrics = await RequestRubrics(parentValue.CODE);
        TreeItemDataKladrModel findNode = FindNode(parentValue.CODE, InitialTreeItems) ?? throw new Exception();

        findNode.Children = ConvertRubrics(rubrics)!;
        //if ()
        //    findNode.Children.ForEach(r => { r.Expandable = false; });

        return findNode.Children;
    }

    async Task<List<ObjectKLADRModelDB>> RequestRubrics(string? parent_code = null)
    {
        await SetBusy();
        List<ObjectKLADRModelDB> rest = await KladrNavRepo.ObjectsList(new() { ParentCode = parent_code });

        rest = [.. rest.OrderBy(x => x.NAME)];

        return rest;
    }
}