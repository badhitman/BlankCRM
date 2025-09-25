////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLib;

/// <summary>
/// Данные о поставщике
/// </summary>
public class SupplierInfoForReceiptItemTBankModel
{
    /// <summary>
    /// Телефон поставщика
    /// </summary>
    [NotMapped]
    public required IEnumerable<string> Phones { get; set; }

    /// <summary>
    /// Наименование поставщика
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// ИНН поставщика
    /// </summary>
    public required string Inn { get; set; }
}