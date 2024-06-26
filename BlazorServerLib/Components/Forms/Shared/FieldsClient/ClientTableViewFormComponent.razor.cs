﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using MudBlazor;
using SharedLib;
using BlazorLib;

namespace BlazorWebLib.Components.Forms.Shared.FieldsClient;

/// <summary>
/// Client table view form
/// </summary>
public partial class ClientTableViewFormComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Inject]
    protected IDialogService DialogServiceRepo { get; set; } = default!;

    /// <inheritdoc/>
    [Inject]
    protected ISnackbar SnackbarRepo { get; set; } = default!;

    /// <inheritdoc/>
    [Inject]
    protected IFormsService FormsRepo { get; set; } = default!;

    /// <inheritdoc/>
    [Parameter]
    public string? Title { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required TabJoinDocumentSchemeConstructorModelDB PageJoinForm { get; set; }

    /// <inheritdoc/>
    [CascadingParameter]
    public SessionOfDocumentDataModelDB? SessionQuestionnaire { get; set; }

    /// <inheritdoc/>
    [CascadingParameter]
    public bool? InUse { get; set; }

    /// <inheritdoc/>
    [CascadingParameter]
    public TabOfDocumentSchemeConstructorModelDB? QuestionnairePage { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required FormConstructorModelDB Form { get; set; }

    /// <inheritdoc/>
    protected static bool IsReadonly(ClaimsPrincipal clp, SessionOfDocumentDataModelDB sq)
    {
        string? email = clp.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Email))?.Value;
        return !clp.Claims.Any(x => x.Type.Equals(ClaimTypes.Role, StringComparison.OrdinalIgnoreCase) && (x.Value.Equals("Admin", StringComparison.OrdinalIgnoreCase))) && sq.SessionStatus >= SessionsStatusesEnum.Sended && !sq.CreatorEmail.Equals(email, StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc/>
    protected bool TableCalculationKit { get; set; } = false;
    
    /// <inheritdoc/>
    protected TableCalculationKitComponent? _table_kit_ref;
    
    /// <inheritdoc/>
    protected void OpenEditRowAction(uint row_num)
    {
        DialogParameters<ClientTableRowEditDialogComponent> parameters = new()
        {
            { x => x.RowNum, row_num },
            { x => x.SessionQuestionnaire, SessionQuestionnaire },
            { x => x.QuestionnairePage, QuestionnairePage },
            { x => x.PageJoinForm, PageJoinForm }
        };

        DialogOptions options = new() { MaxWidth = MaxWidth.ExtraExtraLarge, FullWidth = true, CloseOnEscapeKey = true };
        _ = InvokeAsync(async () =>
        {
            DialogResult result = await DialogServiceRepo.Show<ClientTableRowEditDialogComponent>($"Редактирование строки данных №:{row_num}", parameters, options).Result;
            await ReloadSession();
        });
    }

    /// <inheritdoc/>
    protected void DeleteRowAction(uint row_num)
    {
        if (SessionQuestionnaire is null)
        {
            SnackbarRepo.Add("SessionQuestionnaire is null. error 6146B0D1-0BF3-4CA5-BBF5-5EA64ACA709E", Severity.Error, c => c.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }

        IsBusyProgress = true;
        StateHasChanged();
        _ = InvokeAsync(async () =>
        {
            ValueFieldSessionQuestionnaireBaseModel req = new() { GroupByRowNum = row_num, JoinFormId = PageJoinForm.Id, SessionId = SessionQuestionnaire.Id };
            ResponseBaseModel rest = await FormsRepo.DeleteValuesFieldsByGroupSessionQuestionnaireByRowNum(req);
            IsBusyProgress = false;

            SnackbarRepo.ShowMessagesResponse(rest.Messages);
            if (!rest.Success())
            {
                SnackbarRepo.Add($"Ошибка E7BD5ADD-8CAF-434B-8AB8-94167CCB3337 Action: {rest.Message()}", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
                return;
            }
            await ReloadSession();
        });
    }

    /// <inheritdoc/>
    protected async Task AddRowToTable()
    {
        if (SessionQuestionnaire is null)
        {
            SnackbarRepo.Add("SessionQuestionnaire is null. error DDD591F2-F3DE-4BF1-91DB-B0C4E5D7C93C", Severity.Error, c => c.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }

        FieldSessionQuestionnaireBaseModel row_obj = new()
        {
            JoinFormId = PageJoinForm.Id,
            SessionId = SessionQuestionnaire.Id
        };
        IsBusyProgress = true;
        TResponseStrictModel<int> rest = await FormsRepo.AddRowToTable(row_obj);
        IsBusyProgress = false;

        SnackbarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            SnackbarRepo.Add($"Ошибка B4812CDF-E4F0-46D5-981B-422DC3F966D7 Action: {rest.Message()}", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }
        uint row_num = (uint)rest.Response;
        DialogParameters<ClientTableRowEditDialogComponent> parameters = new()
        {
            { x => x.RowNum, row_num },
            { x => x.SessionQuestionnaire, SessionQuestionnaire },
            { x => x.QuestionnairePage, QuestionnairePage },
            { x => x.PageJoinForm, PageJoinForm }
        };
        DialogOptions options = new() { MaxWidth = MaxWidth.ExtraExtraLarge, FullWidth = true, CloseOnEscapeKey = true };
        DialogResult result = await DialogServiceRepo.Show<ClientTableRowEditDialogComponent>($"Созданная строка данных №{rest.Response}", parameters, options).Result;
        ValueFieldSessionQuestionnaireBaseModel req = new()
        {
            GroupByRowNum = row_num,
            JoinFormId = PageJoinForm.Id,
            SessionId = SessionQuestionnaire.Id,
            IsSelf = true
        };
        _ = await FormsRepo.DeleteValuesFieldsByGroupSessionQuestionnaireByRowNum(req);
        await ReloadSession();
    }

    async Task ReloadSession()
    {
        if (SessionQuestionnaire is null)
        {
            SnackbarRepo.Add("SessionQuestionnaire is null. error BCBB2599-4CC1-433A-A5BC-21114935105F", Severity.Error, c => c.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }
        IsBusyProgress = true;
        TResponseModel<SessionOfDocumentDataModelDB> rest = string.IsNullOrWhiteSpace(SessionQuestionnaire.SessionToken)
        ? await FormsRepo.GetSessionQuestionnaire(SessionQuestionnaire.Id)
        : await FormsRepo.GetSessionQuestionnaire(SessionQuestionnaire.SessionToken);
        IsBusyProgress = false;

        SnackbarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            SnackbarRepo.Add($"Ошибка 3755827F-4811-4927-8ABC-66896D12803B Action: {rest.Message()}", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }

        if (rest.Response is not null)
            SessionQuestionnaire.Reload(rest.Response);

        _table_kit_ref?.Update();

        StateHasChanged();
    }
}