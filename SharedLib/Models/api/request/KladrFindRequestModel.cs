////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// KladrFindRequestModel
/// </summary>
public class KladrFindRequestModel: KladrsRequestBaseModel
{
    /// <summary>
    /// строка поиска
    /// </summary>
    public string? SearchQuery { get; set; }
}