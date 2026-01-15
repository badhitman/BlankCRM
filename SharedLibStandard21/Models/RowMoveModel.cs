////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Сдвинут/переместить строку (на один шаг)
/// </summary>
public class RowMoveModel
{
    /// <summary>
    /// Object Id (row primary key)
    /// </summary>
    /// <remarks>
    /// Идентификатор строки
    /// </remarks>
    public int RowObjectId { get; set; }

    /// <summary>
    /// Направление сдвига/перемещения
    /// </summary>
    public DirectionsEnum Direction { get; set; }

    /// <summary>
    /// ContextName
    /// </summary>
    public string? ContextName { get; set; }
}