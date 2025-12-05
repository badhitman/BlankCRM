////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Constructor.DirectoriesCatalog;

/// <summary>
/// Directory elements-list view
/// </summary>
public partial class ElementsOfDirectoryListViewComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Inject]
    protected IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required int SelectedDirectoryId { get; set; }


    /// <inheritdoc/>
    public List<EntryModel>? EntriesElements { get; set; }

    /// <inheritdoc/>
    public async Task ReloadElements(int? selected_directory_id = 0, bool state_has_change = false)
    {
        if (selected_directory_id is not null)
            SelectedDirectoryId = selected_directory_id.Value;

        if (SelectedDirectoryId <= 0)
        {
            EntriesElements = null;
            return;
        }

        await SetBusyAsync();
        
        TResponseModel<List<EntryModel>> rest = await ConstructorRepo.GetElementsOfDirectoryAsync(SelectedDirectoryId);
        
        if (!rest.Success())
            SnackBarRepo.ShowMessagesResponse(rest.Messages);

        EntriesElements = rest.Response;
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        if (SelectedDirectoryId > 0)
            await ReloadElements();
    }
}