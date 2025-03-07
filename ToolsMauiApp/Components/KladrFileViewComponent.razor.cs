////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using ToolsMauiLib;
using System.Text;
using BlazorLib;
using SharedLib;

namespace ToolsMauiApp.Components;

/// <inheritdoc/>
public partial class KladrFileViewComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ParseDBF Parser { get; set; } = default!;

    [Inject]
    IClientHTTPRestService RemoteClient { get; set; } = default!;


    /// <summary>
    /// FileViewElement
    /// </summary>
    [Parameter, EditorRequired]
    public required IBrowserFile FileViewElement { get; set; }

    /// <summary>
    /// InitHandle
    /// </summary>
    [Parameter, EditorRequired]
    public required Action<(KladrFileViewComponent ParentComponent, string FileName)> InitHandle { get; set; }


    /// <inheritdoc/>
    public static readonly string[] KladrFiles = [.. Enum.GetNames<KladrFilesEnum>()];


    int NumRecordsTotal, numRecordProgress;
    (List<object[]> TableData, FieldDescriptorBase[] Columns)? DemoTable;

    MemoryStream ms = default!;
    string currentEncoding = "cp866";

    /// <inheritdoc/>
    public async Task SeedDemo(string enc = "cp866")
    {
        currentEncoding = enc;
        Parser.CurrentEncoding = Encoding.GetEncoding(currentEncoding);

        if (!IsBusyProgress)
            await SetBusy();

        DemoTable = await Parser.GetRandomRowsAsDataTable(5);
        await SetBusy(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!KladrFiles.Any(x => $"{x}.dbf".Equals(FileViewElement.Name, StringComparison.OrdinalIgnoreCase)))
            return;

        await SetBusy();
        ms = new();
        await FileViewElement.OpenReadStream(long.MaxValue).CopyToAsync(ms);
        NumRecordsTotal = await Parser.Open(ms, FileViewElement.Name);
        await SeedDemo();
        InitHandle((this, FileViewElement.Name));
    }

    /// <inheritdoc/>
    public async Task UploadData()
    {
        numRecordProgress = 1;
        await SetBusy();
        await RemoteClient.ClearTempKladr();
        Parser.CurrentEncoding = Encoding.GetEncoding(currentEncoding);
        Parser.PartUploadNotify += ParserPartUploadNotify;
        await Parser.UploadData(true);
        Parser.PartUploadNotify -= ParserPartUploadNotify;
        await SetBusy(false);
    }

    private void ParserPartUploadNotify(int recordNum)
    {
        numRecordProgress = recordNum;
        InvokeAsync(StateHasChanged);
    }
}