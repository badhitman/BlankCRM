////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Имена HTTP клиентов для IHttpClientFactory (например: IHttpClientFactory.CreateClient(HttpClientsNamesEnum.Insight.ToString()))
/// </summary>
public enum HttpClientsNamesOuterEnum
{
    /// <summary>
    /// api.breez.ru
    /// </summary>
    [Description("https://api.breez.ru/api")]
    ApiBreezRu = 10,

    /// <summary>
    /// daichi.business
    /// </summary>
    [Description("https://api.daichi.ru/b2b/<версия>/<метод>?<параметры>")]
    ApiDaichiBusiness = 20,

    /// <summary>
    /// Русклимат internet-partner API
    /// </summary>
    [Description("https://internet-partner.rusklimat.com/api/")]
    ApiRusklimatCom = 30,

    /// <summary>
    /// Авторизация б2б портала
    /// </summary>
    [Description("https://b2b.rusklimat.com/api/")]
    AuthRusklimatComJWT = 31,

    /// <summary>
    /// RSS Фид каталога кондиционеров Haierproff
    /// </summary>
    [Description("https://haierproff.ru/feeds/cond/?type=partners")]
    FeedsHaierProffRu = 40,
}