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
    public Task<TResponseModel<int>> BankConnectionCreateOrUpdateAsync(BankConnectionModelDB bank, CancellationToken token = default);

    /// <summary>
    /// Подбор
    /// </summary>
    public Task<TPaginationResponseModel<BankConnectionModelDB>> ConnectionsBanksSelectAsync(TPaginationRequestStandardModel<SelectConnectionsBanksRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Создать/обновить
    /// </summary>
    public Task<TResponseModel<int>> AccountTBankCreateOrUpdateAsync(TBankAccountModelDB acc, CancellationToken token = default);

    /// <summary>
    /// Подбор
    /// </summary>
    public Task<TPaginationResponseModel<TBankAccountModelDB>> AccountsTBankSelectAsync(TPaginationRequestStandardModel<SelectAccountsRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Создать/обновить
    /// </summary>
    public Task<TResponseModel<int>> CustomerBankCreateOrUpdateAsync(CustomerBankIdModelDB cust, CancellationToken token = default);

    /// <summary>
    /// Подбор
    /// </summary>
    public Task<TPaginationResponseModel<CustomerBankIdModelDB>> CustomersBanksSelectAsync(TPaginationRequestStandardModel<SelectCustomersBanksIdsRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Создать/обновить
    /// </summary>
    public Task<TResponseModel<int>> BankTransferCreateOrUpdateAsync(BankTransferModelDB trans, CancellationToken token = default);

    /// <summary>
    /// Подбор
    /// </summary>
    public Task<TPaginationResponseModel<BankTransferModelDB>> BanksTransfersSelectAsync(TPaginationRequestStandardModel<SelectTransfersBanksRequestModel> req, CancellationToken token = default);
}