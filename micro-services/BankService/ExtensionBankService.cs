////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using TinkoffPaymentClientApi.ResponseEntity;
using TinkoffPaymentClientApi.Models;
using TinkoffPaymentClientApi.Enums;
using Newtonsoft.Json;
using SharedLib;

namespace BankService;

/// <summary>
/// ExtensionBankService
/// </summary>
public static class ExtensionBankService
{
    #region GetDB
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
            CreatedDateTime = DateTime.UtcNow,
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
            },
            AuthorUserId = span.AuthorUserId,
            CreatedDateTimeUTC = DateTime.UtcNow,
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
    #endregion

    #region TBank
    /// <inheritdoc/>
    public static Receipt GetTBankReceipt(this ReceiptTBankModel sender)
    {
        return new Receipt(sender.Phone, sender.Email, sender.Taxation.Convert(), sender.Items.Select(x => x.GetTBankReceiptItem()))
        {
            EmailCompany = sender.EmailCompany,
            Payments = sender.Payments?.GetTBankPayments(),
        };
    }

    /// <inheritdoc/>
    public static Payments GetTBankPayments(this PaymentsForReceiptTBankModel sender)
    {
        return new Payments(sender.Electronic)
        {
            AdvancePayment = sender.AdvancePayment,
            Cash = sender.Cash,
            Credit = sender.Credit,
            Provision = sender.Provision,
        };
    }

    /// <inheritdoc/>
    public static ReceiptItem GetTBankReceiptItem(this ReceiptItemTBankModel sender)
    {
        return new ReceiptItem(sender.Name, sender.Quantity, sender.Price, sender.Tax.Convert())
        {
            AgentData = sender.AgentData?.GetTBankAgentData(),
            SupplierInfo = sender.SupplierInfo?.GetTBankSupplierInfo(),

            PaymentMethod = sender.PaymentMethod?.Convert(),
            PaymentObject = sender.PaymentObject?.Convert(),

            Ean13 = sender.Ean13,
        };
    }

    /// <inheritdoc/>
    public static SupplierInfo GetTBankSupplierInfo(this SupplierInfoForReceiptItemTBankModel sender)
        => new(sender.Phones, sender.Name, sender.Inn);

    /// <inheritdoc/>
    public static AgentData GetTBankAgentData(this AgentDataForReceiptItemTBankModel sender)
    {
        return new AgentData()
        {
            Phones = sender.Phones,

            OperatorInn = sender.OperatorInn,
            OperatorName = sender.OperatorName,
            OperationName = sender.OperationName,
            ReceiverPhones = sender.ReceiverPhones,
            TransferPhones = sender.TransferPhones,
            OperatorAddress = sender.OperatorAddress,

            AgentSign = sender.AgentSign?.Convert(),
        };
    }

    #region Enum`s
    /// <inheritdoc/>
    public static EPaymentObject Convert(this PaymentObjectsTBankEnum sender)
        => Enum.Parse<EPaymentObject>(sender.ToString());

    /// <inheritdoc/>
    public static EAgentSign Convert(this AgentSignsTBankEnum sender)
        => Enum.Parse<EAgentSign>(sender.ToString());

    /// <inheritdoc/>
    public static EPaymentMethod Convert(this PaymentMethodsTBankEnum sender)
        => Enum.Parse<EPaymentMethod>(sender.ToString());

    /// <inheritdoc/>
    public static ETax Convert(this TaxesTBankEnum sender)
        => Enum.Parse<ETax>(sender.ToString());

    /// <inheritdoc/>
    public static ETaxation Convert(this TaxationsTBankEnum sender)
        => Enum.Parse<ETaxation>(sender.ToString());

    /// <inheritdoc/>
    public static ELanguageForm Convert(this LanguageFormTBankEnum sender)
        => Enum.Parse<ELanguageForm>(sender.ToString());

    /// <inheritdoc/>
    public static EPayType Convert(this PayTypesTBankEnum sender)
        => Enum.Parse<EPayType>(sender.ToString());
    #endregion    
    #endregion
}