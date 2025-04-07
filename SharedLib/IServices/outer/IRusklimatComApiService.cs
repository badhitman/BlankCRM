////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// b2b rusklimat api
/// </summary>
public interface IRusklimatComApiService : IOuterApiBaseService
{
    /// <summary>
    /// Получение единиц измерения
    /// </summary>
    public Task<TResponseModel<UnitsRusklimatResponseModel>> GetUnitsAsync(CancellationToken token = default);

    /// <summary>
    /// Получение категорий товаров
    /// </summary>
    public Task<TResponseModel<CategoriesRusklimatResponseModel>> GetCategoriesAsync(CancellationToken token = default);

    /// <summary>
    /// Получение свойств товаров
    /// </summary>
    public Task<TResponseModel<PropertiesRusklimatResponseModel>> GetPropertiesAsync(CancellationToken token = default);

    /// <summary>
    /// Получение товаров каталога
    /// </summary>
    public Task<TResponseModel<ProductsRusklimatResponseModel>> GetProductsAsync(RusklimatPaginationRequestModel req, CancellationToken token = default);
}