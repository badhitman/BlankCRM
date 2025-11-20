////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// WordprocessingDocumentIndexingFileResponseModel
/// </summary>
public class WordprocessingDocumentIndexingFileResponseModel
{
    /// <inheritdoc/>
    public List<ParagraphWordIndexFileModelDB>? Paragraphs { get; set; }

    /// <inheritdoc/>
    public List<TableWordIndexFileModelDB>? Tables { get; set; }

}