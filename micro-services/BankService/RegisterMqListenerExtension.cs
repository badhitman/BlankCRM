////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;
using SharedLib;
using Transmission.Receives.bank;

namespace BankService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection BankRegisterMqListeners(this IServiceCollection services)
    {
        return services
            .RegisterMqListener<BankConnectionCreateOrUpdateReceive, BankConnectionModelDB, TResponseModel<int>>()
            .RegisterMqListener<ConnectionsBanksSelectReceive, TPaginationRequestStandardModel<SelectConnectionsBanksRequestModel>, TPaginationResponseModel<BankConnectionModelDB>>()
            .RegisterMqListener<CustomerBankCreateOrUpdateReceive, CustomerBankIdModelDB, TResponseModel<int>>()
            .RegisterMqListener<CustomersBanksSelectReceive, TPaginationRequestStandardModel<SelectCustomersBanksIdsRequestModel>, TPaginationResponseModel<CustomerBankIdModelDB>>()
            .RegisterMqListener<BanksTransfersSelectReceive, TPaginationRequestStandardModel<SelectTransfersBanksRequestModel>, TPaginationResponseModel<BankTransferModelDB>>()
            .RegisterMqListener<BankTransferCreateOrUpdateReceive, BankTransferModelDB, TResponseModel<int>>()
            .RegisterMqListener<AccountsTBankSelectReceive, TPaginationRequestStandardModel<SelectAccountsRequestModel>, TPaginationResponseModel<TBankAccountModelDB>>()
            .RegisterMqListener<AccountTBankCreateOrUpdateReceive, TBankAccountModelDB, TResponseModel<int>>()
            .RegisterMqListener<GetTBankAccountsReceive, GetTBankAccountsRequestModel, TResponseModel<List<TBankAccountModelDB>>>()
            .RegisterMqListener<BankAccountCheckReceive, BankAccountCheckRequestModel, TResponseModel<List<BankTransferModelDB>>>()
            .RegisterMqListener<IncomingTBankMerchantPaymentReceive, JObject, ResponseBaseModel>()
            ;
    }
}