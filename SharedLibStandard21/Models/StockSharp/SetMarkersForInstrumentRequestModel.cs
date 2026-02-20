////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Установка маркеров инструменту
/// </summary>
public class SetMarkersForInstrumentRequestModel
{
    /// <inheritdoc/>
    public int InstrumentId { get; set; }

    /// <inheritdoc/>
    public MarkersInstrumentStockSharpEnum[]? SetMarkers {  get; set; }
}