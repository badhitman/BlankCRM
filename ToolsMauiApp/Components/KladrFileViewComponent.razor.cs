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

    string currentEncoding = "cp866";

    /// <inheritdoc/>
    public async Task SeedDemo(string enc = "cp866")
    {
        currentEncoding = enc;
        await SetBusy();
        MemoryStream ms = new();
        await FileViewElement.OpenReadStream(long.MaxValue).CopyToAsync(ms);
        parser.CurrentEncoding = Encoding.GetEncoding(currentEncoding);
        await parser.Open(ms);
        DemoTable = await parser.GetRandomRowsAsDataTable(5);
        await SetBusy(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SeedDemo();
        InitHandle(this);
    }

    /// <inheritdoc/>
    public async Task UploadData()
    {
        await SetBusy();
        MemoryStream ms = new();
        await FileViewElement.OpenReadStream(long.MaxValue).CopyToAsync(ms);
        parser.CurrentEncoding = Encoding.GetEncoding(currentEncoding);
        //await parser.Open(ms);
        await parser.UploadData(true);
        await SetBusy(false);
    }
}