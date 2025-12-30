////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace SharedLib;

/// <summary>
/// UserTelegramBaseModel
/// </summary>
public class UserTelegramBaseModel
{
    /// <summary>
    /// Unique identifier for this user or bot
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public long UserTelegramId { get; set; }

    /// <summary>
    /// <see langword="true"/>, if this user is a bot
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public bool IsBot { get; set; }

    /// <summary>
    /// User's or bot’s first name
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public string? FirstName { get; set; }


    /// <summary>
    /// Optional. User's or bot’s last name
    /// </summary>
    public string? LastName { get; set; }


    /// <summary>
    /// Optional. User's or bot’s username
    /// </summary>
    public string? Username { get; set; }


    /// <summary>
    /// Optional. <a href="https://en.wikipedia.org/wiki/IETF_language_tag">IETF language tag</a> of the
    /// user's language
    /// </summary>
    public string? LanguageCode { get; set; }

    /// <summary>
    /// Optional. <see langword="true"/>, if this user is a Telegram Premium user
    /// </summary>
    public bool? IsPremium { get; set; }

    /// <summary>
    /// Optional. <see langword="true"/>, if this user added the bot to the attachment menu
    /// </summary>
    public bool? AddedToAttachmentMenu { get; set; }


    /// <summary>
    /// GetName
    /// </summary>
    /// <returns></returns>
    public string GetName()
    {
        string name = $"'{FirstName}";

        if (!string.IsNullOrWhiteSpace(LastName))
            name += $" {LastName.Trim()}'";
        else
            name += "'";

        if (!string.IsNullOrWhiteSpace(Username))
            name += $" @{Username.Trim()}";

        return $"{name}{(IsBot ? " [is bot]" : "")}";
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return GetName();
    }
}