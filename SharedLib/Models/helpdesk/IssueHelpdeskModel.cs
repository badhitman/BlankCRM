////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;

namespace SharedLib;

/// <summary>
/// IssueHelpDeskModel
/// </summary>
[Index(nameof(LastUpdateAt)), Index(nameof(CreatedAtUTC)), Index(nameof(StatusDocument)), Index(nameof(NormalizedDescriptionUpper)), Index(nameof(AuthorIdentityUserId))]
public class IssueHelpDeskModel : EntryDescriptionModel
{
    /// <summary>
    /// Шаг/статус обращения: "Создан", "В работе", "На проверке" и "Готово"
    /// </summary>
    [System.Text.Json.Serialization.JsonConverter(typeof(StringEnumConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public StatusesDocumentsEnum StatusDocument { get; set; }

    /// <summary>
    /// IdentityUserId
    /// </summary>
    public required string AuthorIdentityUserId { get; set; }

    /// <inheritdoc/>
    public string? NormalizedDescriptionUpper { get; set; }

    /// <summary>
    /// Исполнитель
    /// </summary>
    public string? ExecutorIdentityUserId { get; set; }

    /// <summary>
    /// ProjectId
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Subscribers
    /// </summary>
    public List<SubscriberIssueHelpDeskModelDB>? Subscribers { get; set; }

    /// <summary>
    /// Rubric Issue
    /// </summary>
    public RubricModelDB? RubricIssue { get; set; }

    /// <summary>
    /// CreatedAt (UTC)
    /// </summary>
    public DateTime CreatedAtUTC { get; set; }

    /// <summary>
    /// LastUpdateAt
    /// </summary>
    public DateTime LastUpdateAt { get; set; }

    /// <summary>
    /// Build
    /// </summary>
    public static IssueHelpDeskModel Build(IssueHelpDeskModelDB sender)
    {
        return new()
        {
            AuthorIdentityUserId = sender.AuthorIdentityUserId,
            ExecutorIdentityUserId = sender.ExecutorIdentityUserId,
            StatusDocument = sender.StatusDocument,
            Name = sender.Name,
            CreatedAtUTC = sender.CreatedAtUTC,
            LastUpdateAt = sender.LastUpdateAt,
            Description = sender.Description,
            Id = sender.Id,
            ProjectId = sender.ProjectId,
            RubricIssue = sender.RubricIssue,
            Subscribers = sender.Subscribers,
        };
    }
}