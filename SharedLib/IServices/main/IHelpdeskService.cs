////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// HelpDesk (service)
/// </summary>
public interface IHelpDeskService : IHelpDeskServiceBase
{
    /// <summary>
    /// ReplaceTags
    /// </summary>
    public static string ReplaceTags(string documentName, DateTime dateCreated, int documentId, StatusesDocumentsEnum? stepIssue, string raw, string? clearBaseUri, string aboutDocument, bool clearMd = false, string documentPagePath = "issue-card")
    {
        return raw.Replace(GlobalStaticConstants.DocumentNameProperty, documentName)
        .Replace(GlobalStaticConstants.DocumentDateProperty, $"{dateCreated.GetCustomTime().ToString("d", GlobalStaticConstants.RU)} {dateCreated.GetCustomTime().ToString("t", GlobalStaticConstants.RU)}")
        .Replace(GlobalStaticConstants.DocumentStatusProperty, stepIssue?.DescriptionInfo())
        .Replace(GlobalStaticConstants.DocumentLinkProperty, clearMd ? $"{clearBaseUri}/{documentPagePath}/{documentId}" : $"<a href='{clearBaseUri}/{documentPagePath}/{documentId}'>{aboutDocument}</a>")
        .Replace(GlobalStaticConstants.HostAddressProperty, clearMd ? clearBaseUri : $"<a href='{clearBaseUri}'>{clearBaseUri}</a>");
    }

    #region pulse
    /// <summary>
    /// Регистрация события из обращения (логи).
    /// </summary>
    /// <remarks>
    /// Плюс рассылка уведомлений участникам события.
    /// </remarks>
    public Task<TResponseModel<bool>> PulsePushAsync(PulseRequestModel req, CancellationToken token = default);

    #endregion

    /// <summary>
    /// SetWebConfig
    /// </summary>
    public Task<ResponseBaseModel> SetWebConfigAsync(HelpDeskConfigModel req, CancellationToken token = default);

    /// <summary>
    /// Очистить кеш сегмента консоли
    /// </summary>
    public Task ConsoleSegmentCacheEmptyAsync(StatusesDocumentsEnum? Status = null, CancellationToken token = default);
}