﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;
using MudBlazor;

namespace BlazorWebLib.Components.ParametersShared;

/// <summary>
/// DecimalParameterStorageComponent
/// </summary>
public partial class DecimalParameterStorageComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IStorageTransmission StoreRepo { get; set; } = default!;


    /// <summary>
    /// Label
    /// </summary>
    [Parameter, EditorRequired]
    public required string Label { get; set; }

    /// <summary>
    /// KeyStorage
    /// </summary>
    [Parameter, EditorRequired]
    public required StorageMetadataModel KeyStorage { get; set; }

    /// <summary>
    /// HelperText
    /// </summary>
    [Parameter]
    public string? HelperText { get; set; }


    decimal _decimalValue;
    decimal DecimalValue
    {
        get => _decimalValue;
        set
        {
            _decimalValue = value;
            InvokeAsync(async () => { await StoreRepo.SaveParameterAsync(_decimalValue, KeyStorage, false); });
        }
    }


    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusy();
        TResponseModel<decimal?> res = await StoreRepo.ReadParameterAsync<decimal?>(KeyStorage);
        IsBusyProgress = false;
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        _decimalValue = res.Response ?? 0;
    }
}