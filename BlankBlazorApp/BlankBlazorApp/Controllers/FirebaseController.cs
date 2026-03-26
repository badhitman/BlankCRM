////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsRoutes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BlankBlazorApp.Controllers;

/// <summary>
/// FirebaseController
/// </summary>
[Route("[controller]/[action]"), ApiController]
[AllowAnonymous]
public class FirebaseController(ILogger<FirebaseController> loggerRepo) : ControllerBase
{
    /// <inheritdoc/>
    [HttpPost($"/{Routes.FIREBASE_CONTROLLER_NAME}/onBackgroundMessage")]
    public async Task<IActionResult> OnBackgroundMessage([FromBody] JObject msg)
    {
        loggerRepo.LogInformation(JsonConvert.SerializeObject(msg));
        return Ok("Ok");
    }

    /// <inheritdoc/>
    [HttpPost($"/{Routes.FIREBASE_CONTROLLER_NAME}/onMessage")]
    public async Task<IActionResult> onMessage([FromBody] JObject msg)
    {
        loggerRepo.LogInformation(JsonConvert.SerializeObject(msg));
        return Ok("Ok");
    }

    /// <inheritdoc/>
    [HttpPost($"/{Routes.FIREBASE_CONTROLLER_NAME}/FirebaseTokenHandle")]
    public async Task<IActionResult> FirebaseTokenHandle([FromBody] JObject token)
    {
        loggerRepo.LogInformation(JsonConvert.SerializeObject(token));
        return Ok("Ok");
    }
}