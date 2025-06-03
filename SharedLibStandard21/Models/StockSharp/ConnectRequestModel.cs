////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;

namespace SharedLib;

/// <summary>
/// ConnectRequestModel
/// </summary>
public class ConnectRequestModel
{
    /// <inheritdoc/>
    public List<BoardStockSharpModel> BoardsFilter { get; set; }
}