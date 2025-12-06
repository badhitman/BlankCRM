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
    /// Курьерская служба
    /// </summary>
    [Description("Курьерская служба")]
    CourierService = 20,

    /// <summary>
    /// Самовывоз
    /// </summary>
    [Description("Самовывоз")]
    Pickup = 40,
}