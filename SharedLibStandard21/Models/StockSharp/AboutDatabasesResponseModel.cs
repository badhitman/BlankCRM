////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// AboutDatabasesResponseModel
/// </summary>
public class AboutDatabasesResponseModel
{
    /// <inheritdoc/>
    public string? DriverDatabase {  get; set; }
    
    /// <inheritdoc/>
    public string? PropertiesDatabase { get; set; }

    /// <inheritdoc/>
    public string? TelegramBotDatabase { get; set; }

    /// <inheritdoc/>
    public string? NLogDatabase { get; set; }
}