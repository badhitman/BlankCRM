////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectRequestBaseModel
/// </summary>
public class SelectRequestBaseModel
{
    /// <summary>
    /// ProjectId
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Строка поиска
    /// </summary>
    public string? SearchQuery { get; set; }
}