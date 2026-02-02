////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// DialogWebChatModelDB
/// </summary>
[Index(nameof(InitiatorContactsNormalized))]
public class DialogWebChatModelDB : DialogWebChatViewModel
{
    /// <summary>
    /// InitiatorContactsNormalized
    /// </summary>
    public string? InitiatorContactsNormalized { get; set; }
}