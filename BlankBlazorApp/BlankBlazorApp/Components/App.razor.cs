using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using MudBlazor;
using SharedLib;

namespace BlankBlazorApp.Components;

/// <summary>
/// App
/// </summary>
public partial class App
{
    [Inject]
    IParametersStorageTransmission StoreRepo { get; set; } = default!;

    [Inject]
    ITelegramTransmission TgRemoteCall { get; set; } = default!;

    [Inject]
    IJSRuntime JS { get; set; } = default!;

    [Inject]
    IOptions<TelegramBotConfigModel> WebConfig { get; set; } = default!;

    [Inject]
    IOptions<ServerConfigModel> ServerConfig { get; set; } = default!;

    [Inject]
    NavigationManager NavigatorRepo { get; set; } = default!;


    static bool _isLoaded = false;
    static bool _includeTelegramBotWeAppScript = false;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        // await JS.InvokeVoidAsync("tryInitJivoSite");

        if (WebConfig.Value.BaseUri is null)
            WebConfig.Value.BaseUri = NavigatorRepo.BaseUri;

        if (!_isLoaded)
        {
            List<Task> _tasks = [Task.Run(async () => {
                TResponseModel<bool> tgWebAppInclude = await StoreRepo.ReadParameterAsync<bool>(GlobalStaticCloudStorageMetadata.ParameterIncludeTelegramBotWebApp);
                _includeTelegramBotWeAppScript = tgWebAppInclude.Success() && tgWebAppInclude.Response == true;
            })];
            _isLoaded = true;
            if (ServiceProviderExtensions.SetRemoteConf?.Success() != true)
            {
                _tasks.AddRange([
                    TgRemoteCall.SetWebConfigTelegramAsync(WebConfig.Value, false),
                    TgRemoteCall.SetWebConfigHelpDeskAsync(WebConfig.Value, false),
                    TgRemoteCall.SetWebConfigStorageAsync(WebConfig.Value, false)]);

                await Task.WhenAll(_tasks);
            }
        }
    }
}