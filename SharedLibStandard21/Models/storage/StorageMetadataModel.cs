////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.IO;

namespace SharedLib;

/// <summary>
/// StorageMetadataModel
/// </summary>

public class StorageMetadataModel : RequestStorageBaseModel
{
    /// <summary>
    /// Префикс имени (опционально)
    /// </summary>
    public string? PrefixPropertyName { get; set; }

    /// <summary>
    /// Связанный PK строки базы данных (опционально)
    /// </summary>
    public int? OwnerPrimaryKey { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        Normalize();
        return $"{PrefixPropertyName}:{OwnerPrimaryKey}:{PropertyName}:{ApplicationName}";
    }

    /// <summary>
    /// Normalize
    /// </summary>
    public override void Normalize()
    {
        base.Normalize();
        PrefixPropertyName = PrefixPropertyName?.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
    }
}