////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using System.Collections.Generic;
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
    public IEnumerable<string>? Phones { get; set; }

    /// <summary>
    /// Наименование поставщика
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// ИНН поставщика
    /// </summary>
    public string? Inn { get; set; }
}