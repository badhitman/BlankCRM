////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <inheritdoc/>
public class DaichiEntryModel
{
    /// <inheritdoc/>
    public required string XML_ID { get; set; }

    /// <inheritdoc/>
    public required string NAME { get; set; }
}