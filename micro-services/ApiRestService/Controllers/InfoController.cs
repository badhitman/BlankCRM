////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SharedLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace ApiRestService.Controllers;

/// <summary>
/// Информация
/// </summary>
[Route("api/[controller]/[action]"), ApiController, ServiceFilter(typeof(UnhandledExceptionAttribute))]
#if !DEBUG
    [LoggerNolog]
#endif
public class InfoController : ControllerBase
{
    /// <summary>
    /// Получить информацию по текущему профилю (проверка токена доступа)
    /// </summary>
    /// <returns>Информация по текущему пользователю (имя и роли)</returns>
    [HttpGet($"/{Routes.API_CONTROLLER_NAME}/{Routes.INFO_CONTROLLER_NAME}/{Routes.MY_CONTROLLER_NAME}"), TypeFilter(typeof(RolesAuthorizationFilter))]
    public ExpressProfileResponseModel GetMyProfile([FromServices] ExpressUserPermissionModel userPerm)
    {
        ExpressProfileResponseModel res = new()
        {
            UserName = userPerm.User,
            Roles = userPerm.Roles?.Select(x => x.ToString()),
        };

        return res;
    }

    /// <summary>
    /// Получить все роли, существующие в системе (публичный доступ без токена)
    /// </summary>
    /// <returns>Все роли, которыми система оперирует 'в принципе'</returns>
    [HttpGet("/api/info/get-all-roles-names")]
    public IEnumerable<string?> GetAllRoles()
    {
        foreach (var item in Enum.GetValues(typeof(ExpressApiRolesEnum)))
        {
            yield return item.ToString();
        }
    }

    /// <summary>
    /// Ответ на запрос от редиректа Identity для входа
    /// </summary>
    /// <returns>Типовой ответ</returns>
    [HttpGet($"/api/values/{nameof(RedirectToLoginPath)}")]
    public ResponseBaseModel RedirectToLoginPath([FromQuery] string? ReturnUrl) => new() { Messages = [new() { Text = "Требуется пройти авторизацию", TypeMessage = MessagesTypesEnum.Error }] };

    /// <summary>
    /// Ответ на запрос от редиректа Identity для запрета доступа Identity
    /// </summary>
    /// <returns>Типовой ответ</returns>
    [HttpGet($"/api/values/{nameof(RedirectToAccessDeniedPath)}")]
    public ResponseBaseModel RedirectToAccessDeniedPath([FromQuery] string? ReturnUrl) => new() { Messages = [new() { Text = "Доступ запрещён", TypeMessage = MessagesTypesEnum.Error }] };
}