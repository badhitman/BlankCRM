////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace BlazorLib.Components.FilesIndexing;

/// <summary>
/// FilesIndexingXLSX
/// </summary>
public partial class FilesIndexingXLSX : FilesIndexingViewBase
{
    SpreadsheetDocumentIndexingFileResponseModel? excelFile;

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
        TResponseModel<SpreadsheetDocumentIndexingFileResponseModel> _file = await FilesIndexingRepo.SpreadsheetDocumentGetIndexAsync(_req);
        excelFile = _file.Response;
        SnackBarRepo.ShowMessagesResponse(_file.Messages);
        await SetBusyAsync(false);
    }
}