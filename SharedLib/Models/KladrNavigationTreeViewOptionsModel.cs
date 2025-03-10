////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// KladrNavigationTreeViewOptionsModel
/// </summary>
public class KladrNavigationTreeViewOptionsModel
{
    /// <summary>
    /// SelectedValuesChangedHandler
    /// </summary>
    public required Action<IReadOnlyCollection<ObjectKLADRModelDB?>> SelectedValuesChangedHandler { get; set; }

    /// <summary>
    /// SelectedNodes
    /// </summary>
    public required int[] SelectedNodes { get; set; }
}