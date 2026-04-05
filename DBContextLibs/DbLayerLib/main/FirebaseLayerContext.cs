////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace DbcLib;

/// <inheritdoc/>
public partial class FirebaseLayerContext : DbContext
{
    /// <inheritdoc/>
    public FirebaseLayerContext(DbContextOptions options)
        : base(options)
    {
        //#if DEBUG
        //        Database.EnsureCreated();
        //#else
        if (Database.IsRelational())
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
}