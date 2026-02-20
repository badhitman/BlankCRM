////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace SharedLib;

/// <summary>
/// ConnectorValuesChangedEventPayloadModel
/// </summary>
public partial class ConnectorValuesChangedEventPayloadModel
{
    /// <inheritdoc/>
    public virtual InstrumentTradeStockSharpModel? Instrument { get; set; }

    /// <inheritdoc/>
    public KeyValuePair<Level1FieldsStockSharpEnum, object>[]? DataPayload { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset OffsetMaster { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset OffsetSlave { get; set; }
}