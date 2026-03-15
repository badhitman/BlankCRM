using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
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
    IOptions<TelegramBotConfigModel> TGConfig { get; set; } = default!;

    [Inject]
    IOptions<ServerConfigModel> WebConfig { get; set; } = default!;

    [Inject]
    NavigationManager NavigatorRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required NavMainMenuModel NavMainMenu { get; set; }


    static bool _isLoaded = false;
    static bool _includeTelegramBotWebAppScript = false;
    Uri? _uri;

    bool FilterLocalPathsForInject(string[]? localPathsForInject)
    {
        if (localPathsForInject is null || localPathsForInject.Length == 0 || _uri is null)
            return true;

        return localPathsForInject.Any(y => y.Equals(_uri.LocalPath, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        _uri = new(NavigatorRepo.Uri);

        if (TGConfig.Value.BaseUri is null)
            TGConfig.Value.BaseUri = NavigatorRepo.BaseUri;

        if (!_isLoaded)
        {
            List<Task> _tasks = [Task.Run(async () => {
                TResponseModel<bool> tgWebAppInclude = await StoreRepo.ReadParameterAsync<bool>(GlobalStaticCloudStorageMetadata.ParameterIncludeTelegramBotWebApp);
                _includeTelegramBotWebAppScript = tgWebAppInclude.Success() && tgWebAppInclude.Response == true;
            })];
            _isLoaded = true;
            if (ServiceProviderExtensions.SetRemoteConf?.Success() != true)
            {
                _tasks.AddRange([
                    TgRemoteCall.SetWebConfigTelegramAsync(TGConfig.Value, false),
                    TgRemoteCall.SetWebConfigHelpDeskAsync(TGConfig.Value, false),
                    TgRemoteCall.SetWebConfigStorageAsync(TGConfig.Value, false)]);

                await Task.WhenAll(_tasks);
            }
        }
        await StoreRepo.SaveParameterAsync(WebConfig.Value.IsDarkMode, GlobalStaticCloudStorageMetadata.ThemeMode, true);
    }
}