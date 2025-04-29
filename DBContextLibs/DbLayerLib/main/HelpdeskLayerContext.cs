////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbcLib;

/// <summary>
/// Промежуточный/общий слой контекста базы данных
/// </summary>
public partial class HelpDeskLayerContext : DbContext
{
    /// <summary>
    /// Промежуточный/общий слой контекста базы данных
    /// </summary>
    public HelpDeskLayerContext(DbContextOptions options)
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
#if DEBUG
        options.EnableSensitiveDataLogging(true);
        options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
#endif
    }

    /// <summary>
    /// Рубрики для обращений
    /// </summary>
    public DbSet<RubricIssueHelpDeskModelDB> Rubrics { get; set; } = default!;

    /// <summary>
    /// Обращения
    /// </summary>
    public DbSet<IssueHelpDeskModelDB> Issues { get; set; } = default!;

    /// <summary>
    /// Подписчики на обращения (участники сверх инициатора и исполнителя. сторонние наблюдатели или помощники/ассистенты)
    /// </summary>
    public DbSet<SubscriberIssueHelpDeskModelDB> SubscribersOfIssues { get; set; } = default!;

    /// <summary>
    /// Отметки когда кто просматривал (открывал) обращение. Используется для отслеживания какие обращения уже прочитаны, а в каких есть изменения.
    /// </summary>
    public DbSet<IssueReadMarkerHelpDeskModelDB> IssueReadMarkers { get; set; } = default!;

    /// <summary>
    /// Сообщения (ответы) в обращении. Любой пользователь имеющий права может написать сообщение в обращении клиента
    /// </summary>
    public DbSet<IssueMessageHelpDeskModelDB> IssuesMessages { get; set; } = default!;

    /// <summary>
    /// Сообщения из обращения любой пользователь может отметить как [являющийся ответом на обращении].
    /// </summary>
    /// <remarks>
    /// Наличие такого признака (или даже нескольких внутри одного обращения) даёт основание полагать, что обращение можно закрывать (отправить в готовое).
    /// </remarks>
    public DbSet<VoteHelpDeskModelDB> Votes { get; set; } = default!;

    /// <summary>
    /// Токены доступа к системе. Благодаря этой связи формируется уникальный персональный URL по которому все действия определяются как авторства этого Telegram аккаунта для запуска WebApp
    /// </summary>
    /// <remarks>
    /// По такой ссылке доступ только в HelpDesk.
    /// </remarks>
    public DbSet<AnonymTelegramAccessHelpDeskModelDB> AccessTokens { get; set; } = default!;

    /// <summary>
    /// Блокировщики
    /// </summary>
    public DbSet<LockUniqueTokenModelDB> Lockers { get; set; } = default!;

    /// <summary>
    /// События в обращениях
    /// </summary>
    public DbSet<PulseIssueModelDB> PulseEvents { get; set; } = default!;

    /// <summary>
    /// Пересланные сообщения
    /// </summary>
    public DbSet<ForwardMessageTelegramBotModelDB> ForwardedMessages { get; set; } = default!;

    /// <summary>
    /// Ответы на пересланные запросы от пользователей
    /// </summary>
    public DbSet<AnswerToForwardModelDB> AnswersToForwards { get; set; } = default!;

    /// <summary>
    /// Articles
    /// </summary>
    public DbSet<ArticleModelDB> Articles { get; set; } = default!;

    /// <summary>
    /// RubricsArticles
    /// </summary>
    public DbSet<RubricArticleJoinModelDB> RubricsArticlesJoins { get; set; } = default!;
}