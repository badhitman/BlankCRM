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
    /// StateFilter
    /// </summary>
    public MarkersInstrumentStockSharpEnum?[] MarkersFilter { get; set; }

    /// <inheritdoc/>
    public InstrumentsStockSharpTypesEnum[] TypesFilter { get; set; }

    /// <inheritdoc/>
    public CurrenciesTypesEnum[] CurrenciesFilter { get; set; }

    /// <inheritdoc/>
    public int[] BoardsFilter { get; set; }
}