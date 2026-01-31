////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;

namespace SharedLib;

/// <summary>
/// Связь пользователя с организацией
/// </summary>
[Index(nameof(OrganizationId), nameof(UserPersonIdentityId), IsUnique = true)]
[Index(nameof(UserStatus))]
public class UserOrganizationModelDB : PersonalEntryUpdatedModel
{
    /// <summary>
    /// Организация
    /// </summary>
    public OrganizationModelDB? Organization { get; set; }

    /// <summary>
    /// Организация
    /// </summary>
    public int OrganizationId { get; set; }

    /// <summary>
    /// Статус
    /// </summary>
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public UsersOrganizationsStatusesEnum UserStatus { get; set; }
}