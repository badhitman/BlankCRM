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
    IClientHTTPRestService remoteClient { get; set; } = default!;


    private string _inputFileId = Guid.NewGuid().ToString();
    private readonly List<IBrowserFile> loadedFiles = [];

    void SelectFilesChange(InputFileChangeEventArgs e)
    {
        loadedFiles.Clear();

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
        //MemoryStream ms;
        foreach (IBrowserFile fileBrowser in loadedFiles)
        {
            //ms = new();
            //await fileBrowser.OpenReadStream(maxAllowedSize: 1024 * 18 * 1024).CopyToAsync(ms);

            //    req.Payload = ms.ToArray();
            //    req.ContentType = fileBrowser.ContentType;
            //    req.FileName = fileBrowser.Name;

            //await ms.DisposeAsync();
            //    res = await FilesRepo.SaveFile(new() { Payload = req, SenderActionUserId = CurrentUserSession.UserId });
            //    SnackbarRepo.ShowMessagesResponse(res.Messages);
        }

        loadedFiles.Clear();
        _inputFileId = Guid.NewGuid().ToString();
        await SetBusy(false);
    }
}