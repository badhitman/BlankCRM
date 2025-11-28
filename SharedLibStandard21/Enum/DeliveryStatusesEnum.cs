////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Статусы доставки
/// </summary>
public enum DeliveryStatusesEnum
{
    /// <summary>
    /// Created
    /// </summary>
    [Description("Created")]
    Created = 10,

    /// <summary>
    /// Progress
    /// </summary>
    [Description("Progress")]
    Progress = 20,

    /// <summary>
    /// Delivered
    /// </summary>
    [Description("Delivered")]
    Delivered = 30,

    /// <summary>
    /// Cancel
    /// </summary>
    [Description("Cancel")]
    Canceled = 1000
}