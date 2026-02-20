////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// BoardStockSharpViewModel
/// </summary>
public class BoardStockSharpViewModel : BoardStockSharpModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public DateTime LastUpdatedAtUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }
}