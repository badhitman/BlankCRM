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
        return Enum.TryParse(req.TableName, true, out KladrFilesEnum currentKladrElement)
            ? await kladrRepo.UploadPartTempKladr(req)
            : ResponseBaseModel.CreateError($"Имя таблицы `{req.TableName}` не валидное. Разрешённые имена: {string.Join(", ", Enum.GetNames<KladrFilesEnum>().Select(x => $"{x}.dbf"))}");
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
}