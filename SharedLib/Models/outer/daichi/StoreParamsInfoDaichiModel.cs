////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public class StoreParamsInfoDaichiModel
{
    /// <summary>
    /// Количество, больше которого не показывется точное значение. При превышении ограничения на портале точное значение заменяется выражением ‘>’
    /// </summary>
    /// <remarks>
    /// (например, > 50 на складе)
    /// </remarks>
    public int LIMIT { get; set; }

    /// <summary>
    /// false (лимит не превышен. STORE_AMOUNT показывает точное значение)
    /// true (лимит превышен. STORE_AMOUNT ограничено значением LIMIT)
    /// </summary>
    public bool HIDE_MORE_LIMIT { get; set; }
}