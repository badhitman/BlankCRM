////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// HelpDesk journal modes: All, ActualOnly, ArchiveOnly
/// </summary>
public enum HelpDeskJournalModesEnum
{
    /// <summary>
    /// All
    /// </summary>
    [Description("Все")]
    All,

    /// <summary>
    /// Actual only
    /// </summary>
    [Description("Актуальные")]
    ActualOnly,

    /// <summary>
    /// Archive only
    /// </summary>
    [Description("Архивные")]
    ArchiveOnly,
}