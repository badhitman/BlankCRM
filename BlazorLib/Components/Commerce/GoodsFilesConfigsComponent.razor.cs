////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Commerce;

/// <summary>
/// GoodsFilesConfigsComponent
/// </summary>
public partial class GoodsFilesConfigsComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IStorageTransmission FilesRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required string OwnerTypeName { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required FilesContextViewComponent FilesContextViewComponent { get; set; }


    List<StorageFileModelDB>? FilesList;
    List<FileGoodsConfigModelDB>? FilesConfigs;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await ReloadData();
    }

    async Task ReloadData()
    {

        await SetBusyAsync();
        TPaginationRequestStandardModel<SelectMetadataRequestModel> req = new()
        {
            Payload = new()
            {
                ApplicationsNames = FilesContextViewComponent.ApplicationsNames,
                IdentityUsersIds = [],
                PropertyName = FilesContextViewComponent.PropertyName,
                OwnerPrimaryKey = FilesContextViewComponent.OwnerPrimaryKey,
                PrefixPropertyName = FilesContextViewComponent.PrefixPropertyName,
            },
            PageNum = 0,
            PageSize = int.MaxValue,
        };

        TPaginationResponseStandardModel<StorageFileModelDB> rest = await FilesRepo
            .FilesSelectAsync(req);

        FilesList = rest.Response;
        await SetBusyAsync(false);
    }
}