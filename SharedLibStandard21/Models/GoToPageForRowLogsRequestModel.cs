////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// GoToPageForRowLogsRequestModel
/// </summary>
public class GoToPageForRowLogsRequestModel
{
    /// <inheritdoc/>
    public int RowId { get; set; }

    /// <summary>
    /// Без загрузки данных строк таблицы
    /// </summary>
    /// <remarks>
    /// Например для определения номера страницы искомой строки
    /// </remarks>
    public bool WithOutRowsData { get; set; }
}