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
    public Task<TResponseModel<int>> BankConnectionCreateOrUpdateAsync(TAuthRequestStandardModel<BankConnectionModelDB> bank, CancellationToken token = default);
    /// <summary>
    /// Подбор
    /// </summary>
    public Task<TPaginationResponseStandardModel<BankConnectionModelDB>> ConnectionsBanksSelectAsync(TPaginationRequestStandardModel<SelectConnectionsBanksRequestModel> req, CancellationToken token = default);


    /// <summary>
    /// Создать/обновить
    /// </summary>
    public Task<TResponseModel<int>> AccountTBankCreateOrUpdateAsync(TAuthRequestStandardModel<TBankAccountModelDB> acc, CancellationToken token = default);
    /// <summary>
    /// Подбор
    /// </summary>
    public Task<TPaginationResponseStandardModel<TBankAccountModelDB>> AccountsTBankSelectAsync(TPaginationRequestStandardModel<SelectAccountsRequestModel> req, CancellationToken token = default);


    /// <summary>
    /// Создать/обновить
    /// </summary>
    public Task<TResponseModel<int>> CustomerBankCreateOrUpdateAsync(TAuthRequestStandardModel<CustomerBankIdModelDB> cust, CancellationToken token = default);
    /// <summary>
    /// Подбор
    /// </summary>
    public Task<TPaginationResponseStandardModel<CustomerBankIdModelDB>> CustomersBanksSelectAsync(TPaginationRequestStandardModel<SelectCustomersBanksIdsRequestModel> req, CancellationToken token = default);


    /// <summary>
    /// Создать/обновить
    /// </summary>
    public Task<TResponseModel<int>> BankTransferCreateOrUpdateAsync(TAuthRequestStandardModel<BankTransferModelDB> trans, CancellationToken token = default);
    /// <summary>
    /// Подбор
    /// </summary>
    public Task<TPaginationResponseStandardModel<BankTransferModelDB>> BanksTransfersSelectAsync(TPaginationRequestStandardModel<SelectTransfersBanksRequestModel> req, CancellationToken token = default);
}