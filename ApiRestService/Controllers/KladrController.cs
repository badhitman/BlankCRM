////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstants;
using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ApiRestService.Controllers;

/// <summary>
/// Tools
/// </summary>
[TypeFilter(typeof(RolesAuthorizationFilter), Arguments = [$"{nameof(ExpressApiRolesEnum.SystemRoot)}"])]
[Route("api/[controller]/[action]"), ApiController, ServiceFilter(typeof(UnhandledExceptionAttribute))]
public class KladrController(IKladrService kladrRepo) : ControllerBase
{
    /// <inheritdoc/>
    [HttpPost($"/{Routes.API_CONTROLLER_NAME}/{Routes.KLADR_CONTROLLER_NAME}/{Routes.TEMP_CONTROLLER_NAME}/{Routes.UPLOAD_ACTION_NAME}-{Routes.PART_CONTROLLER_NAME}"), LoggerNolog]
    public async Task<ResponseBaseModel> UploadPartTempKladr(UploadPartTableDataModel req)
    {
        return await kladrRepo.UploadPartTempKladr(req);
    }

    /// <inheritdoc/>
    [HttpPost($"/{Routes.API_CONTROLLER_NAME}/{Routes.KLADR_CONTROLLER_NAME}/{Routes.METADATA_CONTROLLER_NAME}/{Routes.CALCULATE_ACTION_NAME}"), LoggerNolog]
    public async Task<MetadataKladrModel> GetMetadataKladr(GetMetadataKladrRequestModel req)
    {
        return await kladrRepo.GetMetadataKladr(req);
    }

    /// <inheritdoc/>
    [HttpDelete($"/{Routes.API_CONTROLLER_NAME}/{Routes.KLADR_CONTROLLER_NAME}/{Routes.TEMP_CONTROLLER_NAME}-{Routes.CLEAR_ACTION_NAME}"), LoggerNolog]
    public async Task<ResponseBaseModel> ClearTempKladr()
    {
        return await kladrRepo.ClearTempKladr();
    }

    /// <inheritdoc/>
    [HttpPut($"/{Routes.API_CONTROLLER_NAME}/{Routes.KLADR_CONTROLLER_NAME}/{Routes.TEMP_CONTROLLER_NAME}-{Routes.FLUSH_ACTION_NAME}")]
    public async Task<ResponseBaseModel> TransitTempKladr()
    {
        return await kladrRepo.FlushTempKladr();
    }
}