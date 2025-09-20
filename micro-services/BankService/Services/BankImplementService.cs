////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace BankService;

/// <summary>
/// BankService
/// </summary>
public partial class BankImplementService : IBankService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> BankCreateOrUpdateAsync(BankConnectionModelDB bank, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> BankCreateOrUpdateAsync(TBankAccountModelDB acc, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> BankCreateOrUpdateAsync(CustomerBankIdModelDB acc, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> BankCreateOrUpdateAsync(BankTransferModelDB acc, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<BankConnectionModelDB>> BanksSelectAsync(TPaginationRequestStandardModel<SelectBanksRequestModel> req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<TBankAccountModelDB>> BanksSelectAsync(TPaginationRequestStandardModel<SelectAccountsRequestModel> req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<CustomerBankIdModelDB>> BanksSelectAsync(TPaginationRequestStandardModel<SelectCustomersBanksIdsRequestModel> req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<BankTransferModelDB>> BanksSelectAsync(TPaginationRequestStandardModel<SelectTransfersBanksRequestModel> req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}