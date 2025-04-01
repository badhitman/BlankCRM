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
    /// Русклимат b2b API
    /// </summary>
    [Description("https://b2b.rusklimat.com/api/v1/")]
    ApiRusklimatCom = 30,

    /// <summary>
    /// RSS Фид каталога кондиционеров Haierproff
    /// </summary>
    [Description("https://haierproff.ru/feeds/cond/?type=partners")]
    FeedsHaierproffRu = 40,
}