////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using System;

namespace SharedLib;

/// <summary>
/// PortfolioStockSharpViewModel
/// </summary>
public partial class PortfolioStockSharpViewModel : PortfolioStockSharpModel
{
    /// <inheritdoc/>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Добавлен в "Избранное"
    /// </summary>
    public bool IsFavorite { get; set; }

    /// <inheritdoc/>
    public DateTime LastUpdatedAtUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }

    /// <inheritdoc/>
    public void Reload(PortfolioStockSharpViewModel model)
    {
        Id = model.Id;
        Name = model.Name;
        State = model.State;
        Board = model.Board;
        Currency = model.Currency;
        DepoName = model.DepoName;
        IsFavorite = model.IsFavorite;
        ClientCode = model.ClientCode;
        LastUpdatedAtUTC = model.LastUpdatedAtUTC;
        CurrentValue = model.CurrentValue;
        BeginValue = model.BeginValue;
    }
}