////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// TagModelDB
/// </summary>
[Index(nameof(NormalizedTagNameUpper))]
[Index(nameof(NormalizedTagNameUpper), nameof(OwnerPrimaryKey), nameof(ApplicationName), IsUnique = true, Name = "IX_TagNameOwnerKeyUnique")]
[Index(nameof(CreatedAt))]
[Index(nameof(PrefixPropertyName), nameof(OwnerPrimaryKey))]
[Index(nameof(ApplicationName), nameof(PropertyName))]
public class TagModelDB : TagViewModel
{
    
}