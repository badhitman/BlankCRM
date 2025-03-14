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
    /// Получить элементы по их вышестоящему/предку
    /// </summary>
    public Task<Dictionary<KladrChainTypesEnum, JObject[]>> ObjectsListForParent(KladrFindRequestModel req);

    /// <summary>
    /// Получить объект и его предков
    /// </summary>
    public Task<TResponseModel<KladrResponseModel>> ObjectGet(KladrsRequestBaseModel req);

    /// <summary>
    /// ObjectsSelect
    /// </summary>
    public Task<TPaginationResponseModel<KladrResponseModel>> ObjectsSelect(KladrSelectRequestModel req);
}