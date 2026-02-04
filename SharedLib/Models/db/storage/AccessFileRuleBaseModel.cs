////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Правило доступа к файлу
/// </summary>
public class AccessFileRuleBaseModel
{
    /// <summary>
    /// AccessRuleType
    /// </summary>
    public FileAccessRulesTypesEnum AccessRuleType { get; set; }

    /// <summary>
    /// Option
    /// </summary>
    public required string Option { get; set; }
}