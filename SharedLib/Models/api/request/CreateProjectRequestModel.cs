////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// CreateProjectRequestModel
/// </summary>
public class CreateProjectRequestModel
{
    /// <summary>
    /// Project
    /// </summary>
    public required ProjectViewModel Project { get; set; }

    /// <summary>
    /// UserId
    /// </summary>
    public required string UserId { get; set; }

    /// <summary>
    /// ContextName
    /// </summary>
    public string? ContextName { get; set; }
}