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

    List<object> NodesList = [];

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (string.IsNullOrWhiteSpace(CurrentUserSession?.UserId))
            throw new Exception("string.IsNullOrWhiteSpace(CurrentUserSession?.UserId)");

        await SetBusyAsync();
        TAuthRequestStandardModel<int> _req = new()
        {
            Payload = FileId,
            SenderActionUserId = CurrentUserSession.UserId
        };
        TResponseModel<WordprocessingDocumentIndexingFileResponseModel> _file = await FilesIndexingRepo.WordprocessingDocumentGetIndexAsync(_req);
        wordFile = _file.Response;
        SnackBarRepo.ShowMessagesResponse(_file.Messages);
        await SetBusyAsync(false);

        if (wordFile?.Tables is null || wordFile.Paragraphs is null)
            return;

        for (int i = 0; i < wordFile.Tables.Count + wordFile.Paragraphs.Count; i++)
        {
            TableWordIndexFileModelDB? _findTable = wordFile.Tables.FirstOrDefault(x => x.SortIndex == i);
            if (_findTable is not null)
                NodesList.Add(_findTable);
            else
                NodesList.Add(wordFile.Paragraphs.First(x => x.SortIndex == i));
        }
    }
}