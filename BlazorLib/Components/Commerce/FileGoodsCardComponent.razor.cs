////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Commerce;

/// <summary>
/// FileGoodsCardComponent
/// </summary>
public partial class FileGoodsCardComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required FileGoodsConfigModelDB FileObjectConfig { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required StorageFileModelDB FileObject { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required Action ReloadHandler { get; set; }


    string _fileName = "/img/noimage-simple.png";
    FileGoodsConfigModelDB fileObjectEdit = default!;
    bool IsEdited => fileObjectEdit.IsGallery != FileObjectConfig.IsGallery ||
                     fileObjectEdit.Name != FileObjectConfig.Name ||
                     fileObjectEdit.FullDescription != FileObjectConfig.FullDescription ||
                     fileObjectEdit.ShortDescription != FileObjectConfig.ShortDescription;

    async Task SaveConfig()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await CommerceRepo.FileForGoodUpdateAsync(new()
        {
            Payload = fileObjectEdit,
            SenderActionUserId = CurrentUserSession?.UserId
        });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
        ReloadHandler();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        if (GlobalToolsStandard.IsImageFile(FileObject.FileName))
            _fileName = $"/cloud-fs/read/{FileObject.Id}/{FileObject.FileName}";

        fileObjectEdit = GlobalTools.CreateDeepCopy(FileObjectConfig)!;
        await base.OnInitializedAsync();
    }

    //void HandleOnChange(ChangeEventArgs args)
    //{
    //    fileObjectEdit.FullDescription = args.Value?.ToString();
    //}
}