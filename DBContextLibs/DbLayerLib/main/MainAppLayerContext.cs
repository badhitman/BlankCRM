////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbcLib;

/// <inheritdoc/>
public partial class MainAppLayerContext : DbContext
{
    /// <inheritdoc/>
    public MainAppLayerContext(DbContextOptions options)
        : base(options)
    {
        //#if DEBUG
        //        Database.EnsureCreated();
        //#else
        Database.Migrate();
        //#endif
    }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        //#if DEBUG
        //        options.EnableSensitiveDataLogging(true);
        //        options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        //#endif
    }

    /// <summary>
    /// Dialogs
    /// </summary>
    public DbSet<DialogWebChatModelDB> Dialogs { get; set; } = default!;

    /// <summary>
    /// UsersDialogsJoins
    /// </summary>
    public DbSet<UserJoinDialogWebChatModelDB> UsersDialogsJoins { get; set; } = default!;

    /// <summary>
    /// Messages
    /// </summary>
    public DbSet<MessageWebChatModelDB> Messages { get; set; } = default!;
}