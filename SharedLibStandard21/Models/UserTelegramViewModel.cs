////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using System;

namespace SharedLib;

/// <summary>
/// UserTelegramViewModel
/// </summary>
public class UserTelegramViewModel : UserTelegramBaseModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// User's or bot’s first name
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public string? NormalizedFirstNameUpper { get; set; }

    /// <summary>
    /// Optional. User's or bot’s last name
    /// </summary>
    public string? NormalizedLastNameUpper { get; set; }

    /// <summary>
    /// Optional. User's or bot’s username
    /// </summary>
    public string? NormalizedUsernameUpper { get; set; }


    /// <summary>
    /// LastMessageUtc
    /// </summary>
    public DateTime LastUpdateUtc { get; set; }

    /// <summary>
    /// LastMessageId
    /// </summary>
    public int LastMessageId { get; set; }
}