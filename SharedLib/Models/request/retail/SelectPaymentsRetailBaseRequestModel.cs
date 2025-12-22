////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectPaymentsRetailBaseRequestModel
/// </summary>
public class SelectPaymentsRetailBaseRequestModel
{   
    /// <inheritdoc/>
    public PaymentsRetailTypesEnum[]? TypesFilter { get; set; }

    /// <inheritdoc/>
    public PaymentsRetailStatusesEnum[]? StatusesFilter { get; set; }

    /// <inheritdoc/>
    public DateTime? Start { get; set; }

    /// <inheritdoc/>
    public DateTime? End { get; set; }
}