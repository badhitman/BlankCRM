////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Конфигурация хоста
/// </summary>
public class HostConfigModel : HostConfigBaseModel
{
    /// <summary>
    /// Схема (https/ssl)
    /// </summary>
    public string Scheme { get; set; } = "http";

    /// <summary>
    /// Преобразовать в строку конфигурации хоста
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{Scheme}://{Host}{(Port == 80 ? string.Empty : $":{Port}")}";
}