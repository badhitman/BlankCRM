////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using ToolsMauiLib;
using System.Collections;
using System.Data;
using System.Text;

namespace ToolsMauiApp.Components;

/// <inheritdoc/>
public partial class KladrFileViewComponent
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


    (DataTable TableData, ArrayList Columns) DemoTable = default!;

    /// <inheritdoc/>
    public async Task SeedDemo(string value = "cp866")
    {
        using Stream sm = FileViewElement.OpenReadStream(long.MaxValue);
        ParseDBF parser = new() { CurrentEncoding = Encoding.GetEncoding(value) };
        await parser.Init(sm);
        DemoTable = await parser.GetRandomRowsAsDataTable(10);
        StateHasChanged();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SeedDemo();
        InitHandle(this);
    }
}