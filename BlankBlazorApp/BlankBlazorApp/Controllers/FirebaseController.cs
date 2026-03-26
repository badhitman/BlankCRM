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
    public async Task<IActionResult> OnBackgroundMessage([FromForm] JObject msg)
    {
        string? ticket_session = this.Request.Cookies.FirstOrDefault(x => x.Key == "ticket_session").Value;
        loggerRepo.LogInformation(JsonConvert.SerializeObject(msg));
        return Ok("Ok");
    }

    /// <inheritdoc/>
    [HttpPost($"/{Routes.FIREBASE_CONTROLLER_NAME}/onMessage")]
    public async Task<IActionResult> OnMessage([FromForm] JObject msg)
    {
        string? ticket_session = this.Request.Cookies.FirstOrDefault(x => x.Key == "ticket_session").Value;
        loggerRepo.LogInformation(JsonConvert.SerializeObject(msg));
        return Ok("Ok");
    }

    /// <inheritdoc/>
    [HttpPost($"/{Routes.FIREBASE_CONTROLLER_NAME}/FirebaseTokenHandle")]
    public async Task<IActionResult> FirebaseTokenHandle([FromForm] JObject tokenFirebase)
    {
        string? ticket_session = this.Request.Cookies.FirstOrDefault(x => x.Key == "ticket_session").Value;
        loggerRepo.LogInformation(JsonConvert.SerializeObject(tokenFirebase));
        return Ok("Ok");
    }
}