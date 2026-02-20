////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// MongoConfigModel
/// </summary>
public class MongoConfigModel : HostConfigModel
{
    /// <inheritdoc/>
    public static readonly string Configuration = "MongoDBConfig";

    /// <summary>
    /// Login
    /// </summary>
    public string? Login { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// FilesSystemName
    /// </summary>
    public string FilesSystemName { get; set; } = "files-system";

    /// <summary>
    /// BusTracesSystemName
    /// </summary>
    public string BusTracesSystemName { get; set; } = "receivers-traces-db";

    /// <inheritdoc/>
    public override string ToString()
    {
        return string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password)
            ? $"{Scheme}://{Host}:{Port}"
            : $"{Scheme}://{Login}:{Password}@{Host}:{Port}";
    }
}