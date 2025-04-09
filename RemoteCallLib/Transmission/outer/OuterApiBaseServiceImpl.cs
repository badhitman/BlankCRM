////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Amazon.Runtime;
using SharedLib;
using System.Net.Http;

namespace RemoteCallLib;

/// <summary>
/// OuterApiBaseServiceImpl
/// </summary>
public abstract class OuterApiBaseServiceImpl(IHttpClientFactory HttpClientFactory) : IOuterApiService
{
    /// <summary>
    /// Шаблон имени очереди MQ
    /// </summary>
    public abstract string NameTemplateMQ { get; }

    /// <inheritdoc/>
    public abstract Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default);

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RabbitMqManagementResponseModel>>> HealthCheckAsync(CancellationToken token = default)
    {
        using HttpClient client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.RabbitMqManagement.ToString());
        TResponseModel<List<RabbitMqManagementResponseModel>> resMq = await client.GetStringAsync<List<RabbitMqManagementResponseModel>>($"api/queues", cancellationToken: token);

        if (resMq.Response is not null && resMq.Response.Count != 0)
            resMq.Response.RemoveAll(x => x.name?.StartsWith(NameTemplateMQ) != true);

        return resMq;
    }
}