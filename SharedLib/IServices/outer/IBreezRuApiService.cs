﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading;

namespace SharedLib;

/// <summary>
/// Breez.ru api
/// </summary>
public interface IBreezRuApiService : IOuterApiService
{
    /// <summary>
    /// остатки на складах
    /// </summary>
    public Task<TResponseModel<List<BreezRuLeftoverModel>>> LeftoversGetAsync(string? nc = null, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<List<BrandBreezRuModel>>> GetBrandsAsync(CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<List<CategoryBreezRuModel>>> GetCategoriesAsync(CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<List<ProductBreezRuModel>>> GetProductsAsync(CancellationToken token = default);

    /// <summary>
    /// Технические характеристики Категории
    /// </summary>
    /// <remarks>
    /// В ответ на данный запрос возвращаются все технические характеристики, принадлежащие к категории с указанным идентификатором.
    /// В случае, если категории с указанным идентификатором не существует, в JSON возвращается объект с ключом "error" и значением "Неверный ID".
    /// В XML возвращается объект "categories" с двумя объектами: "cat_id" содержащий запрошенный идентификатор и пустой "techs".
    /// </remarks>
    public Task<TResponseModel<List<TechCategoryBreezRuModel>>> GetTechCategoryAsync(TechRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Технические характеристики Продукта
    /// </summary>
    /// <remarks>
    /// В ответ на данный запрос возвращаются данные по продукту с указанным идентификатором.
    /// Данные включают в себя НС-коды, а также все технические характеристики продукта и их значения.
    /// В случае, если продукта с указанным идентификатором не существует, в JSON возвращается объект с ключом "error" и значением "Неверный ID",
    /// а в XML - пустой объект "product".
    /// </remarks>
    public Task<TResponseModel<List<TechProductBreezRuResponseModel>>> GetTechProductAsync(TechRequestModel req, CancellationToken token = default);
}