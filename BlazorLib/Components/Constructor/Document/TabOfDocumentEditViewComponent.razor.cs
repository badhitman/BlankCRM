﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Constructor;
using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Constructor.Document;

/// <summary>
/// Page questionnaire view
/// </summary>
public partial class TabOfDocumentEditViewComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <summary>
    /// DocumentScheme page
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required TabOfDocumentSchemeConstructorModelDB DocumentPage { get; set; }

    /// <summary>
    /// All forms
    /// </summary>
    [Parameter, EditorRequired]
    public required IEnumerable<FormBaseConstructorModel> AllForms { get; set; }

    /// <summary>
    /// Can up move
    /// </summary>
    [Parameter, EditorRequired]
    public required bool CanUpMove { get; set; }

    /// <summary>
    /// Can down move
    /// </summary>
    [Parameter, EditorRequired]
    public bool CanDownMove { get; set; }

    /// <summary>
    /// Set id for page -  handle action
    /// </summary>
    [Parameter, EditorRequired]
    public required Action<int, TabOfDocumentSchemeConstructorModelDB> SetIdForPageHandle { get; set; }

    /// <summary>
    /// Set name for page - handle action
    /// </summary>
    [Parameter, EditorRequired]
    public Action<int, string?> SetNameForPageHandle { get; set; } = default!;

    /// <summary>
    /// DocumentScheme reload - handle action
    /// </summary>
    [Parameter, EditorRequired]
    public Action DocumentReloadHandle { get; set; } = default!;

    /// <summary>
    /// Set hold handle - action
    /// </summary>
    [Parameter, EditorRequired]
    public Action<bool> SetHoldHandle { get; set; } = default!;

    /// <summary>
    /// Update questionnaire - handle action
    /// </summary>
    [Parameter, EditorRequired]
    public Action<DocumentSchemeConstructorModelDB, TabOfDocumentSchemeConstructorModelDB?> UpdateDocumentHandle { get; set; } = default!;

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required ConstructorMainManageComponent ParentFormsPage { get; set; }


    int _selectedFormForAdding;
    /// <summary>
    /// Selected form for adding
    /// </summary>
    protected int SelectedFormForAdding
    {
        get => _selectedFormForAdding;
        set
        {
            _selectedFormForAdding = value;

            if (_selectedFormForAdding < 1)
                addingFormToTabPageName = null;
        }
    }

    // 

    string? NameOrigin { get; set; }
    /// <summary>
    /// Name
    /// </summary>
    protected string? Name
    {
        get => DocumentPage.Name;
        set
        {
            DocumentPage.Name = value ?? "";
            SetNameForPageHandle(DocumentPage.Id, value);
            SetHoldHandle(IsEdited);
        }
    }
    string? DescriptionOrigin { get; set; }
    /// <summary>
    /// Описание
    /// </summary>
    public string? Description
    {
        get => DescriptionOrigin;
        set
        {
            DocumentPage.Description = value;
            SetHoldHandle(IsEdited);
        }
    }

    bool IsInitDelete = false;

    /// <summary>
    /// Current Template InputRichText ref
    /// </summary>
    protected InputRichTextComponent? _currentTemplateInputRichText_ref;

    /// <summary>
    /// Can`t save?
    /// </summary>
    protected bool CantSave => string.IsNullOrWhiteSpace(DocumentPage.Name) || (DocumentPage.Id > 0 && DocumentPage.Name == NameOrigin && DocumentPage.Description == DescriptionOrigin && DocumentPage.Description == DescriptionOrigin);
    bool IsEdited => DocumentPage.Name != NameOrigin || DocumentPage.Description != DescriptionOrigin;

    /// <summary>
    /// Перемещение страницы опроса/анкеты (сортировка страниц внутри опроса/анкеты)
    /// </summary>
    protected async Task MoveRow(DirectionsEnum direct)
    {
        await SetBusyAsync();

        TResponseModel<DocumentSchemeConstructorModelDB> rest = await ConstructorRepo.MoveTabOfDocumentSchemeAsync(new() { Payload = new() { Id = DocumentPage.Id, Direct = direct }, SenderActionUserId = CurrentUserSession!.UserId });
        IsBusyProgress = false;

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
            return;
        }
        if (rest.Response is null)
        {
            SnackBarRepo.Error($"Ошибка 671CB343-ADD5-46AE-91F8-24175FBF2592 Content [rest.DocumentScheme is null]");
            return;
        }

        UpdateDocumentHandle(rest.Response, DocumentPage);
    }

    /// <summary>
    /// Delete
    /// </summary>
    protected async Task Delete()
    {
        if (!IsInitDelete)
        {
            IsInitDelete = true;
            return;
        }
        await SetBusyAsync();

        ResponseBaseModel rest = await ConstructorRepo.DeleteTabOfDocumentSchemeAsync(new() { Payload = DocumentPage.Id, SenderActionUserId = CurrentUserSession!.UserId });
        IsBusyProgress = false;

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
            return;
        }
        DocumentReloadHandle();
    }

    /// <summary>
    /// Отмена редактирования
    /// </summary>
    protected void CancelEditing()
    {
        IsInitDelete = false;
        DocumentPage.Name = NameOrigin ?? "";
        DocumentPage.Description = DescriptionOrigin;
        SetHoldHandle(IsEdited);
    }

    string? addingFormToTabPageName;

    /// <summary>
    /// Add form to page
    /// </summary>
    protected async Task AddFormToPage()
    {
        await SetBusyAsync();

        ResponseBaseModel rest = await ConstructorRepo.CreateOrUpdateTabDocumentSchemeJoinFormAsync(new()
        {
            Payload = new FormToTabJoinConstructorModelDB()
            {
                FormId = SelectedFormForAdding,
                TabId = DocumentPage.Id,
                Name = addingFormToTabPageName,
            },
            SenderActionUserId = CurrentUserSession!.UserId
        });
        IsBusyProgress = false;

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
            return;
        }

        SelectedFormForAdding = 0;
        await ReloadPage();
    }

    /// <summary>
    /// Save page
    /// </summary>
    protected async Task SavePage()
    {
        await SetBusyAsync();

        TResponseModel<TabOfDocumentSchemeConstructorModelDB> rest = await ConstructorRepo.CreateOrUpdateTabOfDocumentSchemeAsync(new() { Payload = new EntryDescriptionOwnedModel() { Id = DocumentPage.Id, OwnerId = DocumentPage.OwnerId, Name = DocumentPage.Name, Description = DocumentPage.Description }, SenderActionUserId = CurrentUserSession!.UserId });
        IsBusyProgress = false;

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
            return;
        }
        if (rest.Response is null)
        {
            SnackBarRepo.Error($"Ошибка 07653445-0B30-46CB-9B79-3B068BAB9AEB rest.Content.DocumentPage is null");
            return;
        }
        int i = DocumentPage.Id;
        DocumentPage.Id = rest.Response.Id;
        SetIdForPageHandle(i, rest.Response);

        DescriptionOrigin = Description;
        NameOrigin = Name;
        IsInitDelete = false;

        SetHoldHandle(IsEdited);
    }

    async Task ReloadPage()
    {
        if (DocumentPage.Id < 1)
            return;

        await SetBusyAsync();
        TResponseModel<TabOfDocumentSchemeConstructorModelDB> rest = await ConstructorRepo.GetTabOfDocumentSchemeAsync(DocumentPage.Id);
        IsBusyProgress = false;

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            SnackBarRepo.Error($"Ошибка 815BCE17-9180-4C27-8016-BEB5244A3454 Action: {rest.Message()}");
            return;
        }
        if (rest.Response is null)
        {
            SnackBarRepo.Error($"Ошибка 5B879025-EC6E-4989-9A75-5844BD20DF0B Content [rest.Content.DocumentPage is null]");
            return;
        }

        DocumentPage.JoinsForms = rest.Response?.JoinsForms;
        DocumentPage.Owner = rest.Response?.Owner;
        SetIdForPageHandle(DocumentPage.Id, DocumentPage);
        StateHasChanged();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await ReadCurrentUser();

        await ReloadPage();
        NameOrigin = DocumentPage.Name;
        DescriptionOrigin = DocumentPage.Description;
    }
}