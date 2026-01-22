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

    [Inject]
    IKladrNavigationService KladrRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public EntryAltStandardModel? KladrObject { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public EventCallback<EntryAltStandardModel?> KladrObjectChanged { get; set; }

    async Task UpdateKladrObject(EntryAltStandardModel? sender) => await KladrObjectChanged.InvokeAsync(sender);


    /// <inheritdoc/>
    [Parameter]
    public bool ReadOnly { get; set; }


    KladrResponseModel? CurrentKladrObject;

    async void ChangeSelectAction(KladrResponseModel sender)
    {
        CurrentKladrObject = sender;
        if (KladrObject is null)
            KladrObject = new() { Id = sender.Code, Name = sender.GetFullName() };
        else
            KladrObject?.Update(sender.Code, sender.GetFullName());
        StateHasChangedCall();
        await UpdateKladrObject(EntryAltStandardModel.Build(sender.Code, sender.GetFullName()));
    }

    async void ClearInput()
    {
        KladrObject = null;
        await UpdateKladrObject(null);
    }

    async Task<IDialogReference> OpenDialogAsync()
    {
        DialogParameters<KladrSelectDialogComponent> parameters = new()
        {
            { x => x.ChangeSelectHandle, ChangeSelectAction }
        };

        DialogOptions options = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large };
        IDialogReference res = await DialogService.ShowAsync<KladrSelectDialogComponent>("Выбор адреса:", parameters, options);
        await Actualize();
        return res;
    }

    /// <inheritdoc/>
    public async Task Actualize()
    {
        if (!string.IsNullOrWhiteSpace(KladrObject?.Id))
        {
            await SetBusyAsync();
            TResponseModel<KladrResponseModel> res = await KladrRepo.ObjectGetAsync(new() { Code = KladrObject.Id });
            if (!res.Success())
                SnackBarRepo.ShowMessagesResponse(res.Messages);
            CurrentKladrObject = res.Response;
            await SetBusyAsync(false);
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await Actualize();
    }
}