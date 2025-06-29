﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Constructor.FieldsClient;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Constructor.Form;

/// <summary>
/// Fields form view
/// </summary>
public partial class FieldsFormViewComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required FormConstructorModelDB Form { get; set; }

    /// <summary>
    /// Родительская страница форм
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required ConstructorMainManageComponent ParentFormsPage { get; set; }


    FieldFormBaseLowConstructorModel? _field_master;

    /// <inheritdoc/>
    protected bool CanSave => !string.IsNullOrWhiteSpace(field_creating_field_ref?.FieldName);
    /// <inheritdoc/>
    protected AddingFieldFormViewComponent? field_creating_field_ref;

    /// <inheritdoc/>
    protected ClientStandardViewFormComponent? client_standard_ref;

    void ReloadFields(FormConstructorModelDB? form = null)
    {
        if (Form.Id < 1)
            return;
        TResponseModel<FormConstructorModelDB> rest;
        if (form is null)
        {
            _ = InvokeAsync(async () =>
            {
                await SetBusyAsync();
                StateHasChanged();
                rest = await ConstructorRepo.GetFormAsync(Form.Id);
                IsBusyProgress = false;
                SnackbarRepo.ShowMessagesResponse(rest.Messages);

                if (rest.Response is null)
                    SnackbarRepo.Error($"Ошибка 129E30BB-F331-4EA1-961C-33F52E13443F rest.Content.Form is null");

                if (!rest.Success())
                {
                    SnackbarRepo.Error($"Ошибка 32E7BE10-7C10-4FC0-80A0-23CF9D176278 Action: {rest.Message()}");
                    return;
                }

                if (rest.Response is null)
                    return;

                Form.Reload(rest.Response);
                if (client_standard_ref is not null)
                    await client_standard_ref.Update(Form);
                StateHasChanged();
            });
        }
        else
        {
            Form.Reload(form);
            _ = InvokeAsync(async () =>
            {
                if (client_standard_ref is not null)
                    await client_standard_ref.Update(Form);

                StateHasChanged();
            });
        }
    }

    /// <inheritdoc/>
    protected async Task CreateField()
    {
        if (_field_master is null)
        {
            SnackbarRepo.Error("child_field_form is null. error FEF46EC6-F26F-4FE2-B569-FFA5D8470171");
            return;
        }
        ResponseBaseModel rest;
        await SetBusyAsync();

        if (_field_master is FieldFormAkaDirectoryConstructorModelDB directory_field)
        {
            rest = await ConstructorRepo.FormFieldDirectoryUpdateOrCreateAsync(new()
            {
                Payload = new FieldFormAkaDirectoryConstructorModelDB()
                {
                    Description = directory_field.Description,
                    DirectoryId = directory_field.DirectoryId,
                    Hint = directory_field.Hint,
                    Name = directory_field.Name,
                    Required = directory_field.Required,
                    OwnerId = Form.Id,
                    Id = directory_field.Id,
                    IsMultiSelect = directory_field.IsMultiSelect,
                },
                SenderActionUserId = CurrentUserSession!.UserId
            });
        }
        else if (_field_master is FieldFormConstructorModelDB standard_field)
        {
            standard_field.OwnerId = Form.Id;
            rest = await ConstructorRepo.FormFieldUpdateOrCreateAsync(new() { Payload = standard_field, SenderActionUserId = CurrentUserSession!.UserId });
        }
        else
        {
            SnackbarRepo.Error("Тип поля не определён 050A59F3-028D-41C8-81AC-34F66EBF3840");
            IsBusyProgress = false;
            return;
        }

        IsBusyProgress = false;

        SnackbarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
            return;
        }

        ReloadFields();
        field_creating_field_ref?.SetTypeField();
        if (client_standard_ref is not null)
            await client_standard_ref.Update(Form);
    }

    /// <inheritdoc/>
    protected void AddingFieldStateHasChangedAction(FieldFormBaseLowConstructorModel _sender, Type initiator)
    {
        if (_field_master is null)
        {
            _field_master = _sender;
            field_creating_field_ref?.Update(_sender);
            StateHasChanged();
            return;
        }

        bool change_type = _field_master.GetType() != _sender.GetType();

        field_creating_field_ref?.Update(_sender);
        if (_sender is FieldFormAkaDirectoryConstructorModelDB directory_field)
        {
            if (initiator == typeof(AddingFieldFormViewComponent))
            {
                _field_master.Required = directory_field.Required;
                _field_master.Name = directory_field.Name;

                if (_field_master is FieldFormAkaDirectoryConstructorModelDB dl)
                    directory_field.DirectoryId = dl.DirectoryId;
            }
            else
                _field_master.Update(directory_field);
        }
        else if (_sender is FieldFormConstructorModelDB standard_field)
        {

            if (initiator == typeof(AddingFieldFormViewComponent))
            {
                _field_master.Required = standard_field.Required;
                _field_master.Name = standard_field.Name;

                if (!change_type && _field_master is FieldFormConstructorModelDB sfl)
                    standard_field.MetadataValueType = sfl.MetadataValueType;
                else
                    standard_field.MetadataValueType = null;
            }
            else
            {
                _field_master.Required = standard_field.Required;
                _field_master.Name = standard_field.Name;
            }
        }
        else
        {
            string msg = $"Тип поля не распознан: {_sender.GetType().FullName}";
            SnackbarRepo.Error($"{msg} 7AC47E91-6CA2-433F-A981-E1E585E04695");
            throw new Exception(msg);
        }

        StateHasChanged();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
}