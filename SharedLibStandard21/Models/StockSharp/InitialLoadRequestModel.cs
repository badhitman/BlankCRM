////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;

namespace SharedLib;

/// <summary>
/// InitialLoadRequestModel
/// </summary>
public class InitialLoadRequestModel
{
    /// <inheritdoc/>
    public Dictionary<string, bool> BigPriceDifferences { get; set; }
}