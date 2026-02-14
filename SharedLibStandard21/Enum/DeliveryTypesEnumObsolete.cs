////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;
using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Методы доставки
/// </summary>
[Obsolete("use rubrics")]
public enum DeliveryTypesEnumObsolete
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
    /// СДЕК (международный)
    /// </summary>
    [Description("СДЕК (международный)")]
    InternationalCDEK = 40,

    /// <summary>
    /// Самовывоз
    /// </summary>
    [Description("Самовывоз")]
    Pickup = 60,
}