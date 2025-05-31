////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using SharedLib;
using DbcLib;

namespace ServerLib;

/// <inheritdoc/>
public class ManufactureService(
    IIdentityTransmission IdentityRepo,
    IDbContextFactory<ConstructorContext> mainDbFactory,
    IHttpContextAccessor httpContextAccessor) : IManufactureService
{
    /// <inheritdoc/>
    public async Task CreateSnapshotAsync(StructureProjectModel dump, int projectId, string name, CancellationToken token = default)
    {
        string user_id = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception();

        TResponseModel<UserInfoModel[]> users_find = await IdentityRepo.GetUsersIdentityAsync([user_id], token);
        UserInfoModel current_user = users_find.Response![0];

        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(token);
        using IDbContextTransaction transaction = context_forms.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);

        ProjectSnapshotModelDB _project_snapshot = new()
        {
            UserId = current_user.UserId,
            ProjectId = projectId,
            Name = name,
            Directories = [],
            Documents = []
        };

        _project_snapshot.Directories = [.. dump.Enumerations.Select(_enum_fit =>
        {
            DirectoryEnumSnapshotModelDB _d = new()
            {
                Elements = [],
                Name = _enum_fit.Name,
                SystemName = _enum_fit.SystemName,
                Description = _enum_fit.Description,
                TokenUniqueRoute = $"enum > `{_enum_fit.SystemName}` /{_project_snapshot.Token}",
                Owner = _project_snapshot,
            };

            _d.Elements = [.. _enum_fit.EnumItems.Select(_enum_element_of_directory =>
            {
                DirectoryEnumElementSnapshotModelDB _df = new()
                {
                    Name = _enum_element_of_directory.Name,
                    SystemName = _enum_element_of_directory.SystemName,
                    SortIndex = _enum_element_of_directory.SortIndex,
                    Description = _enum_element_of_directory.Description,
                    TokenUniqueRoute = $"enum: `{_enum_fit.SystemName}`.[{_enum_element_of_directory.SortIndex}] /{_project_snapshot.Token}",
                    Owner = _d,
                };

                return _df;
            })];

            return _d;
        })];

        await context_forms.AddAsync(_project_snapshot, token);
        await context_forms.SaveChangesAsync(token);

        _project_snapshot.Documents = [.. dump.Documents.Select(_doc_fit =>
        {
            DocumentSnapshotModelDB _doc = new()
            {
                Name = _doc_fit.Name,
                SystemName = _doc_fit.SystemName,
                Tabs = [],
                TokenUniqueRoute = $"doc > `{_doc_fit.SystemName}` /{_project_snapshot.Token}",
                Description = _doc_fit.Description,
                Owner = _project_snapshot,
            };
            _doc.Tabs = [.. _doc_fit.Tabs.Select(_tab_fit =>
            {
                TabSnapshotModelDB _tab = new()
                {
                    Name = _tab_fit.Name,
                    SystemName = _tab_fit.SystemName,
                    TokenUniqueRoute = $"tab > `{_doc.SystemName}`.[{_tab_fit.SortIndex}] /{_project_snapshot.Token}",
                    Description = _tab_fit.Description,
                    Owner = _doc,
                    Forms = [],
                    SortIndex = _tab_fit.SortIndex,
                };
                _tab.Forms = [.. _tab_fit.Forms.Select(_form_fit =>
                {
                    FormSnapshotModelDB _form = new()
                    {
                        Fields = [],
                        SortIndex = _form_fit.SortIndex,
                        SystemName = _form_fit.SystemName,
                        TokenUniqueRoute = $"form `{_doc_fit.SystemName}`.[{_tab_fit.SortIndex}].[{_form_fit.SortIndex}] /{_project_snapshot.Token}",
                        Name = _form_fit.Name,
                        Description = _form_fit.Description,
                        Owner = _tab,
                    };
                    if (_form_fit.FieldsAtDirectories?.Count > 0)
                    {
                        _form.Fields ??= [];
                        _form.Fields.AddRange(_form_fit.FieldsAtDirectories.Select(r =>
                        {
                            return new FieldAkaDirectorySnapshotModelDB()
                            {
                                Name = r.Name,
                                SortIndex = r.SortIndex,
                                SystemName = r.SystemName,
                                TokenUniqueRoute = $"fd `{_doc_fit.SystemName}`.[{_tab_fit.SortIndex}].[{_form_fit.SortIndex}].[i:{r.SortIndex}] /{_project_snapshot.Token}",
                                Description = r.Description,
                                Owner = _form,
                                Directory = _project_snapshot.Directories.First(w => w.SystemName == r.DirectorySystemName),
                            };
                        }));
                    }

                    if (_form_fit.SimpleFields?.Count > 0)
                    {
                        _form.Fields ??= [];
                        _form.Fields.AddRange(_form_fit.SimpleFields.Select(e =>
                        {
                            return new FieldSnapshotModelDB()
                            {
                                Name = e.Name,
                                SortIndex = e.SortIndex,
                                SystemName = e.SystemName,
                                TokenUniqueRoute = $"fs `{_doc_fit.SystemName}`.[{_tab_fit.SortIndex}].[{_form_fit.SortIndex}].[i:{e.SortIndex}] /{_project_snapshot.Token}",
                                Description = e.Description,
                                Owner = _form,
                                TypeField = e.TypeField,
                            };
                        }));
                    }
                    return _form;
                })];
                return _tab;
            })];
            return _doc;
        })];

        await context_forms.AddRangeAsync(_project_snapshot.Documents, token);
        await context_forms.SaveChangesAsync(token);
        await transaction.CommitAsync(token);
    }

    /// <inheritdoc/>
    public async Task<List<SystemNameEntryModel>> GetSystemNamesAsync(int manufactureId, CancellationToken token = default)
    {
        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(token);
        return await context_forms
            .SystemNamesManufactures
            .Where(x => x.ManufactureId == manufactureId)
            .Select(x => new SystemNameEntryModel() { Qualification = x.Qualification, TypeDataName = x.TypeDataName, SystemName = x.SystemName, TypeDataId = x.TypeDataId })
            .ToListAsync(cancellationToken: token);
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<ManageManufactureModelDB?>> ReadManufactureConfigAsync(int projectId, string userId, CancellationToken token = default)
    {
        TResponseModel<ManageManufactureModelDB?> res = new();
        TResponseModel<UserInfoModel[]> findUsers = await IdentityRepo.GetUsersIdentityAsync([userId], token);
        if (!findUsers.Success() || findUsers.Response is null)
        {
            res.AddRangeMessages(findUsers.Messages);
            return res;
        }

        UserInfoModel user = findUsers.Response.Single();
        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(token);

        res.Response = await context_forms
            .Manufactures
            .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.UserId == user.UserId, cancellationToken: token);

        if (res.Response is null)
        {
            var project_db = await context_forms
                        .Projects
                        .Where(x => x.Id == projectId)
                        .Select(x => new { x.Id, x.Name })
                        .FirstAsync(cancellationToken: token);

            res.Response = new ManageManufactureModelDB() { UserId = user.UserId, Namespace = GlobalTools.TranslitToSystemName(project_db.Name), ProjectId = project_db.Id };
            await context_forms.AddAsync(res.Response, token);
            await context_forms.SaveChangesAsync(token);
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetOrDeleteSystemNameAsync(UpdateSystemNameModel request, CancellationToken token = default)
    {
        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(token);
        ManufactureSystemNameModelDB? snMan = await context_forms
            .SystemNamesManufactures
            .FirstOrDefaultAsync(x => x.Qualification == request.Qualification && x.TypeDataName == request.TypeDataName && x.ManufactureId == request.ManufactureId && x.TypeDataId == request.TypeDataId, cancellationToken: token);

        if (string.IsNullOrWhiteSpace(request.SystemName))
        {
            if (snMan == null)
                return ResponseBaseModel.CreateInfo("Значение отсутствует. Удаление не требуется.");
            else
            {
                context_forms.Remove(snMan);
                await context_forms.SaveChangesAsync(token);
                return ResponseBaseModel.CreateInfo("Значение удалено.");
            }
        }
        else if (!Regex.IsMatch(request.SystemName, GlobalStaticConstants.SYSTEM_NAME_TEMPLATE))
            return ResponseBaseModel.CreateError(GlobalStaticConstants.SYSTEM_NAME_TEMPLATE_MESSAGE);
        else
        {
            if (snMan == null)
            {
                if (await context_forms.SystemNamesManufactures.AnyAsync(x => x.SystemName == request.SystemName && x.TypeDataName == request.TypeDataName && x.ManufactureId == request.ManufactureId && x.TypeDataId == request.TypeDataId, cancellationToken: token))
                    return ResponseBaseModel.CreateError("Имя не уникально. Задайте другое имя");

                await context_forms.AddAsync(ManufactureSystemNameModelDB.Build(request), token);
                await context_forms.SaveChangesAsync(token);
                return ResponseBaseModel.CreateInfo("Значение создано.");
            }
            else
            {
                if (snMan.SystemName == request.SystemName)
                    return ResponseBaseModel.CreateInfo("Обновления системного имени не требуется.");

                if (await context_forms.SystemNamesManufactures.AnyAsync(x => x.Id != snMan.Id && x.SystemName == request.SystemName && x.TypeDataName == request.TypeDataName && x.ManufactureId == request.ManufactureId && x.TypeDataId == request.TypeDataId, cancellationToken: token))
                    return ResponseBaseModel.CreateError("Имя не уникально. Задайте другое имя");

                snMan.SystemName = request.SystemName;
                context_forms.Update(snMan);
                await context_forms.SaveChangesAsync(token);
                return ResponseBaseModel.CreateInfo("Значение обновлено.");
            }
        }
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateManufactureConfigAsync(ManageManufactureModelDB manufacture, CancellationToken token = default)
    {
        (bool IsValid, List<ValidationResult> ValidationResults) = GlobalTools.ValidateObject(manufacture);
        if (!IsValid)
            return ResponseBaseModel.CreateError(ValidationResults);

        string?[] folder_names = [manufacture.AccessDataDirectoryPath, manufacture.EnumDirectoryPath, manufacture.DocumentsMastersDbDirectoryPath];
        folder_names = [.. folder_names
            .GroupBy(x => x)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key)];

        if (folder_names.Length != 0)
            return ResponseBaseModel.CreateError($"Имена папок должны быть уникальные. Есть дубликаты: {string.Join(";", folder_names)}");

        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(token);
        ManageManufactureModelDB manufacture_db = await context_forms.Manufactures.FirstAsync(x => x.Id == manufacture.Id, cancellationToken: token);
        if (manufacture_db.Equals(manufacture))
            return ResponseBaseModel.CreateInfo("Обновление не требуется. Объекты равны");

        manufacture_db.Reload(manufacture);
        context_forms.Update(manufacture_db);
        await context_forms.SaveChangesAsync(token);

        return ResponseBaseModel.CreateSuccess("Обновление успешно выполнено");
    }
}