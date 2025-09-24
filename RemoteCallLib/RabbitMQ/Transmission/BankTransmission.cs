////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// BankTransmission
/// </summary>
public partial class BankTransmission(IRabbitClient rabbitClient) : IBankService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> BankConnectionCreateOrUpdateAsync(BankConnectionModelDB bank, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.BankConnectionCreateOrUpdateReceive, bank, token: token) ?? new();
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<BankConnectionModelDB>> ConnectionsBanksSelectAsync(TPaginationRequestStandardModel<SelectConnectionsBanksRequestModel> req, CancellationToken token = default)
      => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<BankConnectionModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionsBanksSelectReceive, req, token: token) ?? new();


    /// <inheritdoc/>
    public async Task<TResponseModel<int>> AccountTBankCreateOrUpdateAsync(TBankAccountModelDB acc, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.AccountTBankCreateOrUpdateReceive, acc, token: token) ?? new();
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<TBankAccountModelDB>> AccountsTBankSelectAsync(TPaginationRequestStandardModel<SelectAccountsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<TBankAccountModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.AccountsTBankSelectReceive, req, token: token) ?? new();


    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CustomerBankCreateOrUpdateAsync(CustomerBankIdModelDB cust, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.CustomerBankCreateOrUpdateReceive, cust, token: token) ?? new();
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<CustomerBankIdModelDB>> CustomersBanksSelectAsync(TPaginationRequestStandardModel<SelectCustomersBanksIdsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<CustomerBankIdModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.CustomersBanksSelectReceive, req, token: token) ?? new();


    /// <inheritdoc/>
    public async Task<TResponseModel<int>> BankTransferCreateOrUpdateAsync(BankTransferModelDB trans, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.BankTransferCreateOrUpdateReceive, trans, token: token) ?? new();
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<BankTransferModelDB>> BanksTransfersSelectAsync(TPaginationRequestStandardModel<SelectTransfersBanksRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<BankTransferModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.BanksTransfersSelectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<TBankAccountModelDB>>> GetTBankAccountsAsync(GetTBankAccountsRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<TBankAccountModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetTBankConnectionAccountsReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<BankTransferModelDB>>> BankAccountCheckAsync(BankAccountCheckRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<BankTransferModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.BankAccountCheckReceive, req, token: token) ?? new();
}