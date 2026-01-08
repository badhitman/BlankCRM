////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace BlazorLib.Components.HelpDesk;

/// <summary>
/// ParametersHelpDeskComponent
/// </summary>
public partial class ToolsHelpDeskComponent
{
    static readonly EntryAltModel[] showMarkersRoles = [new() { Id = GlobalStaticConstantsRoles.Roles.CommerceClient, Name = "Покупатель" }];
    static readonly IEnumerable<string> rolesKit = GlobalStaticConstantsRoles.Roles.AllHelpDeskRoles.Union(
        [GlobalStaticConstantsRoles.Roles.CommerceManager,
        GlobalStaticConstantsRoles.Roles.CommerceClient,
        GlobalStaticConstantsRoles.Roles.Debug,
        GlobalStaticConstantsRoles.Roles.RetailManage,
        GlobalStaticConstantsRoles.Roles.RetailReports,
        GlobalStaticConstantsRoles.Roles.GoodsManage]);
}