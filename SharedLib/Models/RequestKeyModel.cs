////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// RequestKeyModel
/// </summary>
public class RequestKeyModel
{
    /// <inheritdoc/>
    public string? RequestKey {  get; set; }
    
    /// <inheritdoc/>
    public DateTime Expire { get; set; }
}