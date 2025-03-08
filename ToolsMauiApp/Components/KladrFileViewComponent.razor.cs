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


    /// <summary>
    /// OwnerComponent
    /// </summary>
    [Parameter, EditorRequired]
    public required IBrowserFile FileViewElement { get; set; }

    /// <summary>
    /// InitHandle
    /// </summary>
    [Parameter, EditorRequired]
    public required KladrUploadComponent OwnerComponent { get; set; }


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

        OwnerComponent.ChildBusyVote(1);
        OwnerComponent.InitHandleAction((this, FileViewElement.Name));

        await SetBusy();
        ms = new();
        await FileViewElement.OpenReadStream(long.MaxValue).CopyToAsync(ms);
        NumRecordsTotal = await Parser.Open(ms, FileViewElement.Name);
        await SeedDemo();
        OwnerComponent.ChildBusyVote(-1);
    }

    /// <inheritdoc/>
    public async Task UploadData()
    {
        OwnerComponent.ChildBusyVote(1);
        numRecordProgress = 1;
        await SetBusy();
        Parser.CurrentEncoding = Encoding.GetEncoding(currentEncoding);
        Parser.PartUploadNotify += ParserPartUploadNotify;
        await Parser.UploadData(true);
        Parser.PartUploadNotify -= ParserPartUploadNotify;
        await SetBusy(false);
        OwnerComponent.ChildBusyVote(-1);
    }

    private void ParserPartUploadNotify(int recordNum)
    {
        numRecordProgress = recordNum;
        InvokeAsync(StateHasChanged);
    }
}