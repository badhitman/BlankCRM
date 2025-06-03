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
    /// Created
    /// </summary>
    [Description("Created")]
    Created = 0,

    /// <summary>
    /// Returned to work
    /// </summary>
    [Description("Returned")]
    Reopen = 10,

    /// <summary>
    /// Pause
    /// </summary>
    [Description("Pause")]
    Pause = 20,

    /// <summary>
    /// Progress
    /// </summary>
    [Description("Progress")]
    Progress = 30,

    /// <summary>
    /// Review
    /// </summary>
    [Description("Review")]
    Check = 40,

    /// <summary>
    /// Done
    /// </summary>
    [Description("Done")]
    Done = 50,

    /// <summary>
    /// Cancel
    /// </summary>
    [Description("Cancel")]
    Canceled = 1000
}