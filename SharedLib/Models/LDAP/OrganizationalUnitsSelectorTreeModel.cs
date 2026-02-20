////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Дерево OU структуры
/// </summary>
public class OrganizationalUnitsSelectorTreeModel
{
    /// <summary>
    /// Перечень деревьев OU
    /// </summary>
    public List<OrganizationalUnitsSelectorNodeModel> OrganizationalUnitsSelectorTree { get; set; } = [];

    /// <summary>
    /// Конструктор
    /// </summary>
    public OrganizationalUnitsSelectorTreeModel() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="organizational_units_dn">Имена DN для загрузки/инициализации дерева OU</param>
    public OrganizationalUnitsSelectorTreeModel(IEnumerable<string> organizational_units_dn)
    {
        foreach (string organizational_unit_dn in organizational_units_dn)
        {
            List<OuRouteSegmentModel> ou_dn_route = [];

            GlobalTools.OrganizationalUnitsSections(ref ou_dn_route, organizational_unit_dn);


            List<OuRouteSegmentModel> ou_list = [.. ou_dn_route.Where(x => x.LdapRouteSegmentType == LdapRouteSegmentsTypesEnum.Ou)];
            ou_list.Reverse();

            List<OuRouteSegmentModel> dc_list = [.. ou_dn_route.Where(x => x.LdapRouteSegmentType == LdapRouteSegmentsTypesEnum.Dc)];
            dc_list.Reverse();

            ou_dn_route = [.. dc_list, .. ou_list];

            List<OuRouteSegmentModel> doner = [.. ou_dn_route.Where(x => x.LdapRouteSegmentType == LdapRouteSegmentsTypesEnum.Ou)];

            OuRouteSegmentModel? curr_control = doner.FirstOrDefault(x => x.LdapRouteSegmentType == LdapRouteSegmentsTypesEnum.Ou);

            string? control_name = curr_control?.Name;

            if (string.IsNullOrWhiteSpace(control_name))
                return;

            OrganizationalUnitsSelectorNodeModel? ou_node = OrganizationalUnitsSelectorTree.FirstOrDefault(x => x.Name == control_name);
            doner = [.. ou_dn_route];
            doner.RemoveAt(doner.FindLastIndex(x => x.LdapRouteSegmentType == LdapRouteSegmentsTypesEnum.Ou));
            if (ou_node is null)
            {
                ou_node = new OrganizationalUnitsSelectorNodeModel()
                {
                    Name = control_name,
                    OrganizationalUnitDistinguishedNameRoute = doner,
                    Childs = []
                };

                OrganizationalUnitsSelectorTree.Add(ou_node);
            }
            doner = [.. ou_list];
            doner.RemoveAt(doner.FindIndex(x => x.LdapRouteSegmentType == LdapRouteSegmentsTypesEnum.Ou));

            BuildRoute(ref ou_node, doner);
        }
    }

    static void BuildRoute(ref OrganizationalUnitsSelectorNodeModel node, List<OuRouteSegmentModel> route_stack)
    {
        if (route_stack.Count == 0)
            return;

        OuRouteSegmentModel next_node = route_stack.First();
        OrganizationalUnitsSelectorNodeModel? control_node = node.Childs.FirstOrDefault(x => x.Name == next_node.Name);
        if (control_node is null)
        {
            List<OuRouteSegmentModel> acc = [.. node.OrganizationalUnitDistinguishedNameRoute, route_stack.First()];
            control_node = new OrganizationalUnitsSelectorNodeModel()
            {
                Name = next_node.Name,
                OrganizationalUnitDistinguishedNameRoute = acc,
                Parent = node
            };
            node.Childs.Add(control_node);
        }

        List<OuRouteSegmentModel> doner = [.. route_stack];
        doner.RemoveAt(doner.FindIndex(x => x.LdapRouteSegmentType == LdapRouteSegmentsTypesEnum.Ou));
        if (doner.Count == 0)
            return;

        BuildRoute(ref control_node, doner);
    }
}