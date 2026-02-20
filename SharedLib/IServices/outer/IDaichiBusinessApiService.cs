////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// API Daichi Бизнес (daichi.business)
/// </summary>
public interface IDaichiBusinessApiService : IOuterApiService
{
    /// <summary>
    /// Получение списка доступных складов
    /// </summary>
    public Task<TResponseModel<StoresDaichiBusinessResponseModel?>> StoresGetAsync(CancellationToken token = default);

    /// <summary>
    /// Получение информации об остатках товара на складе
    /// </summary>
    public Task<TResponseModel<ProductsDaichiBusinessResultModel?>> ProductsGetAsync(ProductsRequestDaichiModel req, CancellationToken token = default);

    /// <summary>
    /// Получение информации о характеристиках товаров
    /// </summary>
    public Task<TResponseModel<ProductsParamsDaichiBusinessResponseModel?>> ProductsParamsGetAsync(ProductParamsRequestDaichiModel req, CancellationToken token = default);

    /// <summary>
    /// ParameterUpdateAsync
    /// </summary>
    public Task<ResponseBaseModel> ParameterUpdateAsync(ParameterEntryDaichiModelDB req, CancellationToken token = default);

    /// <summary>
    /// ProductUpdateAsync
    /// </summary>
    public Task<ResponseBaseModel> ProductUpdateAsync(ProductDaichiModelDB req, CancellationToken token = default);

    /// <summary>
    /// ProductsSelect
    /// </summary>
    public Task<TPaginationResponseStandardModel<ProductDaichiModelDB>> ProductsSelectAsync(DaichiRequestModel req, CancellationToken token = default);
}