////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// AllFilesMetadataModel
/// </summary>
public class FilesAreaMetadataModel
{
    /// <summary>
    /// ApplicationName
    /// </summary>
    public string? ApplicationName { get; set; }

    /// <summary>
    /// Количество файлов
    /// </summary>
    public int CountFiles { get; set; }

    /// <summary>
    /// Размер файлов (суммарно)
    /// </summary>
    public long SizeFilesSum { get; set; }
}