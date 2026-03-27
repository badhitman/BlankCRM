////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using FirebaseAdmin.Messaging;
using SharedLib;

namespace FirebaseService;

/// <summary>
/// FirebaseService
/// </summary>
public class FirebaseServiceImplement() : IFirebaseService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<List<string>>> SendFirebaseMessageAsync(TAuthRequestStandardModel<SendFirebaseMessageRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "req.Payload is null" }] };

        if (!req.Payload.IsValid)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "!req.Payload.IsValid" }] };

        // See documentation on defining a message payload.
        MulticastMessage message = new()
        {
            Tokens = req.Payload.TokensFCM,
            Data = req.Payload.Data,
            Notification = new()
            {
                Title = req.Payload.Title,
                Body = req.Payload.TextBody,
                ImageUrl = req.Payload.ImageUrl,
            },
        };
        TResponseModel<List<string>> res = new();
        BatchResponse response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message, token);
        if (response.FailureCount > 0)
        {
            res.Response = [];
            for (int i = 0; i < response.Responses.Count; i++)
            {
                if (!response.Responses[i].IsSuccess)
                    res.Response.Add(req.Payload.TokensFCM[i]);
            }
        }

        return res;
    }
}