////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Threading;

namespace SharedLib;

/// <summary>
/// Интеграция API https://api.breez.ru
/// </summary>
public interface IBreezRuApiService : IOuterApiService
{
    /// <summary>
    /// CategoryUpdateAsync
    /// </summary>
    public Task<ResponseBaseModel> CategoryUpdateAsync(CategoryBreezRuModelDB req, CancellationToken token = default);

    /// <summary>
    /// TechProductUpdateAsync
    /// </summary>
    public Task<ResponseBaseModel> TechProductUpdateAsync(TechProductBreezRuModelDB req, CancellationToken token = default);

    /// <summary>
    /// TechCategoryUpdateAsync
    /// </summary>
    public Task<ResponseBaseModel> TechCategoryUpdateAsync(TechCategoryBreezRuModelDB req, CancellationToken token = default);

    /// <summary>
    /// ProductUpdateAsync
    /// </summary>
    public Task<ResponseBaseModel> ProductUpdateAsync(ProductBreezRuModelDB req, CancellationToken token = default);

    /// <summary>
    /// остатки на складах
    /// </summary>
    public Task<TResponseModel<List<BreezRuLeftoverModel>?>> LeftoversGetAsync(string? nc = null, CancellationToken token = default);

    /// <summary>
    /// Бренды
    /// </summary>
    public Task<TResponseModel<List<BrandRealBreezRuModel>>> GetBrandsAsync(CancellationToken token = default);

    /// <summary>
    /// Категории
    /// </summary>
    public Task<TResponseModel<List<CategoryRealBreezRuModel>>> GetCategoriesAsync(CancellationToken token = default);

    /// <summary>
    /// Продукты
    /// </summary>
    public Task<TResponseModel<List<ProductRealBreezRuModel>>> GetProductsAsync(CancellationToken token = default);

    /// <summary>
    /// ProductsSelect
    /// </summary>
    public Task<TPaginationResponseStandardModel<ProductViewBreezRuModeld>> ProductsSelectAsync(BreezRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Технические характеристики Категории
    /// </summary>
    /// <remarks>
    /// В ответ на данный запрос возвращаются все технические характеристики, принадлежащие к категории с указанным идентификатором.
    /// В случае, если категории с указанным идентификатором не существует, в JSON возвращается объект с ключом "error" и значением "Неверный ID".
    /// В XML возвращается объект "categories" с двумя объектами: "cat_id" содержащий запрошенный идентификатор и пустой "techs".
    /// </remarks>
    public Task<TResponseModel<List<TechCategoryRealBreezRuModel>>> GetTechCategoryAsync(TechRequestBreezModel req, CancellationToken token = default);

    /// <summary>
    /// Технические характеристики Продукта
    /// </summary>
    /// <remarks>
    /// В ответ на данный запрос возвращаются данные по продукту с указанным идентификатором.
    /// Данные включают в себя НС-коды, а также все технические характеристики продукта и их значения.
    /// В случае, если продукта с указанным идентификатором не существует, в JSON возвращается объект с ключом "error" и значением "Неверный ID",
    /// а в XML - пустой объект "product".
    /// </remarks>
    public Task<TResponseModel<List<TechProductRealBreezRuModel>>> GetTechProductAsync(TechRequestBreezModel req, CancellationToken token = default);
}