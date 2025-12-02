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
}