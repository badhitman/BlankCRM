////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Daichi business api
/// </summary>
public interface IDaichiBusinessApiService : IOuterApiBaseService
{
    /// <summary>
    /// Получение списка доступных складов
    /// </summary>
    public Task<TResponseModel<StoresDaichiBusinessResponseModel>> StoresGetAsync(CancellationToken token = default);

    /// <summary>
    /// Получение информации об остатках товара на складе
    /// </summary>
    public Task<TResponseModel<ProductsDaichiBusinessResultModel>> ProductsGetAsync(ProductsRequestDaichiModel req, CancellationToken token = default);

    /// <summary>
    /// Получение информации о характеристиках товаров
    /// </summary>
    public Task<TResponseModel<ProductsParamsDaichiBusinessResponseModel>> ProductsParamsGetAsync(ProductParamsRequestDaichiModel req, CancellationToken token = default);
}