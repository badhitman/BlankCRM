////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using System.Collections.Generic;

namespace SharedLib;

/// <summary>
/// Данные о поставщике
/// </summary>
public class SupplierInfoForReceiptItemTBankModel
{
    /// <summary>
    /// Телефон поставщика
    /// </summary>
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