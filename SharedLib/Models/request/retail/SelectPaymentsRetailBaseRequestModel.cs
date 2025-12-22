////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectPaymentsRetailBaseRequestModel
/// </summary>
public class SelectPaymentsRetailBaseRequestModel : PeriodBaseModel
{
    /// <summary>
    /// Типы платежей
    /// </summary>
    /// <remarks>
    /// Если в перечне есть NULL, тогда он расценивается как перевод/конвертация
    /// </remarks>
    public List<PaymentsRetailTypesEnum?>? TypesFilter { get; set; }
}