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
    public async Task<TResponseModel<SendFirebaseMessageResultModel>> SendFirebaseMessageAsync(TAuthRequestStandardModel<SendFirebaseMessageRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "req.Payload is null" }] };

        if (!req.Payload.IsValid)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "!req.Payload.IsValid" }] };

        TResponseModel<SendFirebaseMessageResultModel> res = new();
        res.Response = new();
        if (req.Payload.TokensFCM.Count == 1)
        {
            Message message = new()
            {
                Data = req.Payload.Data,
                Token = req.Payload.TokensFCM[0],
                Notification = new()
                {
                    Title = req.Payload.Title,
                    Body = req.Payload.TextBody,
                },
                Webpush = new()
                {
                    Data = req.Payload.Data,
                    Notification = new()
                    {
                        Title = req.Payload.Title,
                        Body = req.Payload.TextBody,
                    },
                }
            };
            if (!string.IsNullOrWhiteSpace(req.Payload.ImageUrl))
            {
                message.Notification.ImageUrl = req.Payload.ImageUrl;
                message.Webpush.Notification.Image = req.Payload.ImageUrl;
            }

            string responseSimple = await FirebaseMessaging.DefaultInstance.SendAsync(message, token);
            if (!string.IsNullOrWhiteSpace(responseSimple))
                res.Response.SuccessfulMessagesIds = [responseSimple];
            else
                res.AddError($"Не удалось отправить сообщение токену/клиенту {req.Payload.TokensFCM[0]}");

            return res;
        }

        MulticastMessage messages = new()
        {
            Tokens = req.Payload.TokensFCM,
            Data = req.Payload.Data,
            Notification = new()
            {
                Title = req.Payload.Title,
                Body = req.Payload.TextBody,
            },
            Webpush = new()
            {
                Notification = new()
                {
                    Title = req.Payload.Title,
                    Body = req.Payload.TextBody,
                    Data = req.Payload.Data,
                }
            }
        };
        if (!string.IsNullOrWhiteSpace(req.Payload.ImageUrl))
        {
            messages.Notification.ImageUrl = req.Payload.ImageUrl;
            messages.Webpush.Notification.Image = req.Payload.ImageUrl;
        }

        BatchResponse response;
        res.Response.SuccessfulMessagesIds = [];
        try
        {
            response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(messages, token);
        }
        catch (Exception ex)
        {
            res.Messages.InjectException(ex);
            return res;
        }

        for (int i = 0; i < response.Responses.Count; i++)
        {
            if (response.Responses[i].IsSuccess)
                res.Response.SuccessfulMessagesIds.Add(req.Payload.TokensFCM[i]);
        }

        return res;
    }
}