////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// RubricUniMiddleModel
/// </summary>
public class RubricUniMiddleModel : UniversalLayerModel
{
    /// <inheritdoc/>
    public List<RubricIssueHelpDeskModelDB>? NestedRubrics { get; set; }
}