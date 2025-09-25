////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using System.Collections.Generic;

namespace SharedLib;

/// <summary>
/// Данные об агенте
/// </summary>
public class AgentDataForReceiptItemTBankModel
{
    /// <summary>
    /// Признак агента
    /// </summary>
    public AgentSignsTBankEnum? AgentSign { get; set; }

    /// <summary>
    /// Наименование операции. Обязателен в случае если <see cref="AgentSign"/> принимает одно из значений:
    /// <list type="bullet">
    /// <item><see cref="AgentSignsTBankEnum.BankPayingAgent"/></item>
    /// <item><see cref="AgentSignsTBankEnum.BankPayingSubagent"/></item>
    /// </list>
    /// </summary>
    public string? OperationName { get; set; }

    /// <summary>
    /// Телефоны платежного агента. Обязателен в случае если <see cref="AgentSign"/> принимает одно из значений:
    /// <list type="bullet">
    /// <item><see cref="AgentSignsTBankEnum.BankPayingAgent"/></item>
    /// <item><see cref="AgentSignsTBankEnum.BankPayingSubagent"/></item>
    /// <item><see cref="AgentSignsTBankEnum.PayingAgent"/></item>
    /// <item><see cref="AgentSignsTBankEnum.PayingSubagent"/></item>
    /// </list>
    /// </summary>
    public IEnumerable<string>? Phones { get; set; }

    /// <summary>
    /// Телефоны оператора по приему платежей. Обязателен в случае если <see cref="AgentSign"/> принимает одно из значений:
    /// <list type="bullet">
    /// <item><see cref="AgentSignsTBankEnum.PayingAgent"/></item>
    /// <item><see cref="AgentSignsTBankEnum.PayingSubagent"/></item>
    /// </list>
    /// </summary>
    public IEnumerable<string>? ReceiverPhones { get; set; }

    /// <summary>
    /// Телефоны оператора перевода. Обязателен в случае если <see cref="AgentSign"/> принимает одно из значений:
    /// <list type="bullet">
    /// <item><see cref="AgentSignsTBankEnum.BankPayingAgent"/></item>
    /// <item><see cref="AgentSignsTBankEnum.BankPayingSubagent"/></item>
    /// </list>
    /// </summary>
    public List<string>? TransferPhones { get; set; }

    /// <summary>
    /// Наименование оператора перевода. Обязателен в случае если <see cref="AgentSign"/> принимает одно из значений:
    /// <list type="bullet">
    /// <item><see cref="AgentSignsTBankEnum.BankPayingAgent"/></item>
    /// <item><see cref="AgentSignsTBankEnum.BankPayingSubagent"/></item>
    /// </list>
    /// </summary>
    public string? OperatorName { get; set; }

    /// <summary>
    /// Адрес оператора перевода. Обязателен в случае если <see cref="AgentSign"/> принимает одно из значений:
    /// <list type="bullet">
    /// <item><see cref="AgentSignsTBankEnum.BankPayingAgent"/></item>
    /// <item><see cref="AgentSignsTBankEnum.BankPayingSubagent"/></item>
    /// </list>
    /// </summary>
    public string? OperatorAddress { get; set; }

    /// <summary>
    /// ИНН оператора перевода. Обязателен в случае если <see cref="AgentSign"/> принимает одно из значений:
    /// <list type="bullet">
    /// <item><see cref="AgentSignsTBankEnum.BankPayingAgent"/></item>
    /// <item><see cref="AgentSignsTBankEnum.BankPayingSubagent"/></item>
    /// </list>
    /// </summary>
    public string? OperatorInn { get; set; }
}