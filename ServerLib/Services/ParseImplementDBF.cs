////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace ServerLib;

/// <summary>
/// ParseImplementDBF
/// </summary>
public class ParseImplementDBF(IKladrService RemoteClient) : ParserAbstractDBF
{
    /// <inheritdoc/>
    public override event PartUploadHandler? PartUploadNotify;

    /// <inheritdoc/>
    protected override void NotifyUploadAction(int position)
    {
        if (PartUploadNotify is not null)
            PartUploadNotify(position);
    }

    /// <inheritdoc/>
    public override async Task<ResponseBaseModel> UploadPartTempKladrAsync(UploadPartTableDataModel req, CancellationToken token = default)
    {
        return await RemoteClient.UploadPartTempKladrAsync(new()
        {
            Columns = req.Columns,
            RowsData = req.RowsData,
            TableName = req.TableName,
        }, token);
    }
}