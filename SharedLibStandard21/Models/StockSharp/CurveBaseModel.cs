////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System;

namespace SharedLib;

/// <summary>
/// Класс для работы с кривой
/// </summary>
public class CurveBaseModel
{
    /// <inheritdoc/>
    public List<SBond> BondList { get; private set; } = [];

    /// <inheritdoc/>
    public DateTime CurveDate { get; protected set; }
}