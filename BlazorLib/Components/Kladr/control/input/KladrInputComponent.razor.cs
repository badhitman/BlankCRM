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
    public required Action<KladrResponseModel?> ChangeSelectHandle { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public EntryAltModel? KladrObject { get; set; }


    void ChangeSelectAction(KladrResponseModel sender)
    {
        if (KladrObject is null)
            KladrObject = new() { Id = sender.Code, Name = sender.GetFullName() };
        else
            KladrObject?.Update(sender.Code, sender.GetFullName());
        StateHasChangedCall();        
        ChangeSelectHandle(sender);
    }

    void ClearInput()
    {
        KladrObject = null;
        ChangeSelectHandle(null);
    }

    async Task<IDialogReference> OpenDialogAsync()
    {
        DialogParameters<KladrSelectDialogComponent> parameters = new()
        {
            { x => x.ChangeSelectHandle, ChangeSelectAction }
        };

        DialogOptions options = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large };
        IDialogReference res = await DialogService.ShowAsync<KladrSelectDialogComponent>("Выбор адреса:", parameters, options);

        return res;
    }
}