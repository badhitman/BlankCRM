////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace BlankBlazorApp.Controllers;

/// <summary>
/// CultureController
/// </summary>
[Route("[controller]/[action]")]
[AllowAnonymous]
public class CultureController : Controller
{
    /// <summary>
    /// Set Culture
    /// </summary>
    public IActionResult Set(string culture, string redirectUri)
    {
        if (culture != null)
        {
            RequestCulture requestCulture = new(culture, culture);
            string cookieName = CookieRequestCultureProvider.DefaultCookieName;
            string cookieValue = CookieRequestCultureProvider.MakeCookieValue(requestCulture);

            HttpContext.Response.Cookies.Append(cookieName, cookieValue);
        }

        return LocalRedirect(redirectUri);
    }
}