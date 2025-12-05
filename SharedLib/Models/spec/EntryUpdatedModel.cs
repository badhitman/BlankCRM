////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Базовая DB модель объекта с поддержкой -> int:Id +string:Name AND UpdatedAt
/// </summary>
public class EntryUpdatedModel : EntryUpdatedLiteModel
{
    /// <summary>
    /// Описание/примечание для объекта
    /// </summary>
    public string? Description { get; set; }
}