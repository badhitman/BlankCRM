////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// InstrumentsRequestModel
/// </summary>
public class InstrumentsRequestModel : SimplePaginationRequestModel
{
    /// <summary>
    /// FavoriteFilter
    /// </summary>
    public bool? FavoriteFilter { get; set; }
}