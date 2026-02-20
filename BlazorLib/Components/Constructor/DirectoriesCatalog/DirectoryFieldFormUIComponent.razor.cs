////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Constructor.DirectoriesCatalog;

/// <summary>
/// Directory field form UI
/// </summary>
public partial class DirectoryFieldFormUIComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required FieldFormAkaDirectoryConstructorModelDB FieldObject { get; set; }

    /// <inheritdoc/>
    [CascadingParameter]
    public Action<FieldFormBaseLowConstructorModel, Type> StateHasChangedHandler { get; set; } = default!;

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required FormConstructorModelDB Form { get; set; }


    /// <inheritdoc/>
    protected IEnumerable<EntryStandardModel> Entries = [];
    /// <inheritdoc/>
    public int SelectedDirectoryField
    {
        get => FieldObject.DirectoryId;
        private set
        {
            FieldObject.DirectoryId = value;
            StateHasChangedHandler(FieldObject, GetType());
        }
    }

    /// <summary>
    /// Мульти-выбор
    /// </summary>
    public bool IsMultiCheckBox
    {
        get => FieldObject.IsMultiSelect;
        set
        {
            FieldObject.IsMultiSelect = value;
            StateHasChangedHandler(FieldObject, GetType());
        }
    }

    /// <inheritdoc/>
    public void Update(FieldFormBaseLowConstructorModel field)
    {
        FieldObject.Update(field);
        StateHasChanged();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        
        TResponseModel<EntryStandardModel[]> rest = await ConstructorRepo.GetDirectoriesAsync(new() { ProjectId = Form.ProjectId });
        await SetBusyAsync(false);
        StateHasChangedHandler(FieldObject, GetType());

        if (rest.Response is null)
            throw new Exception();
        Entries = rest.Response;
    }
}