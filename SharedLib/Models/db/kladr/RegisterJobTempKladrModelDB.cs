////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <inheritdoc/>
[Index(nameof(Name), IsUnique = true)]
public class RegisterJobTempKladrModelDB : EntryModel
{
    /// <inheritdoc/>
    public required int VoteValue { get; set; }
}