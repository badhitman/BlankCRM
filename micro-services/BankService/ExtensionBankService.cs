////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using TinkoffPaymentClientApi.ResponseEntity;
using Newtonsoft.Json;
using SharedLib;

namespace BankService;

/// <summary>
/// ExtensionBankService
/// </summary>
public static class ExtensionBankService
{
    /// <inheritdoc/>
    public static IncomingMerchantPaymentTBankModelDB GetDB(this TinkoffNotification span)
    {
        return new IncomingMerchantPaymentTBankModelDB()
        {
            Amount = span.Amount,
            CardId = span.CardId,
            ExpDate = span.ExpDate,
            OrderId = span.OrderId,
            PaymentId = span.PaymentId,
            Pan = span.Pan,
            RebillId = span.RebillId,
            Status = span.Status,
        };
    }

    /// <inheritdoc/>
    public static PaymentInitTBankResultModelDB GetDB(this InitMerchantTBankRequestModel span)
    {
        PaymentInitTBankResultModelDB res = new()
        {
            Amount = span.Amount,
            OrderId = span.OrderId,
            Receipt = new ReceiptTBankModelDB()
            {
                Email = span.Receipt.Email,
                EmailCompany = span.Receipt.EmailCompany,
                Taxation = span.Receipt.Taxation,
                Phone = span.Receipt.Phone,
                Payments = span.Receipt.Payments?.GetDB(),
            }
        };

        if (span.Receipt.Items is not null && span.Receipt.Items.Count != 0)
            res.Receipt.Items = [.. span.Receipt.Items.Select(x => x.GetDB(res.Receipt))];

        return res;
    }

    /// <inheritdoc/>
    public static PaymentsForReceiptTBankModelDB GetDB(this PaymentsForReceiptTBankModel span)
    {
        PaymentsForReceiptTBankModelDB res = new()
        {
            AdvancePayment = span.AdvancePayment,
            Cash = span.Cash,
            Credit = span.Credit,
            Electronic = span.Electronic,
            Provision = span.Provision,
        };

        return res;
    }

    /// <inheritdoc/>
    public static ReceiptItemModelDB GetDB(this ReceiptItemTBankModel span, ReceiptTBankModelDB rec)
    {
        ReceiptItemModelDB res = new()
        {
            Receipt = rec,

            Ean13 = span.Ean13,
            Name = span.Name,
            PaymentMethod = span.PaymentMethod,
            PaymentObject = span.PaymentObject,
            Price = span.Price,
            Quantity = span.Quantity,
            Tax = span.Tax,
        };

        res.AgentData = span.AgentData?.GetDB(res);
        res.SupplierInfo = span.SupplierInfo?.GetDB(res);

        return res;
    }

    /// <inheritdoc/>
    public static AgentDataModelDB GetDB(this AgentDataForReceiptItemTBankModel span, ReceiptItemModelDB rec)
    {
        AgentDataModelDB res = new()
        {
            ReceiptItem = rec,

            AgentSign = span.AgentSign,
            OperationName = span.OperationName,
            OperatorAddress = span.OperatorAddress,
            OperatorInn = span.OperatorInn,
            OperatorName = span.OperatorName,

            Phones = span.Phones,
            PhonesJsonSource = JsonConvert.SerializeObject(span.Phones),

            ReceiverPhones = span.ReceiverPhones,
            ReceiverPhonesJsonSource = JsonConvert.SerializeObject(span.ReceiverPhones),

            TransferPhones = span.TransferPhones,
            TransferPhonesJsonSource = JsonConvert.SerializeObject(span.TransferPhones),
        };

        return res;
    }

    /// <inheritdoc/>
    public static SupplierInfoModelDB GetDB(this SupplierInfoForReceiptItemTBankModel span, ReceiptItemModelDB rec)
    {
        SupplierInfoModelDB res = new()
        {
            ReceiptItem = rec,

            Inn = span.Inn,
            Name = span.Name,

            Phones = span.Phones,
            PhonesJsonSource = JsonConvert.SerializeObject(span.Phones),
        };

        return res;
    }
}