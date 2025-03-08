////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// RegisterJobTempKladrRequestModel
/// </summary>
public class RegisterJobTempKladrRequestModel
{
    /// <summary>
    /// TableName
    /// </summary>
    public required string TableName { get; set; }

    /// <summary>
    /// VoteVal
    /// </summary>
    public required int VoteVal { get; set; }
}