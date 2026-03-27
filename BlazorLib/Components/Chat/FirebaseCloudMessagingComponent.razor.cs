////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Chat;

/// <summary>
/// FirebaseCloudMessagingComponent
/// </summary>
public partial class FirebaseCloudMessagingComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IFirebaseServiceTransmission FirebaseRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required DialogWebChatModelDB ChatDialog { get; set; }


    string? titleMsg, textBodyMsg, nameMsg, imageMsg;

    bool CanSendMessage =>
        !string.IsNullOrWhiteSpace(titleMsg) &&
        !string.IsNullOrWhiteSpace(textBodyMsg);

    async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(ChatDialog.FirebaseCloudMessagingToken))
            throw new Exception("string.IsNullOrWhiteSpace(ChatDialog.FirebaseCloudMessagingToken)");

        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        if (string.IsNullOrWhiteSpace(titleMsg))
            throw new Exception("string.IsNullOrWhiteSpace(titleMsg)");

        if (string.IsNullOrWhiteSpace(textBodyMsg))
            throw new Exception("string.IsNullOrWhiteSpace(textBodyMsg)");

        TAuthRequestStandardModel<SendFirebaseMessageRequestModel> req = new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = new()
            {
                TextBody = textBodyMsg,
                TokensFCM = [ChatDialog.FirebaseCloudMessagingToken],
                Title = titleMsg,
                Name = nameMsg,
                ExpandViewMode = true,
                ImageUrl = imageMsg,
            }
        };
        await SetBusyAsync();
        TResponseModel<List<string>> res = await FirebaseRepo.SendFirebaseMessageAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        
         titleMsg = "";
         textBodyMsg = "";
         nameMsg = "";
         imageMsg = "";
         
        await SetBusyAsync(false);
    }
}