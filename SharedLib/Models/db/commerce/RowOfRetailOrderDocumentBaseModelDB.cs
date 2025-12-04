////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// RowOfRetailOrderDocumentBaseModelDB
/// </summary>
public class RowOfRetailOrderDocumentBaseModelDB : RowOfMiddleDocumentModel
{
    /// <summary>
    /// Сумма
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Comment
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Version
    /// </summary>
    [ConcurrencyCheck]
    public Guid Version { get; set; }
}