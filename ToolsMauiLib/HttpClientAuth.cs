////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace ToolsMauiApp;

public abstract class HttpClientAuth(ApiRestConfigModelDB _conf)
{
    protected HttpClient GetClient()
    {
        HttpClient _client = new() { BaseAddress = new(_conf.AddressBaseUri ?? "localhost") };
        _client.DefaultRequestHeaders.Add(_conf.HeaderName, _conf.TokenAccess);
        return _client;
    }
}
