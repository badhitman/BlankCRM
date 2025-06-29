﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Constructor;
using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Constructor;

/// <summary>
/// Sessions view
/// </summary>
public partial class SessionsViewComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IDialogService DialogServiceRepo { get; set; } = default!;

    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;

    [Inject]
    IUsersProfilesService UsersProfilesRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required ConstructorMainManageComponent ParentFormsPage { get; set; }


    IEnumerable<DocumentSchemeConstructorModelDB> DocumentsAll = [];

    int _selectedDocumentSchemeId;
    int SelectedDocumentSchemeId
    {
        get => _selectedDocumentSchemeId;
        set
        {
            _selectedDocumentSchemeId = value;
            if (table is not null)
                InvokeAsync(table.ReloadServerData);
        }
    }

    string? NameSessionForCreate { get; set; }
    string? searchString = null;

    /// <inheritdoc/>
    protected string CreateSessionButtonTitle
    {
        get
        {
            if (ParentFormsPage is null)
                return "Не выбран основной/рабочий проект";

            if (SelectedDocumentSchemeId < 1)
                return "Укажите анкету";

            return "Создать новую ссылку";
        }
    }

    protected private async Task<TableData<SessionOfDocumentDataModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(CurrentUserSession!.UserId))
            throw new Exception("Не определён текущий пользователь");

        if (ParentFormsPage.MainProject is null)
            throw new Exception("Не установлен основной проект");

        RequestSessionsDocumentsRequestPaginationModel req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            FindQuery = searchString,
            DocumentSchemeId = SelectedDocumentSchemeId,
            FilterUserId = CurrentUserSession!.UserId,
            ProjectId = ParentFormsPage.MainProject.Id
        };
        await SetBusyAsync(token: token);
        await Task.Delay(1, token);
        TPaginationResponseModel<SessionOfDocumentDataModelDB> rest = await ConstructorRepo.RequestSessionsDocumentsAsync(req, token);

        IsBusyProgress = false;

        if (rest.Response is null)
        {
            SnackBarRepo.Error($"rest.Content.Sessions is null. error B1F8BCC4-952B-4C5E-B573-6FA5AD7F3A8A");
            return new TableData<SessionOfDocumentDataModelDB>() { TotalItems = totalItems, Items = sessions };
        }

        totalItems = rest.TotalRowsCount;
        sessions = new(rest.Response);

        return new TableData<SessionOfDocumentDataModelDB>() { TotalItems = totalItems, Items = sessions };
    }

    /// <inheritdoc/>
    protected async Task EditSession(SessionOfDocumentDataModelDB session)
    {
        await SetBusyAsync();
        TResponseModel<SessionOfDocumentDataModelDB> rest = await ConstructorRepo.GetSessionDocumentAsync(new() { SessionId = session.Id, IncludeExtra = false });
        IsBusyProgress = false;

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            SnackBarRepo.Error($"Ошибка E42D6754-5044-4D2E-BB8B-549CA385CCC2 Action: {rest.Message()}");
            return;
        }
        StateHasChanged();

        DialogParameters<EditSessionDialogComponent> parameters = new()
        {
            { x => x.Session, rest.Response }
        };
        DialogOptions options = new() { MaxWidth = MaxWidth.ExtraExtraLarge, FullWidth = true, CloseOnEscapeKey = true };
        IDialogReference result = await DialogServiceRepo.ShowAsync<EditSessionDialogComponent>($"Редактирование сессии. Опрос/анкета: '{rest.Response?.Owner?.Name}'", parameters, options);
        if (table is not null)
            await table.ReloadServerData();
    }

    /// <inheritdoc/>
    protected async Task DeleteSession(int session_id)
    {
        await SetBusyAsync();
        ResponseBaseModel rest = await ConstructorRepo.DeleteSessionDocumentAsync(session_id);
        IsBusyProgress = false;

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
            return;

        if (table is not null)
            await table.ReloadServerData();
    }

    /// <inheritdoc/>
    protected async Task OnSearch(string text)
    {
        searchString = text;
        if (table is not null)
            await table.ReloadServerData();
    }

    /// <inheritdoc/>
    protected MudTable<SessionOfDocumentDataModelDB>? table;

    /// <inheritdoc/>
    protected int totalItems;
    List<SessionOfDocumentDataModelDB> sessions = [];

    /// <inheritdoc/>
    protected async Task CreateNewSession()
    {
        if (string.IsNullOrWhiteSpace(NameSessionForCreate))
        {
            SnackBarRepo.Error("Укажите имя ссылки");
            return;
        }

        if (ParentFormsPage.MainProject is null)
            throw new Exception("Не выбран основной/текущий проект");

        SessionOfDocumentDataModelDB req = new()
        {
            Name = NameSessionForCreate,
            NormalizedUpperName = NameSessionForCreate.Trim().ToUpper(),
            OwnerId = SelectedDocumentSchemeId,
            AuthorUser = CurrentUserSession!.UserId,
            ProjectId = ParentFormsPage.MainProject.Id
        };
        await SetBusyAsync();
        TResponseModel<SessionOfDocumentDataModelDB> rest = await ConstructorRepo.UpdateOrCreateSessionDocumentAsync(req);
        IsBusyProgress = false;

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
            return;

        if (rest.Response is null)
        {
            SnackBarRepo.Error($"rest.Content.SessionDocument is null. error 9B2E03C0-0434-4F1A-B4E9-7020575DBDDF");
            return;
        }

        if (sessions.Count != 0)
            sessions.Insert(0, rest.Response);
        else
            sessions.Add(rest.Response);

        SelectedDocumentSchemeId = 0;
        NameSessionForCreate = null;

        if (table is not null)
            await table.ReloadServerData();
    }

    async Task RestUpdate()
    {
        if (ParentFormsPage.MainProject is null)
            throw new Exception("No main/used project selected");

        await SetBusyAsync();

        TPaginationResponseModel<DocumentSchemeConstructorModelDB> rest = await ConstructorRepo.RequestDocumentsSchemesAsync(new() { RequestPayload = new() { PageNum = 0, PageSize = 1000 }, ProjectId = ParentFormsPage.MainProject.Id });

        IsBusyProgress = false;

        if (rest.Response is null)
        {
            SnackBarRepo.Error($"rest.Content.Documents is null. error 0A875193-08AA-4678-824D-213BCE33080F");
            return;
        }

        DocumentsAll = rest.Response;
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await ReadCurrentUser();
        await SetBusyAsync();
        await RestUpdate();
        IsBusyProgress = false;
    }
}