﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbLayerLib;

/// <summary>
/// Промежуточный/общий слой контекста базы данных
/// </summary>
public partial class LayerContext : DbContext
{
    /// <summary>
    /// Промежуточный/общий слой контекста базы данных
    /// </summary>
    public LayerContext(DbContextOptions options)
        : base(options)
    {
        Database.Migrate();
    }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
#if DEBUG
        options.LogTo(Console.WriteLine);
#endif
    }

    
}