////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Создает и отправляет токен подтверждения электронной почты для указанного пользователя.
/// </summary>
/// <remarks>
/// Этот API поддерживает инфраструктуру ASP.NET Core Identity и не предназначен для использования в качестве абстракции электронной почты общего назначения.
/// Он должен быть реализован в приложении, чтобы  Identity инфраструктура могла отправлять электронные письма с подтверждением.
/// </remarks>
public class GenerateEmailConfirmationIdentityReceive(IIdentityTools IdentityRepo, IHistoryIndexing indexingRepo)
    : IResponseReceive<SimpleUserIdentityModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GenerateEmailConfirmationIdentityReceive;

    /// <summary>
    /// Создает и отправляет токен подтверждения электронной почты для указанного пользователя.
    /// </summary>
    /// <remarks>
    /// Этот API поддерживает инфраструктуру ASP.NET Core Identity и не предназначен для использования в качестве абстракции электронной почты общего назначения.
    /// Он должен быть реализован в приложении, чтобы  Identity инфраструктура могла отправлять электронные письма с подтверждением.
    /// </remarks>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(SimpleUserIdentityModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, null, req);
        ResponseBaseModel res = await IdentityRepo.GenerateEmailConfirmationAsync(req, token);
        await indexingRepo.SaveHistoryForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}