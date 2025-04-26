////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// OrderPositionEffectsEnum
/// </summary>
public enum OrderPositionEffectsEnum
{
    /// <summary>
    /// Default behavior.
    /// </summary>
    [Display(Name = "Default", Description = "DefaultBehaviour")]
    Default,
    /// <summary>
    /// A trade should open a position.
    /// </summary>
    [Display(Name = "OpenOnly", Description = "PositionEffectOpenOnly")]
    OpenOnly,
    /// <summary>
    /// Сделка должна привести позицию к нулю, т.е. закрыть максимально возможную часть любой существующей позиции
    /// и открыть противоположную позицию на любой остаток.
    /// </summary>
    [Display(Name = "CloseOnly", Description = "PositionEffectCloseOnly")]
    CloseOnly
}
