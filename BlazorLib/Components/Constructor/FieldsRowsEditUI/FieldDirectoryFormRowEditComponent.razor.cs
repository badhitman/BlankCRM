////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Constructor.FieldsRowsEditUI;

/// <summary>
/// Field directory form row edit
/// </summary>
public partial class FieldDirectoryFormRowEditComponent : BlazorBusyComponentBaseModel, IDomBaseComponent
{
    /// <inheritdoc/>
    [Inject]
    protected IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required Action<FieldFormAkaDirectoryConstructorModelDB> StateHasChangedHandler { get; set; } 

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required FieldFormAkaDirectoryConstructorModelDB Field { get; set; }

    /// <summary>
    /// Форма
    /// </summary>
    [Parameter, EditorRequired]
    public required FormConstructorModelDB Form { get; set; }

    /// <summary>
    /// Форма
    /// </summary>
    [Parameter, EditorRequired]
    public required bool CanEdit { get; set; }


    /// <inheritdoc/>
    protected IEnumerable<EntryStandardModel>? Entries;

    /// <inheritdoc/>
    public string DomID => $"{Field.GetType().FullName}_{Field.Id}";


    /// <inheritdoc/>
    public int SelectedDirectoryField
    {
        get => Field.DirectoryId;
        private set
        {
            Field.DirectoryId = value;
            if (Field.Directory is not null)
                Field.Directory.Id = value;
            StateHasChangedHandler(Field);
        }
    }

    /// <inheritdoc/>
    public bool IsMultiDirectoryField
    {
        get => Field.IsMultiSelect;
        private set
        {
            Field.IsMultiSelect = value;
            StateHasChangedHandler(Field);
        }
    }

    /// <inheritdoc/>
    public void Update(FieldFormAkaDirectoryConstructorModelDB field)
    {
        Field.Update(field);
        StateHasChanged();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        TResponseModel<EntryStandardModel[]> rest = await ConstructorRepo.GetDirectoriesAsync(new() { ProjectId = Form.ProjectId });
        Entries = rest.Response ?? throw new Exception();
        await SetBusyAsync(false);
    }
}