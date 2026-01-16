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
    public async Task<TResponseModel<int>> BankConnectionCreateOrUpdateAsync(TAuthRequestStandardModel<BankConnectionModelDB> bank, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.BankConnectionCreateOrUpdateReceive, bank, token: token) ?? new();
    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<BankConnectionModelDB>> ConnectionsBanksSelectAsync(TPaginationRequestStandardModel<SelectConnectionsBanksRequestModel> req, CancellationToken token = default)
      => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<BankConnectionModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionsBanksSelectReceive, req, token: token) ?? new();


    /// <inheritdoc/>
    public async Task<TResponseModel<int>> AccountTBankCreateOrUpdateAsync(TAuthRequestStandardModel<TBankAccountModelDB> acc, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.AccountTBankCreateOrUpdateReceive, acc, token: token) ?? new();
    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TBankAccountModelDB>> AccountsTBankSelectAsync(TPaginationRequestStandardModel<SelectAccountsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<TBankAccountModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.AccountsTBankSelectReceive, req, token: token) ?? new();


    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CustomerBankCreateOrUpdateAsync(TAuthRequestStandardModel<CustomerBankIdModelDB> cust, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.CustomerBankCreateOrUpdateReceive, cust, token: token) ?? new();
    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<CustomerBankIdModelDB>> CustomersBanksSelectAsync(TPaginationRequestStandardModel<SelectCustomersBanksIdsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<CustomerBankIdModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.CustomersBanksSelectReceive, req, token: token) ?? new();


    /// <inheritdoc/>
    public async Task<TResponseModel<int>> BankTransferCreateOrUpdateAsync(TAuthRequestStandardModel<BankTransferModelDB> trans, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.BankTransferCreateOrUpdateReceive, trans, token: token) ?? new();
    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<BankTransferModelDB>> BanksTransfersSelectAsync(TPaginationRequestStandardModel<SelectTransfersBanksRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<BankTransferModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.BanksTransfersSelectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<TBankAccountModelDB>>> GetTBankAccountsAsync(GetTBankAccountsRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<TBankAccountModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetTBankConnectionAccountsReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<BankTransferModelDB>>> BankAccountCheckAsync(BankAccountCheckRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<BankTransferModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.BankAccountCheckReceive, req, token: token) ?? new();
}