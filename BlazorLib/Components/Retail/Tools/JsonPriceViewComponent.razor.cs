////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using SharedLib;

namespace BlazorLib.Components.Retail.Tools;

/// <summary>
/// JsonPriceViewComponent
/// </summary>
public partial class JsonPriceViewComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    string? sampleJson;
    List<NomenclatureScopeModel>? CatalogData;

    async Task WriteOffers()
    {
        if (CatalogData is null)
        {
            SnackBarRepo.Error("CatalogData is null");
            return;
        }

        await SetBusyAsync();
        ResponseBaseModel res = await CommerceRepo.UploadOffersAsync(CatalogData);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
    }

    void TryParseJson()
    {
        if (string.IsNullOrWhiteSpace(sampleJson))
        {
            SnackBarRepo.Error("string.IsNullOrWhiteSpace(sampleJson)");
            return;
        }

        try
        {
            CatalogData = JsonConvert.DeserializeObject<List<NomenclatureScopeModel>>(sampleJson);
        }
        catch (Exception ex)
        {
            SnackBarRepo.Error(ex.Message);
        }
    }
}