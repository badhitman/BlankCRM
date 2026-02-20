////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;
using System.Globalization;

namespace BlazorLib.Components.Commerce;

/// <summary>
/// GoodsFilesConfigsComponent
/// </summary>
public partial class GoodsFilesConfigsComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IStorageTransmission FilesRepo { get; set; } = default!;

    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required string OwnerTypeName { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required int OwnerId { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required FilesContextViewComponent FilesContextViewComponent { get; set; }


    List<StorageFileModelDB>? FilesList;
    List<FileGoodsConfigModelDB>? FilesConfigs;

    IEnumerable<StorageFileModelDB> _selectedFiles = new HashSet<StorageFileModelDB>();
    IEnumerable<StorageFileModelDB> SelectedFiles
    {
        get => _selectedFiles;
        set
        {
            _selectedFiles = value;
            InvokeAsync(FilesSelectedSet);
        }
    }

    MoveRowStatesEnum GetMoveStatus(FileGoodsConfigModelDB conf, StorageFileModelDB file)
    {
        if (FilesConfigs is null || FilesConfigs.Count <= 1 || FilesList is null)
            return MoveRowStatesEnum.Singleton;

        bool _isGal = GlobalToolsStandard.IsImageFile(file.FileName);
        List<FileGoodsConfigModelDB> prepareList = [.. FilesConfigs.Where(x => GlobalToolsStandard.IsImageFile(FilesList.First(y => y.Id == x.FileId).FileName) == _isGal).OrderBy(x => x.SortIndex)];
        if (prepareList.Count <= 1)
            return MoveRowStatesEnum.Singleton;

        int _fi = prepareList.FindIndex(x => x.Id == conf.Id);
        if (_fi == 0)
            return MoveRowStatesEnum.Start;

        if (_fi == prepareList.Count - 1)
            return MoveRowStatesEnum.End;

        return MoveRowStatesEnum.Between;
    }

    async Task FilesSelectedSet()
    {
        if (FilesList is null || FilesConfigs is null || FilesContextViewComponent.ApplicationsNames is null)
            return;

        StorageFileModelDB[] _filesControl = [.. FilesList.Where(x => FilesConfigs.Any(y => y.FileId == x.Id))];
        if (SelectedFiles.Count() == _filesControl.Length && _filesControl.All(x => SelectedFiles.Any(y => y.Id == x.Id)))
            return;

        await SetBusyAsync();
        TAuthRequestStandardModel<FilesForGoodSetRequestModel> req = new()
        {
            SenderActionUserId = CurrentUserSession?.UserId,
            Payload = new()
            {
                OwnerId = OwnerId,
                OwnerTypeName = OwnerTypeName,
                SelectedFiles = SelectedFiles.Select(x => x.Id),
                ApplicationName = FilesContextViewComponent.ApplicationsNames.Single(),
                PrefixPropertyName = FilesContextViewComponent.PropertyName,
                PropertyName = FilesContextViewComponent.PropertyName,
            }
        };
        ResponseBaseModel res = await CommerceRepo.FilesForGoodSetAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await ReloadData();
        await SetBusyAsync(false);
    }

    readonly Func<StorageFileModelDB, string?> convertFunc = ci => ci?.FileName;

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

        TPaginationResponseStandardModel<StorageFileModelDB> res = await FilesRepo.FilesSelectAsync(req);

        FilesList = res.Response;

        TPaginationRequestStandardModel<FilesForGoodSelectRequestModel> req2 = new()
        {
            Payload = new()
            {
                OwnerId = OwnerId,
                OwnerTypeName = OwnerTypeName,
            },
            PageNum = 0,
            PageSize = int.MaxValue,
        };
        TPaginationResponseStandardModel<FileGoodsConfigModelDB> res2 = await CommerceRepo.FilesForGoodSelectAsync(req2);
        FilesConfigs = res2.Response?.OrderBy(x => x.SortIndex).ToList();
        if (FilesList is not null && FilesConfigs is not null)
            _selectedFiles = new HashSet<StorageFileModelDB>(FilesList.Where(x => FilesConfigs.Any(y => y.FileId == x.Id)));

        await SetBusyAsync(false);
    }
}