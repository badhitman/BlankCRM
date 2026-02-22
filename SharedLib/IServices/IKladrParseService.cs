////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Text;
using static SharedLib.ParserAbstractDBF;

namespace SharedLib;

/// <summary>
/// IKladrParseService
/// </summary>
public interface IKladrParseService
{
    /// <inheritdoc/>
    public int CurrentNumRecord { get; } 

    /// <inheritdoc/>
    event PartUploadHandler? PartUploadNotify;

    /// <inheritdoc/>
    public Encoding CurrentEncoding { get; set; }


    /// <inheritdoc/>
    public Task<int> Open(MemoryStream _dbfFile, string name);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UploadPartTempKladrAsync(UploadPartTableDataModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<(List<object[]> TableData, FieldDescriptorBase[] Columns)> GetRandomRowsAsDataTable(int limit_row, bool del_row_inc = true);

    /// <inheritdoc/>
    public Task UploadData(bool inc_del, int limitParts = -1);
}
