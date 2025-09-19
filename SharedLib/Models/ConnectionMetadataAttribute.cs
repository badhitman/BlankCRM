////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ConnectionMetadataAttribute
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class ConnectionMetadataAttribute : Attribute
{
    /// <inheritdoc/>
    public required string BaseUrl { get; init; }

    /// <summary>
    /// Выписка операций
    /// </summary>
    public required string GetStatementRequest { get; init; }

    /// <summary>
    /// Перечня счетов
    /// </summary>
    public string? AccListRequest { get; init; }
}