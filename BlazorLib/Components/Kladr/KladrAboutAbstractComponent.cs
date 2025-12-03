////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace BlazorLib.Components.Kladr;

/// <summary>
/// KladrAboutAbstractComponent
/// </summary>
public abstract class KladrAboutAbstractComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    protected MetadataKladrModel? tmp, prod;


    /// <summary>
    /// Отправить данные из временных таблиц в основную КЛАДР
    /// </summary>
    protected abstract Task TransitData();

    /// <summary>
    /// Очистить временные таблицы КЛАДР
    /// </summary>
    protected abstract Task ClearTempTables();

    /// <summary>
    /// Получить метаданные/состояние таблиц КЛАДР
    /// </summary>
    protected abstract Task ReloadData();

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await ReloadData();
    }
}