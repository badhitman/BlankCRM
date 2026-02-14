////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;

namespace CommerceService;

/// <summary>
/// Commerce
/// </summary>
public partial class CommerceImplementService : ICommerceService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FilesForGoodSetAsync(TAuthRequestStandardModel<FilesForGoodSetRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<FileGoodsConfigModelDB> q = context.GoodsFilesConfigs
            .Where(x => x.OwnerId == req.Payload.OwnerId && x.OwnerTypeName == req.Payload.OwnerTypeName);

        if (req.Payload.SelectedFiles is null || !req.Payload.SelectedFiles.Any())
            return ResponseBaseModel.CreateSuccess($"Удалено: {await q.ExecuteUpdateAsync(set => set.SetProperty(p => p.IsDisabled, true), cancellationToken: token)}");

        int _disable = await q.Where(x => !x.IsDisabled && !req.Payload.SelectedFiles.Contains(x.FileId))
               .ExecuteUpdateAsync(set => set.SetProperty(p => p.IsDisabled, true), cancellationToken: token);

        int _enable = await q.Where(x => x.IsDisabled && req.Payload.SelectedFiles.Contains(x.FileId))
               .ExecuteUpdateAsync(set => set.SetProperty(p => p.IsDisabled, false), cancellationToken: token);

        int[] filesIds = [.. q.Where(x => !x.IsDisabled).Select(x => x.FileId)];
        req.Payload.SelectedFiles = [.. req.Payload.SelectedFiles.Where(x => !filesIds.Contains(x))];
        bool containsElements = await q.AnyAsync(x => !x.IsDisabled, cancellationToken: token);
        if (req.Payload.SelectedFiles.Any())
        {
            FileGoodsConfigModelDB[] _prepare = [.. req.Payload.SelectedFiles.Select(x => new FileGoodsConfigModelDB()
            {
                OwnerTypeName = req.Payload.OwnerTypeName,
                FileId = x,
                Name = "",
                OwnerId = req.Payload.OwnerId,
            })];

            foreach (FileGoodsConfigModelDB fileGoodsConfig in _prepare)
            {
                fileGoodsConfig.SortIndex = containsElements
                    ? await q.Where(x => !x.IsDisabled).MaxAsync(x => x.SortIndex, cancellationToken: token)
                    : 0;
                fileGoodsConfig.SortIndex++;
                await context.GoodsFilesConfigs.AddAsync(fileGoodsConfig, token);
                await context.SaveChangesAsync(token);
            }

            await SortIndexUpdate(q, new()
            {
                PageNum = 0,
                PageSize = int.MaxValue,
                Payload = new()
                {
                    ApplicationsNames = [req.Payload.ApplicationName],
                    OwnerPrimaryKey = req.Payload.OwnerId,
                    PrefixPropertyName = req.Payload.PrefixPropertyName,
                    PropertyName = req.Payload.PropertyName,
                }
            }, token);
            return ResponseBaseModel.CreateSuccess($"Добавлено: {_prepare.Length}. Включено: {_enable}; Выключено: {_disable}");
        }

        return ResponseBaseModel.CreateSuccess($"Включено: {_enable}; Выключено: {_disable}");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<FileGoodsConfigModelDB>> FilesForGoodSelectAsync(TPaginationRequestStandardModel<FilesForGoodSelectRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new() { Status = new() { Messages = [new() { Text = "req.Payload is null", TypeMessage = MessagesTypesEnum.Error }] } };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<FileGoodsConfigModelDB> q = context.GoodsFilesConfigs
            .Where(x => !x.IsDisabled && x.OwnerId == req.Payload.OwnerId && x.OwnerTypeName == req.Payload.OwnerTypeName);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortBy = req.SortBy,
            SortingDirection = req.SortingDirection,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.OrderBy(x => x.SortIndex).Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToListAsync(cancellationToken: token),
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FileForGoodUpdateAsync(TAuthRequestStandardModel<FileGoodsConfigModelDB> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        return ResponseBaseModel.CreateSuccess($"Выполнено: {await context.GoodsFilesConfigs
            .Where(x => x.Id == req.Payload.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.IsGallery, req.Payload.IsGallery)
                .SetProperty(p => p.Name, req.Payload.Name)
                .SetProperty(p => p.FullDescription, req.Payload.FullDescription)
                .SetProperty(p => p.ShortDescription, req.Payload.ShortDescription), cancellationToken: token)}");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> MoveFileForGoodsAsync(TAuthRequestStandardModel<MoveMetaObjectModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        FileGoodsConfigModelDB confDb = await context.GoodsFilesConfigs
            .Where(x => x.Id == req.Payload.Id)
            .FirstAsync(cancellationToken: token);

        IQueryable<FileGoodsConfigModelDB> q = context.GoodsFilesConfigs
            .Where(x => x.OwnerId == confDb.OwnerId && x.OwnerTypeName == confDb.OwnerTypeName);

        List<FileGoodsConfigModelDB> goodsFilesDb = await q.OrderBy(x => x.SortIndex).ToListAsync(cancellationToken: token);

        TPaginationRequestStandardModel<SelectMetadataRequestModel> req2 = new()
        {
            Payload = new()
            {
                ApplicationsNames = [req.Payload.ApplicationName],
                IdentityUsersIds = [],
                PropertyName = req.Payload.PropertyName,
                OwnerPrimaryKey = confDb.OwnerId,
                PrefixPropertyName = req.Payload.PrefixPropertyName,
            },
            PageNum = 0,
            PageSize = int.MaxValue,
        };

        TPaginationResponseStandardModel<StorageFileModelDB> filesList = await FilesRepo.FilesSelectAsync(req2, token);

        if (goodsFilesDb.Count == 1)
            return ResponseBaseModel.CreateInfo("Нет других файлов для перемещения");

        int _ind = goodsFilesDb.FindIndex(x => x.Id == confDb.Id);
        if (_ind == 0 && req.Payload.Direct == DirectionsEnum.Up)
            return ResponseBaseModel.CreateInfo("Файл уже вверху");
        if (_ind == goodsFilesDb.Count - 1 && req.Payload.Direct == DirectionsEnum.Down)
            return ResponseBaseModel.CreateInfo("Файл уже внизу");

        uint _i;
        if (req.Payload.Direct == DirectionsEnum.Up)
        {
            _i = goodsFilesDb[_ind - 1].SortIndex;
            goodsFilesDb[_ind - 1].SortIndex = goodsFilesDb[_ind].SortIndex;
            goodsFilesDb[_ind].SortIndex = _i;
            context.GoodsFilesConfigs.UpdateRange([goodsFilesDb[_ind - 1], goodsFilesDb[_ind]]);
        }
        else if (req.Payload.Direct == DirectionsEnum.Down)
        {
            _i = goodsFilesDb[_ind + 1].SortIndex;
            goodsFilesDb[_ind + 1].SortIndex = goodsFilesDb[_ind].SortIndex;
            goodsFilesDb[_ind].SortIndex = _i;
            context.GoodsFilesConfigs.UpdateRange([goodsFilesDb[_ind], goodsFilesDb[_ind + 1]]);
        }
        await context.SaveChangesAsync(token);
        await SortIndexUpdate(q, new()
        {
            PageNum = 0,
            PageSize = int.MaxValue,
            Payload = new()
            {
                ApplicationsNames = [req.Payload.ApplicationName],
                OwnerPrimaryKey = confDb.OwnerId,
                PrefixPropertyName = req.Payload.PrefixPropertyName,
                PropertyName = req.Payload.PropertyName,
            }
        }, token);
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    async Task SortIndexUpdate(IQueryable<FileGoodsConfigModelDB> q, TPaginationRequestStandardModel<SelectMetadataRequestModel> storedFilesReq, CancellationToken token = default)
    {
        TPaginationResponseStandardModel<StorageFileModelDB>? resFilesStore = null;
        FileGoodsConfigModelDB[]? arrConfigs = null;

        List<Task> tasks = [
            Task.Run(async () => { resFilesStore = await FilesRepo.FilesSelectAsync(storedFilesReq, token); }, token),
            Task.Run(async () => { arrConfigs = await q.ToArrayAsync(cancellationToken: token); }, token)];
        await Task.WhenAll(tasks);

        if (resFilesStore?.Response is null || arrConfigs is null)
            return;

        foreach (IGrouping<bool, FileGoodsConfigModelDB> filesG in arrConfigs.GroupBy(x => GlobalToolsStandard.IsImageFile(resFilesStore.Response.First(y => y.Id == x.FileId).FileName)))
        {
            uint sortIndex = 1;
            foreach (FileGoodsConfigModelDB fileGoodsConfig in filesG.OrderBy(x => x.SortIndex))
            {
                if (fileGoodsConfig.SortIndex != sortIndex)
                {
                    await q.Where(x => x.Id == fileGoodsConfig.Id)
                        .ExecuteUpdateAsync(set => set.SetProperty(p => p.SortIndex, sortIndex), cancellationToken: token);
                }
                sortIndex++;
            }
        }
    }
}