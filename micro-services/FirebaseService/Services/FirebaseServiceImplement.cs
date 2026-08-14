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
    public async Task<TResponseModel<SendFirebaseMessageResultModel>> SendFirebaseNotificationAsync(TAuthRequestStandardModel<SendFirebaseMessageRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "req.Payload is null" }] };

        if (!req.Payload.IsValid)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "!req.Payload.IsValid" }] };

        TResponseModel<SendFirebaseMessageResultModel> res = new()
        {
            Response = new()
        };

        MulticastMessage messages = new()
        {
            Fids = req.Payload.FidsFCM,
            Data = req.Payload.Data,
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
                    Direction = Direction.Auto,
                }
            }
        };
        if (!string.IsNullOrWhiteSpace(req.Payload.ImageUrl))
        {
            messages.Notification.ImageUrl = req.Payload.ImageUrl;
            messages.Webpush.Notification.Image = req.Payload.ImageUrl;
        }
        if (!string.IsNullOrWhiteSpace(req.Payload.LinkURL))
            messages.Webpush.FcmOptions = new() { Link = req.Payload.LinkURL };

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
                res.Response.SuccessfulMessagesIds.Add(req.Payload.FidsFCM[i]);
        }

        if (res.Response.SuccessfulMessagesIds.Count == req.Payload.FidsFCM.Count)
            res.AddSuccess("Отправка успешно выполнена");

        return res;
    }
}