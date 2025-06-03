////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text;
using SharedLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace ToolsMauiLib;

/// <summary>
/// ToolsSystemHTTPRestService
/// </summary>
public class ToolsSystemHTTPRestService(ApiRestConfigModelDB ApiConnect, IHttpClientFactory HttpClientFactory) : IClientRestToolsService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<ExpressProfileResponseModel>> GetMeAsync(CancellationToken cancellationToken = default)
    {
        using HttpClient client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Kladr.ToString());

        TResponseModel<ExpressProfileResponseModel> res = await client.GetStringAsync<ExpressProfileResponseModel>($"{ApiConnect.AddressBaseUri.NormalizedUriEnd()}{Routes.API_CONTROLLER_NAME}/{Routes.INFO_CONTROLLER_NAME}/{Routes.MY_CONTROLLER_NAME}", cancellationToken: cancellationToken);

        if (string.IsNullOrWhiteSpace(res.Response?.UserName))
            res.AddError("Пользователь не настроен");
        else if (res.Response.Roles is null || !res.Response.Roles.Any())
            res.AddWarning("Не установлены роли доступа");
        else
            res.AddSuccess("Токен доступа валидный");

        return res;
    }

    #region files (ext)
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FilePartUploadAsync(SessionFileRequestModel req, CancellationToken cancellationToken = default)
    {
        using HttpClient client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Kladr.ToString());

        MultipartFormDataContent form = new()
        {
            { new ByteArrayContent(req.Data, 0, req.Data.Length), "uploadedFile", Path.GetFileName(req.FileName) }
        };

        string routeUri = $"{ApiConnect.AddressBaseUri.NormalizedUriEnd()}{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.PART_CONTROLLER_NAME}-{Routes.UPLOAD_ACTION_NAME}";

        routeUri += $"?{Routes.SESSION_CONTROLLER_NAME}_{Routes.TOKEN_CONTROLLER_NAME}={Convert.ToBase64String(Encoding.UTF8.GetBytes(req.SessionId))}";
        routeUri += $"&{Routes.FILE_CONTROLLER_NAME}_{Routes.TOKEN_CONTROLLER_NAME}={Convert.ToBase64String(Encoding.UTF8.GetBytes(req.FileId))}";

        HttpResponseMessage response = await client.PostAsync(routeUri, form, cancellationToken);

        response.EnsureSuccessStatusCode();
        string rj = await response.Content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<ResponseBaseModel>(rj)!;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<PartUploadSessionModel>> FilePartUploadSessionStartAsync(PartUploadSessionStartRequestModel req, CancellationToken cancellationToken = default)
    {
        using HttpClient client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Kladr.ToString());
        using HttpResponseMessage response = await client.PostAsJsonAsync($"{ApiConnect.AddressBaseUri.NormalizedUriEnd()}{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.SESSION_CONTROLLER_NAME}-{Routes.PART_CONTROLLER_NAME}-{Routes.UPLOAD_ACTION_NAME}-{Routes.START_ACTION_NAME}", req, cancellationToken: cancellationToken);
        string rj = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<TResponseModel<PartUploadSessionModel>>(rj)!;
    }
    #endregion

    #region files (base)
    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> DeleteFileAsync(DeleteRemoteFileRequestModel req, CancellationToken cancellationToken = default)
    {
        using HttpClient client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Kladr.ToString());
        using HttpResponseMessage response = await client.PostAsJsonAsync($"{ApiConnect.AddressBaseUri.NormalizedUriEnd()}{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.FILE_CONTROLLER_NAME}-{Routes.DELETE_ACTION_NAME}", req, cancellationToken: cancellationToken);
        string rj = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<TResponseModel<bool>>(rj)!;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<string>> ExeCommandAsync(ExeCommandModelDB req, CancellationToken cancellationToken = default)
    {
        using HttpClient client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Kladr.ToString());
        using HttpResponseMessage response = await client.PostAsJsonAsync($"{ApiConnect.AddressBaseUri.NormalizedUriEnd()}{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.CMD_CONTROLLER_NAME}/{Routes.EXE_ACTION_NAME}", req, cancellationToken: cancellationToken);
        string rj = await response.Content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<TResponseModel<string>>(rj)!;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<ToolsFilesResponseModel>>> GetDirectoryDataAsync(ToolsFilesRequestModel req, CancellationToken cancellationToken = default)
    {
        using HttpClient client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Kladr.ToString());
        using HttpResponseMessage response = await client.PostAsJsonAsync($"{ApiConnect.AddressBaseUri.NormalizedUriEnd()}{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.DIRECTORY_CONTROLLER_NAME}/{Routes.GET_ACTION_NAME}-{Routes.DATA_ACTION_NAME}", req, cancellationToken: cancellationToken);
        string rj = await response.Content.ReadAsStringAsync(cancellationToken);
        TResponseModel<List<ToolsFilesResponseModel>> res = JsonConvert.DeserializeObject<TResponseModel<List<ToolsFilesResponseModel>>>(rj)!;

        if (res.Response is null)
            res.AddError("Ошибка обработки запроса");
        else if (res.Response.Count == 0)
            res.AddInfo($"Файлов в удалённой папке нет");
        else
        {
            res.AddInfo($"Файлов в удалённой папке: {res.Response.Count} ({GlobalToolsStandard.SizeDataAsString(res.Response.Sum(x => x.Size))})");
            res.Response.ForEach(x =>
            {
                x.FullName = x.FullName.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
                x.ScopeName = x.ScopeName.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            });
        }
        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DirectoryExistAsync(string remoteDirectory, CancellationToken cancellationToken = default)
    {
        using HttpClient client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Kladr.ToString());
        string q = $"{ApiConnect.AddressBaseUri.NormalizedUriEnd()}{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.DIRECTORY_CONTROLLER_NAME}/{Routes.CHECK_ACTION_NAME}";
        using HttpResponseMessage response = await client.PostAsJsonAsync(q, remoteDirectory, cancellationToken: cancellationToken);
        string rj = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<ResponseBaseModel>(rj)!;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<string>> UpdateFileAsync(string fileScopeName, string remoteDirectory, byte[] bytes, CancellationToken cancellationToken = default)
    {
        TResponseModel<string> res = new();
        if (string.IsNullOrWhiteSpace(remoteDirectory))
        {
            res.AddError($"Установите обязательный параметр: {nameof(remoteDirectory)}");
            return res;
        }

        using HttpClient client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Kladr.ToString());
        MultipartFormDataContent form = new()
        {
            { new ByteArrayContent(bytes, 0, bytes.Length), "uploadedFile", fileScopeName }
        };

        string routeUri = $"{ApiConnect.AddressBaseUri.NormalizedUriEnd()}{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.FILE_CONTROLLER_NAME}-{Routes.UPDATE_ACTION_NAME}";
        routeUri += $"?{Routes.REMOTE_CONTROLLER_NAME}_{Routes.DIRECTORY_CONTROLLER_NAME}={Convert.ToBase64String(Encoding.UTF8.GetBytes(remoteDirectory))}";

        HttpResponseMessage response = await client.PostAsync(routeUri, form, cancellationToken);

        response.EnsureSuccessStatusCode();

        string sd = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<TResponseModel<string>>(sd)!;
    }
    #endregion

    #region КЛАДР 4.0
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClearTempKladrAsync(CancellationToken cancellationToken = default)
    {
        using HttpClient client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Kladr.ToString());

        string routeUri = $"{Routes.API_CONTROLLER_NAME}/{Routes.KLADR_CONTROLLER_NAME}/{Routes.TEMP_CONTROLLER_NAME}-{Routes.CLEAR_ACTION_NAME}";

        HttpResponseMessage response = await client.DeleteAsync(routeUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        string sd = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<ResponseBaseModel>(sd)!;
    }

    /// <inheritdoc/>
    public async Task<MetadataKladrModel> GetMetadataKladrAsync(GetMetadataKladrRequestModel req, CancellationToken cancellationToken = default)
    {
        using HttpClient client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Kladr.ToString());
        string routeUri = $"/{Routes.API_CONTROLLER_NAME}/{Routes.KLADR_CONTROLLER_NAME}/{Routes.METADATA_CONTROLLER_NAME}/{Routes.CALCULATE_ACTION_NAME}";

        HttpResponseMessage response = await client.PostAsJsonAsync(routeUri, req, cancellationToken: cancellationToken);
        response.EnsureSuccessStatusCode();

        string sd = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<MetadataKladrModel>(sd)!;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UploadPartTempKladrAsync(UploadPartTableDataModel req, CancellationToken cancellationToken = default)
    {
        using HttpClient client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Kladr.ToString());
        string routeUri = $"/{Routes.API_CONTROLLER_NAME}/{Routes.KLADR_CONTROLLER_NAME}/{Routes.TEMP_CONTROLLER_NAME}/{Routes.UPLOAD_ACTION_NAME}-{Routes.PART_CONTROLLER_NAME}";

        HttpResponseMessage response = await client.PostAsJsonAsync(routeUri, req, cancellationToken: cancellationToken);
        response.EnsureSuccessStatusCode();

        string sd = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<ResponseBaseModel>(sd)!;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FlushTempKladrAsync(CancellationToken cancellationToken = default)
    {
        using HttpClient client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Kladr.ToString());

        string routeUri = $"{Routes.API_CONTROLLER_NAME}/{Routes.KLADR_CONTROLLER_NAME}/{Routes.TEMP_CONTROLLER_NAME}-{Routes.FLUSH_ACTION_NAME}";

        HttpResponseMessage response = await client.PutAsync(routeUri, null, cancellationToken);
        response.EnsureSuccessStatusCode();

        string sd = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<ResponseBaseModel>(sd)!;
    }
    #endregion
}