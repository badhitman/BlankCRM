////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// TreeViewOptionsHandler
/// </summary>
public class TreeViewOptionsHandler
{
    /// <summary>
    /// SelectedValuesChangedHandler
    /// </summary>
    public required Action<IReadOnlyCollection<RubricNestedModel?>> SelectedValuesChangedHandler { get; set; }

    /// <summary>
    /// SelectedNodes
    /// </summary>
    public required int[] SelectedNodes { get; set; }
}