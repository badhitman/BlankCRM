////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Constructor.FieldsRowsEditUI;

/// <summary>
/// Field form row view
/// </summary>
public partial class FieldFormRowViewComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IJSRuntime JsRuntimeRepo { get; set; } = default!;

    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <summary>
    /// Форма
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required FormConstructorModelDB Form { get; set; }

    /// <summary>
    /// Родительская страница форм
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required ConstructorMainManageComponent ParentFormsPage { get; set; }

    /// <summary>
    /// Поле формы
    /// </summary>
    [Parameter, EditorRequired]
    public required FieldFormBaseLowConstructorModel Field { get; set; }

    /// <summary>
    /// Перезагрузка полей (обработчик события)
    /// </summary>
    [Parameter, EditorRequired]
    public required Action<FormConstructorModelDB?> ReloadFieldsHandler { get; set; }


    FieldFormBaseLowConstructorModel _field_master = default!;

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
                    _type_name = sf.TypeField switch
                    {
                        TypesFieldsFormsEnum.Generator => $"<span class='badge bg-info text-dark text-wrap'>{sf.TypeField.DescriptionInfo()}</span>",
                        TypesFieldsFormsEnum.ProgramCalculationDouble => $"<span class='badge bg-primary text-wrap'>{sf.TypeField.DescriptionInfo()}</span>",
                        _ => sf.TypeField.DescriptionInfo(),
                    };
                }
                else if (_field_master is FieldFormAkaDirectoryConstructorModelDB df)
                    _type_name = $"<span class='badge bg-success text-wrap position-relative'>Справочник/Список{(df.IsMultiSelect ? "<span title='мульти-выбор' class='position-absolute top-0 start-100 translate-middle p-1 ms-1 bg-danger border border-light rounded-circle'>ml<span class='visually-hidden'>multi select</span></span>" : "")}</span>";
                else
                {
                    string msg = "ошибка CDAD94BA-51E8-49F4-9B15-6901494B8EE4";
                    SnackBarRepo.Error(msg);
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
                    string? _descriptor = sf.GetMetadataValue(MetadataExtensionsFormFieldsEnum.Descriptor)?.ToString();
                    string? _parameter = sf.GetMetadataValue(MetadataExtensionsFormFieldsEnum.Parameter)?.ToString();

                    if (!string.IsNullOrEmpty(_descriptor))
                    {
                        DeclarationAbstraction? _d = string.IsNullOrEmpty(_descriptor) ? null : DeclarationAbstraction.GetHandlerService(_descriptor);
                        _information_field = $"<b title='Имя вызываемого метода'>{_d?.Name ?? _descriptor}</b> {(string.IsNullOrWhiteSpace(_parameter) ? "" : $"<code title='параметры запуска'>{_parameter}</code>")}";
                    }

                    if (!string.IsNullOrEmpty(_parameter) && _parameter.TryParseJson(out string[]? out_res) && out_res is not null && out_res.Length != 0)
                    {
                        string[] lost_fields = [.. out_res.Where(x => !Form.AllFields.Any(y => y.Name.Equals(x)))];

                        if (lost_fields.Length != 0)
                        {
                            string msg = $"Некоторых полей нет в форме: {string.Join("; ", lost_fields)};";
                            SnackBarRepo.Error(msg);
                            _information_field = $"{_information_field} <span class='font-monospace text-danger'>{msg}</span>";
                        }
                    }
                }
                else if (_field_master is FieldFormAkaDirectoryConstructorModelDB df)
                    _information_field = df.Directory?.Name;
                else
                {
                    string msg = "ошибка 640D6DCE-0027-425E-81D1-00C16A2D5FCB";
                    SnackBarRepo.Error(msg);
                    _information_field = msg;
                    return (MarkupString)_information_field;
                }
            }
            _information_field ??= "";
            return (MarkupString)_information_field;
        }
    }

    /// <inheritdoc/>
    protected bool CanDown
    {
        get
        {
            if (Field is FieldFormConstructorModelDB sf)
                return sf.SortIndex < (Form.Fields?.Count + Form.FieldsDirectoriesLinks?.Count);
            else if (Field is FieldFormAkaDirectoryConstructorModelDB df)
                return df.SortIndex < (Form.Fields?.Count + Form.FieldsDirectoriesLinks?.Count);
            else
                SnackBarRepo.Error("ошибка C0688447-05EE-4982-B9E0-D48C7DA89C3F");

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
            else if (Field is FieldFormAkaDirectoryConstructorModelDB df)
                return df.SortIndex > 1;
            else
                SnackBarRepo.Error("ошибка EAAC696C-1CDE-41C3-8009-8F8FD4CC2D8E");

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
            else if (_field_master is FieldFormAkaDirectoryConstructorModelDB df)
            {
                if (df.DirectoryId < 1)
                    return false;
            }
            else
                SnackBarRepo.Error($"`{_field_master.GetType().Name}`. ошибка 418856D6-DBCA-4AC3-9322-9C86D6EF115B");

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
                if (deadline_date > DateTime.UtcNow)
                    return $"В работе до {deadline_date}";
                else
                    return "Просрочен";
        }
    }

    /// <inheritdoc/>
    protected async Task ClearValuesForFieldName(int? session_id)
    {
        await SetBusyAsync();
        ResponseBaseModel rest = await ConstructorRepo.ClearValuesForFieldNameAsync(new() { FormId = Form.Id, FieldName = Field.Name, SessionId = session_id });

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            await SetBusyAsync(false);
            return;
        }
        _elements = null;
        if (_sessionsValuesOfFieldViewComponent_Ref is not null)
            await _sessionsValuesOfFieldViewComponent_Ref.FindFields();

        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected void ShowReferrals(EntryDictModel[] elements)
    {
        _elements = elements;
        if (_elements.Length == 0)
            SnackBarRepo.Info("Ссылок нет");

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
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        ResponseBaseModel rest;
        await SetBusyAsync();
        Action act;
        if (_field_master is FieldFormConstructorModelDB sf)
        {
            string? field_valid = sf.GetMetadataValue(MetadataExtensionsFormFieldsEnum.Descriptor)?.ToString();
            if (field_valid is not null && Enum.TryParse(field_valid, out PropsTypesMDFieldsEnum myDescriptor))
            {
                string? parameter = sf.GetMetadataValue(MetadataExtensionsFormFieldsEnum.Parameter)?.ToString();

                switch (myDescriptor)
                {
                    case PropsTypesMDFieldsEnum.TextMask:
                        if (string.IsNullOrWhiteSpace(parameter))
                        {
                            await SetBusyAsync(false);
                            SnackBarRepo.Error("Укажите маску. Выбран режим [Маска], но сама маска не установлена.");
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
            rest = await ConstructorRepo.FormFieldUpdateOrCreateAsync(new() { Payload = req, SenderActionUserId = CurrentUserSession.UserId });
            act = () => { ((FieldFormConstructorModelDB)Field).Update(sf); };
        }
        else if (_field_master is FieldFormAkaDirectoryConstructorModelDB df)
        {
            FieldFormAkaDirectoryConstructorModelDB req = new()
            {
                Id = df.Id,
                Name = df.Name,
                Description = df.Description,
                DirectoryId = df.DirectoryId,
                Css = df.Css,
                Hint = df.Hint,
                OwnerId = df.OwnerId,
                Required = df.Required,
                SortIndex = df.SortIndex,
                IsMultiSelect = df.IsMultiSelect,
            };
            rest = await ConstructorRepo.FormFieldDirectoryUpdateOrCreateAsync(new() { Payload = req, SenderActionUserId = CurrentUserSession.UserId });
            _field_master.Update(req);
            act = () => { ((FieldFormAkaDirectoryConstructorModelDB)Field).Update(df); };
        }
        else
        {
            SnackBarRepo.Error("Ошибка 9ACCA3B7-52ED-4687-BEC2-C16AC6A2C3C0");
            await SetBusyAsync(false);
            return;
        }

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
            await SetBusyAsync(false);
            return;
        }
        act();

        IsEditRow = false;

        _type_name = null;
        _information_field = null;

        await ReloadForm();
        await SetBusyAsync(false);
    }

    async Task ReloadForm()
    {
        await SetBusyAsync();
        TResponseModel<FormConstructorModelDB> rest = await ConstructorRepo.GetFormAsync(Field.OwnerId);

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            SnackBarRepo.Error($"Ошибка CD1DAE53-0199-40BE-9EF2-4A3347BAF5E9 Action: {rest.Message()}");
            await SetBusyAsync(false);
            return;
        }

        if (rest.Response?.Fields is null || rest.Response?.FieldsDirectoriesLinks is null)
        {
            SnackBarRepo.Error($"Ошибка DA9D4B08-EBB7-47C3-BA72-F3BB81E1A7E3 rest.Content.Form?.Fields is null || rest.Content.Form?.FormsDirectoriesLinks is null");
            await SetBusyAsync(false);
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
        else if (_field_master is FieldFormAkaDirectoryConstructorModelDB df)
        {
            if (rest.Response.FieldsDirectoriesLinks.Any(x => x.Id == _field_master.Id))
            {
                Field.Update(rest.Response.FieldsDirectoriesLinks.First(x => x.Id == _field_master.Id));
                _field_master = new FieldFormAkaDirectoryConstructorModelDB()
                {
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
                    IsMultiSelect = df.IsMultiSelect,
                };
            }
        }
        else
        {
            SnackBarRepo.Error($"{_field_master.GetType().FullName}. ошибка C5CB2F55-D973-405F-B92E-144C1ABE2591");
            await SetBusyAsync(false);
            return;
        }
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await ReadCurrentUser();

        SetFieldAction(Field);
    }

    void ChildUpdates()
    {
        if (_field_master is FieldFormConstructorModelDB sf)
            FieldEditUI_ref?.Update(sf);
        else if (_field_master is FieldFormAkaDirectoryConstructorModelDB df)
            FieldDirUI_ref?.Update(df);
    }

    /// <summary>
    /// Удаление поля
    /// </summary>
    protected async Task DeleteClick()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        await SetBusyAsync();
        ResponseBaseModel rest;
        if (_field_master is FieldFormConstructorModelDB sf)
            rest = await ConstructorRepo.FormFieldDeleteAsync(new() { Payload = sf.Id, SenderActionUserId = CurrentUserSession.UserId });
        else if (_field_master is FieldFormAkaDirectoryConstructorModelDB df)
            rest = await ConstructorRepo.FormFieldDirectoryDeleteAsync(new() { Payload = df.Id, SenderActionUserId = CurrentUserSession.UserId });
        else
        {
            SnackBarRepo.Error($"{_field_master.GetType().FullName}. ошибка 1BCDEFB4-55F5-4A5A-BA61-3EAD2E9063D2");
            await SetBusyAsync(false);
            return;
        }

        SnackBarRepo.ShowMessagesResponse(rest.Messages);

        if (!rest.Success())
        {
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
            await SetBusyAsync(false);
            return;
        }

        await ReloadForm();
        await SetBusyAsync(false);
    }

    void SetFieldAction(FieldFormBaseLowConstructorModel field)
    {
        if (field is FieldFormConstructorModelDB sf)
            _field_master = new FieldFormConstructorModelDB()
            {
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
        else if (field is FieldFormAkaDirectoryConstructorModelDB df)
            _field_master = new FieldFormAkaDirectoryConstructorModelDB()
            {
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
                SortIndex = df.SortIndex,
                IsMultiSelect = df.IsMultiSelect,
            };
        else
            SnackBarRepo.Error("error 81F06C12-3641-473B-A2DA-9EFC853A0709");

        _type_name = null;
        _information_field = null;

        StateHasChanged();
    }

    /// <summary>
    /// Сдвиг поля на одну позицию в сторону конца
    /// </summary>
    protected async Task MoveFieldUp()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        await SetBusyAsync();
        TResponseModel<FormConstructorModelDB> rest;
        if (_field_master is FieldFormConstructorModelDB sf)
            rest = await ConstructorRepo.FieldFormMoveAsync(new() { Payload = new() { Id = sf.Id, Direct = DirectionsEnum.Up }, SenderActionUserId = CurrentUserSession.UserId });
        else if (_field_master is FieldFormAkaDirectoryConstructorModelDB df)
            rest = await ConstructorRepo.FieldDirectoryFormMoveAsync(new() { Payload = new() { Id = df.Id, Direct = DirectionsEnum.Up }, SenderActionUserId = CurrentUserSession.UserId });
        else
        {
            SnackBarRepo.Error("ошибка 591195A4-959D-4CDD-9410-F8984F790CBE");
            await SetBusyAsync(false);
            return;
        }

        SnackBarRepo.ShowMessagesResponse(rest.Messages);

        if (!rest.Success())
        {
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
            await SetBusyAsync(false);
            return;
        }

        if (rest.Response is null)
        {
            SnackBarRepo.Error($"Ошибка AA01EFE2-DF81-4CDC-8CAB-D2CAC6B34912 rest.Content.Form is null");
            await SetBusyAsync(false);
            return;
        }
        Form.Reload(rest.Response);
        ReloadFieldsHandler(Form);
        await SetBusyAsync(false);
    }

    /// <summary>
    /// Сдвиг поля на одну позицию в сторону конца
    /// </summary>
    protected async Task MoveFieldDown()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        await SetBusyAsync();
        TResponseModel<FormConstructorModelDB> rest;
        if (_field_master is FieldFormConstructorModelDB sf)
            rest = await ConstructorRepo.FieldFormMoveAsync(new() { Payload = new() { Id = sf.Id, Direct = DirectionsEnum.Down }, SenderActionUserId = CurrentUserSession.UserId });
        else if (_field_master is FieldFormAkaDirectoryConstructorModelDB df)
            rest = await ConstructorRepo.FieldDirectoryFormMoveAsync(new() { Payload = new() { Id = df.Id, Direct = DirectionsEnum.Down }, SenderActionUserId = CurrentUserSession.UserId });
        else
        {
            SnackBarRepo.Error("ошибка 8768E090-BE63-4FE4-A693-7E24ED1A1876");
            await SetBusyAsync(false);
            return;
        }

        SnackBarRepo.ShowMessagesResponse(rest.Messages);

        if (!rest.Success())
        {
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
            await SetBusyAsync(false);
            return;
        }

        if (rest.Response is null)
        {
            SnackBarRepo.Error($"Ошибка 04BD92F1-0B55-46C5-93B3-4DACB7374565 rest.Content.Form is null");
            await SetBusyAsync(false);
            return;
        }
        Form.Reload(rest.Response);
        ReloadFieldsHandler(Form);
        await SetBusyAsync(false);
    }
}