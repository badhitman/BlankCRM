﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Типы сообщений результат выполнения запроса
/// </summary>
public enum MessagesTypesEnum
{
    /// <summary>
    /// Ошибка
    /// </summary>
    [Description("Ошибка")]
    Error = -1,

    /// <summary>
    /// Сообщение об успешном выполнении команды
    /// </summary>
    [Description("Успешно")]
    Success = 0,

    /// <summary>
    /// Информационное сообщение
    /// </summary>
    [Description("Информация")]
    Info = 2,

    /// <summary>
    /// Важное сообщение
    /// </summary>
    [Description("Важно")]
    Warning = 3
}