////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib;
using BlazorLib.Components;
using Microsoft.AspNetCore.Components;
using SharedLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace BlazorWebLib.Components.HelpDesk;

/// <summary>
/// ConsoleHelpDeskComponent
/// </summary>
public partial class ConsoleHelpDeskComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IParametersStorageTransmission StorageRepo { get; set; } = default!;


    readonly List<StatusesDocumentsEnum> Steps = [.. Enum.GetValues<StatusesDocumentsEnum>()];
    byte stepNum;
    bool IsLarge;
    string? FilterUserId;
    UserSelectInputComponent? selectorInputRef;


    async void SelectUserHandler(UserInfoModel? selected)
    {
        if (string.IsNullOrWhiteSpace(FilterUserId) && string.IsNullOrWhiteSpace(selected?.UserId) || FilterUserId == selected?.UserId)
            return;

        FilterUserId = selected?.UserId;
        stepNum = 0;

        if (CurrentUserSession is not null)
        {
            await SetBusyAsync();
            TResponseModel<int> res = await StorageRepo.SaveParameterAsync(FilterUserId, GlobalStaticCloudStorageMetadata.ConsoleFilterForUser(CurrentUserSession.UserId), false);
            await SetBusyAsync(false);
            if (!res.Success())
                SnackBarRepo.ShowMessagesResponse(res.Messages);
        }
    }

    StorageMetadataModel SizeColumnsKeyStorage => new()
    {
        ApplicationName = Path.Combine(Routes.CONSOLE_CONTROLLER_NAME, Routes.HELPDESK_CONTROLLER_NAME),
        PropertyName = Routes.SIZE_CONTROLLER_NAME,
        PrefixPropertyName = CurrentUserSession?.UserId,
    };

    async Task ToggleSize()
    {
        IsLarge = !IsLarge;
        stepNum = 0;

        await SetBusyAsync();

        TResponseModel<int> res = await StorageRepo.SaveParameterAsync(IsLarge, SizeColumnsKeyStorage, true);
        IsBusyProgress = false;
        if (!res.Success())
            SnackBarRepo.ShowMessagesResponse(res.Messages);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        if (selectorInputRef is not null)
            selectorInputRef.StateHasChangedCall();

        TResponseModel<bool> res = await StorageRepo.ReadParameterAsync<bool>(SizeColumnsKeyStorage);
        IsLarge = res.Response == true;

        TResponseModel<string?> current_filter_user_res = await StorageRepo.ReadParameterAsync<string>(GlobalStaticCloudStorageMetadata.ConsoleFilterForUser(CurrentUserSession.UserId));
        FilterUserId = current_filter_user_res.Response;

        IsBusyProgress = false;
        if (!res.Success())
            SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (!current_filter_user_res.Success())
            SnackBarRepo.ShowMessagesResponse(current_filter_user_res.Messages);
    }
}