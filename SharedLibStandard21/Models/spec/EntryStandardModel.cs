////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Базовая DB модель объекта с поддержкой -> int:Id +string:Name
/// </summary>
public class EntryStandardModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Имя объекта
    /// </summary>
    [NameValid, Required]
    public virtual string Name { get; set; } = "";

    /// <inheritdoc/>
    public static EntryStandardModel Build(string name) => new() { Name = name };

    /// <inheritdoc/>
    public static EntryStandardModel Build(EntryStandardModel sender) => new() { Name = sender.Name };

    /// <inheritdoc/>
    public static EntryStandardModel BuildEmpty() => new() { Name = "" };

    /// <inheritdoc/>
    public virtual void Update(EntryStandardModel elementObjectEdit)
    {
        Name = elementObjectEdit.Name;
        Id = elementObjectEdit.Id;
    }


    /// <inheritdoc/>
    public static bool operator ==(EntryStandardModel e1, EntryStandardModel e2)
        => (e1 is null && e2 is null) || (e1?.Id == e2?.Id && e1?.Name == e2?.Name);

    /// <inheritdoc/>
    public static bool operator !=(EntryStandardModel e1, EntryStandardModel e2)
        => !(e1 == e2);

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;

        if (obj is EntryStandardModel _e)
            return Id == _e.Id && Name == _e.Name;

        return base.Equals(obj);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
        => $"{Id} {Name}".GetHashCode();
}