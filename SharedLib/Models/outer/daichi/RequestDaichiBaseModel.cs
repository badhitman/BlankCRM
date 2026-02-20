////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// RequestDaichiBaseModel
/// </summary>
public class RequestDaichiBaseModel
{
    /// <summary>
    /// Фильтр результатов по артикулу товара.
    /// </summary>
    [Description("filter[NAME]")]
    public string? NameFilter { get; set; }

    /// <summary>
    /// Фильтр результатов по идентификатору товара.
    /// </summary>
    [Description("filter[XML_ID]")]
    public Guid? IdFilter { get; set; }
}