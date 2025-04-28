////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// classPortfolioTradeModelDB
/// </summary>
[Index(nameof(IsFavorite)), Index(nameof(LastUpdatedAtUTC))]
public class PortfolioTradeModelDB : PortfolioStockSharpModel, IBaseStockSharpModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Добавлен в "Избранное"
    /// </summary>
    public bool IsFavorite { get; set; }

    /// <inheritdoc/>
    public new BoardStockSharpModelDB Board { get; set; }
    /// <inheritdoc/>
    public int? BoardId { get; set; }

    /// <inheritdoc/>
    public List<OrderStockSharpModelDB> Orders { get; set; }

    /// <inheritdoc/>
    public DateTime LastUpdatedAtUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }

    /// <inheritdoc/>
    public void SetUpdate(PortfolioStockSharpModel req)
    {
        LastUpdatedAtUTC = DateTime.UtcNow;
        State = req.State;
        Name = req.Name;
        Currency = req.Currency;
        DepoName = req.DepoName;
        ClientCode = req.ClientCode;
    }
}