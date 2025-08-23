////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <inheritdoc/>
public class StorageBaseModel : StorageMetadataModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Создание
    /// </summary>
    public DateTime CreatedAt { get; set; }  
}