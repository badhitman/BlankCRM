////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ConnectorValuesChangedEventPayloadModel
/// </summary>
public class ConnectorValuesChangedEventPayloadModel
{
    /// <inheritdoc/>
    public required InstrumentTradeModel Instrument { get; set; }

    /// <inheritdoc/>
    public required IEnumerable<KeyValuePair<Level1FieldsStockSharpEnum, object>> DataPayload { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset OffsetMaster { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset OffsetSlave { get; set; }
}