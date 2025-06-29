////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.ParametersShared;

/// <summary>
/// CKEditorParameterStorageComponent
/// </summary>
public partial class CKEditorParameterStorageComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IParametersStorageTransmission StoreRepo { get; set; } = default!;


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


    InputRichTextComponent _currentTemplateInputRichText_ref = default!;

    string? _textValue;
    string? TextValue
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
        //await SetBusy();
        //
        await StoreRepo.SaveParameterAsync(_textValue, KeyStorage, true);
        //IsBusyProgress = false;
        //StateHasChanged();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        TResponseModel<string?> res = await StoreRepo.ReadParameterAsync<string?>(KeyStorage);
        IsBusyProgress = false;
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        _textValue = res.Response;
    }
}