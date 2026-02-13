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

        int _enable = await q.Where(x => !req.Payload.SelectedFiles.Contains(x.FileId))
               .ExecuteUpdateAsync(set => set.SetProperty(p => p.IsDisabled, true), cancellationToken: token);

        int _disable = await q.Where(x => req.Payload.SelectedFiles.Contains(x.FileId))
               .ExecuteUpdateAsync(set => set.SetProperty(p => p.IsDisabled, false), cancellationToken: token);

        int[] filesIds = [.. q.Where(x => !x.IsDisabled).Select(x => x.FileId)];
        req.Payload.SelectedFiles = req.Payload.SelectedFiles.Where(x => !filesIds.Contains(x));
        if (req.Payload.SelectedFiles.Any())
        {
            await context.GoodsFilesConfigs.AddRangeAsync(req.Payload.SelectedFiles.Select(x => new FileGoodsConfigModelDB()
            {
                OwnerTypeName = req.Payload.OwnerTypeName,
                FileId = x,
                Name = "",
                OwnerId = req.Payload.OwnerId,
            }), token);
            return ResponseBaseModel.CreateSuccess($"Добавлено: {context.SaveChangesAsync(token)}. Включено: {_enable}; Выключено: {_disable}");
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
            .Where(x => x.OwnerId == req.Payload.OwnerId && x.OwnerTypeName == req.Payload.OwnerTypeName);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortBy = req.SortBy,
            SortingDirection = req.SortingDirection,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToListAsync(cancellationToken: token),
        };
    }
}