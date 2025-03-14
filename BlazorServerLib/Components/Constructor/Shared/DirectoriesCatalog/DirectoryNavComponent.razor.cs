﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorWebLib.Components.Constructor.Pages;
using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.Constructor.Shared.DirectoriesCatalog;

/// <summary>
/// Directory Navigation
/// </summary>
public partial class DirectoryNavComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <summary>
    /// Родительская страница форм
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required ConstructorPage ParentFormsPage { get; set; }

    /// <summary>
    /// Событие изменения выбранного справочника/списка
    /// </summary>
    [Parameter, EditorRequired]
    public required Action<int> SelectedDirectoryChangeHandler { get; set; }


    /// <summary>
    /// Current Template InputRichText ref
    /// </summary>
    protected InputRichTextComponent? _currentTemplateInputRichText_ref;
    string images_upload_url = default!;
    Dictionary<string, object> editorConf = default!;

    EntryModel[] allDirectories = default!;

    EntryDescriptionModel? selectedDirectory;

    /// <summary>
    /// Выбранный справочник/список
    /// </summary>
    public int SelectedDirectoryId
    {
        get => _selected_dir_id;
        private set
        {
            if (_selected_dir_id != value)
                SelectedDirectoryChangeHandler(value);

            _selected_dir_id = value;
            if (_selected_dir_id > 0)
                InvokeAsync(async () =>
                {
                    await SetBusy();

                    TResponseModel<EntryDescriptionModel> rest = await ConstructorRepo.GetDirectory(value);
                    IsBusyProgress = false;

                    if (rest.Response is null)
                        throw new Exception();

                    selectedDirectory = rest.Response;
                    Description = selectedDirectory.Description;
                });
        }
    }
    int _selected_dir_id;

    EntryModel directoryObject = default!;
    string? Description { get; set; }

    static readonly DirectoryNavStatesEnum[] ModesForHideSelector =
        [
        DirectoryNavStatesEnum.Create,
        DirectoryNavStatesEnum.Rename,
        DirectoryNavStatesEnum.Delete
        ];

    /// <summary>
    /// Directory navigation state
    /// </summary>
    protected DirectoryNavStatesEnum DirectoryNavState = DirectoryNavStatesEnum.None;

    /// <summary>
    /// Текст кнопки создания справочника
    /// </summary>
    protected string GetTitleForButtonCreate
    {
        get
        {
            if (string.IsNullOrWhiteSpace(directoryObject.Name))
                return "Введите название";

            return "Создать";
        }
    }

    void InitRenameDirectory()
    {
        directoryObject = allDirectories.First(x => x.Id == SelectedDirectoryId);
        Description = selectedDirectory?.Description;
        DirectoryNavState = DirectoryNavStatesEnum.Rename;
    }

    /// <inheritdoc/>
    protected async Task DeleteSelectedDirectory()
    {
        await SetBusy();

        ResponseBaseModel rest = await ConstructorRepo.DeleteDirectory(new() { Payload = SelectedDirectoryId, SenderActionUserId = CurrentUserSession!.UserId });
        IsBusyProgress = false;
        SnackbarRepo.ShowMessagesResponse(rest.Messages);

        if (!rest.Success())
        {
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
        }

        await ReloadDirectories();
        SelectedDirectoryChangeHandler(SelectedDirectoryId);
    }

    /// <inheritdoc/>
    protected async Task CreateDirectory()
    {
        if (ParentFormsPage.MainProject is null)
            throw new Exception("Не выбран текущий/основной проект");

        await SetBusy();

        TResponseModel<int> rest = await ConstructorRepo.UpdateOrCreateDirectory(new() { Payload = new() { Name = directoryObject.Name, ProjectId = ParentFormsPage.MainProject.Id, Description = Description }, SenderActionUserId = CurrentUserSession!.UserId });
        SnackbarRepo.ShowMessagesResponse(rest.Messages);
        if (rest.Success())
        {
            ResetNavForm();
            await ReloadDirectories();
            SelectedDirectoryId = rest.Response;
        }
        else
        {
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
        }
    }

    async Task CancelCreatingDirectory()
    {
        ResetNavForm();
        await SetBusy();

        TResponseModel<EntryDescriptionModel> res = await ConstructorRepo.GetDirectory(_selected_dir_id);
        IsBusyProgress = false;

        if (res.Response is null)
            throw new Exception();

        selectedDirectory = res.Response;
        Description = selectedDirectory.Description;
    }

    /// <inheritdoc/>
    protected async Task SaveRenameDirectory()
    {
        if (selectedDirectory is null || ParentFormsPage.MainProject is null)
            throw new Exception("Не выбран текущий/основной проект");

        await SetBusy();

        TResponseModel<int> rest = await ConstructorRepo.UpdateOrCreateDirectory(new()
        {
            Payload = EntryConstructedModel.Build(directoryObject, ParentFormsPage.MainProject.Id, Description),
            SenderActionUserId = CurrentUserSession!.UserId
        });
        IsBusyProgress = false;
        SnackbarRepo.ShowMessagesResponse(rest.Messages);
        selectedDirectory.Description = Description;
        if (!rest.Success())
        {
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
        }

        await ReloadDirectories();
    }

    void ResetNavForm(bool stateHasChanged = false)
    {
        directoryObject = EntryModel.BuildEmpty();
        Description = null;
        DirectoryNavState = DirectoryNavStatesEnum.None;

        if (stateHasChanged)
            StateHasChanged();
    }

    /// <summary>
    /// Перезагрузить селектор справочников/списков
    /// </summary>
    public async Task ReloadDirectories(bool stateHasChanged = false)
    {
        if (ParentFormsPage.MainProject is null)
            throw new Exception("Не выбран основной/используемый проект");

        ResetNavForm();

        await SetBusy();

        TResponseModel<EntryModel[]> rest = await ConstructorRepo.GetDirectories(new() { ProjectId = ParentFormsPage.MainProject.Id });

        allDirectories = rest.Response ?? throw new Exception();

        if (allDirectories.Length == 0)
            SelectedDirectoryId = -1;
        else if (allDirectories.Any(x => x.Id == SelectedDirectoryId) != true)
            SelectedDirectoryId = allDirectories.FirstOrDefault()?.Id ?? 0;

        SelectedDirectoryChangeHandler(SelectedDirectoryId);
        await SetBusy(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        images_upload_url = $"{GlobalStaticConstants.TinyMCEditorUploadImage}{GlobalStaticConstants.Routes.CONSTRUCTOR_CONTROLLER_NAME}/{GlobalStaticConstants.Routes.DIRECTORY_CONTROLLER_NAME}?{nameof(StorageMetadataModel.PrefixPropertyName)}={GlobalStaticConstants.Routes.DEFAULT_CONTROLLER_NAME}&{nameof(StorageMetadataModel.OwnerPrimaryKey)}={SelectedDirectoryId}";
        editorConf = GlobalStaticConstants.TinyMCEditorConf(images_upload_url);

        await SetBusy();
        await ReadCurrentUser();
        await ReloadDirectories();
    }
}