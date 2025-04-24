////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Электронная торговая биржа
/// </summary>
public class ExchangeBoardModelDB : EntryUpdatedModel
{
    /// <summary>
    /// Инструменты (биржевые торговые)
    /// </summary>
    public List<InstrumentTradeModelDB>? Instruments { get; set; }
}