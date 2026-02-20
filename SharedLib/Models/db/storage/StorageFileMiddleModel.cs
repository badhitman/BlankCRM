////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Хранимый файл (локальное хранилище)
/// </summary>
[Index(nameof(PointId)), Index(nameof(AuthorIdentityId)), Index(nameof(FileName))]
[Index(nameof(CreatedAt))]
[Index(nameof(PrefixPropertyName), nameof(OwnerPrimaryKey))]
[Index(nameof(ApplicationName), nameof(PropertyName))]
public class StorageFileMiddleModel : StorageBaseModel
{
    /// <summary>
    /// AuthorIdentityId
    /// </summary>
    public required string AuthorIdentityId { get; set; }

    /// <summary>
    /// PointId (grid-fs)
    /// </summary>
    public required string PointId { get; set; }

    /// <summary>
    /// FileName
    /// </summary>
    public required string FileName { get; set; }
}