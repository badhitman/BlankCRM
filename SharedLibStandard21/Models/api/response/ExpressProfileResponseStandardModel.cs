////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Collections.Generic;

namespace SharedLib;

/// <summary>
/// Текущее состояние экспресс авторизации
/// </summary>
public class ExpressProfileResponseStandardModel
{
    /// <summary>
    /// Имя в системе, закреплённое за токеном доступа
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Роли доступные токену доступа
    /// </summary>
    public IEnumerable<string>? Roles { get; set; }
}