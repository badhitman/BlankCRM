////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IBankService
/// </summary>
public partial interface IBankService
{
    /// <summary>
    /// BankAccountCheck
    /// </summary>
    public Task<TResponseModel<List<BankTransferModelDB>>> BankAccountCheckAsync(BankAccountCheckRequestModel req, CancellationToken token = default);

    /// <summary>
    /// GetTBankAccounts
    /// </summary>
    public Task<TResponseModel<List<TBankAccountModelDB>>> GetTBankAccountsAsync(GetTBankAccountsRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Создать/обновить
    /// </summary>
    public Task<TResponseModel<int>> BankConnectionCreateOrUpdateAsync(BankConnectionModelDB bank, CancellationToken token = default);
    /// <summary>
    /// Подбор
    /// </summary>
    public Task<TPaginationResponseStandardModel<BankConnectionModelDB>> ConnectionsBanksSelectAsync(TPaginationRequestStandardModel<SelectConnectionsBanksRequestModel> req, CancellationToken token = default);


    /// <summary>
    /// Создать/обновить
    /// </summary>
    public Task<TResponseModel<int>> AccountTBankCreateOrUpdateAsync(TBankAccountModelDB acc, CancellationToken token = default);
    /// <summary>
    /// Подбор
    /// </summary>
    public Task<TPaginationResponseStandardModel<TBankAccountModelDB>> AccountsTBankSelectAsync(TPaginationRequestStandardModel<SelectAccountsRequestModel> req, CancellationToken token = default);


    /// <summary>
    /// Создать/обновить
    /// </summary>
    public Task<TResponseModel<int>> CustomerBankCreateOrUpdateAsync(CustomerBankIdModelDB cust, CancellationToken token = default);
    /// <summary>
    /// Подбор
    /// </summary>
    public Task<TPaginationResponseStandardModel<CustomerBankIdModelDB>> CustomersBanksSelectAsync(TPaginationRequestStandardModel<SelectCustomersBanksIdsRequestModel> req, CancellationToken token = default);


    /// <summary>
    /// Создать/обновить
    /// </summary>
    public Task<TResponseModel<int>> BankTransferCreateOrUpdateAsync(BankTransferModelDB trans, CancellationToken token = default);
    /// <summary>
    /// Подбор
    /// </summary>
    public Task<TPaginationResponseStandardModel<BankTransferModelDB>> BanksTransfersSelectAsync(TPaginationRequestStandardModel<SelectTransfersBanksRequestModel> req, CancellationToken token = default);
}