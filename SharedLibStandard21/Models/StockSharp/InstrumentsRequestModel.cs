////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;

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

    /// <inheritdoc/>
    public InstrumentsStockSharpTypesEnum[] TypesFilter { get; set; }

    /// <inheritdoc/>
    public CurrenciesTypesEnum[] CurrenciesFilter { get; set; }

    /// <inheritdoc/>
    public int[] SelectedBoards { get; set; }
}