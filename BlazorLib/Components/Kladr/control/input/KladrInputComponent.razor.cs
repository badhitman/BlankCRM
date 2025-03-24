////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Kladr.control.input;

/// <summary>
/// KladrInputComponent
/// </summary>
public partial class KladrInputComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Inject]
    IDialogService DialogService { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required Action<KladrResponseModel> ChangeSelectHandle { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public EntryAltModel? KladrObject { get; set; }


    private Task<IDialogReference> OpenDialogAsync()
    {
        DialogOptions options = new() { CloseOnEscapeKey = true };

        return DialogService.ShowAsync<KladrSelectDialogComponent>("Simple Dialog", options);
    }
}