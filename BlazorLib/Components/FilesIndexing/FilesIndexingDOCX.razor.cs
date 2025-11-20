////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace BlazorLib.Components.FilesIndexing;

/// <summary>
/// FilesIndexingDOCX
/// </summary>
public partial class FilesIndexingDOCX : FilesIndexingViewBase
{
    WordprocessingDocumentIndexingFileResponseModel? wordFile;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (string.IsNullOrWhiteSpace(CurrentUserSession?.UserId))
            throw new Exception("string.IsNullOrWhiteSpace(CurrentUserSession?.UserId)");

        await SetBusyAsync();
        TAuthRequestModel<int> _req = new()
        {
            Payload = FileId,
            SenderActionUserId = CurrentUserSession.UserId
        };
        TResponseModel<WordprocessingDocumentIndexingFileResponseModel> _file = await FilesIndexingRepo.WordprocessingDocumentGetIndexAsync(_req);
        wordFile = _file.Response;
        SnackBarRepo.ShowMessagesResponse(_file.Messages);
        await SetBusyAsync(false);
    }
}