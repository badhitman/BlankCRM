////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <inheritdoc/>
[Index(nameof(XML_ID)), Index(nameof(NAME))]
public class DaichiEntryModel
{
    /// <inheritdoc/>
    public required string XML_ID { get; set; }

    /// <inheritdoc/>
    public required string NAME { get; set; }
}