﻿////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;

#if DEBUG

#endif
using MudBlazor;

namespace BlazorLib;

/// <summary>
/// Базовый компонент с поддержкой состояния "занят". Компоненты, которые выполняют запросы
/// на время обработки переходят в состояние "IsBusyProgress" с целью обеспечения визуализации смены этого изменения
/// </summary>
public abstract class BlazorBusyComponentBaseModel : ComponentBase, IDisposable
{
    /// <summary>
    /// Snackbar
    /// </summary>
    [Inject]
    public ISnackbar SnackBarRepo { get; set; } = default!;


    bool _isBusyProgress;
    /// <summary>
    /// Компонент занят отправкой REST запроса и обработки ответа
    /// </summary>
    public virtual bool IsBusyProgress
    {
        get => _isBusyProgress;
        set
        {
            //#if DEBUG
            //            Logger.LogDebug($"{nameof(IsBusyProgress)}:{value}");
            //#endif
            _isBusyProgress = value;
        }
    }

    /// <summary>
    /// SetBusy
    /// </summary>
    public async Task SetBusyAsync(bool is_busy = true, CancellationToken token = default)
    {
        _isBusyProgress = is_busy;
        await InvokeAsync(StateHasChanged);

        try
        {
            await Task.Delay(1, token);
        }
        finally
        {
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Уведомляет компонент об изменении его состояния.
    /// Когда применимо, это вызовет повторную визуализацию компонента.
    /// </summary>
    public virtual void StateHasChangedCall() => InvokeAsync(StateHasChanged);

    /// <summary>
    /// Signals to a System.Threading.CancellationToken that it should be canceled.
    /// </summary>
    protected CancellationTokenSource? _cts;
    /// <summary>
    /// Propagates notification that operations should be canceled.
    /// </summary>
    protected CancellationToken CancellationToken => (_cts ??= new()).Token;

    /// <inheritdoc/>
    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
        _cts?.Cancel();
        _cts?.Dispose();
    }
}