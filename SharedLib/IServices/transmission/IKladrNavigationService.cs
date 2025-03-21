﻿////////////////////////////////////////////////
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
    public Task<Dictionary<KladrChainTypesEnum, JObject[]>> ObjectsListForParent(KladrsRequestBaseModel req);

    /// <summary>
    /// Получить объект и его предков
    /// </summary>
    public Task<TResponseModel<KladrResponseModel>> ObjectGet(KladrsRequestBaseModel req);

    /// <summary>
    /// Select objects
    /// </summary>
    public Task<TPaginationResponseModel<KladrResponseModel>> ObjectsSelect(KladrSelectRequestModel req);

    /// <summary>
    /// Find objects
    /// </summary>
    public Task<TPaginationResponseModel<KladrResponseModel>> ObjectsFind(KladrFindRequestModel req);
}