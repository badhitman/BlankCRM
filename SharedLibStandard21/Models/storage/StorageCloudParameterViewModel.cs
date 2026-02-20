////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// StorageCloudParameterViewModel
/// </summary>
public class StorageCloudParameterViewModel : StorageBaseModel
{
    /// <summary>
    /// Данные (сериализованные)
    /// </summary>
    public string? SerializedDataJson { get; set; }

    /// <summary>
    /// Тип сериализуемого параметра
    /// </summary>
    public string? TypeName { get; set; }
}