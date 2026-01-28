////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Прочитать массив байт слева и справа от указанной точки указанного размера в байтах
/// </summary>
public class ReadFileDataAboutPositionRequestModel
{
    /// <summary>
    /// Путь к файлу
    /// </summary>
    public required string FileFullPath { get; set; }

    /// <summary>
    /// Точка от которой читать данные
    /// </summary>
    public long Position { get; set; }

    /// <summary>
    /// Желаемый размер данных в каждом из направлений от точки (в начало и в конец)
    /// </summary>
    public uint SizeArea { get; set; }
}