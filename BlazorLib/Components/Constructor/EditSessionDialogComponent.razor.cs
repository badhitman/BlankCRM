﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Constructor;

/// <summary>
/// Edit session dialog
/// </summary>
public partial class EditSessionDialogComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Inject]
    protected NavigationManager NavManagerRepo { get; set; } = default!;

    /// <inheritdoc/>
    [Inject]
    protected IJSRuntime JsRuntimeRepo { get; set; } = default!;

    /// <inheritdoc/>
    [Inject]
    protected IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter]
    public IMudDialogInstance MudDialog { get; set; } = default!;

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required SessionOfDocumentDataModelDB Session { get; set; }


    string UrlSession => $"{NavManagerRepo.BaseUri}{GlobalStaticConstantsRoutes.Routes.QUESTIONNAIRE_CONTROLLER_NAME}/{GlobalStaticConstantsRoutes.Routes.SESSION_CONTROLLER_NAME}/{Session.SessionToken}";

    /// <inheritdoc/>
    protected async Task ClipboardCopyHandle()
    {
        await JsRuntimeRepo.InvokeVoidAsync("clipboardCopy.copyText", UrlSession);
        SnackBarRepo.Info($"Ссылка {Session.SessionToken} скопирована в буфер обмена");
    }

    /// <inheritdoc/>
    protected async Task DestroyLinkAccess()
    {
        session_origin.SessionToken = null;
        await SaveForm();
        SnackBarRepo.Info($"Ссылка аннулирована");
    }

    /// <inheritdoc/>
    protected async Task ReGenerate()
    {
        session_origin.SessionToken = Guid.Empty.ToString();
        await SaveForm();
        SnackBarRepo.Info($"Ссылка перевыпущена");
    }

    /// <inheritdoc/>
    protected bool IsEdited => Session.Name != session_origin.Name || Session.SessionStatus != session_origin.SessionStatus || (Session.Description ?? "") != (session_origin.Description ?? "") || Session.EmailsNotifications != session_origin.EmailsNotifications || Session.ShowDescriptionAsStartPage != session_origin.ShowDescriptionAsStartPage || Session.DeadlineDate != session_origin.DeadlineDate;

    /// <inheritdoc/>
    protected InputRichTextComponent? _currentTemplateInputRichText;

    SessionOfDocumentDataModelDB session_origin = default!;

    /// <inheritdoc/>
    protected string GetHelperTextForDeadlineDate
    {
        get
        {
            string _res = "Срок действия токена";
            if (!session_origin.DeadlineDate.HasValue || Session.SessionStatus >= SessionsStatusesEnum.Sended)
                return _res;

            if (session_origin.DeadlineDate.Value < DateTime.UtcNow)
                _res += $": просрочен [{(DateTime.UtcNow - session_origin.DeadlineDate.Value):%d}] дней";
            else if (session_origin.DeadlineDate.Value > DateTime.UtcNow)
                _res += $": осталось {(session_origin.DeadlineDate.Value - DateTime.UtcNow):%d} дней";

            return _res;
        }
    }

    /// <inheritdoc/>
    protected void Close() => MudDialog.Close(DialogResult.Ok(Session));

    void ResetForm()
    {
        if (session_origin is null)
            session_origin = (SessionOfDocumentDataModelDB)Session.Clone();
        else
            session_origin.Reload(Session);

        _currentTemplateInputRichText?.SetValue(session_origin.Description);

        StateHasChanged();
    }

    async Task SaveForm()
    {
        await SetBusyAsync();
        TResponseModel<SessionOfDocumentDataModelDB> rest = await ConstructorRepo.UpdateOrCreateSessionDocumentAsync(session_origin);
        IsBusyProgress = false;
        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            SnackBarRepo.Error($"Ошибка 0D394723-0AEC-4CF0-9005-32CB3C39F17C Action: {rest.Message()}");
            return;
        }

        if (rest.Response is null)
        {
            SnackBarRepo.Error($"Ошибка C4F58BEC-547A-4F61-9D40-D9B6F8FC051D rest.Content.Form is null");
            return;
        }

        Session = rest.Response;
        ResetForm();
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        ResetForm();
    }
}