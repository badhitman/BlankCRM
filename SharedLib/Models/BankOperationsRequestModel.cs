////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Получить выписку по счету.
/// Метод для получения списка операций по счету за определенный период по указанным критериям поиска. Данные доступны с июня 2023 года.
/// </summary>
public class BankOperationsRequestModel : BankOperationsRequestBaseModel
{
    /// <summary>
    /// Номер счета.
    /// </summary>
    /// <remarks>
    /// Requirements: Value must match regular expression ^(\d{20})$
    /// </remarks>
    public required string AccountNumber { get; set; }
}
