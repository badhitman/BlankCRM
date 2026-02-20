////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Collections.Generic;

namespace SharedLib;

/// <inheritdoc/>
public class TPaginationResponseStandardModel<T> : PaginationResponseStandardModel
{
    /// <inheritdoc/>
    public TPaginationResponseStandardModel() { }
    /// <inheritdoc/>
    public TPaginationResponseStandardModel(PaginationRequestStandardModel req)
    {
        PageNum = req.PageNum;
        PageSize = req.PageSize;
        SortingDirection = req.SortingDirection;
        SortBy = req.SortBy;
    }

    /// <inheritdoc/>
    public List<T>? Response { get; set; }
}