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
    public Task<Dictionary<KladrChainTypesEnum, JObject[]>> ObjectsListForParentAsync(KladrsRequestBaseModel req, CancellationToken token = default);

    /// <summary>
    /// Получить объект и его предков
    /// </summary>
    public Task<TResponseModel<KladrResponseModel>> ObjectGetAsync(KladrsRequestBaseModel req, CancellationToken token = default);

    /// <summary>
    /// Select objects
    /// </summary>
    public Task<TPaginationResponseModel<KladrResponseModel>> ObjectsSelectAsync(KladrSelectRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Find objects
    /// </summary>
    public Task<TPaginationResponseModel<KladrResponseModel>> ObjectsFindAsync(KladrFindRequestModel req, CancellationToken token = default);
}