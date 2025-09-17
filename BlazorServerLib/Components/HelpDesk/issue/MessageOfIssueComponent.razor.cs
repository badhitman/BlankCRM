﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SharedLib;
using System.ComponentModel;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace BlazorWebLib.Components.HelpDesk.issue;

/// <summary>
/// MessageOfIssueComponent
/// </summary>
public partial class MessageOfIssueComponent : IssueWrapBaseModel
{
    /// <summary>
    /// JS
    /// </summary>
    [Inject]
    protected IJSRuntime JS { get; set; } = default!;


    /// <summary>
    /// Message 
    /// </summary>
    [Parameter]
    public IssueMessageHelpDeskModelDB? Message { get; set; }


    /// <summary>
    /// ParentListIssues
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required IssueMessagesComponent ParentListIssues { get; set; }


    string images_upload_url = default!;
    Dictionary<string, object> editorConf = default!;
    private readonly string _guid = Guid.NewGuid().ToString();

    bool IsCreatingNewMessage => Message is null || Message.Id < 1;

    bool IsEditMode;

    string? TextMessage { get; set; }

    bool CanSave => (IsCreatingNewMessage || TextMessage != Message?.MessageText) && !string.IsNullOrEmpty(TextMessage);

    MarkupString DescriptionHtml => (MarkupString)(Message?.MessageText ?? "");

    /// <summary>
    /// типы отношений к сообщению
    /// </summary>
    enum AuthorsTypesEnum
    {
        /// <summary>
        /// Обычный: не является автором или исполнителем обращения
        /// </summary>
        [Description("secondary")]
        Simple,

        /// <summary>
        /// Системное (сообщение от имени системы)
        /// </summary>
        [Description("success-emphasis")]
        System,

        /// <summary>
        /// Сообщение ваше (вы автор сообщения)
        /// </summary>
        [Description("primary-emphasis")]
        My,

        /// <summary>
        /// Исполнитель
        /// </summary>
        [Description("info-emphasis")]
        Executor,
    }

    AuthorsTypesEnum _currentType = AuthorsTypesEnum.Simple;

    async Task SaveMessage()
    {
        if (string.IsNullOrWhiteSpace(TextMessage))
            throw new ArgumentNullException(nameof(TextMessage));

        await SetBusyAsync();
        TResponseModel<int> rest = await HelpDeskRepo
            .MessageCreateOrUpdateAsync(new()
            {
                SenderActionUserId = CurrentUserSession!.UserId,
                Payload = new()
                {
                    MessageText = TextMessage,
                    IssueId = Issue.Id,
                    Id = Message?.Id ?? 0
                }
            });
        IsBusyProgress = false;
        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        ParentListIssues.AddingNewMessage = false;
        IsEditMode = false;
        await ParentListIssues.ReloadMessages();
    }

    void Cancel()
    {
        TextMessage = Message?.MessageText;
        if (Message is null || Message.Id < 1)
        {
            ParentListIssues.AddingNewMessage = false;
            ParentListIssues.StateHasChangedCall();
        }
        else
        {
            IsEditMode = false;
        }
    }

    /// <inheritdoc/>
    protected override async void OnInitialized()
    {
        await base.OnInitializedAsync();

        images_upload_url = $"{GlobalStaticConstants.TinyMCEditorUploadImage}{Routes.ISSUE_CONTROLLER_NAME}/{Routes.MESSAGE_CONTROLLER_NAME}-{Routes.IMAGE_ACTION_NAME}?{nameof(StorageMetadataModel.PrefixPropertyName)}={Message?.Id}&{nameof(StorageMetadataModel.OwnerPrimaryKey)}={Issue.Id}";
        editorConf = GlobalStaticConstants.TinyMCEditorConf(images_upload_url);

        if (Message is null || Message.Id < 1)
            IsEditMode = true;

        TextMessage = Message?.MessageText;

        if (Message?.AuthorUserId == GlobalStaticConstantsRoles.Roles.System)
            _currentType = AuthorsTypesEnum.System;
        else if (Message?.AuthorUserId == CurrentUserSession!.UserId)
            _currentType = AuthorsTypesEnum.My;
        else if (Message?.AuthorUserId == Issue.ExecutorIdentityUserId)
            _currentType = AuthorsTypesEnum.Executor;
    }

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!IsEditMode)
            await JS.InvokeAsync<int>("FrameHeightUpdate.Reload", _guid);
    }
}