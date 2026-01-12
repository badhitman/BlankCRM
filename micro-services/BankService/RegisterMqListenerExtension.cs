////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Transmission.Receives.bank;
using Newtonsoft.Json.Linq;
using SharedLib;

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
            .RegisterMqListener<BankConnectionCreateOrUpdateReceive, TAuthRequestStandardModel<BankConnectionModelDB>, TResponseModel<int>>()
            .RegisterMqListener<ConnectionsBanksSelectReceive, TPaginationRequestStandardModel<SelectConnectionsBanksRequestModel>, TPaginationResponseStandardModel<BankConnectionModelDB>>()
            .RegisterMqListener<CustomerBankCreateOrUpdateReceive, TAuthRequestStandardModel<CustomerBankIdModelDB>, TResponseModel<int>>()
            .RegisterMqListener<CustomersBanksSelectReceive, TPaginationRequestStandardModel<SelectCustomersBanksIdsRequestModel>, TPaginationResponseStandardModel<CustomerBankIdModelDB>>()
            .RegisterMqListener<BanksTransfersSelectReceive, TPaginationRequestStandardModel<SelectTransfersBanksRequestModel>, TPaginationResponseStandardModel<BankTransferModelDB>>()
            .RegisterMqListener<BankTransferCreateOrUpdateReceive, BankTransferModelDB, TResponseModel<int>>()
            .RegisterMqListener<AccountsTBankSelectReceive, TPaginationRequestStandardModel<SelectAccountsRequestModel>, TPaginationResponseStandardModel<TBankAccountModelDB>>()
            .RegisterMqListener<AccountTBankCreateOrUpdateReceive, TBankAccountModelDB, TResponseModel<int>>()
            .RegisterMqListener<GetTBankAccountsReceive, GetTBankAccountsRequestModel, TResponseModel<List<TBankAccountModelDB>>>()
            .RegisterMqListener<BankAccountCheckReceive, BankAccountCheckRequestModel, TResponseModel<List<BankTransferModelDB>>>()
            .RegisterMqListener<IncomingTBankMerchantPaymentReceive, JObject, ResponseBaseModel>()
            .RegisterMqListener<InitPaymentMerchantTBankReceive, InitMerchantTBankRequestModel, TResponseModel<PaymentInitTBankResultModelDB>>()
            .RegisterMqListener<IncomingMerchantPaymentsSelectTBankReceive, TPaginationRequestStandardModel<SelectIncomingMerchantPaymentsTBankRequestModel>, TPaginationResponseStandardModel<IncomingMerchantPaymentTBankModelDB>>()
            .RegisterMqListener<PaymentsInitSelectTBankReceive, TPaginationRequestStandardModel<SelectInitPaymentsTBankRequestModel>, TPaginationResponseStandardModel<PaymentInitTBankResultModelDB>>()
            ;
    }
}