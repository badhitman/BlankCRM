////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.HelpDesk.issue;

/// <summary>
/// Обращения/заявки для чата
/// </summary>
public partial class IssuesForChatComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ILogger<IssuesForChatComponent> LoggerRepo { get; set; } = default!;

    /// <summary>
    /// Issues
    /// </summary>
    [Parameter, EditorRequired]
    public required IssueHelpDeskModel[] Issues { get; set; }


    bool IsExpand;
}