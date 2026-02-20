////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using SharedLib;

namespace ToolsMauiLib;

public class ParseDBF(IClientRestToolsService RemoteClient) : ParserAbstractDBF
{
    /// <inheritdoc/>
    public override event PartUploadHandler? PartUploadNotify;

    protected override void NotifyUploadAction(int position)
    {
        if (PartUploadNotify is not null)
            PartUploadNotify(position);
    }

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