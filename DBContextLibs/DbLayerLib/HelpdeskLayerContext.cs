﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbcLib;

/// <summary>
/// Промежуточный/общий слой контекста базы данных
/// </summary>
public partial class HelpdeskLayerContext : DbContext
{
    /// <summary>
    /// Промежуточный/общий слой контекста базы данных
    /// </summary>
    public HelpdeskLayerContext(DbContextOptions options)
        : base(options)
    {
#if DEBUG
        Database.Migrate();
#else
        Database.EnsureCreated();
#endif
    }


    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
#if DEBUG
        options.LogTo(Console.WriteLine);
#endif
    }

    /// <summary>
    /// Themes for issues
    /// </summary>
    public DbSet<IssueThemeHelpdeskModelDB> ThemesForIssues { get; set; }

    /// <summary>
    /// Issues
    /// </summary>
    public DbSet<IssueHelpdeskModelDB> Issues { get; set; }

    /// <summary>
    /// Подписчики
    /// </summary>
    public DbSet<SubscriberIssueHelpdeskModelDB> SubscribersOfIssues { get; set; }

    /// <summary>
    /// IssueReadMarkers
    /// </summary>
    public DbSet<IssueReadMarkerHelpdeskModelDB> IssueReadMarkers { get; set; }

    /// <summary>
    /// IssuesMessages
    /// </summary>
    public DbSet<IssueMessageHelpdeskModelDB> IssuesMessages { get; set; }

    /// <summary>
    /// MarkAsResponses
    /// </summary>
    public DbSet<MarkAsResponseHelpdeskModelDB> MarkAsResponses { get; set; }

    /// <summary>
    /// AccessTokens
    /// </summary>
    public DbSet<AnonymTelegramAccessHelpdeskModelDB> AccessTokens { get; set; }
}