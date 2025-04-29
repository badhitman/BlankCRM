////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;
using BlazorLib;
using MudBlazor;

namespace BlazorWebLib.Components.HelpDesk.issue;

/// <summary>
/// NotificationTelegramIssueConfigComponent
/// </summary>
public partial class NotificationTelegramIssueConfigComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IStorageTransmission StorageRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required string Title { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required string Hint { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required List<ChatTelegramModelDB> ChatsTelegram { get; set; }

    /// <summary>
    /// Имя приложения, которое обращается к службе облачного хранения параметров
    /// </summary>
    [Parameter, EditorRequired]
    public required StorageMetadataModel KeyStorage { get; set; }


    long initValue;

    bool IsEdited => initValue != SelectedChatSet.ChatTelegramId;

    private ChatTelegramModelDB SelectedChatSet { get; set; } = default!;

    async Task SaveConfig()
    {
        await SetBusyAsync();
        TResponseModel<int> rest = await StorageRepo.SaveParameterAsync(SelectedChatSet.ChatTelegramId, KeyStorage, false);
        SnackbarRepo.ShowMessagesResponse(rest.Messages);
        IsBusyProgress = false;
        initValue = SelectedChatSet.ChatTelegramId;
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        TResponseModel<long?> rest = await StorageRepo.ReadParameterAsync<long?>(KeyStorage);
        IsBusyProgress = false;
        SnackbarRepo.ShowMessagesResponse(rest.Messages);
        initValue = rest.Response ?? 0;
        SelectedChatSet = ChatsTelegram.First(x => x.ChatTelegramId == initValue);
    }
}