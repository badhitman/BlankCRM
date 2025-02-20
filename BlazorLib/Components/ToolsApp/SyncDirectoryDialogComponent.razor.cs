////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;

namespace BlazorLib.Components.ToolsApp;

public partial class SyncDirectoryDialogComponent : BlazorBusyComponentBaseModel
{
    /// <summary>
    /// SyncRuleId
    /// </summary>
    [Parameter, EditorRequired]
    public int SyncRuleId { get; set; }


    string? LocalDirectory { get; set; }


    string? RemoteDirectory { get; set; }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (SyncRuleId != 0)
        {
            await SetBusy();

            await SetBusy(false);
        }
    }
}