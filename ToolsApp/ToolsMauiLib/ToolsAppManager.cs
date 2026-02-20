////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;

namespace ToolsMauiLib;

/// <summary>
/// Управление настройками для управления удалённого системами
/// </summary>
/// <remarks>
/// Токены, синхронизации, логи
/// </remarks>
public class ToolsAppManager(IDbContextFactory<ToolsAppContext> toolsDbFactory) : IToolsAppManager
{
    /// <inheritdoc/>
    public async Task<ApiRestConfigModelDB[]> GetAllConfigurationsAsync(CancellationToken token = default)
    {
        using ToolsAppContext context = await toolsDbFactory.CreateDbContextAsync(token);
        return await context.Configurations.ToArrayAsync(cancellationToken: token);
    }

    /// <inheritdoc/>
    public async Task<ApiRestConfigModelDB> ReadConfigurationAsync(int confId, CancellationToken token = default)
    {
        using ToolsAppContext context = await toolsDbFactory.CreateDbContextAsync(token);

        return await context
            .Configurations
            .Include(x => x.SyncDirectories)
            .Include(x => x.CommandsRemote)
            .FirstAsync(x => x.Id == confId, cancellationToken: token);
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> UpdateOrCreateConfigAsync(ApiRestConfigModelDB req, CancellationToken token = default)
    {
        req.Name = req.Name.Trim();

        req.HeaderName = req.HeaderName.Trim();
        req.TokenAccess = req.TokenAccess.Trim();
        req.AddressBaseUri = req.AddressBaseUri.Trim();

        TResponseModel<int> res = new();
        ValidateReportModel ch = GlobalTools.ValidateObject(req);
        if (!ch.IsValid)
        {
            res.Messages.InjectException(ch.ValidationResults);
            return res;
        }

        if (!req.AddressBaseUri.EndsWith('/'))
            req.AddressBaseUri = $"{req.AddressBaseUri}/";

        using ToolsAppContext context = await toolsDbFactory.CreateDbContextAsync(token);
        if (req.Id < 1)
        {
            req.SyncDirectories?.ForEach(x => x.Parent = req);
            req.CommandsRemote?.ForEach(x => x.Parent = req);

            req.Id = 0;
            await context.AddAsync(req, token);
            await context.SaveChangesAsync(token);
            res.AddSuccess("Токен успешно добавлен");
            res.Response = req.Id;
            return res;
        }

        res.Response = await context
             .Configurations
             .Where(x => x.Id == req.Id)
             .ExecuteUpdateAsync(set => set
             .SetProperty(p => p.AddressBaseUri, req.AddressBaseUri)
             .SetProperty(p => p.Name, req.Name)
             .SetProperty(p => p.TokenAccess, req.TokenAccess), cancellationToken: token);

        res.AddInfo(res.Response == 0 ? "Изменений нет" : "Токен успешно обновлён");
        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteConfigAsync(int confId, CancellationToken token = default)
    {
        using ToolsAppContext context = await toolsDbFactory.CreateDbContextAsync(token);

        if (1 > await context
             .Configurations
             .Where(x => x.Id == confId)
             .ExecuteDeleteAsync(cancellationToken: token))
            return ResponseBaseModel.CreateInfo("Объекта не существует");

        return ResponseBaseModel.CreateInfo("Токен успешно удалён");
    }

    /// <inheritdoc/>
    public async Task<ExeCommandModelDB[]> GetExeCommandsForConfigAsync(int confId, CancellationToken token = default)
    {
        using ToolsAppContext context = await toolsDbFactory.CreateDbContextAsync(token);
        return await context.ExeCommands.Where(x => x.ParentId == confId).ToArrayAsync(cancellationToken: token);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteExeCommandAsync(int exeCommandId, CancellationToken token = default)
    {
        using ToolsAppContext context = await toolsDbFactory.CreateDbContextAsync(token);

        if (0 != await context
            .ExeCommands
            .Where(x => x.Id == exeCommandId)
            .ExecuteDeleteAsync(cancellationToken: token))
            return ResponseBaseModel.CreateInfo("Команды не существует");

        return ResponseBaseModel.CreateInfo("Команда успешно удалена");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateOrCreateExeCommandAsync(ExeCommandModelDB req, CancellationToken token = default)
    {
        using ToolsAppContext context = await toolsDbFactory.CreateDbContextAsync(token);

        if (req.Id < 1)
        {
            req.Parent = null;
            req.Id = 0;
            await context.AddAsync(req, token);
            await context.SaveChangesAsync(token);

            return ResponseBaseModel.CreateSuccess("Команда успешно добавлена");
        }

        if (1 > await context
             .ExeCommands
             .Where(x => x.Id == req.Id)
             .ExecuteUpdateAsync(set => set
             .SetProperty(p => p.Name, req.Name)
             .SetProperty(p => p.FileName, req.FileName)
             .SetProperty(p => p.Arguments, req.Arguments)
, cancellationToken: token))
            return ResponseBaseModel.CreateInfo("Изменений нет");

        return ResponseBaseModel.CreateInfo("Команда успешно обновлена");
    }


    /// <inheritdoc/>
    public async Task<SyncDirectoryModelDB[]> GetSyncDirectoriesForConfigAsync(int confId, CancellationToken token = default)
    {
        using ToolsAppContext context = await toolsDbFactory.CreateDbContextAsync(token);
        return await context.SyncDirectories.Where(x => x.ParentId == confId).ToArrayAsync(cancellationToken: token);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteSyncDirectoryAsync(int syncDirectoryId, CancellationToken token = default)
    {
        using ToolsAppContext context = await toolsDbFactory.CreateDbContextAsync(token);

        if (1 > await context
           .SyncDirectories
           .Where(x => x.Id == syncDirectoryId)
           .ExecuteDeleteAsync(cancellationToken: token))
            return ResponseBaseModel.CreateInfo("Синхронизация не существует");

        return ResponseBaseModel.CreateInfo("Синхронизация успешно удалена");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateOrCreateSyncDirectoryAsync(SyncDirectoryModelDB req, CancellationToken token = default)
    {
        using ToolsAppContext context = await toolsDbFactory.CreateDbContextAsync(token);

        if (req.Id < 1)
        {
            req.Parent = null;
            req.Id = 0;
            await context.AddAsync(req, token);
            await context.SaveChangesAsync(token);

            return ResponseBaseModel.CreateSuccess("Синхронизация успешно добавлена");
        }

        if (1 > await context
             .SyncDirectories
             .Where(x => x.Id == req.Id)
             .ExecuteUpdateAsync(set => set
             .SetProperty(p => p.Name, req.Name)
             .SetProperty(p => p.LocalDirectory, req.LocalDirectory)
             .SetProperty(p => p.RemoteDirectory, req.RemoteDirectory)
, cancellationToken: token))
            return ResponseBaseModel.CreateInfo("Изменений нет");

        return ResponseBaseModel.CreateInfo("Синхронизация успешно обновлена");
    }

    /// <inheritdoc/>
    public async Task<SyncDirectoryModelDB> ReadSyncDirectoryAsync(int syncDirId, CancellationToken token = default)
    {
        using ToolsAppContext context = await toolsDbFactory.CreateDbContextAsync(token);
        return await context
            .SyncDirectories
            .FirstAsync(x => x.Id == syncDirId, cancellationToken: token);
    }

    /// <inheritdoc/>
    public async Task<ExeCommandModelDB> ReadExeCommandAsync(int comId, CancellationToken token = default)
    {
        using ToolsAppContext context = await toolsDbFactory.CreateDbContextAsync(token);
        return await context
            .ExeCommands
            .FirstAsync(x => x.Id == comId, cancellationToken: token);
    }
}