////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// AdaptersRequestModel
/// </summary>
public class AdaptersRequestModel
{
    /// <summary>
    /// Только активные (фильтр)?
    /// true - для получения только активных адаптеров.
    /// false - будут возвращены только неактивные адаптеры.
    /// (по умолчанию: NULL)
    /// </summary>
    /// <remarks>
    /// если NULL (по умолчанию) - тогда в выдачу попадают все (в этом случае данная фильтрация игнорируется)
    /// </remarks>
    public bool? OnlineOnly { get; set; }
}