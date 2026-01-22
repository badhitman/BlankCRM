////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Constructor.DirectoriesCatalog;

/// <summary>
/// ElementDirectoryFieldSet
/// </summary>
public partial class ElementDirectoryFieldSetComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required int SelectedDirectoryId { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public EntryStandardModel ElementObject { get; set; } = default!;

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public Action<int> DeleteElementOfDirectoryHandler { get; set; } = default!;

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required ElementsOfDirectoryListViewComponent ParentDirectoryElementsList { get; set; }

    /// <summary>
    /// Родительская страница форм
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required ConstructorMainManageComponent ParentFormsPage { get; set; }


    EntryDescriptionModel? ElementObjectOrign;
    EntryDescriptionModel? ElementObjectEdit;

    string images_upload_url = default!;
    Dictionary<string, object> editorConf = default!;

    /// <inheritdoc/>
    protected bool IsEdited => ElementObjectOrign is not null && !ElementObjectOrign.Equals(ElementObjectEdit);

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await ReadCurrentUser();

        images_upload_url = $"{GlobalStaticConstants.TinyMCEditorUploadImage}{GlobalStaticConstantsRoutes.Routes.CONSTRUCTOR_CONTROLLER_NAME}/{GlobalStaticConstantsRoutes.Routes.DIRECTORY_CONTROLLER_NAME}?{nameof(StorageMetadataModel.PrefixPropertyName)}={GlobalStaticConstantsRoutes.Routes.SET_ACTION_NAME}&{nameof(StorageMetadataModel.OwnerPrimaryKey)}={SelectedDirectoryId}";
        editorConf = GlobalStaticConstants.TinyMCEditorConf(images_upload_url);
    }

    /// <inheritdoc/>
    protected async Task UpdateElementOfDirectory()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        ArgumentNullException.ThrowIfNull(ElementObjectEdit);

        await SetBusyAsync();

        ResponseBaseModel rest = await ConstructorRepo.UpdateElementOfDirectoryAsync(new() { Payload = ElementObjectEdit, SenderActionUserId = CurrentUserSession.UserId });
        await SetBusyAsync(false);

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
            return;

        ElementObjectOrign = GlobalTools.CreateDeepCopy(ElementObjectEdit);

        IsEdit = false;
        await ParentDirectoryElementsList.ReloadElements(null, true);
        StateHasChanged();

        if (!rest.Success())
        {
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
        }
    }


    bool IsEdit = false;
    async Task EditToggle()
    {
        if (IsEdit)
        {
            ElementObjectOrign = null;
            ElementObjectEdit = null;
            IsEdit = false;
            return;
        }

        await SetBusyAsync();

        TResponseModel<EntryDescriptionModel> res = await ConstructorRepo.GetElementOfDirectoryAsync(ElementObject.Id);
        ElementObjectOrign = res.Response ?? throw new Exception();
        ElementObjectEdit = GlobalTools.CreateDeepCopy(ElementObjectOrign);
        IsEdit = true;
        await SetBusyAsync(false);
    }

    void RsetEdit()
    {
        ElementObjectEdit = GlobalTools.CreateDeepCopy(ElementObjectOrign);
    }

    /// <summary>
    /// Is the element in its topmost (extreme) position?
    /// </summary>
    bool IsMostUp
    {
        get
        {
            if (ParentDirectoryElementsList.EntriesElements is null)
                throw new Exception("Элементы справочника IsNull");

            return ParentDirectoryElementsList
                .EntriesElements
                .FindIndex(x => x.Id == ElementObject.Id) == 0;
        }
    }
    /// <summary>
    /// Is the element in its lowest (extreme) position?
    /// </summary>
    bool IsMostDown
    {
        get
        {
            if (ParentDirectoryElementsList.EntriesElements is null)
                throw new Exception("IsNull Directory Elements");

            return ParentDirectoryElementsList
                .EntriesElements
                .FindIndex(x => x.Id == ElementObject.Id) == ParentDirectoryElementsList.EntriesElements.Count - 1;
        }
    }

    async Task MoveUp()
    {
        if (IsMostUp)
            return;

        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        IsEdit = false;
        await SetBusyAsync();

        ResponseBaseModel rest = await ConstructorRepo.UpMoveElementOfDirectoryAsync(new() { Payload = ElementObject.Id, SenderActionUserId = CurrentUserSession.UserId });
        await SetBusyAsync(false);
        if (!rest.Success())
        {
            SnackBarRepo.ShowMessagesResponse(rest.Messages);
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
        }
        await ParentDirectoryElementsList.ReloadElements(null, true);
    }

    async Task MoveDown()
    {
        if (IsMostDown)
            return;

        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        IsEdit = false;


        ResponseBaseModel rest = await ConstructorRepo.DownMoveElementOfDirectoryAsync(new() { Payload = ElementObject.Id, SenderActionUserId = CurrentUserSession.UserId });
        await SetBusyAsync(false);
        if (!rest.Success())
        {
            SnackBarRepo.ShowMessagesResponse(rest.Messages);
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
        }
        await ParentDirectoryElementsList.ReloadElements(null, true);
        await SetBusyAsync();
    }

    /// <inheritdoc/>
    protected async Task DeleteElementOfDirectory()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        await SetBusyAsync();

        ResponseBaseModel rest = await ConstructorRepo.DeleteElementFromDirectoryAsync(new()
        {
            Payload = new()
            {
                DeleteElementFromDirectoryId = ElementObject.Id
            },
            SenderActionUserId = CurrentUserSession.UserId
        });

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
        }
        await ConstructorRepo.CheckAndNormalizeSortIndexForElementsOfDirectoryAsync(SelectedDirectoryId);
        await ParentDirectoryElementsList.ReloadElements(null, true);
        await SetBusyAsync(false);
    }
}