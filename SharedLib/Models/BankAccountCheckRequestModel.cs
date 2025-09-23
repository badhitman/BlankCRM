////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// GetTBankAccountsRequest
/// </summary>
public class BankAccountCheckRequestModel
{
    /// <inheritdoc/>
    public int BankConnectionId { get; set; }

    /// <summary>
    /// Номер счета.
    /// </summary>
    /// <remarks>
    /// Requirements: Value must match regular expression ^(\d{20})$
    /// </remarks>
    public required string AccountNumber { get; set; }

    /// <summary>
    /// Дата начала периода, включительно.
    /// </summary>
    public required DateTime FromDate { get; set; }
}