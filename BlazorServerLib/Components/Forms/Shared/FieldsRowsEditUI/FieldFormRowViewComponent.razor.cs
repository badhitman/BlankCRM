﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using BlazorLib;
using BlazorWebLib.Components.Forms.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using SharedLib;

namespace BlazorWebLib.Components.Forms.Shared.FieldsRowsEditUI;

/// <summary>
/// Field form row view
/// </summary>
public partial class FieldFormRowViewComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IJSRuntime JsRuntimeRepo { get; set; } = default!;

    [Inject]
    ISnackbar SnackbarRepo { get; set; } = default!;

    [Inject]
    IFormsService FormsRepo { get; set; } = default!;


    /// <summary>
    /// Форма
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required FormConstructorModelDB Form { get; set; }

    /// <summary>
    /// Родительская страница форм
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required FormsPage ParentFormsPage { get; set; }

    /// <summary>
    /// Поле формы
    /// </summary>
    [Parameter, EditorRequired]
    public required ConstructorFieldFormBaseLowModel Field { get; set; }

    /// <summary>
    /// Перезагрузка полей (обработчик события)
    /// </summary>
    [Parameter, EditorRequired]
    public required Action<FormConstructorModelDB?> ReloadFieldsHandler { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required UserInfoModel CurrentUser { get; set; }


    ConstructorFieldFormBaseLowModel _field_master = default!;

    /// <inheritdoc/>
    protected FieldDirectoryFormRowEditComponent? FieldDirUI_ref;
    /// <inheritdoc/>
    protected FieldFormRowEditComponent? FieldEditUI_ref;

    /// <inheritdoc/>
    protected InputRichTextComponent? _currentTemplateInputRichText_ref;
    /// <inheritdoc/>
    protected SessionsValuesOfFieldViewComponent? _sessionsValuesOfFieldViewComponent_Ref;
    EntryDictModel[]? _elements = null;

    #region row of table (visual)

    /// <inheritdoc/>
    protected MarkupString TypeNameMS => (MarkupString)TypeName;

    string? _type_name;
    string TypeName
    {
        get
        {
            if (_type_name is null)
            {
                if (_field_master is FieldFormConstructorModelDB sf)
                {
                    switch (sf.TypeField)
                    {
                        case TypesFieldsFormsEnum.Generator:
                            _type_name = $"<span class='badge bg-info text-dark text-wrap'>{sf.TypeField.DescriptionInfo()}</span>";
                            break;
                        case TypesFieldsFormsEnum.ProgramCalculationDouble:
                            _type_name = $"<span class='badge bg-primary text-wrap'>{sf.TypeField.DescriptionInfo()}</span>";
                            break;
                        default:
                            _type_name = sf.TypeField.DescriptionInfo();
                            break;
                    }
                }
                else if (_field_master is LinkDirectoryToFormConstructorModelDB df)
                    _type_name = "<span class='badge bg-success text-wrap'>Справочник/Список</span>";
                else
                {
                    string msg = "ошибка CDAD94BA-51E8-49F4-9B15-6901494B8EE4";
                    SnackbarRepo.Add(msg, Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
                    _type_name = msg;
                }
            }

            return _type_name ?? "<ошибка 72FB2301-9AD0-44A7-A99F-D2186F73FE34>";
        }
        set { _type_name = value; }
    }

    string? _information_field;

    /// <inheritdoc/>
    protected MarkupString InformationField
    {
        get
        {
            if (_information_field is null)
            {
                if (_field_master is FieldFormConstructorModelDB sf)
                {
                    string? descriptor = sf.GetValueObjectOfMetadata(MetadataExtensionsFormFieldsEnum.Descriptor)?.ToString();
                    DeclarationAbstraction? _d = DeclarationAbstraction.GetHandlerService(descriptor ?? "");
                    _information_field = $"<b>{_d?.Name ?? descriptor}</b> <u>{sf.GetValueObjectOfMetadata(MetadataExtensionsFormFieldsEnum.Parameter)}</u>";
                }
                else if (_field_master is LinkDirectoryToFormConstructorModelDB df)
                    _information_field = df.Directory?.Name;
                else
                {
                    string msg = "ошибка 640D6DCE-0027-425E-81D1-00C16A2D5FCB";
                    SnackbarRepo.Add(msg, Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
                    _information_field = msg;
                    return (MarkupString)_information_field;
                }
            }
            _information_field ??= $"<ошибка ACE8845D-6DA2-41E1-B420-727BDD5791E1> : {_field_master.GetType().FullName}";
            return (MarkupString)_information_field;
        }
    }

    /// <inheritdoc/>
    protected bool CanDown
    {
        get
        {
            if (Field is FieldFormConstructorModelDB sf)
                return sf.SortIndex < (Form.Fields?.Count() + Form.FormsDirectoriesLinks?.Count());
            else if (Field is LinkDirectoryToFormConstructorModelDB df)
                return df.SortIndex < (Form.Fields?.Count() + Form.FormsDirectoriesLinks?.Count());
            else
                SnackbarRepo.Add("ошибка C0688447-05EE-4982-B9E0-D48C7DA89C3F", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);

            return false;
        }
    }

    /// <inheritdoc/>
    protected bool CanUp
    {
        get
        {
            if (Field is FieldFormConstructorModelDB sf)
                return sf.SortIndex > 1;
            else if (Field is LinkDirectoryToFormConstructorModelDB df)
                return df.SortIndex > 1;
            else
                SnackbarRepo.Add("ошибка EAAC696C-1CDE-41C3-8009-8F8FD4CC2D8E", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);

            return false;
        }
    }
    #endregion

    /// <inheritdoc/>
    protected bool CanSave
    {
        get
        {
            if (_field_master is FieldFormConstructorModelDB sf)
            {

            }
            else if (_field_master is LinkDirectoryToFormConstructorModelDB df)
            {
                if (df.DirectoryId < 1)
                    return false;
            }
            else
                SnackbarRepo.Add($"`{_field_master.GetType().Name}`. ошибка 418856D6-DBCA-4AC3-9322-9C86D6EF115B", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);

            return true;
        }
    }

    /// <inheritdoc/>
    protected bool IsEditRow = false;

    /// <inheritdoc/>
    protected static string GetInfoRow(SessionsStatusesEnum? _mark_as_done, DateTime deadline_date)
    {
        switch (_mark_as_done)
        {
            case SessionsStatusesEnum.Accepted:
                return "Принято";
            case SessionsStatusesEnum.Sended:
                return "На проверке";
            default:
                if (deadline_date > DateTime.Now)
                    return $"В работе до {deadline_date}";
                else
                    return "Просрочен";
        }
    }

    /// <inheritdoc/>
    protected async Task ClearValuesForFieldName(int? session_id)
    {
        IsBusyProgress = true;
        ResponseBaseModel rest = await FormsRepo.ClearValuesForFieldName(new() { FormId = Form.Id, FieldName = Field.Name, SessionId = session_id });
        IsBusyProgress = false;

        SnackbarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
            return;

        _elements = null;
        if (_sessionsValuesOfFieldViewComponent_Ref is not null)
            await _sessionsValuesOfFieldViewComponent_Ref.FindFields();
    }

    /// <inheritdoc/>
    protected void ShowReferrals(EntryDictModel[] elements)
    {
        _elements = elements;
        if (_elements.Length == 0)
            SnackbarRepo.Add("Ссылок нет");

        StateHasChanged();
    }

    /// <inheritdoc/>
    protected void ResetEditField()
    {
        SetFieldAction(Field);
        ChildUpdates();
        _currentTemplateInputRichText_ref?.SetValue(_field_master.Description);
    }

    /// <inheritdoc/>
    protected async Task SaveEditField()
    {
        ResponseBaseModel rest;
        IsBusyProgress = true;
        Action act;
        if (_field_master is FieldFormConstructorModelDB sf)
        {
            string? field_valid = sf.GetValueObjectOfMetadata(MetadataExtensionsFormFieldsEnum.Descriptor)?.ToString();
            if (field_valid is not null && Enum.TryParse(field_valid, out PropsTypesMDFieldsEnum myDescriptor))
            {
                string? parameter = sf.GetValueObjectOfMetadata(MetadataExtensionsFormFieldsEnum.Parameter)?.ToString();

                switch (myDescriptor)
                {
                    case PropsTypesMDFieldsEnum.TextMask:
                        if (string.IsNullOrWhiteSpace(parameter))
                        {
                            IsBusyProgress = false;
                            SnackbarRepo.Error("Укажите маску. Выбран режим [Маска], но сама маска не установлена.");
                            return;
                        }
                        break;
                    case PropsTypesMDFieldsEnum.Template:

                        break;
                    default:

                        break;
                }
            }

            FieldFormConstructorModelDB req = new()
            {
                SystemName = sf.SystemName,
                Css = sf.Css,
                Description = sf.Description,
                Hint = sf.Hint,
                Id = sf.Id,
                MetadataValueType = sf.MetadataValueType,
                Name = sf.Name,
                OwnerId = sf.OwnerId,
                Required = sf.Required,
                SortIndex = sf.SortIndex,
                TypeField = sf.TypeField
            };
            rest = await FormsRepo.FormFieldUpdateOrCreate(req, CurrentUser.UserId);
            act = () => { ((FieldFormConstructorModelDB)Field).Update(sf); };
        }
        else if (_field_master is LinkDirectoryToFormConstructorModelDB df)
        {
            LinkDirectoryToFormConstructorModelDB req = new()
            {
                SystemName = df.SystemName,
                Id = df.Id,
                Name = df.Name,
                Description = df.Description,
                DirectoryId = df.DirectoryId,
                Css = df.Css,
                Hint = df.Hint,
                OwnerId = df.OwnerId,
                Required = df.Required,
                SortIndex = df.SortIndex
            };
            rest = await FormsRepo.FormFieldDirectoryUpdateOrCreate(req, CurrentUser.UserId);
            act = () => { ((LinkDirectoryToFormConstructorModelDB)Field).Update(df); };
        }
        else
        {
            SnackbarRepo.Add("Ошибка 9ACCA3B7-52ED-4687-BEC2-C16AC6A2C3C0", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
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
        act();

        IsEditRow = false;

        _type_name = null;
        _information_field = null;

        await ReloadForm();
    }

    async Task ReloadForm()
    {
        IsBusyProgress = true;
        TResponseModel<FormConstructorModelDB> rest = await FormsRepo.GetForm(Field.OwnerId);
        IsBusyProgress = false;

        SnackbarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            SnackbarRepo.Add($"Ошибка CD1DAE53-0199-40BE-9EF2-4A3347BAF5E9 Action: {rest.Message()}", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }

        if (rest.Response?.Fields is null || rest.Response?.FormsDirectoriesLinks is null)
        {
            SnackbarRepo.Add($"Ошибка DA9D4B08-EBB7-47C3-BA72-F3BB81E1A7E3 rest.Content.Form?.Fields is null || rest.Content.Form?.FormsDirectoriesLinks is null", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }

        ReloadFieldsHandler(rest.Response);

        if (_field_master is FieldFormConstructorModelDB sf)
        {
            if (rest.Response.Fields.Any(x => x.Id == _field_master.Id))
            {
                Field.Update(rest.Response.Fields.First(x => x.Id == _field_master.Id));
                //_field_master.Update(Field);
                _field_master = FieldFormConstructorModelDB.Build(Field);
                //_field_master = new FieldFormConstructorModelDB()
                //{
                //    Css = Field.Css,
                //    Description = Field.Description,
                //    Hint = Field.Hint,
                //    Id = Field.Id,
                //    Name = Field.Name,
                //    OwnerId = Field.OwnerId,
                //    Required = Field.Required
                //};
            }
        }
        else if (_field_master is LinkDirectoryToFormConstructorModelDB df)
        {
            if (rest.Response.FormsDirectoriesLinks.Any(x => x.Id == _field_master.Id))
            {
                Field.Update(rest.Response.FormsDirectoriesLinks.First(x => x.Id == _field_master.Id));
                _field_master = new LinkDirectoryToFormConstructorModelDB()
                {
                    SystemName = Field.SystemName,
                    Css = Field.Css,
                    Description = Field.Description,
                    Hint = Field.Hint,
                    Id = Field.Id,
                    Name = Field.Name,
                    OwnerId = Field.OwnerId,
                    Required = Field.Required,
                    DirectoryId = df.DirectoryId,
                    Directory = df.Directory,
                    Owner = df.Owner,
                };
            }
        }
        else
        {
            SnackbarRepo.Add($"{_field_master.GetType().FullName}. ошибка C5CB2F55-D973-405F-B92E-144C1ABE2591", Severity.Error, cf => cf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            IsBusyProgress = false;
            return;
        }
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        SetFieldAction(Field);
        base.OnInitialized();
    }

    void ChildUpdates()
    {
        if (_field_master is FieldFormConstructorModelDB sf)
            FieldEditUI_ref?.Update(sf);
        else if (_field_master is LinkDirectoryToFormConstructorModelDB df)
            FieldDirUI_ref?.Update(df);
    }

    /// <summary>
    /// Удаление поля
    /// </summary>
    protected async Task DeleteClick()
    {
        IsBusyProgress = true;
        ResponseBaseModel rest;
        if (_field_master is FieldFormConstructorModelDB sf)
            rest = await FormsRepo.FormFieldDelete(sf.Id, CurrentUser.UserId);
        else if (_field_master is LinkDirectoryToFormConstructorModelDB df)
            rest = await FormsRepo.FormFieldDirectoryDelete(df.Id, CurrentUser.UserId);
        else
        {
            SnackbarRepo.Add($"{_field_master.GetType().FullName}. ошибка 1BCDEFB4-55F5-4A5A-BA61-3EAD2E9063D2", Severity.Error, cf => cf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
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
     ;
        await ReloadForm();
    }

    void SetFieldAction(ConstructorFieldFormBaseLowModel field)
    {
        if (field is FieldFormConstructorModelDB sf)
            _field_master = new FieldFormConstructorModelDB()
            {
                SystemName = sf.SystemName,
                Css = sf.Css,
                Description = sf.Description,
                Hint = sf.Hint,
                Id = sf.Id,
                MetadataValueType = sf.MetadataValueType,
                Name = sf.Name,
                Owner = sf.Owner,
                OwnerId = sf.OwnerId,
                Required = sf.Required,
                SortIndex = sf.SortIndex,
                TypeField = sf.TypeField
            };
        else if (field is LinkDirectoryToFormConstructorModelDB df)
            _field_master = new LinkDirectoryToFormConstructorModelDB()
            {
                SystemName = df.SystemName,
                Css = df.Css,
                Description = df.Description,
                Directory = df.Directory,
                DirectoryId = df.DirectoryId,
                Hint = df.Hint,
                Id = df.Id,
                Name = df.Name,
                Owner = df.Owner,
                OwnerId = df.OwnerId,
                Required = df.Required,
                SortIndex = df.SortIndex
            };
        else
            SnackbarRepo.Add("error 81F06C12-3641-473B-A2DA-9EFC853A0709", Severity.Error, cf => cf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);

        _type_name = null;
        _information_field = null;

        StateHasChanged();
    }

    /// <summary>
    /// Сдвиг поля на одну позицию в сторону конца
    /// </summary>
    protected async Task MoveFieldUp()
    {
        IsBusyProgress = true;
        TResponseModel<FormConstructorModelDB> rest;
        if (_field_master is FieldFormConstructorModelDB sf)
            rest = await FormsRepo.FieldFormMove(sf.Id, CurrentUser.UserId, VerticalDirectionsEnum.Up);
        else if (_field_master is LinkDirectoryToFormConstructorModelDB df)
            rest = await FormsRepo.FieldDirectoryFormMove(df.Id, CurrentUser.UserId, VerticalDirectionsEnum.Up);
        else
        {
            SnackbarRepo.Add("ошибка 591195A4-959D-4CDD-9410-F8984F790CBE", Severity.Error, cf => cf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
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

        if (rest.Response is null)
        {
            SnackbarRepo.Add($"Ошибка AA01EFE2-DF81-4CDC-8CAB-D2CAC6B34912 rest.Content.Form is null", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }
        Form.Reload(rest.Response);
        ReloadFieldsHandler(Form);
    }

    /// <summary>
    /// Сдвиг поля на одну позицию в сторону конца
    /// </summary>
    protected async Task MoveFieldDown()
    {
        IsBusyProgress = true;
        TResponseModel<FormConstructorModelDB> rest;
        if (_field_master is FieldFormConstructorModelDB sf)
            rest = await FormsRepo.FieldFormMove(sf.Id, CurrentUser.UserId, VerticalDirectionsEnum.Down);
        else if (_field_master is LinkDirectoryToFormConstructorModelDB df)
            rest = await FormsRepo.FieldDirectoryFormMove(df.Id, CurrentUser.UserId, VerticalDirectionsEnum.Down);
        else
        {
            SnackbarRepo.Add("ошибка 8768E090-BE63-4FE4-A693-7E24ED1A1876", Severity.Error, cf => cf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
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

        if (rest.Response is null)
        {
            SnackbarRepo.Add($"Ошибка 04BD92F1-0B55-46C5-93B3-4DACB7374565 rest.Content.Form is null", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }
        Form.Reload(rest.Response);
        ReloadFieldsHandler(Form);
    }
}