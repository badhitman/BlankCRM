////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Projected Request
/// </summary>
public class TProjectedRequestStandardModel<T>
{
    /// <summary>
    /// Project id
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// T - Request
    /// </summary>
    public T? Request { get; set; }
}