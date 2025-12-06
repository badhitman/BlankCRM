////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Адрес организации (базовая модель)
/// </summary>
public class AddressOrganizationBaseModel : EntryModel
{
    /// <summary>
    /// Название
    /// </summary>
    public override required string Name { get; set; }

    #region address 
    /// <summary>
    /// Рубрика
    /// </summary>
    public required int ParentId { get; set; }

    /// <inheritdoc/>
    [Required]
    public required string KladrCode { get; set; }

    /// <inheritdoc/>
    [Required]
    public required string KladrTitle { get; set; }

    /// <summary>
    /// Адрес 
    /// </summary>
    public string? AddressUserComment { get; set; }
    #endregion

    /// <summary>
    /// Контакты
    /// </summary>
    [Required]
    public string? Contacts { get; set; }

    /// <summary>
    /// Организация
    /// </summary>
    public int OrganizationId { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Name} {AddressUserComment} {(string.IsNullOrEmpty(Contacts) ? "" : $" (контакты:{Contacts})")}".Trim().Replace("  ", " ");
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return $"{Id}{Name}{OrganizationId}{AddressUserComment}{ParentId}{Contacts}".GetHashCode();
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        if (obj is AddressOrganizationBaseModel add)
            return
                add.Id == Id &&
                add.Contacts == Contacts &&
                add.OrganizationId == OrganizationId &&
                add.AddressUserComment == AddressUserComment &&
                add.KladrCode == KladrCode &&
                add.Name == Name &&
                add.ParentId == ParentId;

        return base.Equals(obj);
    }

    /// <inheritdoc/>
    public static bool operator ==(AddressOrganizationBaseModel off1, AddressOrganizationBaseModel off2) =>
        off1.Id == off2.Id &&
        off1.Name == off2.Name &&
        off1.ParentId == off2.ParentId &&
        off1.Contacts == off2.Contacts &&
        off1.KladrCode == off2.KladrCode &&
        off1.AddressUserComment == off2.AddressUserComment;

    /// <inheritdoc/>
    public static bool operator !=(AddressOrganizationBaseModel off1, AddressOrganizationBaseModel off2)
    {
        return
                off1.Id != off2.Id ||
                off1.Name != off2.Name ||
                off1.ParentId != off2.ParentId ||
                off1.Contacts != off2.Contacts ||
                off1.KladrCode != off2.KladrCode ||
                off1.AddressUserComment != off2.AddressUserComment;
    }
}