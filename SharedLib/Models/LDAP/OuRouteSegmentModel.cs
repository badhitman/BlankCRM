////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Сегмент маршрута DN пути/имени Ldap:AD
/// </summary>
public class OuRouteSegmentModel
{
    /// <summary>
    /// Тип сегмента маршрута
    /// </summary>
    public LdapRouteSegmentsTypesEnum LdapRouteSegmentType { get; set; }

    /// <summary>
    /// Имя сегмента
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <inheritdoc/>        
    public override string ToString()
    {
        return $"{{{LdapRouteSegmentType}:{Name}}}";
    }
}