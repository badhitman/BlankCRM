////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json.Converters;

namespace SharedLib;

/// <summary>
/// ConsoleIssuesRequestModel
/// </summary>
public class ConsoleIssuesRequestModel : SimpleBaseRequestModel
{
    /// <summary>
    /// FilterUserId
    /// </summary>
    public string? FilterUserId { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public required StatusesDocumentsEnum Status { get; set; }
}