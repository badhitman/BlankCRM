////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorLib.Components.HelpDesk.Pages;

/// <summary>
/// OrdersAttendancesListComponent
/// </summary>
public partial class OrdersAttendancesListComponent(ICommerceTransmission CommerceRepo) : BlazorBusyComponentBaseAuthModel
{
    /// <summary>
    /// OrdersAttendances
    /// </summary>
    [Parameter, EditorRequired]
    public required RecordsAttendanceModelDB[] OrdersAttendances { get; set; }

    /// <summary>
    /// HelpDeskIssueId
    /// </summary>
    [Parameter, EditorRequired]
    public int HelpDeskIssueId { get; set; }

    /// <summary>
    /// UpdateRecords
    /// </summary>
    [Parameter, EditorRequired]
    public required Action<RecordsAttendanceModelDB[]> UpdateRecords { get; set; }

    int? _initDeleteOrder;

    bool CanDeleteRecord(RecordsAttendanceModelDB rec) => CurrentUserSession?.IsAdmin == true || rec.AuthorIdentityUserId == CurrentUserSession?.UserId;

    async Task DeleteRecord(RecordsAttendanceModelDB rec)
    {
        if (CurrentUserSession is null)
            return;

        if (_initDeleteOrder != rec.Id)
        {
            _initDeleteOrder = rec.Id;
            return;
        }

        await SetBusyAsync();
        TResponseModel<RecordsAttendanceModelDB[]> resDel = await CommerceRepo.AttendanceRecordsDeleteAsync(new() { Payload = [rec.Id], SenderActionUserId = CurrentUserSession.UserId });
        SnackBarRepo.ShowMessagesResponse(resDel.Messages);

        TResponseModel<RecordsAttendanceModelDB[]> resReload = await CommerceRepo.RecordsAttendancesByIssuesGetAsync(new() { IssueIds = [HelpDeskIssueId] });
        SnackBarRepo.ShowMessagesResponse(resReload.Messages);

        if (resReload.Success() && resReload.Response is not null)
        {
            OrdersAttendances = resReload.Response;
            UpdateRecords(OrdersAttendances);
        }

        await SetBusyAsync(false);
    }
}
