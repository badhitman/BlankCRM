////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.ParametersShared;

/// <summary>
/// Bool simple storage component
/// </summary>
public partial class BoolSimpleStorageComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IParametersStorageTransmission StorageTransmissionRepo { get; set; } = default!;


    /// <summary>
    /// Title
    /// </summary>
    [Parameter, EditorRequired]
    public required string Title { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    [Parameter, EditorRequired]
    public required string Label { get; set; }

    /// <summary>
    /// Storage metadata
    /// </summary>
    [Parameter, EditorRequired]
    public required StorageMetadataModel StorageMetadata { get; set; }

    /// <summary>
    /// Hint for true
    /// </summary>
    [Parameter]
    public string? HintTrue { get; set; }

    /// <summary>
    /// Hint for false
    /// </summary>
    [Parameter]
    public string? HintFalse { get; set; }


    bool _storeValue;
    bool StoreValue
    {
        get => _storeValue;
        set
        {
            _storeValue = value;
            InvokeAsync(SaveParameter);
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        TResponseModel<bool?> showCreatingIssue = await StorageTransmissionRepo.ReadParameterAsync<bool?>(StorageMetadata);
        _storeValue = showCreatingIssue.Success() && showCreatingIssue.Response == true;
        await SetBusyAsync(false);
    }

    async void SaveParameter()
    {
        await SetBusyAsync();
        TResponseModel<int> res = await StorageTransmissionRepo.SaveParameterAsync<bool?>(StoreValue, StorageMetadata, true);
        SnackBarRepo.ShowMessagesResponse(res.Messages.Where(x => x.TypeMessage > MessagesTypesEnum.Info));
        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
    }
}