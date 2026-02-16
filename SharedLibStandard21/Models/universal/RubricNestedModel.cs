////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;

namespace SharedLib;

/// <summary>
/// Рубрики
/// </summary>
public class RubricNestedModel : UniversalLayerModel
{
    /// <inheritdoc/>
    public virtual List<RubricStandardModel>? NestedRubrics { get; set; }
}