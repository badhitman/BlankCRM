////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using BlazorLib.Components.Shared.Layouts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using MudBlazor;
using SharedLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace BlazorLib.Components.Chat;

/// <summary>
/// ChatWrapperComponent
/// </summary>
public partial class ChatWrapperComponent : BlazorBusyComponentUsersCachedModel
{
    [Inject]
    IStorageTransmission StorageRepo { get; set; } = default!;

    [Inject]
    IJSRuntime JsRuntime { get; set; } = default!;

    [Inject]
    IWebChatService WebChatRepo { get; set; } = default!;

    [Inject]
    NavigationManager NavRepo { get; set; } = default!;

    [Inject]
    IEventsWebChatsNotifies EventsWebChatsHandleRepo { get; set; } = default!;

    [Inject]
    IEventNotifyReceive<NewMessageWebChatEventModel> NewMessageWebChatEventRepo { get; set; } = default!;

    [Inject]
    IEventNotifyReceive<GetStateWebChatEventModel> StateGetWebChatEventRepo { get; set; } = default!;

    [Inject]
    IEventNotifyReceive<StateWebChatModel> StateSetWebChatEventRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required RealtimeCoreComponent RealtimeCore { get; set; }


    bool muteSound;
    AboutUserAgentModel? UserAgent;
    DialogWebChatModelDB? dialogSession, dialogSessionEdit;
    string? lastUserId;
    Virtualize<MessageWebChatModelDB>? virtualizeComponent;
    string? _textSendMessage;
    byte missingMessages;
    bool CannotSendMessage => string.IsNullOrWhiteSpace(_textSendMessage) || IsBusyProgress;
    static readonly int virtualCacheSize = 50;
    readonly List<PongClientsWebChatEventModel> UsersSessions = [];
    readonly string LayoutContainerId = Guid.NewGuid().ToString();
    string _inputFileId = Guid.NewGuid().ToString();
    readonly List<IBrowserFile> loadedFiles = [];

    /// <inheritdoc/>
    public bool ChatDialogOpen { get; private set; }

    async Task ShowToggle()
    {
        ChatDialogOpen = !ChatDialogOpen;

        if (ChatDialogOpen)
        {
            missingMessages = 0;
            await InitSession();
            await RealtimeCore.PingAllUsers();
        }

        if (dialogSession is not null)
            await EventsWebChatsHandleRepo.StateEchoWebChatAsync(new StateWebChatModel()
            {
                StateDialog = ChatDialogOpen,
                DialogId = dialogSession.Id,
                UserIdentityId = CurrentUserSession?.UserId,
            });
    }

    void SelectFilesChange(InputFileChangeEventArgs e)
    {
        loadedFiles.Clear();

        foreach (IBrowserFile file in e.GetMultipleFiles())
        {
            loadedFiles.Add(file);
        }
    }

    async ValueTask<ItemsProviderResult<MessageWebChatModelDB>> LoadMessages(ItemsProviderRequest request)
    {
        if (dialogSession is null)
            return new ItemsProviderResult<MessageWebChatModelDB>([], 0);

        SelectMessagesForWebChatRequestModel req = new()
        {
            SessionTicketId = dialogSession.SessionTicketId,
            StartIndex = request.StartIndex,
            Count = request.Count,
        };
        await SetBusyAsync(token: request.CancellationToken);
        TResponseModel<SelectMessagesForWebChatResponseModel> res = await WebChatRepo.SelectMessagesWebChatAsync(req, request.CancellationToken);
        await SetBusyAsync(false, token: request.CancellationToken);
        if (res.Response is null)
            return new ItemsProviderResult<MessageWebChatModelDB>([], 0);

        if (res.Response.Count != 0)
            await CacheUsersUpdate([.. res.Response.Messages.Where(x => !string.IsNullOrWhiteSpace(x.SenderUserIdentityId)).Select(x => x.SenderUserIdentityId)!]);

        return new ItemsProviderResult<MessageWebChatModelDB>(res.Response.Messages, res.Response.TotalRowsCount);
    }

    async Task SevaDialog(MouseEventArgs args)
    {
        if (dialogSessionEdit is null)
            return;

        await SetBusyAsync();
        ResponseBaseModel res = await WebChatRepo.UpdateDialogWebChatInitiatorAsync(new()
        {
            SenderActionUserId = CurrentUserSession?.UserId,
            Payload = dialogSessionEdit
        });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await InitSession();

        await SetBusyAsync(false);
    }

