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
    IOptions<TelegramBotConfigModel> WebConfig { get; set; } = default!;

    [Inject]
    IOptions<ServerConfigModel> ServerConfig { get; set; } = default!;

    [Inject]
    NavigationManager NavigatorRepo { get; set; } = default!;


    static string? _headerHtmlDomInject;
    static bool _isLoaded = false;
    static bool _includeTelegramBotWeAppScript = false;
    Uri? _uri;

    bool FilterLocalPathsForInject(string[]? localPathsForInject)
    {
        if (localPathsForInject is null || localPathsForInject.Length == 0 || _uri is null)
            return true;

        return localPathsForInject.Any(y => y.Equals(_uri.LocalPath, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {//
        _uri = new(NavigatorRepo.Uri);

        if (WebConfig.Value.BaseUri is null)
            WebConfig.Value.BaseUri = NavigatorRepo.BaseUri;

        if (_headerHtmlDomInject is null)
        {
            TResponseModel<string?> resHeaderDom = await StoreRepo.ReadParameterAsync<string?>(GlobalStaticCloudStorageMetadata.HeaderHtmlDomInject);
            if (resHeaderDom.Success())
                _headerHtmlDomInject = resHeaderDom.Response;
        }

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