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
        ResponseBaseModel res1 = default!, res2 = default!;
        await Task.WhenAll([Task.Run(async () => { res1 = await kladrRepo.UploadPartTempKladr(req); }), Task.Run(async () => { res2 = await kladrRepo.RegisterJobTempKladr(new RegisterJobTempKladrRequestModel() { TableName = req.TableName, VoteVal = 1 }); })]);
        return ResponseBaseModel.Create(res1.Messages.Union(res2.Messages));
    }

    /// <inheritdoc/>
    [HttpPut($"/{Routes.API_CONTROLLER_NAME}/{Routes.KLADR_CONTROLLER_NAME}/{Routes.TEMP_CONTROLLER_NAME}-{Routes.JOB_CONTROLLER_NAME}/{Routes.VOTE_ACTION_NAME}-{Routes.REGISTRATION_ACTION_NAME}"), LoggerNolog]
    public async Task<ResponseBaseModel> RegisterJobTempKladr(RegisterJobTempKladrRequestModel req)
    {
        return await kladrRepo.RegisterJobTempKladr(req);
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