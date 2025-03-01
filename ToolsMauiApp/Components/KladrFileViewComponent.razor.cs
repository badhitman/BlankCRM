////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using ToolsMauiLib;
using System.Collections;
using System.Data;

namespace ToolsMauiApp.Components;

/// <inheritdoc/>
public partial class KladrFileViewComponent
{
    /// <summary>
    /// FileViewElement
    /// </summary>
    [Parameter, EditorRequired]
    public required IBrowserFile FileViewElement { get; set; }

    (DataTable TableData, ArrayList Columns) DemoTable = default!;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        using Stream sm = FileViewElement.OpenReadStream(long.MaxValue);
        ParseDBF parser = new();
        await parser.Init(sm);
        DemoTable = await parser.GetRandomRowsAsDataTable(10);
    }
}