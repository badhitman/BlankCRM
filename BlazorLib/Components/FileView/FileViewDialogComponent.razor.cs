////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.FileView;

public partial class FileViewDialogComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IStorageTransmission StorageRepo { get; set; } = default!;


    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; } = default!;

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required DirectoryItemModel DirectoryItem { get; set; }


    string FileSizeString = "0", _valueSliderString = "0", RawData = "";

    long _valueSlider;
    long ValueSlider
    {
        get => _valueSlider;
        set
        {
            _valueSlider = value;
            _valueSliderString = GlobalToolsStandard.SizeDataAsString(ValueSlider);
            StateHasChanged();
        }
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        FileSizeString = GlobalToolsStandard.SizeDataAsString(DirectoryItem.FileSizeBytes);
    }

    void Submit() => MudDialog.Close(DialogResult.Ok(true));

    void Cancel() => MudDialog.Cancel();
}