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
    ParseDBF parser { get; set; } = default!;


    /// <summary>
    /// FileViewElement
    /// </summary>
    [Parameter, EditorRequired]
    public required IBrowserFile FileViewElement { get; set; }

    /// <summary>
    /// InitHandle
    /// </summary>
    [Parameter, EditorRequired]
    public required Action<KladrFileViewComponent> InitHandle { get; set; }


    (List<object[]> TableData, FieldDescriptorBase[] Columns) DemoTable = default!;
    MemoryStream ms = default!;
    string currentEncoding = "cp866";

    /// <inheritdoc/>
    public async Task SeedDemo(string enc = "cp866")
    {
        currentEncoding = enc;
        parser.CurrentEncoding = Encoding.GetEncoding(currentEncoding);
        await SetBusy();
        DemoTable = await parser.GetRandomRowsAsDataTable(5);
        await SetBusy(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusy();
        ms = new();
        await FileViewElement.OpenReadStream(long.MaxValue).CopyToAsync(ms);
        await parser.Open(ms);
        await SeedDemo();
        await SetBusy(false);
        InitHandle(this);
    }

    /// <inheritdoc/>
    public async Task UploadData()
    {
        await SetBusy();
        parser.CurrentEncoding = Encoding.GetEncoding(currentEncoding);
        //await parser.Open(ms);
        await parser.UploadData(true);
        await SetBusy(false);
    }
}