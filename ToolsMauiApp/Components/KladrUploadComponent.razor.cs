////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components.Forms;
using BlazorLib;
using Microsoft.AspNetCore.Components;
using SharedLib;

namespace ToolsMauiApp.Components;

/// <summary>
/// KladrUploadComponent
/// </summary>
public partial class KladrUploadComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IClientHTTPRestService RemoteClient { get; set; } = default!;

    private string _inputFileId = Guid.NewGuid().ToString();
    private readonly List<IBrowserFile> loadedFiles = [];

    private readonly List<(KladrFileViewComponent ParentComponent, string FileName)> ViewsChilds = [];

    bool CanSend
    {
        get
        {
            return loadedFiles.All(x => KladrFileViewComponent.KladrFiles.Any(y => $"{y}.dbf".Equals(x.Name, StringComparison.OrdinalIgnoreCase))) &&
                KladrFileViewComponent.KladrFiles.All(x => loadedFiles.Any(y => y.Name.Equals($"{x}.dbf", StringComparison.OrdinalIgnoreCase)));
        }
    }

    string _value = "cp866";
    string SelectedEncoding
    {
        get => _value;
        set
        {
            _value = value;

            InvokeAsync(async () => await SetBusy());
            InvokeAsync(async () => await Task.WhenAll(ViewsChilds.Select(x => Task.Run(async () => await x.ParentComponent.SeedDemo(_value)))));
            InvokeAsync(async () => await SetBusy(false));
        }
    }

    void InitHandleAction((KladrFileViewComponent ParentComponent, string FileName) sender)
    {
        ViewsChilds.Add(sender);
    }

    void SelectFilesChange(InputFileChangeEventArgs e)
    {
        loadedFiles.Clear();
        ViewsChilds.Clear();
        foreach (IBrowserFile file in e.GetMultipleFiles())
        {
            loadedFiles.Add(file);
        }
    }

    async Task SendFile()
    {
        if (loadedFiles.Count == 0)
            throw new Exception();

        await SetBusy();
        await RemoteClient.ClearTempKladr();
        await Task.WhenAll(ViewsChilds.Select(x => Task.Run(async () => await x.ParentComponent.UploadData())));
        loadedFiles.Clear();
        _inputFileId = Guid.NewGuid().ToString();
        await SetBusy(false);
    }
}