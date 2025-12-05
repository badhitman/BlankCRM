////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Constructor;

/// <summary>
/// ConstructorMainManageComponent
/// </summary>
public partial class ConstructorMainManageComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <inheritdoc/>
    public List<SystemNameEntryModel>? SystemNamesManufacture;

    /// <inheritdoc/>
    public MainProjectViewModel? MainProject { get; private set; }

    /// <summary>
    /// Проверка разрешения редактировать проект
    /// </summary>
    public bool CanEditProject { get; private set; }


    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await ReadCurrentUser();
        await ReadCurrentMainProject();
        await SetBusyAsync(false);
    }

    /// <summary>
    /// Прочитать данные о текущем/основном проекте
    /// </summary>
    public async Task ReadCurrentMainProject()
    {
        CanEditProject = false;

        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        await SetBusyAsync();

        TResponseModel<MainProjectViewModel> currentMainProject = await ConstructorRepo.GetCurrentMainProjectAsync(CurrentUserSession.UserId);

        if (!currentMainProject.Success())
            SnackBarRepo.ShowMessagesResponse(currentMainProject.Messages);

        MainProject = currentMainProject.Response;
        CanEditProject = MainProject is not null && (!MainProject.IsDisabled || MainProject.OwnerUserId.Equals(CurrentUserSession.UserId) || CurrentUserSession.IsAdmin);
        await SetBusyAsync(false);
    }
}