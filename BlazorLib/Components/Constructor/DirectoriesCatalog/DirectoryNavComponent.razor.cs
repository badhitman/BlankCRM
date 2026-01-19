////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Constructor.DirectoriesCatalog;

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
    public required ConstructorMainManageComponent ParentFormsPage { get; set; }

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
                    await SetBusyAsync();

                    TResponseModel<EntryDescriptionModel> rest = await ConstructorRepo.GetDirectoryAsync(value);
                    await SetBusyAsync(false);

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
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        await SetBusyAsync();

        ResponseBaseModel rest = await ConstructorRepo.DeleteDirectoryAsync(new() { Payload = new() { DeleteDirectoryId = SelectedDirectoryId }, SenderActionUserId = CurrentUserSession.UserId });
        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(rest.Messages);

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
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        if (ParentFormsPage.MainProject is null)
            throw new Exception("Не выбран текущий/основной проект");

        await SetBusyAsync();

        TResponseModel<int> rest = await ConstructorRepo.UpdateOrCreateDirectoryAsync(new() { Payload = new() { Name = directoryObject.Name, ProjectId = ParentFormsPage.MainProject.Id, Description = Description }, SenderActionUserId = CurrentUserSession.UserId });
        SnackBarRepo.ShowMessagesResponse(rest.Messages);
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
        await SetBusyAsync(false);
    }

    async Task CancelCreatingDirectory()
    {
        ResetNavForm();
        await SetBusyAsync();

        TResponseModel<EntryDescriptionModel> res = await ConstructorRepo.GetDirectoryAsync(_selected_dir_id);
        await SetBusyAsync(false);

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

        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        await SetBusyAsync();

        TResponseModel<int> rest = await ConstructorRepo.UpdateOrCreateDirectoryAsync(new()
        {
            Payload = EntryConstructedModel.Build(directoryObject, ParentFormsPage.MainProject.Id, Description),
            SenderActionUserId = CurrentUserSession.UserId
        });
        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(rest.Messages);
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
            throw new Exception("No main/used project selected");

        ResetNavForm();

        await SetBusyAsync();

        TResponseModel<EntryModel[]> rest = await ConstructorRepo.GetDirectoriesAsync(new() { ProjectId = ParentFormsPage.MainProject.Id });

        allDirectories = rest.Response ?? throw new Exception();

        if (allDirectories.Length == 0)
            SelectedDirectoryId = -1;
        else if (allDirectories.Any(x => x.Id == SelectedDirectoryId) != true)
            SelectedDirectoryId = allDirectories.FirstOrDefault()?.Id ?? 0;

        SelectedDirectoryChangeHandler(SelectedDirectoryId);
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        images_upload_url = $"{GlobalStaticConstants.TinyMCEditorUploadImage}{GlobalStaticConstantsRoutes.Routes.CONSTRUCTOR_CONTROLLER_NAME}/{GlobalStaticConstantsRoutes.Routes.DIRECTORY_CONTROLLER_NAME}?{nameof(StorageMetadataModel.PrefixPropertyName)}={GlobalStaticConstantsRoutes.Routes.DEFAULT_CONTROLLER_NAME}&{nameof(StorageMetadataModel.OwnerPrimaryKey)}={SelectedDirectoryId}";
        editorConf = GlobalStaticConstants.TinyMCEditorConf(images_upload_url);

        await SetBusyAsync();
        await ReadCurrentUser();
        await ReloadDirectories();
        await SetBusyAsync(false);
    }
}