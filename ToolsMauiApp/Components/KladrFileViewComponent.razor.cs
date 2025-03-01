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
        using Stream sm = FileViewElement.OpenReadStream(long.MaxValue);
        ParseDBF parser = new() { CurrentEncoding = Encoding.GetEncoding(currentEncoding) };
        await parser.Init(sm);
        DemoTable = await parser.GetRandomRowsAsDataTable(5);
        //(List<object[]> TableData, SharedLib.FieldDescriptorBase[] Columns) v = await parser.GetRandomRowsAsDataTable(5);
        //string v = JsonConvert.SerializeObject(DemoTable.Columns.ToArray().Cast<FieldDescriptor>().Select(x => new { x.fieldName, x.fieldType, x.fieldLen }));

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
        using Stream sm = FileViewElement.OpenReadStream(long.MaxValue);
        ParseDBF parser = new() { CurrentEncoding = Encoding.GetEncoding(currentEncoding) };
        await parser.Init(sm);
        await parser.UploadData(true, UploadPart, FinishUpload);
        await SetBusy(false);
    }

    // List<object[]> TableData
    async void UploadPart(FieldDescriptorBase[] Columns, List<object[]> tableData)
    {

    }

    async void FinishUpload(int totalRows)
    {

    }
}