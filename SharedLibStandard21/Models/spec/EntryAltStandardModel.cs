////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// string Id
/// <code>AND</code>
/// string? Name
/// </summary>
public class EntryAltStandardModel
{
    /// <inheritdoc/>
    [Key]
    public string Id { get; set; } = default!;

    /// <inheritdoc/>
    public  string? Name { get; set; }

    /// <inheritdoc/>
    public static EntryAltStandardModel Build(string id, string name) => new() { Id = id, Name = name };

    /// <inheritdoc/>
    public void Update(EntryAltStandardModel other)
    {
        Id = other.Id;
        Name = other.Name;
    }

    /// <inheritdoc/>
    public void Update(string id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <inheritdoc/>
    public static bool operator ==(EntryAltStandardModel e1, EntryAltStandardModel e2)
        => e1.Id == e2.Id && e1.Name == e2.Name;

    /// <inheritdoc/>
    public static bool operator !=(EntryAltStandardModel e1, EntryAltStandardModel e2)
        => e1.Id != e2.Id || e1.Name != e2.Name;

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;

        if (obj is EntryAltStandardModel _e)
            return _e.Id == Id && _e.Name == Name;

        return false;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return $"{Id} {Name}".GetHashCode();
    }
}