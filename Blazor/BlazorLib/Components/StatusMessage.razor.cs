////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components;

/// <inheritdoc/>
public partial class StatusMessage : ComponentBase
{
    /// <summary>
    /// Сообщения для вывода
    /// </summary>
    [Parameter]
    public IEnumerable<ResultMessage>? Messages { get; set; }

    /// <inheritdoc/>
    public static string getCssItem(MessagesTypesEnum res_type) => res_type switch
    {
        MessagesTypesEnum.Success => "success",
        MessagesTypesEnum.Info => "primary",
        MessagesTypesEnum.Warning => "warning",
        MessagesTypesEnum.Error => "danger",
        _ => "secondary"
    };
}