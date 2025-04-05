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
    public Task<UnitsRusklimatResponseModel> GetUnitsAsync(CancellationToken token = default);

    /// <summary>
    /// Получение категорий товаров
    /// </summary>
    public Task<CategoriesRusklimatResponseModel> GetCategoriesAsync(CancellationToken token = default);

    /// <summary>
    /// Получение свойств товаров
    /// </summary>
    public Task<PropertiesRusklimatResponseModel> GetProperties(CancellationToken token = default);

    /// <summary>
    /// Получение товаров каталога
    /// </summary>
    public Task<ProductsRusklimatResponseModel> GetProductsAsync(PaginationRequestModel req, CancellationToken token = default);
}