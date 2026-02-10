////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Chat;

/// <summary>
/// MessagesForWebChatComponent
/// </summary>
public partial class MessagesForWebChatComponent : BlazorBusyComponentUsersCachedModel
{

    [Inject]
    IJSRuntime JsRuntime { get; set; } = default!;

    [Inject]
    IWebChatService WebChatRepo { get; set; } = default!;

    [Inject]
    IStorageTransmission StorageRepo { get; set; } = default!;

    [Inject]
    NavigationManager NavRepo { get; set; } = default!;

    [Inject]
    IEventNotifyReceive<NewMessageWebChatEventModel> NewMessageWebChatEventRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required DialogWebChatModelDB DialogWebChat { get; set; }


    MessageWebChatModelDB? _selectedMessage;
    private string _inputFileId = Guid.NewGuid().ToString();
    string? _textSendMessage;
    MudMenu? _contextMenu;
    MudTable<MessageWebChatModelDB>? tableRef;

    /// <inheritdoc/>
    public bool SoundIsMute { get; set; }

    readonly string LayoutContainerId = Guid.NewGuid().ToString();

    private readonly List<IBrowserFile> loadedFiles = [];
    bool CanNotSendMessage => string.IsNullOrWhiteSpace(_textSendMessage) || IsBusyProgress;


    void SelectFilesChange(InputFileChangeEventArgs e)
    {
        loadedFiles.Clear();

        foreach (IBrowserFile file in e.GetMultipleFiles())
        {
            loadedFiles.Add(file);
        }
    }

    void BanUser()
    {
        if (_selectedMessage is not null)
        {
            SnackBarRepo.Add($"`` has been banned!", Severity.Error);
        }
    }

    void ShowHiddenInfo()
    {
        if (_selectedMessage is not null)
        {
            SnackBarRepo.Add($"Hidden information for ``", Severity.Info);
        }
    }

    async Task RightClickMessage(MouseEventArgs args, MessageWebChatModelDB message)
    {
        _selectedMessage = message;
        if (_contextMenu != null)
            await _contextMenu.OpenMenuAsync(args);
    }

    async Task ClickMessage(MouseEventArgs args, MessageWebChatModelDB message)
    {
        _selectedMessage = message;
        SnackBarRepo.Add("Message clicked: " + message.Text, Severity.Info);
        await Task.CompletedTask;
    }

    async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(_textSendMessage))
        {
            SnackBarRepo.Error("string.IsNullOrWhiteSpace(_textSendMessage)");
            return;
        }
        if (string.IsNullOrWhiteSpace(CurrentUserSession?.UserId))
        {
            SnackBarRepo.Error("string.IsNullOrWhiteSpace(CurrentUserSession?.UserId)");
            return;
        }

        MessageWebChatModelDB req = new()
        {
            SenderUserIdentityId = CurrentUserSession.UserId,
            Text = _textSendMessage,
            CreatedAtUTC = DateTime.UtcNow,
            DialogOwnerId = DialogWebChat.Id,
            InitiatorMessageSender = false,
            AttachesFiles = loadedFiles.Count == 0 ? null : []
        };
        SoundIsMute = true;
        await SetBusyAsync();

        TResponseModel<int> res = await WebChatRepo.CreateMessageWebChatAsync(req);
        _textSendMessage = null;
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        req.Id = res.Response;

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
                        PrefixPropertyName = DialogWebChat.Id.ToString(),
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
                FileTokenAccess = x.AccessRules?.First(x=>x.AccessRuleType == FileAccessRulesTypesEnum.Token).Option,
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

        await SetBusyAsync(false);
    }

    async Task<TableData<MessageWebChatModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        if (CurrentUserSession is null)
            return new TableData<MessageWebChatModelDB>();

        await SetBusyAsync(token: token);
        TPaginationResponseStandardModel<MessageWebChatModelDB> res = await WebChatRepo.SelectMessagesForRoomWebChatAsync(new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = new()
            {
                DialogId = DialogWebChat.Id,
            }
        }, token);

        if (res.Response is not null && res.Response.Count != 0)
            await CacheUsersUpdate([.. res.Response.Where(x => !string.IsNullOrWhiteSpace(x.SenderUserIdentityId)).Select(x => x.SenderUserIdentityId)!]);

        await SetBusyAsync(false, token);
        // Return the data
        return new TableData<MessageWebChatModelDB>()
        {
            TotalItems = res.TotalRowsCount,
            Items = res.Response
        };
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (CurrentUserSession is not null)
            UsersCache.Add(CurrentUserSession);

        await SetBusyAsync();
        await NewMessageWebChatEventRepo.RegisterAction(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.NewMessageWebChatNotifyReceive, DialogWebChat.Id.ToString()), NewMessageWebChatHandler, LayoutContainerId, CurrentUserSessionBytes);
        await SetBusyAsync(false);

        if (tableRef is not null)
            await tableRef.ReloadServerData();
    }

    void NewMessageWebChatHandler(NewMessageWebChatEventModel model)
    {
        if (tableRef is not null)
            InvokeAsync(async () =>
            {
                await tableRef.ReloadServerData();
                StateHasChanged();
                if (!SoundIsMute)
                    await JsRuntime.InvokeVoidAsync("methods.PlayAudio", "audioPlayerMessagesForWebChatComponent");
                else
                    SoundIsMute = false;
            });
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        NewMessageWebChatEventRepo.UnregisterAction();
        base.Dispose();
    }
}