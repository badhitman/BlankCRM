////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public partial interface ICommerceServiceBase
{
    /// <summary>
    /// Получить остатки
    /// </summary>
    public Task<TPaginationResponseStandardModel<OfferAvailabilityModelDB>> OffersRegistersSelectAsync(TPaginationRequestStandardModel<RegistersSelectRequestBaseModel> req, CancellationToken token = default);

    /// <summary>
    /// PricesRulesGetForOffers
    /// </summary>
    public Task<TResponseModel<List<PriceRuleForOfferModelDB>>> PricesRulesGetForOffersAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default);

    /// <summary>
    /// OffersRead
    /// </summary>
    public Task<TResponseModel<OfferModelDB[]>> OffersReadAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default);

    /// <summary>
    /// OffersSelect
    /// </summary>
    public Task<TResponseModel<TPaginationResponseStandardModel<OfferModelDB>>> OffersSelectAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<OffersSelectRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// OfferUpdate
    /// </summary>
    public Task<TResponseModel<int>> OfferUpdateOrCreateAsync(TAuthRequestStandardModel<OfferModelDB> req, CancellationToken token = default);

    /// <summary>
    /// Загрузка справочника номенклатуры/офферов
    /// </summary>
    public Task<ResponseBaseModel> UploadOffersAsync(List<NomenclatureScopeModel> req, CancellationToken token = default);
}