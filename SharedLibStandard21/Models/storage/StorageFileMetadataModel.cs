////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Collections.Generic;

namespace SharedLib;

/// <summary>
/// StorageImageMetadataModel
/// </summary>
public class StorageFileMetadataModel : StorageMetadataModel
{
    /// <summary>
    /// FileName
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// ContentType
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// Адрес страницы, с которой была произведена загрузка
    /// </summary>
    public string? Referrer { get; set; }

    /// <summary>
    /// Payload
    /// </summary>
    public byte[]? Payload { get; set; }

    /// <summary>
    /// Параметры доступа к файлу
    /// </summary>
    public Dictionary<FileAccessRulesTypesEnum, List<string>>? RulesTypes { get; set; }
}