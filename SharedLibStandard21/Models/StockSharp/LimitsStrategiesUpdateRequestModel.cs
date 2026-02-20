////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// LimitsStrategiesUpdateRequestModel
/// </summary>
public class LimitsStrategiesUpdateRequestModel
{
    /// <summary>
    /// Operator
    /// </summary>
    public OperatorsEnum Operator { get; set; }

    /// <summary>
    /// Operand
    /// </summary>
    public decimal Operand { get; set; }
}