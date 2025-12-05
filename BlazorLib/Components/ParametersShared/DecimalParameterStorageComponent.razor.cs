////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.ParametersShared;

/// <summary>
/// DecimalParameterStorageComponent
/// </summary>
public partial class DecimalParameterStorageComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IParametersStorageTransmission StoreRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required string Label { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required StorageMetadataModel KeyStorage { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public decimal? InitValueIfNotExist { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public string? HelperText { get; set; }


    decimal _decimalValue;
    decimal DecimalValue
    {
        get => _decimalValue;
        set
        {
            _decimalValue = value;
            InvokeAsync(async () => { await StoreRepo.SaveParameterAsync(_decimalValue, KeyStorage, true); });
        }
    }


    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        TResponseModel<decimal?> res = await StoreRepo.ReadParameterAsync<decimal?>(KeyStorage);

        SnackBarRepo.ShowMessagesResponse(res.Messages.Where(x => x.TypeMessage > MessagesTypesEnum.Warning));

        if (InitValueIfNotExist.HasValue && res.Response is null)
        {
            res.Response = InitValueIfNotExist.Value;
            await StoreRepo.SaveParameterAsync(res.Response, KeyStorage, false);
        }

        _decimalValue = res.Response ?? 0;

        await SetBusyAsync(false);
    }
}