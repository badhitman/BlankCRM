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
    Created = 0,

    /// <summary>
    /// Повторное открытие
    /// </summary>
    [Description("Повторное открытие")]
    Reopen = 10,

    /// <summary>
    /// Приостановлен
    /// </summary>
    [Description("Приостановлен")]
    Pause = 20,

    /// <summary>
    /// В работе
    /// </summary>
    [Description("В работе")]
    Progress = 30,

    /// <summary>
    /// Проверка
    /// </summary>
    [Description("Проверка")]
    Check = 40,

    /// <summary>
    /// Выполнен
    /// </summary>
    [Description("Выполнен")]
    Done = 50,

    /// <summary>
    /// Аннулирован
    /// </summary>
    [Description("Аннулирован")]
    Canceled = 1000
}