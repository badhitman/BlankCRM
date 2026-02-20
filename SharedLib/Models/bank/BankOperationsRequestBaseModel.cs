////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Получить выписку по счету.
/// Метод для получения списка операций по счету за определенный период по указанным критериям поиска. Данные доступны с июня 2023 года.
/// </summary>
public class BankOperationsRequestBaseModel
{
    /// <summary>
    /// Дата начала периода, включительно.
    /// </summary>
    public required DateTime FromDate { get; set; }
}
