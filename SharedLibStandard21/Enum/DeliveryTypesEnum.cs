////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Методы доставки
/// </summary>
public enum DeliveryTypesEnum
{
    /// <summary>
    /// Почта
    /// </summary>
    [Description("Почта")]
    StandardPost = 0,

    /// <summary>
    /// СДЕК
    /// </summary>
    [Description("СДЕК")]
    CDEK = 20,

    /// <summary>
    /// Самовывоз
    /// </summary>
    [Description("Самовывоз")]
    Pickup = 40,
}