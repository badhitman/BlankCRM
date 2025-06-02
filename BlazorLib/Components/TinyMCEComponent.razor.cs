﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components;

/// <summary>
/// TinyMCEComponent
/// </summary>
public partial class TinyMCEComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IParametersStorageTransmission StoreRepo { get; set; } = default!;

    [Inject]
    NavigationManager NavRepo { get; set; } = default!;


    /// <summary>
    /// ReadOnly
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; }

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


    string images_upload_url = default!;
    Dictionary<string, object> editorConf = default!;

    string? _textValue;
    string? TextValue
    {
        get => _textValue;
        set
        {
            bool nu = _textValue != value;
            _textValue = value;
            if (nu)
                InvokeAsync(StoreData);
        }
    }

    async Task StoreData()
    {
        //await SetBusy();
        await StoreRepo.SaveParameterAsync(_textValue, KeyStorage, true);
        //await SetBusy(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        images_upload_url = $"{GlobalStaticConstants.TinyMCEditorUploadImage}{KeyStorage.ApplicationName.Replace("\\", "_").Replace("/", "_")}/{KeyStorage.PropertyName.Replace("\\", "_").Replace("/", "_")}?{nameof(KeyStorage.PrefixPropertyName)}={KeyStorage.PrefixPropertyName}&{nameof(KeyStorage.OwnerPrimaryKey)}={KeyStorage.OwnerPrimaryKey}";
        editorConf = GlobalStaticConstants.TinyMCEditorConf(images_upload_url);

        await SetBusyAsync();
        TResponseModel<string?> res = await StoreRepo.ReadParameterAsync<string?>(KeyStorage);

        if (!res.Success())
            SnackbarRepo.ShowMessagesResponse(res.Messages);

        _textValue = res.Response;
        IsBusyProgress = false;
    }
}