////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// RubricUniMiddleModel
/// </summary>
[Index(nameof(NormalizedNameUpper)), Index(nameof(ContextName))]
[Index(nameof(SortIndex), nameof(ParentId), nameof(ContextName), IsUnique = true)]
[Index(nameof(Name)), Index(nameof(IsDisabled))]
public class RubricUniMiddleModel : UniversalLayerModel
{
    /// <inheritdoc/>
    public List<RubricIssueHelpDeskModelDB>? NestedRubrics { get; set; }
}