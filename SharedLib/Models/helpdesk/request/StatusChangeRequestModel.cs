////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json.Converters;

namespace SharedLib;

/// <summary>
/// Status change request
/// </summary>
public class StatusChangeRequestModel
{
    /// <summary>
    /// Step
    /// </summary>
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public required StatusesDocumentsEnum Step { get; set; }

    /// <summary>
    /// Document Id
    /// </summary>
    public required int DocumentId { get; set; }
}