﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Constructor;
using BlazorLib.Components.Shared.tabs;
using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Constructor.Form;

/// <summary>
/// Edit form dialog
/// </summary>
public partial class EditFormDialogComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;


    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; } = default!;

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public FormConstructorModelDB Form { get; set; } = default!;

    /// <summary>
    /// Родительская страница форм
    /// </summary>
    [Parameter, EditorRequired]
    public required ConstructorMainManageComponent ParentFormsPage { get; set; }


    TabSetComponent tab_set_ref = default!;

    /// <inheritdoc/>
    protected FieldsFormViewComponent? _fields_view_ref;

    /// <inheritdoc/>
    protected bool IsEdited => !Form.Equals(FormEditObject) || FormEditObject.Id == 0;

    /// <inheritdoc/>
    protected InputRichTextComponent? _currentTemplateInputRichText;

    /// <inheritdoc/>
    protected FormConstructorModelDB FormEditObject = default!;

    /// <inheritdoc/>
    protected void Close() => MudDialog.Close(DialogResult.Ok(Form));

    void ResetForm()
    {
        FormEditObject = FormConstructorModelDB.Build(Form);
        _currentTemplateInputRichText?.SetValue(FormEditObject.Description);
    }

    /// <inheritdoc/>
    protected async Task SaveForm()
    {
        await SetBusyAsync();
        TResponseModel<FormConstructorModelDB> rest = await ConstructorRepo.FormUpdateOrCreateAsync(new() { Payload = FormEditObject, SenderActionUserId = CurrentUserSession!.UserId });
        IsBusyProgress = false;
        SnackbarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
            return;
        }
        if (rest.Response is null)
        {
            SnackbarRepo.Error($"Ошибка B64393D8-9C60-4A84-9790-15EFA6A74ABB rest content form is null");
            return;
        }

        Form.Reload(rest.Response);
        ResetForm();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await ReadCurrentUser();
        ResetForm();
    }
}