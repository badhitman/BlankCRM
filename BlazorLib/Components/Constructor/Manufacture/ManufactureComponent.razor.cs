﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Constructor.Manufacture;
using BlazorLib.Components.Constructor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using CodegeneratorLib;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Constructor.Manufacture;

/// <summary>
/// Manufacture
/// </summary>
public partial class ManufactureComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;

    //[Inject]
    //IManufactureService ManufactureRepo { get; set; } = default!;

    /// <inheritdoc/>
    [Inject]
    protected IJSRuntime JsRuntimeRepo { get; set; } = default!;


    /// <summary>
    /// Родительская страница форм
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required ConstructorMainManageComponent ParentFormsPage { get; set; }


    ConfigManufactureComponent _conf = default!;

    EnumerationsManufactureComponent enumerations_ref = default!;
    DocumentsManufactureComponent documents_ref = default!;

    /// <summary>
    /// DirectoryConstructorModelDB
    /// </summary>
    public static readonly string DirectoryTypeName = nameof(DirectoryConstructorModelDB);
    /// <summary>
    /// ElementOfDirectoryConstructorModelDB
    /// </summary>
    public static readonly string ElementOfDirectoryConstructorTypeName = nameof(ElementOfDirectoryConstructorModelDB);

    /// <summary>
    /// DocumentSchemeConstructorModelDB
    /// </summary>
    public static readonly string DocumentSchemeConstructorTypeName = nameof(DocumentSchemeConstructorModelDB);
    /// <summary>
    /// FieldFormConstructorModelDB
    /// </summary>
    public static readonly string FieldFormConstructorTypeName = nameof(FieldFormConstructorModelDB);
    /// <summary>
    /// FieldFormAkaDirectoryConstructorModelDB
    /// </summary>
    public static readonly string FieldFormAkaDirectoryConstructorTypeName = nameof(FieldFormAkaDirectoryConstructorModelDB);

    /// <summary>
    /// Текущий проект
    /// </summary>
    public ProjectModelDb CurrentProject { get; private set; } = default!;

    /// <summary>
    /// Manufacture
    /// </summary>
    public ManageManufactureModelDB? Manufacture { get; private set; }

    TResponseModel<Stream>? downloadSource;
    readonly List<string> _errors = [];
    /// <summary>
    /// Build tree doneAction
    /// </summary>
    public void TreeBuildDoneAction(IEnumerable<TreeItemDataModel> treeItems)
    {
        foreach (TreeItemDataModel tree_item in treeItems)
        {
            if (!string.IsNullOrEmpty(tree_item.ErrorMessage))
                _errors.Add(tree_item.ErrorMessage);

            if (tree_item.Children is not null)
                TreeBuildDoneAction(tree_item.Children.Select(x => new TreeItemDataModel(x)));
        }
    }

    /// <summary>
    /// Reload project data
    /// </summary>
    public async Task ReloadProjectData()
    {
        await SetBusyAsync();

        List<ProjectModelDb> rest_project = await ConstructorRepo.ProjectsReadAsync([ParentFormsPage.MainProject!.Id]);
        CurrentProject = rest_project.Single();

        //TResponseModel<ManageManufactureModelDB> rest_manufacture = await ManufactureRepo.ReadManufactureConfig(ParentFormsPage.MainProject.Id, user.UserId);
        //if (!rest_manufacture.Success())
        //    SnackbarRepo.ShowMessagesResponse(rest_manufacture.Messages);
        //Manufacture = rest_manufacture.Response ?? throw new Exception();
        IsBusyProgress = false;
    }

    async Task Download()
    {
        if (ParentFormsPage.SystemNamesManufacture is null)
            return;

        ArgumentNullException.ThrowIfNull(CurrentProject.Directories);
        ArgumentNullException.ThrowIfNull(CurrentProject.Documents);
        ArgumentNullException.ThrowIfNull(ParentFormsPage.MainProject);

        if (Manufacture is null)
            throw new Exception();

        CodeGeneratorConfigModel conf_gen = Manufacture;
        GeneratorCSharpService gen = new(conf_gen, ParentFormsPage.MainProject);

        StructureProjectModel struct_project = new()
        {
            Enumerations = [.. CurrentProject.Directories.Select(dir => IJournalUniversalService.EnumConvert(dir, ParentFormsPage.SystemNamesManufacture))],
            Documents = [.. CurrentProject.Documents.Select(x => IJournalUniversalService.DocumentConvert(x, ParentFormsPage.SystemNamesManufacture))],
        };

        var _err = struct_project
            .Enumerations
            .GroupBy(e => e.SystemName)
            .Select(x => new { SystemName = x.Key, Count = x.Count() })
            .Where(x => x.Count > 1)
            .ToArray();

        if (_err.Length != 0)
        {
            SnackbarRepo.Error($"Существуют конфликты имён перечислений: {string.Join(";", _err.Select(x => $"{x.SystemName} - {x.Count}"))};");
            return;
        }

        _err = struct_project
            .Documents
            .GroupBy(e => e.SystemName)
            .Select(x => new { SystemName = x.Key, Count = x.Count() })
            .Where(x => x.Count > 1)
            .ToArray();

        if (_err.Length != 0)
        {
            SnackbarRepo.Error($"Существуют конфликты имён документов: {string.Join(";", _err.Select(x => $"{x.SystemName} - {x.Count}"))};");
            return;
        }

        //await ManufactureRepo.CreateSnapshot(struct_project, ParentFormsPage.MainProject.Id, Guid.NewGuid().ToString());

        downloadSource = await gen.GetZipArchive(struct_project);
        if (!downloadSource.Success())
        {
            SnackbarRepo.ShowMessagesResponse(downloadSource.Messages);
            return;
        }
        ArgumentNullException.ThrowIfNull(downloadSource.Response);
        string fileName = $"project-{CurrentProject.Id}-codebase-{DateTime.UtcNow}.zip";

        using DotNetStreamReference streamRef = new(stream: downloadSource.Response);
        await JsRuntimeRepo.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        downloadSource.Response.Close();
    }

    /// <inheritdoc/>
    public override void StateHasChangedCall()
    {
        enumerations_ref.ReloadTree();
        enumerations_ref.StateHasChangedCall();

        documents_ref.ReloadTree();
        documents_ref.StateHasChangedCall();
        base.StateHasChangedCall();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await ReloadProjectData();
    }

    /// <inheritdoc/>
    public static string? CheckName(string name)
    {
        if (!string.IsNullOrEmpty(name) && name[..1] != name[..1].ToUpper())
            return "<span class=\"text-danger font-monospace\">Первый символ лучше сделать прописным</span>";

        return null;
    }
}