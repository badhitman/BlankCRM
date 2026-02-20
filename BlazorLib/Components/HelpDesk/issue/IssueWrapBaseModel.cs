////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorLib.Components.HelpDesk.issue;

/// <summary>
/// IssueWrapBaseModel
/// </summary>
public abstract class IssueWrapBaseModel : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    internal IHelpDeskTransmission HelpDeskRepo { get; set; } = default!;

    [Inject]
    internal IRubricsTransmission RubricsRepo { get; set; } = default!;


    [Inject]
    internal NavigationManager NavRepo { get; set; } = default!;


    /// <summary>
    /// Issue
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required IssueHelpDeskModelDB Issue { get; set; }

    /// <summary>
    /// can edit: is admin || executor || author || admin || helpdesk-manager
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required bool CanEdit { get; set; }

    /// <summary>
    /// UsersIdentityDump
    /// </summary>
    [CascadingParameter]
    public List<UserInfoModel> UsersIdentityDump { get; set; } = [];
}