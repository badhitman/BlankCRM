////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLib;

/// <summary>
/// UserTelegramViewModel
/// </summary>
public class UserTelegramViewModel : UserTelegramBaseModel
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <inheritdoc/>
    public virtual List<JoinUserChatViewModel>? ChatsJoins { get; set; }

    /// <inheritdoc/>
    [JsonProperty(Required = Required.Always)]
    public string? NormalizedFirstNameUpper { get; set; }

    /// <inheritdoc/>
    public string? NormalizedLastNameUpper { get; set; }

    /// <inheritdoc/>
    public string? NormalizedUsernameUpper { get; set; }

    /// <inheritdoc/>
    public DateTime LastUpdateUtc { get; set; }

    /// <inheritdoc/>
    public int LastMessageId { get; set; }

    /// <inheritdoc/>
    [NotMapped]
    public virtual List<RoleUserTelegramViewModel>? UserRoles { get; set; }
}