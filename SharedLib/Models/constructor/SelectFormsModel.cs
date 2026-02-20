////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectFormsModel
/// </summary>
public class SelectFormsModel
{
    /// <summary>
    /// Request
    /// </summary>
    public required SimplePaginationRequestStandardModel Request { get; set; }

    /// <summary>
    /// ProjectId
    /// </summary>
    public int ProjectId { get; set; }
}
