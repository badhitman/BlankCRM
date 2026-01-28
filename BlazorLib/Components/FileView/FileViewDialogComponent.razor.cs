////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;
using System.Text;

namespace BlazorLib.Components.FileView;

/// <summary>
/// FileViewDialogComponent
/// </summary>
public partial class FileViewDialogComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IStorageTransmission StorageRepo { get; set; } = default!;


    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; } = default!;

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required DirectoryItemModel DirectoryItem { get; set; }


    string
        FileSizeString = "0",
        _valueSliderString = "0",
        RawDataStart = "",
        RawDataEnd = "";

    CancellationTokenSource cts = new();
    CancellationToken token = default!;

    Dictionary<DirectionsEnum, byte[]>? rawData;
    Encoding EncodingMode { get; set; } = Encoding.UTF8;

    EncodingsEnum _selectedEncoding = EncodingsEnum.UTF8;
    EncodingsEnum SelectedEncoding
    {
        get => _selectedEncoding;
        set
        {
            _selectedEncoding = value;
            EncodingMode = _selectedEncoding.ToString().ToLower() switch
            {
                "utf8" => Encoding.UTF8,
                "ascii" => Encoding.ASCII,
                "unicode" => Encoding.Unicode,
                "bigendianunicode" => Encoding.BigEndianUnicode,
                "utf32" => Encoding.UTF32,
                _ => Encoding.Default,
            };
            FlushEncoding();
        }
    }

    long _valueSlider;
    long ValueSlider
    {
        get => _valueSlider;
        set
        {
            _valueSlider = value;
            _valueSliderString = GlobalToolsStandard.SizeDataAsString(ValueSlider);
            InvokeAsync(ReadFileDataAboutPositionAsync);
        }
    }

    async Task ReadFileDataAboutPositionAsync()
    {
        cts.Cancel();
        token = cts.Token;
        TResponseModel<Dictionary<DirectionsEnum, byte[]>> res = await StorageRepo.ReadFileDataAboutPositionAsync(new() 
        { 
            FileFullPath = DirectoryItem.FullPath, 
            Position = ValueSlider, 
            SizeArea = 512
        }, token);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        rawData = res.Response;

        FlushEncoding();
    }

    void FlushEncoding()
    {
        RawDataStart = "";
        RawDataEnd = "";

        if (rawData is null)
            return;

        foreach (KeyValuePair<DirectionsEnum, byte[]> nodeElement in rawData)
        {
            switch (nodeElement.Key)
            {
                case DirectionsEnum.Up:
                    RawDataStart = EncodingMode.GetString([.. nodeElement.Value]);
                    break;
                case DirectionsEnum.Down:
                    RawDataEnd = EncodingMode.GetString([.. nodeElement.Value]);
                    break;
            }
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        base.OnInitialized();
        FileSizeString = GlobalToolsStandard.SizeDataAsString(DirectoryItem.FileSizeBytes);
        await ReadFileDataAboutPositionAsync();
    }

    void Submit() => MudDialog.Close(DialogResult.Ok(true));

    void Cancel() => MudDialog.Cancel();
}