    async Task SendMessage(MouseEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(_textSendMessage) || dialogSession is null)
            return;

        MessageWebChatModelDB req = new()
        {
            Text = _textSendMessage.Trim(),
            SenderUserIdentityId = CurrentUserSession?.UserId,
            DialogOwnerId = dialogSession.Id,
            InitiatorMessageSender = true,
            AttachesFiles = loadedFiles.Count == 0 ? null : []
        };

        muteSound = true;
        await SetBusyAsync();
        TResponseModel<int> res = await WebChatRepo.CreateMessageWebChatAsync(req);
        req.Id = res.Response;
        if (!string.IsNullOrWhiteSpace(CurrentUserSession?.UserId))
        {
            List<StorageFileModelDB> filesUpd = [];
            MemoryStream ms;
            if (loadedFiles.Count != 0 && res.Response > 0)
            {
                req.AttachesFiles = [];

                foreach (var fileBrowser in loadedFiles)
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
                            OwnerPrimaryKey = res.Response,
                            ApplicationName = Path.Combine($"{GlobalStaticConstantsRoutes.Routes.WEB_CONTROLLER_NAME}-{GlobalStaticConstantsRoutes.Routes.CHAT_CONTROLLER_NAME}"),
                            PrefixPropertyName = dialogSession.Id.ToString(),
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
            }

            if (filesUpd.Count != 0)
            {
                req.AttachesFiles = [.. filesUpd.Select(x => new AttachesMessageWebChatModelDB()
                {
                    FileAttachId = x.Id,
                    FileAttachName = x.FileName,
                    FileLength = x.FileLength,
                    MessageOwnerId = req.Id,
                    FileTokenAccess = x.AccessRules?.First(x => x.AccessRuleType == FileAccessRulesTypesEnum.Token).Option,
                })];

                ResponseBaseModel _updFiles = await WebChatRepo.UpdateMessageWebChatAsync(new()
                {
                    SenderActionUserId = CurrentUserSession.UserId,
                    Payload = req
                });
                SnackBarRepo.ShowMessagesResponse(_updFiles.Messages);
            }

            loadedFiles.Clear();
            _inputFileId = Guid.NewGuid().ToString();
        }

        _textSendMessage = null;

