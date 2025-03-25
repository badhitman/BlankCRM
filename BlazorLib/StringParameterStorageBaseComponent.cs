////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib;

/// <summary>
/// StringParameterStorageBaseComponent
/// </summary>
public class StringParameterStorageBaseComponent : BlazorBusyComponentBaseModel
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


    string? _textValue;
    /// <summary>
    /// TextValue
    /// </summary>
    protected string? TextValue
    {
        get => _textValue;
        set
        {
            _textValue = value;
            InvokeAsync(StoreData);
        }
    }

    async Task StoreData()
    {
        await SetBusyAsync();

        await StoreRepo.SaveParameterAsync(_textValue, KeyStorage, false);
        IsBusyProgress = false;
        StateHasChanged();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        TResponseModel<string?> res = await StoreRepo.ReadParameterAsync<string?>(KeyStorage);
        IsBusyProgress = false;
        if (!res.Success())
            SnackbarRepo.ShowMessagesResponse(res.Messages);

        _textValue = res.Response;
    }
}