////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// OrderRegisterRequestResponseModel
/// </summary>
public class OrderRegisterRequestResponseModel : ResponseBaseModel
{
    /// <inheritdoc/>
    public OrderRegisterRequestModel? OrderRegisterRequest { get; set; }
}