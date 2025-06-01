////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorWebLib.Components;

/// <summary>
/// TelegramConfigComponent
/// </summary>
public partial class TelegramConfigComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IParametersStorageTransmission SerializeStorageRepo { get; set; } = default!;


    bool _isCommandModeTelegramBot;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        TResponseModel<bool?> res_IsCommandModeTelegramBot = await SerializeStorageRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.ParameterIsCommandModeTelegramBot);
        IsBusyProgress = false;

        if (!res_IsCommandModeTelegramBot.Success())
            SnackbarRepo.ShowMessagesResponse(res_IsCommandModeTelegramBot.Messages);

        _isCommandModeTelegramBot = res_IsCommandModeTelegramBot.Response == true;
    }
}