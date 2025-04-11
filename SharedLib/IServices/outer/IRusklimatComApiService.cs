////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// b2b.rusklimat - REST API каталога ИП (Интернет Партнер)
/// </summary>
public interface IRusklimatComApiService : IOuterApiService
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

    /// <summary>
    /// Обновить/создать товар
    /// </summary>
    public Task<ResponseBaseModel> UpdateProductAsync(ProductRusklimatModelDB req, CancellationToken token = default);
}