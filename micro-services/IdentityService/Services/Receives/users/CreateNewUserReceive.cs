////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Net.Mail;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Регистрация нового email/пользователя без пароля (Identity)
/// </summary>
public class CreateNewUserReceive(IIdentityTools idRepo, ITracesIndexing indexingRepo)
    : IResponseReceive<string?, RegistrationNewUserResponseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RegistrationNewUserReceive;

    /// <inheritdoc/>
    public async Task<RegistrationNewUserResponseModel?> ResponseHandleActionAsync(string? req, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(req) || !MailAddress.TryCreate(req, out _))
            throw new ArgumentNullException(nameof(req));

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, null, req);
        RegistrationNewUserResponseModel res = await idRepo.CreateNewUserEmailAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}