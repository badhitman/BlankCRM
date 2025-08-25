////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// OrderRegisterRequestModel
/// </summary>
public class OrderRegisterRequestModel
{
    /// <summary>
    /// Board
    /// </summary>
    public BoardStockSharpModel? Board {  get; set; }

    /// <inheritdoc/>
    public string? SecCode { get; set; }

    /// <inheritdoc/>
    public decimal Price { get; set; }

    /// <inheritdoc/>
    public decimal Volume { get; set; }
    
    /// <inheritdoc/>
    public SidesEnum Direction { get; set; }

    /// <inheritdoc/>
    public string? ClientCode { get; set; }

    /// <inheritdoc/>
    public string? ConfirmRequestToken { get; set; }
}