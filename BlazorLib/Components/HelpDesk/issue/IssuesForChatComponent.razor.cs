////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using BlazorLib;
using SharedLib;

namespace BlazorLib.Components.HelpDesk.issue;

/// <summary>
/// Обращения/заявки для чата
/// </summary>
public partial class IssuesForChatComponent : BlazorBusyComponentBaseModel
{
    /// <summary>
    /// Issues
    /// </summary>
    [Parameter, EditorRequired]
    public required IssueHelpDeskModel[] Issues { get; set; }


    bool IsExpand;
}