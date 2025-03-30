////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// KladrBaseElementModel
/// </summary>
public class KladrBaseElementModel
{
    /// <summary>
    /// Code
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// Socr
    /// </summary>
    public required string Socr { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public required string Name { get; set; }


    /// <inheritdoc/>
    public CodeKladrModel Metadata => CodeKladrModel.Build(Code);


    /// <inheritdoc/>
    public static bool operator ==(KladrBaseElementModel a, KladrBaseElementModel b)
        => a.Equals(b);

    /// <inheritdoc/>
    public static bool operator !=(KladrBaseElementModel a, KladrBaseElementModel b)
       => !a.Code.Equals(b.Code) || !a.Name.Equals(b.Name) || !a.Socr.Equals(b.Socr);


    /// <inheritdoc/>
    public override int GetHashCode()
        => $"{Code}'{Socr}'{Name}".GetHashCode();

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is KladrBaseElementModel other)
            return other.Name == Name && other.Code == Code && other.Socr == Socr;

        return false;
    }

    /// <inheritdoc/>
    public static KladrBaseElementModel Build(RootKLADRModelDB item)
        => new() { Code = item.CODE, Name = item.NAME, Socr = item.SOCR };

    /// <inheritdoc/>
    public static KladrBaseElementModel Build(KladrResponseModel item)
        => new() { Code = item.Code, Name = item.Name, Socr = item.Socr };


    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Name} {Socr}";
    }
}