////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;

namespace BlazorLib.Components.Kladr.control;

/// <summary>
/// KladrFindInputComponent
/// </summary>
public partial class KladrFindInputComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IDialogService DialogService { get; set; } = default!;

    string? FindText { get; set; }

    Task<IDialogReference> OpenDialogAsync()
    {
        DialogParameters<KladrFindDialogComponent> parameters = new()
        {
            { x => x.FindText, FindText }
        };

        DialogOptions options = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        return DialogService.ShowAsync<KladrFindDialogComponent>("Поиск адреса", parameters, options);
    }
}