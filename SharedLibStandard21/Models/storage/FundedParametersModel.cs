////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// FundedParametersModel
/// </summary>
public class FundedParametersModel<T> : StorageBaseModel
{
    /// <summary>
    /// Payload
    /// </summary>
    public T Payload { get; set; }
}