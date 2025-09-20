////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Bank
/// </summary>
public partial interface IBankService
{
    /// <summary>
    /// Создать/обновить
    /// </summary>
    public Task<TResponseModel<int>> BankCreateOrUpdateAsync(BankConnectionModelDB bank, CancellationToken token = default);

    /// <summary>
    /// Подбор
    /// </summary>
    public Task<TPaginationResponseModel<BankConnectionModelDB>> BanksSelectAsync(TPaginationRequestStandardModel<SelectBanksRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Создать/обновить
    /// </summary>
    public Task<TResponseModel<int>> BankCreateOrUpdateAsync(TBankAccountModelDB acc, CancellationToken token = default);

    /// <summary>
    /// Подбор
    /// </summary>
    public Task<TPaginationResponseModel<TBankAccountModelDB>> BanksSelectAsync(TPaginationRequestStandardModel<SelectAccountsRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Создать/обновить
    /// </summary>
    public Task<TResponseModel<int>> BankCreateOrUpdateAsync(CustomerBankIdModelDB acc, CancellationToken token = default);

    /// <summary>
    /// Подбор
    /// </summary>
    public Task<TPaginationResponseModel<CustomerBankIdModelDB>> BanksSelectAsync(TPaginationRequestStandardModel<SelectCustomersBanksIdsRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Создать/обновить
    /// </summary>
    public Task<TResponseModel<int>> BankCreateOrUpdateAsync(BankTransferModelDB acc, CancellationToken token = default);

    /// <summary>
    /// Подбор
    /// </summary>
    public Task<TPaginationResponseModel<BankTransferModelDB>> BanksSelectAsync(TPaginationRequestStandardModel<SelectTransfersBanksRequestModel> req, CancellationToken token = default);
}