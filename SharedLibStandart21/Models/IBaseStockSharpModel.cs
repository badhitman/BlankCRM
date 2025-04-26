////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// IBaseStockSharpModel
/// </summary>
public interface IBaseStockSharpModel
{
    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    [Required]
    public DateTime LastAtUpdatedUTC { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    [Required]
    public DateTime CreatedAtUTC { get; set; }
}