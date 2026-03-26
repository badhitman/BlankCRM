////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Options;
using BlankBlazorApp.Properties;
using System.Text;
using SharedLib;

/// <summary>
/// 
/// </summary>
public partial class FirebaseMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    /// <inheritdoc/>
    public async Task Invoke(HttpContext http_context, IOptions<FirebaseSDKConfigModel> fireOpt)
    {
        http_context.Response.Headers.Append($"Content-type", "text/javascript; charset=UTF-8");
        string _raw = "";
        switch (http_context.Request.PathBase)
        {
            case "/firebase-messaging-sw.js":
                _raw = Resources.firebase_messaging_sw;
                break;
            case "/FirebaseSDK.js":
                _raw = Resources.FirebaseSDK;
                break;
            default:
                return;
        }// 

        _raw = _raw
            .Replace("**measurementId**", fireOpt.Value.MeasurementId)
            .Replace("**appId**", fireOpt.Value.AppId)
            .Replace("**messagingSenderId**", fireOpt.Value.MessagingSenderId)
            .Replace("**storageBucket**", fireOpt.Value.StorageBucket)
            .Replace("**projectId**", fireOpt.Value.ProjectId)
            .Replace("**databaseURL**", fireOpt.Value.DatabaseURL)
            .Replace("**authDomain**", fireOpt.Value.AuthDomain)
            .Replace("**apiKey**", fireOpt.Value.ApiKey)
            ;

        byte[] _bytes = Encoding.UTF8.GetBytes(_raw);
        http_context.Response.Headers.Append($"Content-Length", _bytes.Length.ToString());
        await http_context.Response.BodyWriter.WriteAsync(_bytes);

        await _next.Invoke(http_context);
    }
}