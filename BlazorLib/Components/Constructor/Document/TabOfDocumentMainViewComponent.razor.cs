////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SharedLib;

namespace BlazorLib.Components.Constructor.Document;

/// <summary>
/// Page questionnaire form main view
/// </summary>
public partial class TabOfDocumentMainViewComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    ILogger<TabOfDocumentMainViewComponent> LoggerRepo { get; set; } = default!;

    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public bool CanEdit { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required Action ReloadHandler { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required TabOfDocumentSchemeConstructorModelDB DocumentPage { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required FormToTabJoinConstructorModelDB PageJoinForm { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public bool CanUp { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public bool CanDown { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public int CurrentFormJoinEdit { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required Action<int> JoinFormHoldHandle { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required Action<TabOfDocumentSchemeConstructorModelDB?> UpdatePageActionHandle { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public SessionOfDocumentDataModelDB? SessionOfDocumentData { get; set; }


    /// <summary>
    /// Признак того, что поле находится в состоянии реального использования, а не в конструкторе или режим demo
    /// </summary>
    public bool InUse => PageJoinForm is not null && SessionOfDocumentData is not null;

    /// <inheritdoc/>
    protected async Task DeleteJoinForm()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        await SetBusyAsync();
        ResponseBaseModel rest = await ConstructorRepo.DeleteTabDocumentSchemeJoinFormAsync(new()
        {
            Payload = new()
            {
                DeleteTabDocumentSchemeJoinFormId = PageJoinForm.Id
            },
            SenderActionUserId = CurrentUserSession.UserId
        });

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            ReloadHandler();
        }
        UpdatePageActionHandle(null);
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected string TitleFormJoin => PageJoinForm.ShowTitle == true ? string.IsNullOrWhiteSpace(PageJoinForm.Name) ? PageJoinForm.Form!.Name : PageJoinForm.Name : "";

    bool _join_set_title_origin = false;
    bool SetTitleForm
    {
        get => PageJoinForm.ShowTitle == true;
        set
        {
            PageJoinForm.ShowTitle = value;
            JoinFormHoldHandle(IsEdited ? PageJoinForm.Id : 0);
        }
    }

    string? _join_name_origin;
    string? PageJoinFormName
    {
        get => PageJoinForm.Name;
        set
        {
            PageJoinForm.Name = value;
            JoinFormHoldHandle(IsEdited ? PageJoinForm.Id : 0);
        }
    }

    bool _is_table_origin;
    bool IsTable
    {
        get => PageJoinForm.IsTable;
        set
        {
            PageJoinForm.IsTable = value;
            JoinFormHoldHandle(IsEdited ? PageJoinForm.Id : 0);
        }
    }

    /// <inheritdoc/>
    protected bool IsDisabled => CurrentFormJoinEdit > 0 && CurrentFormJoinEdit != PageJoinForm.Id;
    bool IsEdited => _join_name_origin != PageJoinFormName || SetTitleForm != _join_set_title_origin || IsTable != _is_table_origin;

    /// <inheritdoc/>
    protected async Task DocumentPageJoinFormMove(DirectionsEnum direct)
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        await SetBusyAsync();
        TResponseModel<TabOfDocumentSchemeConstructorModelDB> rest = await ConstructorRepo.MoveTabDocumentSchemeJoinFormAsync(new() { Payload = new() { Id = PageJoinForm.Id, Direct = direct }, SenderActionUserId = CurrentUserSession.UserId });

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            ReloadHandler();
            await SetBusyAsync(false);
            return;
        }
        UpdatePageActionHandle(null);
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected async Task SaveJoinForm()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        FormToTabJoinConstructorModelDB req = new()
        {
            Description = PageJoinForm.Description,
            FormId = PageJoinForm.FormId,
            Id = PageJoinForm.Id,
            IsTable = PageJoinForm.IsTable,
            Name = PageJoinForm.Name,
            TabId = PageJoinForm.TabId,
            ShowTitle = PageJoinForm.ShowTitle,
            SortIndex = PageJoinForm.SortIndex
        };

        await SetBusyAsync();

        ResponseBaseModel rest = await ConstructorRepo.CreateOrUpdateTabDocumentSchemeJoinFormAsync(new() { Payload = req, SenderActionUserId = CurrentUserSession.UserId });

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            ReloadHandler();
            await SetBusyAsync(false);
            return;
        }

        _join_name_origin = PageJoinForm.Name;
        _join_set_title_origin = PageJoinForm.ShowTitle == true;
        _is_table_origin = PageJoinForm.IsTable;
        JoinFormHoldHandle(IsEdited ? PageJoinForm.Id : 0);
        UpdatePageActionHandle(null);
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected void ResetFormJoin()
    {
        PageJoinForm.Name = _join_name_origin;
        PageJoinForm.ShowTitle = _join_set_title_origin;
        PageJoinForm.IsTable = _is_table_origin;
        JoinFormHoldHandle(0);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await ReadCurrentUser();

        _join_name_origin = PageJoinForm.Name;
        _join_set_title_origin = PageJoinForm.ShowTitle == true;
        _is_table_origin = PageJoinForm.IsTable;

        if (PageJoinForm.Form is null)
        {
            LoggerRepo.LogWarning("Дозагрузка [Form] для [PageJoinForm]...");
            await SetBusyAsync();
            TResponseModel<FormConstructorModelDB> rest = await ConstructorRepo.GetFormAsync(PageJoinForm.FormId);

            SnackBarRepo.ShowMessagesResponse(rest.Messages);
            PageJoinForm.Form = rest.Response;
            await SetBusyAsync(false);
        }
    }
}