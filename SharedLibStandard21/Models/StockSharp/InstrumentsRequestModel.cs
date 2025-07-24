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
    /// StateFilter
    /// </summary>
    public ObjectStatesEnum[] StatesFilter { get; set; }

    /// <inheritdoc/>
    public InstrumentsStockSharpTypesEnum[] TypesFilter { get; set; }

    /// <inheritdoc/>
    public CurrenciesTypesEnum[] CurrenciesFilter { get; set; }

    /// <inheritdoc/>
    public int[] BoardsFilter { get; set; }
}