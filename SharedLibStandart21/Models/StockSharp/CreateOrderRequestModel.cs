////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// CreateOrderRequestModel
/// </summary>
public class CreateOrderRequestModel
{
    /// <summary>
    /// Тип заявки
    /// </summary>
    public OrderTypesEnum OrderType { get; set; }

    /// <summary>
    /// портфель для исполнения заявки
    /// </summary>
    [Required]
    public PortfolioStockSharpModel? Portfolio {  get; set; }

    /// <summary>
    /// устанавливается объём заявки
    /// </summary>
    public decimal Volume { get; set; }


    /// <summary>
    /// устанавливается цена заявки
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// инструмент
    /// </summary>
    public InstrumentTradeStockSharpViewModel? Instrument {  get; set; }

    /// <summary>
    /// направление заявки
    /// </summary>
    public SidesEnum Side {  get; set; }
}