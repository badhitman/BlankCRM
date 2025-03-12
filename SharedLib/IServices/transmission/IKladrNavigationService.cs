////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;

namespace SharedLib;

/// <summary>
/// IKladrNavigationService
/// </summary>
public interface IKladrNavigationService
{
    /// <summary>
    /// Получить objects
    /// </summary>
    public Task<Dictionary<KladrTypesResultsEnum, JObject[]>> ObjectsList(KladrsListModel req);
}