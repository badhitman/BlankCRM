﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components.Forms;
using BlazorLib;

namespace ToolsMauiApp.Components;

/// <summary>
/// KladrUploadComponent
/// </summary>
public partial class KladrUploadComponent : BlazorBusyComponentBaseModel
{
    private string _inputFileId = Guid.NewGuid().ToString();
    private readonly List<IBrowserFile> loadedFiles = [];

    private readonly List<KladrFileViewComponent> ViewsChilds = [];

    string _value = "cp866";
    string SelectedEncoding
    {
        get => _value;
        set
        {
            _value = value;

            InvokeAsync(async () => await SetBusy());
            InvokeAsync(async () => await Task.WhenAll(ViewsChilds.Select(x => Task.Run(async () => await x.SeedDemo(_value)))));
            InvokeAsync(async () => await SetBusy(false));
        }
    }

    void InitHandleAction(KladrFileViewComponent sender)
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
        await Task.WhenAll(ViewsChilds.Select(x => Task.Run(async () => await x.UploadData())));
        loadedFiles.Clear();
        _inputFileId = Guid.NewGuid().ToString();
        await SetBusy(false);
    }

    
}