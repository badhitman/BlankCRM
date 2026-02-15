////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Rubrics Set
/// </summary>
public class RubricsSetModel
{
    /// <summary>
    /// OwnerId
    /// </summary>
    public required int OwnerId { get; set; }

    /// <summary>
    /// RubricsIds
    /// </summary>
    public required int[] RubricsIds { get; set; }
}