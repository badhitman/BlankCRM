﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public class StorageCloudParameterReadModel : StorageMetadataModel
{
    /// <summary>
    /// Тип сериализуемого параметра
    /// </summary>
    public required string TypeName { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{base.ToString()}~{TypeName}";
    }
}