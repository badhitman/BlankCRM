////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Стадии/шаги обращения: "Создан", "В работе", "На проверке" и "Готово"
/// </summary>
public enum StatusesDocumentsEnum
{
    /// <summary>
    /// Создан
    /// </summary>
    [Description("Создан")]
    Created = 20,

    /// <summary>
    /// Повторное открытие
    /// </summary>
    [Description("Возвращён/Исправить")]
    Reopen = 40,

    /// <summary>
    /// Приостановлен
    /// </summary>
    [Description("Приостановлен")]
    Pause = 60,

    /// <summary>
    /// В работе
    /// </summary>
    [Description("В работе")]
    Progress = 80,

    /// <summary>
    /// Проверка
    /// </summary>
    [Description("Проверка")]
    Check = 100,

    /// <summary>
    /// Выполнен
    /// </summary>
    [Description("Выполнен")]
    Done = 120,

    /// <summary>
    /// Аннулирован
    /// </summary>
    [Description("Отменён")]
    Canceled = 1000
}