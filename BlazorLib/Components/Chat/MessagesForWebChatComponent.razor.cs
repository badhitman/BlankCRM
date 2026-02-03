////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Chat;

/// <summary>
/// MessagesForWebChatComponent
/// </summary>
public partial class MessagesForWebChatComponent : BlazorBusyComponentUsersCachedModel
{
    [Inject]
    IWebChatService WebChatRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public int DialogId { get; set; }


    private string _inputFileId = Guid.NewGuid().ToString();
    string? _textSendMessage;
    MudTable<MessageWebChatModelDB>? tableRef;
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

    async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(_textSendMessage))
            throw new ArgumentNullException(nameof(_textSendMessage));

        await SetBusyAsync();

        //MemoryStream ms;
        if (loadedFiles.Count != 0)
        {
            //req.Files = [];

            //foreach (var fileBrowser in loadedFiles)
            //{
            //    ms = new();
            //    await fileBrowser.OpenReadStream(maxAllowedSize: 1024 * 18 * 1024).CopyToAsync(ms);
            //    req.Files.Add(new() { ContentType = fileBrowser.ContentType, Name = fileBrowser.Name, Data = ms.ToArray() });
            //    await ms.DisposeAsync();
            //}
        }

        //SnackBarRepo.ShowMessagesResponse(rest.Messages);
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
                DialogId = DialogId,
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

        if (tableRef is not null)
            await tableRef.ReloadServerData();
    }
}