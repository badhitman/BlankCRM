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
            .RegisterListenerRabbitMQ<BankConnectionCreateOrUpdateReceive, TAuthRequestStandardModel<BankConnectionModelDB>, TResponseModel<int>>()
            .RegisterListenerRabbitMQ<ConnectionsBanksSelectReceive, TPaginationRequestStandardModel<SelectConnectionsBanksRequestModel>, TPaginationResponseStandardModel<BankConnectionModelDB>>()
            .RegisterListenerRabbitMQ<CustomerBankCreateOrUpdateReceive, TAuthRequestStandardModel<CustomerBankIdModelDB>, TResponseModel<int>>()
            .RegisterListenerRabbitMQ<CustomersBanksSelectReceive, TPaginationRequestStandardModel<SelectCustomersBanksIdsRequestModel>, TPaginationResponseStandardModel<CustomerBankIdModelDB>>()
            .RegisterListenerRabbitMQ<BanksTransfersSelectReceive, TPaginationRequestStandardModel<SelectTransfersBanksRequestModel>, TPaginationResponseStandardModel<BankTransferModelDB>>()
            .RegisterListenerRabbitMQ<BankTransferCreateOrUpdateReceive, TAuthRequestStandardModel<BankTransferModelDB>, TResponseModel<int>>()
            .RegisterListenerRabbitMQ<AccountsTBankSelectReceive, TPaginationRequestStandardModel<SelectAccountsRequestModel>, TPaginationResponseStandardModel<TBankAccountModelDB>>()
            .RegisterListenerRabbitMQ<AccountTBankCreateOrUpdateReceive, TAuthRequestStandardModel<TBankAccountModelDB>, TResponseModel<int>>()
            .RegisterListenerRabbitMQ<GetTBankAccountsReceive, GetTBankAccountsRequestModel, TResponseModel<List<TBankAccountModelDB>>>()
            .RegisterListenerRabbitMQ<BankAccountCheckReceive, BankAccountCheckRequestModel, TResponseModel<List<BankTransferModelDB>>>()
            .RegisterListenerRabbitMQ<IncomingTBankMerchantPaymentReceive, JObject, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<InitPaymentMerchantTBankReceive, TAuthRequestStandardModel<InitMerchantTBankRequestModel>, TResponseModel<PaymentInitTBankResultModelDB>>()
            .RegisterListenerRabbitMQ<IncomingMerchantPaymentsSelectTBankReceive, TPaginationRequestStandardModel<SelectIncomingMerchantPaymentsTBankRequestModel>, TPaginationResponseStandardModel<IncomingMerchantPaymentTBankModelDB>>()
            .RegisterListenerRabbitMQ<PaymentsInitSelectTBankReceive, TPaginationRequestStandardModel<SelectInitPaymentsTBankRequestModel>, TPaginationResponseStandardModel<PaymentInitTBankResultModelDB>>()
            ;
    }
}