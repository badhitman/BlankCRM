////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Constructor.DirectoriesCatalog;

/// <summary>
/// Directory view
/// </summary>
public partial class DirectoryViewComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <summary>
    /// Родительская страница форм
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required ConstructorMainManageComponent ParentFormsPage { get; set; }


    /// <inheritdoc/>
    protected ElementsOfDirectoryListViewComponent elementsListOfDirectoryView_ref = default!;

    /// <inheritdoc/>
    protected DirectoryNavComponent? directoryNav_ref;

    OwnedNameModel createNewElementForDict = OwnedNameModel.BuildEmpty(0);

    /// <inheritdoc/>
    protected async void AddElementIntoDirectory()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        if (createNewElementForDict.OwnerId < 1)
            throw new Exception("No directory/list selected");

        ValidateReportModel validate_obj = GlobalTools.ValidateObject(createNewElementForDict);
        if (!validate_obj.IsValid)
        {
            SnackBarRepo.Error(validate_obj.ValidationResults);
            return;
        }

        await SetBusyAsync();

        TResponseModel<int> rest = await ConstructorRepo.CreateElementForDirectoryAsync(new() { Payload = createNewElementForDict, SenderActionUserId = CurrentUserSession.UserId });
        createNewElementForDict = OwnedNameModel.BuildEmpty(createNewElementForDict.OwnerId);
        await SetBusyAsync(false);
        StateHasChanged();
        SnackBarRepo.ShowMessagesResponse(rest.Messages);

        if (directoryNav_ref is not null)
            await directoryNav_ref.SetBusyAsync();

        if (rest.Success())
            await elementsListOfDirectoryView_ref.ReloadElements(directoryNav_ref?.SelectedDirectoryId, true);
        else
        {
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
        }

        if (directoryNav_ref is not null)
            await directoryNav_ref.SetBusyAsync(false);

        await SetBusyAsync(false);
    }

    async void SelectedDirectoryChangeAction(int selectedDirectoryId)
    {
        createNewElementForDict = OwnedNameModel.BuildEmpty(selectedDirectoryId);
        await elementsListOfDirectoryView_ref.ReloadElements(selectedDirectoryId, true);
        StateHasChanged();
    }
}