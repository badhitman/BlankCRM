////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Модель (view) выбора узла OU 
/// </summary>
public class OrganizationalUnitsSelectorNodeModel
{
    /// <summary>
    /// Имя
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// DN объекта Ldap:AD
    /// </summary>
    public string DistinguishedName
    {
        get
        {
            List<OuRouteSegmentModel> ou_list = [.. OrganizationalUnitDistinguishedNameRoute.Where(x => x.LdapRouteSegmentType == LdapRouteSegmentsTypesEnum.Ou)];
            ou_list.Reverse();

            List<OuRouteSegmentModel> dc_list = [.. OrganizationalUnitDistinguishedNameRoute.Where(x => x.LdapRouteSegmentType == LdapRouteSegmentsTypesEnum.Dc)];
            dc_list.Reverse();

            return $"{string.Join(",", ou_list.Select(x => $"{LdapRouteSegmentsTypesEnum.Ou.ToString().ToUpper()}={x.Name}"))},{string.Join(",", dc_list.Select(x => $"{LdapRouteSegmentsTypesEnum.Dc.ToString().ToUpper()}={x.Name}"))}";
        }
    }

    /// <summary>
    /// Узел выбран (или нет?)
    /// </summary>
    public bool IsSet { get; set; } = false;

    /// <summary>
    /// Маршрут сегментов DN объекта Ldap:AD
    /// </summary>
    public IEnumerable<OuRouteSegmentModel> OrganizationalUnitDistinguishedNameRoute { get; set; } = [];

    /// <summary>
    /// Родительская нода текущего объекта
    /// </summary>
    public OrganizationalUnitsSelectorNodeModel? Parent { get; set; }

    /// <summary>
    /// Вложенные ноды данных
    /// </summary>
    public List<OrganizationalUnitsSelectorNodeModel> Childs { get; set; } = [];

    /// <summary>
    /// Список раскрыт (или нет?)
    /// </summary>
    public bool IsExpand { get; set; }
}