////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// RootKLADREquatableModel
/// </summary>
public class RootKLADREquatableModel(string code, string name, string socr) : IEquatable<RootKLADRModel>
{
    /// <inheritdoc/>
    public string Code { get; } = code;

    /// <inheritdoc/>
    public string Name { get; } = name;

    /// <inheritdoc/>
    public string Socr { get; } = socr;

    /// <inheritdoc/>
    public bool Equals(RootKLADRModel? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Code == other.CODE;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is RootKLADRModel state && Equals(state);

    /// <inheritdoc/>
    public override int GetHashCode() => Code.GetHashCode();

    /// <inheritdoc/>
    public override string ToString() => $"{Name} {Socr}";
}