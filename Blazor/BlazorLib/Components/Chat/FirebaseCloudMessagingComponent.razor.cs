////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SharedLib;

namespace BlazorLib.Components.Chat;

/// <summary>
/// FirebaseCloudMessagingComponent
/// </summary>
public partial class FirebaseCloudMessagingComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IStorageTransmission StorageRepo { get; set; } = default!;

    [Inject]
    IFirebaseServiceTransmission FirebaseRepo { get; set; } = default!;

    [Inject]
    NavigationManager NavRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required DialogWebChatModelDB ChatDialog { get; set; }


    string? titleMsg, textBodyMsg, nameMsg, imageMsg, clickUrl;
    readonly List<IBrowserFile> loadedFiles = [];
    string _inputFileId = Guid.NewGuid().ToString();

    bool CanSendMessage =>
        !string.IsNullOrWhiteSpace(titleMsg) &&
        !string.IsNullOrWhiteSpace(textBodyMsg);

    void SelectFilesChange(InputFileChangeEventArgs e)
    {
        loadedFiles.Clear();

        foreach (IBrowserFile file in e.GetMultipleFiles())
        {
            loadedFiles.Add(file);
        }
    }

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
                LinkURL = clickUrl,
                Data = new Dictionary<string, string>()
                {
                    { "score", "850" },
                    { "time", "2:45" },
                },
            }
        };
        List<StorageFileModelDB> filesUpd = [];
        MemoryStream? ms = null;
        await SetBusyAsync();
        if (loadedFiles.Count != 0)
        {
            foreach (IBrowserFile fileBrowser in loadedFiles)
            {
                ms = new();
                await fileBrowser.OpenReadStream(maxAllowedSize: 1024 * 18 * 1024).CopyToAsync(ms);
                TAuthRequestStandardModel<StorageFileMetadataModel> reqF = new()
                {
                    SenderActionUserId = CurrentUserSession.UserId,
                    Payload = new()
                    {
                        Payload = ms.ToArray(),
                        FileName = fileBrowser.Name,
                        ContentType = fileBrowser.ContentType,
                        OwnerPrimaryKey = ChatDialog.Id,
                        ApplicationName = Path.Combine($"{GlobalStaticConstantsRoutes.Routes.FIREBASE_CONTROLLER_NAME}-{GlobalStaticConstantsRoutes.Routes.NOTIFICATION_CONTROLLER_NAME}"),
                        PropertyName = GlobalStaticConstantsRoutes.Routes.ATTACHMENT_CONTROLLER_NAME,
                        Referrer = NavRepo.Uri,
                        RulesTypes = new() { { FileAccessRulesTypesEnum.Token, [Guid.NewGuid().ToString()] } },
                    }
                };
                TResponseModel<StorageFileModelDB> storeFile = await StorageRepo.SaveFileAsync(reqF);
                SnackBarRepo.ShowMessagesResponse(storeFile.Messages);

                if (storeFile.Response is not null)
                    filesUpd.Add(storeFile.Response);
                await ms.DisposeAsync();
            }

            if (filesUpd.Count != 0)
            {
                StorageFileModelDB _f = filesUpd.First();
                req.Payload.ImageUrl = $"{NavRepo.BaseUri}cloud-fs/read/{_f.Id}/image{Path.GetExtension(_f.FileName)}?{GlobalStaticConstantsRoutes.Routes.TOKEN_CONTROLLER_NAME}={_f.AccessRules?.First(x => x.AccessRuleType == FileAccessRulesTypesEnum.Token).Option}";
            }

            loadedFiles.Clear();
            _inputFileId = Guid.NewGuid().ToString();
        }
        TResponseModel<SendFirebaseMessageResultModel> res = await FirebaseRepo.SendFirebaseNotificationAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);

        titleMsg = nameMsg = imageMsg = textBodyMsg = clickUrl = "";

        await SetBusyAsync(false);
    }
}