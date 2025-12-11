////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Статусы доставки
/// </summary>
public enum DeliveryStatusesEnum
{
    /// <summary>
    /// Комплектуется
    /// </summary>
    [Description("Комплектуется")]
    Created = 10,

    /// <summary>
    /// Отправлен
    /// </summary>
    [Description("Отправлен")]
    Progress = 20,

    /// <summary>
    /// Доставлен
    /// </summary>
    [Description("Доставлен")]
    Delivered = 30,

    /// <summary>
    /// Отменён
    /// </summary>
    [Description("Отменён")]
    Canceled = 1000
}