        await SetBusyAsync(false);
    }

    async Task OnKeyPresHandler(KeyboardEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(_textSendMessage) || dialogSession is null)
            return;

        if (args.Key == "Enter" && !args.ShiftKey)
        {
            MessageWebChatModelDB req = new()
            {
                Text = _textSendMessage.Trim(),
                SenderUserIdentityId = CurrentUserSession?.UserId,
                DialogOwnerId = dialogSession.Id,
                InitiatorMessageSender = true,
            };
            muteSound = true;
            await SetBusyAsync();
            await WebChatRepo.CreateMessageWebChatAsync(req);
            _textSendMessage = null;
            await SetBusyAsync(false);
        }
    }

    async Task InitSession()
    {
        UserAgent = await JsRuntime.InvokeAsync<AboutUserAgentModel?>("methods.AboutUserAgent");
        if (UserAgent?.CookieEnabled != true)
            return;

        string
            _sessionCookieName = Path.Combine(Routes.TICKET_CONTROLLER_NAME, Routes.SESSION_CONTROLLER_NAME).Replace("\\", "/"),
            _lastUserIdCookieName = Path.Combine(_sessionCookieName, $"{Routes.USER_CONTROLLER_NAME}-{Routes.IDENTITY_CONTROLLER_NAME}").Replace("\\", "/");

        lastUserId = await JsRuntime.InvokeAsync<string?>("methods.ReadCookie", _lastUserIdCookieName);
        if (!string.IsNullOrWhiteSpace(CurrentUserSession?.UserId))
        {
            if (lastUserId != _lastUserIdCookieName)
                await JsRuntime.InvokeVoidAsync("methods.CreateCookie", _lastUserIdCookieName, CurrentUserSession.UserId, 60 * 60 * 24 * 90, "/");
        }

        string? currentSessionTicket = await JsRuntime.InvokeAsync<string?>("methods.ReadCookie", _sessionCookieName);
        TResponseModel<DialogWebChatModelDB> initSessionTicket = await WebChatRepo.InitWebChatSessionAsync(new()
        {
            SessionTicket = currentSessionTicket,
            UserIdentityId = CurrentUserSession?.UserId,
            UserAgent = UserAgent.UserAgent,
            Language = UserAgent.Language,
            BaseUri = NavRepo.BaseUri,
        });
        dialogSession = initSessionTicket.Response;
        dialogSessionEdit = GlobalTools.CreateDeepCopy(dialogSession);

        if (initSessionTicket.Response is null)
        {
            SnackBarRepo.Error("initSessionTicket.Response is null");
            return;
        }

        await JsRuntime.InvokeVoidAsync("methods.CreateCookie", _sessionCookieName, initSessionTicket.Response.SessionTicketId, (initSessionTicket.Response.DeadlineUTC - DateTime.UtcNow).TotalSeconds, "/");
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await InitSession();
        if (dialogSession is not null)
        {
            await NewMessageWebChatEventRepo.RegisterAction(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.NewMessageWebChatNotifyReceive, dialogSession.Id.ToString()), NewMessageWebChatHandler, LayoutContainerId, CurrentUserSessionBytes, propertiesValues: [new(Routes.DIALOG_CONTROLLER_NAME, BitConverter.GetBytes(dialogSession.Id))]);
            await StateGetWebChatEventRepo.RegisterAction(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.StateGetWebChatNotifyReceive, dialogSession.Id.ToString()), GetStateWebChatWebChatHandler, LayoutContainerId, CurrentUserSessionBytes, isMute: true);
            await StateSetWebChatEventRepo.RegisterAction(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.StateSetWebChatNotifyReceive, dialogSession.Id.ToString()), SetStateWebChatHandler, LayoutContainerId, CurrentUserSessionBytes, isMute: true);
        }
    }

    async void SetStateWebChatHandler(StateWebChatModel req)
    {
        ChatDialogOpen = req.StateDialog;
        if (dialogSession is not null)
            await EventsWebChatsHandleRepo.StateEchoWebChatAsync(new()
            {
                StateDialog = ChatDialogOpen,
                DialogId = dialogSession.Id,
                UserIdentityId = CurrentUserSession?.UserId,
            });
        await InvokeAsync(StateHasChanged);
    }

    async void GetStateWebChatWebChatHandler(GetStateWebChatEventModel req)
    {
        try
        {
            UserAgent = await JsRuntime.InvokeAsync<AboutUserAgentModel?>("methods.AboutUserAgent", timeout: TimeSpan.FromSeconds(2));
        }
        catch (TaskCanceledException)
        {
            RealtimeCore.Dispose();
            Dispose();
            return;
        }
        catch (OperationCanceledException)
        {
            RealtimeCore.Dispose();
            Dispose();
            return;
        }

        await EventsWebChatsHandleRepo.StateEchoWebChatAsync(new StateWebChatModel()
        {
            StateDialog = ChatDialogOpen,
            DialogId = req.DialogId,
            UserIdentityId = CurrentUserSession?.UserId,
        });
    }

    async void NewMessageWebChatHandler(NewMessageWebChatEventModel req)
    {
        try
        {
            List<Task> tasks = [];
            if (virtualizeComponent is not null)
                tasks.Add(virtualizeComponent.RefreshDataAsync());

            if (!ChatDialogOpen)
                missingMessages++;
            else
                missingMessages = 0;

            if (!ChatDialogOpen)
            {
                tasks.Add(Task.Run(async () => await JsRuntime.InvokeVoidAsync("effects.JQuery", "pulsate", "missingMessagesBadge")));
                tasks.Add(Task.Run(async () => await JsRuntime.InvokeVoidAsync("effects.Toast", "Новое сообщение в чате", req.TextMessage, "info", true, "#9EC600")));
            }

            if (!muteSound)
                tasks.Add(Task.Run(async () => await JsRuntime.InvokeVoidAsync("methods.PlayAudio", "audioPlayerChatWrapperComponent")));

            await Task.WhenAll(tasks);
        }
        finally
        {
            muteSound = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        NewMessageWebChatEventRepo.UnregisterAction();
        StateGetWebChatEventRepo.UnregisterAction(isMute: true);
        StateSetWebChatEventRepo.UnregisterAction(isMute: true);
    }

    /// <inheritdoc/>
    public void UsersEcho(List<PongClientsWebChatEventModel> usersEcho)
    {
        lock (UsersSessions)
        {
            UsersSessions.Clear();
            UsersSessions.AddRange(usersEcho);
        }
        InvokeAsync(StateHasChangedCall);
    }
}