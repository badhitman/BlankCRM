////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// FindStorageBaseModel
/// </summary>
public class FindStorageBaseModel : RequestStorageBaseModel
{
    /// <summary>
    /// OwnersPrimaryKeys
    /// </summary>
    public int[]? OwnersPrimaryKeys { get; set; }
}