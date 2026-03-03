////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Query;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using System.Net.Mail;
using Newtonsoft.Json;
using SharedLib;
using DbcLib;

namespace ConstructorService;

/// <summary>
/// Constructor служба
/// </summary>
public partial class FormsConstructorService : IConstructorService
{
    /////////////// Контекст работы конструктора: работы в системе над какими-либо сущностями всегда принадлежат какому-либо проекту/контексту.
    // При переключении контекста (текущий/основной проект) становятся доступны только работы по этому проекту
    // В проект можно добавлять участников, что бы те могли работать вместе с владельцем => вносить изменения в конструкторе данного проекта/контекста
    // Если проект отключить (есть у него такой статус: IsDisabled), то работы с проектом блокируются для всех участников, кроме владельца

    /// <inheritdoc/>
    public async Task<TResponseModel<ProjectViewModel[]>> GetProjectsForUserAsync(GetProjectsForUserRequestModel req, CancellationToken token = default)
    {
        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(token);
        IQueryable<ProjectModelDb> q = context_forms
            .Projects
            .Where(x => x.ContextName == req.ContextName && (x.OwnerUserId == req.UserId || context_forms.MembersOfProjects.Any(y => y.ProjectId == x.Id && y.UserId == req.UserId)))
            .Include(x => x.Members)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.NameFilter))
            q = q.Where(x => EF.Functions.Like(x.Name.ToUpper(), $"%{req.NameFilter.ToUpper()}%"));

        ProjectModelDb[] raw_data = await q.ToArrayAsync(cancellationToken: token);

        string[] usersIds = [.. raw_data
            .Where(x => x.Members is not null)
            .SelectMany(x => x.Members!)
            .Select(x => x.UserId)
            .Union(raw_data.Select(x => x.OwnerUserId))
            .Distinct()];

        EntryAltStandardModel[]? usersIdentity = null;

        if (usersIds.Length != 0)
        {
            TResponseModel<UserInfoModel[]> restUsers = await identityRepo.GetUsersOfIdentityAsync(usersIds, token);
            if (!restUsers.Success())
                throw new Exception(restUsers.Message());

            usersIdentity = [.. restUsers.Response!.Select(x => new EntryAltStandardModel { Id = x.UserId, Name = x.UserName })];
        }

        List<EntryAltStandardModel>? ReadMembersData(List<MemberOfProjectConstructorModelDB>? members)
        {
            if (members is null || usersIdentity is null)
                return null;

            return [.. usersIdentity.Where(identityUser => members.Any(memberOfProject => memberOfProject.UserId == identityUser.Id))];
        }

        ProjectViewModel cast_expression(ProjectModelDb project) => new()
        {
            OwnerUserId = project.OwnerUserId,
            Name = project.Name,
            Description = project.Description,
            Id = project.Id,
            IsDisabled = project.IsDisabled,
            SchemeLastUpdated = project.SchemeLastUpdated,
            Members = ReadMembersData(project.Members),
        };

        return new() { Response = [.. raw_data.Select(cast_expression)] };
    }

    /// <inheritdoc/>
    public async Task<List<ProjectModelDb>> ProjectsReadAsync(int[] projects_ids, CancellationToken token = default)
    {
        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(token);
        return await context_forms
            .Projects
            .Where(x => projects_ids.Any(y => y == x.Id))

            .Include(x => x.Forms!)
            .ThenInclude(x => x.Fields)

            .Include(x => x.Forms!)
            .ThenInclude(x => x.FieldsDirectoriesLinks)

            .Include(x => x.Directories!)
            .ThenInclude(x => x.Elements)

            .Include(x => x.Documents!)
            .ThenInclude(x => x.Tabs!)
            .ThenInclude(x => x.JoinsForms)

            .ToListAsync(cancellationToken: token);
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateProjectAsync(CreateProjectRequestModel req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        req.Project.Name = MyRegexSpices().Replace(req.Project.Name.Trim(), " ");

        (bool IsValid, List<ValidationResult> ValidationResults) = GlobalTools.ValidateObject(req.Project);
        if (!IsValid)
        {
            res.Messages.InjectException(ValidationResults);
            return res;
        }
        TResponseModel<UserInfoModel[]> restUsers = await identityRepo.GetUsersOfIdentityAsync([req.UserId], token);
        if (!restUsers.Success())
            throw new Exception(restUsers.Message());

        UserInfoModel? userDb = restUsers.Response?.Single();

        if (userDb is null)
        {
            res.AddError($"Пользователь #{req.UserId} не найден в БД");
            return res;
        }
        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(token);
        ProjectModelDb? projectDb = await context_forms
            .Projects
            .FirstOrDefaultAsync(x => x.ContextName == req.ContextName && x.OwnerUserId == userDb.UserId && x.Name == req.Project.Name, cancellationToken: token);

        if (projectDb is not null)
        {
            res.AddError($"Проект должен иметь уникальное имя и код. Похожий проект есть в БД: #{projectDb.Id} '{projectDb.Name}'");
            return res;
        }

        projectDb = new()
        {
            Name = req.Project.Name,
            NormalizedUpperName = req.Project.Name.ToUpper(),
            OwnerUserId = userDb.UserId,
            Description = req.Project.Description,
            IsDisabled = req.Project.IsDisabled,
            ContextName = req.ContextName,
        };

        await context_forms.AddAsync(projectDb, token);
        await context_forms.SaveChangesAsync(token);

        res.Response = projectDb.Id;
        res.AddSuccess("Проект создан");

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetMarkerDeleteProjectAsync(SetMarkerProjectRequestModel req, CancellationToken token = default)
    {
        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(token);

        ProjectModelDb? project = await context_forms
            .Projects
            .FirstOrDefaultAsync(x => x.Id == req.ProjectId, cancellationToken: token);

        if (project is null)
            return ResponseBaseModel.CreateError($"Проект #{req.ProjectId} не найден в БД");

        project.IsDisabled = req.Marker;
        context_forms.Update(project);
        await context_forms.SaveChangesAsync(token);

        return ResponseBaseModel.CreateSuccess($"Проект '{project.Name}' {(req.Marker ? "выключен" : "включён")}");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateProjectAsync(ProjectViewModel project, CancellationToken token = default)
    {
        (bool IsValid, List<ValidationResult> ValidationResults) = GlobalTools.ValidateObject(project);
        if (!IsValid)
            return ResponseBaseModel.CreateError(ValidationResults);

        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(token);
        project.Name = MyRegexSpices().Replace(project.Name.Trim(), " ");
        string upName = project.Name.ToUpper();
        ProjectModelDb? projectDb = await context_forms
            .Projects
            .FirstOrDefaultAsync(x => x.Id != project.Id && x.NormalizedUpperName == upName, cancellationToken: token);

        if (projectDb is not null)
            return ResponseBaseModel.CreateError($"Проект должен иметь уникальное имя. Похожий проект есть в БД: #{projectDb.Id} '{projectDb.Name}'");

        projectDb = await context_forms
            .Projects
            .FirstOrDefaultAsync(x => x.Id == project.Id, cancellationToken: token);

        if (projectDb is null)
            return ResponseBaseModel.CreateError($"Проект #{project.Id} не найден в БД");

        if (project.Name == projectDb.Name && project.Description == projectDb.Description && project.IsDisabled == projectDb.IsDisabled)
            return ResponseBaseModel.CreateInfo("Объект не изменён");

        await context_forms
            .Projects.Where(x => x.Id == project.Id)
            .ExecuteUpdateAsync(set => set
            .SetProperty(p => p.IsDisabled, project.IsDisabled)
            .SetProperty(p => p.Description, project.Description)
            .SetProperty(p => p.NormalizedUpperName, upName)
            .SetProperty(p => p.Name, project.Name), cancellationToken: token);

        await context_forms.SaveChangesAsync(token);

        return ResponseBaseModel.CreateSuccess("Проект обновлён");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryAltStandardModel[]>> GetMembersOfProjectAsync(int project_id, CancellationToken token = default)
    {
        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(token);

        string[] members_users_ids = await context_forms
            .MembersOfProjects
            .Where(x => x.ProjectId == project_id)
            .Select(x => x.UserId)
            .ToArrayAsync(cancellationToken: token);

        if (members_users_ids.Length == 0)
            return new();

        TResponseModel<UserInfoModel[]> restUsers = await identityRepo.GetUsersOfIdentityAsync(members_users_ids, token);
        if (!restUsers.Success())
            throw new Exception(restUsers.Message());

        return new()
        {
            Response = [.. restUsers.Response!.Select(x => new EntryAltStandardModel() { Id = x.UserId, Name = x.UserName })]
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> AddMembersToProjectAsync(UsersProjectModel req, CancellationToken token = default)
    {
        TResponseModel<UserInfoModel[]> restUsers = await identityRepo.GetUsersOfIdentityAsync(req.UsersIds, token);
        if (!restUsers.Success())
            throw new Exception(restUsers.Message());

        if (restUsers.Response is null || restUsers.Response.Length != req.UsersIds.Length)
            return ResponseBaseModel.CreateError($"Пользователи #[{string.Join(";", req.UsersIds)}] не найдены в БД");

        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(token);
        MemberOfProjectConstructorModelDB[] membersDb = await context_forms
            .MembersOfProjects
            .Where(x => x.ProjectId == req.ProjectId && req.UsersIds.Any(y => y == x.UserId))
            .ToArrayAsync(cancellationToken: token)
            ;

        UserInfoModel[] usersForAdd = [.. req.UsersIds
            .Where(x => !req.UsersIds.Any(y => y == x))
            .Select(x => restUsers.Response.First(y => y.UserId.Equals(x)))];

        if (usersForAdd.Length == 0)
            return ResponseBaseModel.CreateInfo("Пользователи уже является участниками проекта");

        ProjectModelDb? projectDb = await context_forms
            .Projects
            .FirstOrDefaultAsync(x => x.Id == req.ProjectId, cancellationToken: token);

        if (projectDb is null)
            return ResponseBaseModel.CreateError($"Проект #{req.ProjectId} не найден в БД");

        restUsers = await identityRepo.GetUsersOfIdentityAsync([projectDb.OwnerUserId], token);
        if (!restUsers.Success())
            throw new Exception(restUsers.Message());

        UserInfoModel? ownerProject = restUsers.Response?.Single();

        if (ownerProject is null)
            return ResponseBaseModel.CreateError($"Владелец проекта #{projectDb.OwnerUserId} не найден в БД");

        usersForAdd = [.. usersForAdd.Where(x => x.Email?.Equals(ownerProject.Email, StringComparison.OrdinalIgnoreCase) == true)];

        if (usersForAdd.Length == 0)
            return ResponseBaseModel.CreateInfo($"Пользователь является владельцем проекта, поэтому не может быть добавлен как участник.");

        MemberOfProjectConstructorModelDB[] usersDb = [.. usersForAdd.Select(x => new MemberOfProjectConstructorModelDB() { UserId = x.UserId, ProjectId = req.ProjectId })];

        await context_forms.MembersOfProjects.AddRangeAsync(usersDb);
        await context_forms.SaveChangesAsync(token);

        return ResponseBaseModel.CreateSuccess($"Пользователи/участники х{string.Join("; ", usersForAdd.Select(x => x.Email))}ъ добавлены к проекту '{projectDb.Name}'");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteMembersFromProjectAsync(UsersProjectModel req, CancellationToken token = default)
    {
        TResponseModel<UserInfoModel[]> restUsers = await identityRepo.GetUsersOfIdentityAsync(req.UsersIds, token);
        if (!restUsers.Success())
            throw new Exception(restUsers.Message());

        if (restUsers.Response is null || restUsers.Response.Length != req.UsersIds.Length)
            return ResponseBaseModel.CreateError($"Пользователи #{string.Join(";", req.UsersIds)} не найдены в БД");

        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(token);
        MemberOfProjectConstructorModelDB[] membersDb = await context_forms
            .MembersOfProjects
            .Include(x => x.Project)
            .Where(x => x.ProjectId == req.ProjectId && req.UsersIds.Any(y => y == x.UserId))
            .ToArrayAsync(cancellationToken: token);

        var usersForDelete = membersDb
            .Where(x => !req.UsersIds.Any(y => y == x.UserId))
            .Select(x => new { x.Id, x.UserId })
            .ToArray();

        if (usersForDelete.Length == 0)
            return ResponseBaseModel.CreateInfo("Пользователи не являются участниками проекта. Удаление не требуется");

        await context_forms.MembersOfProjects
             .Where(x => usersForDelete.Any(y => y.Id == x.Id))
             .ExecuteDeleteAsync(cancellationToken: token);

        await context_forms.ProjectsUse
            .Where(x => usersForDelete.Any(y => y.UserId == x.UserId) && x.ProjectId == req.ProjectId)
            .ExecuteDeleteAsync(cancellationToken: token);

        await context_forms.SaveChangesAsync(token);
        return ResponseBaseModel.CreateSuccess($"Пользователь успешно исключён из проекта");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetProjectAsMainAsync(UserProjectModel req, CancellationToken token = default)
    {
        TResponseModel<UserInfoModel[]> restUsers = await identityRepo.GetUsersOfIdentityAsync([req.UserId], token);
        if (!restUsers.Success())
            throw new Exception(restUsers.Message());

        UserInfoModel? userDb = restUsers.Response?.Single();

        if (userDb is null)
            return ResponseBaseModel.CreateError($"Пользователь #{req.UserId} не найден в БД");

        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(token);
        ProjectModelDb? projectDb = await context_forms.Projects.FirstOrDefaultAsync(x => x.Id == req.ProjectId, cancellationToken: token);
        if (projectDb is null)
            return ResponseBaseModel.CreateError($"Проект #{req.ProjectId} не найден в БД");

        ProjectUseConstructorModelDb? mainProjectDb = await context_forms.ProjectsUse.FirstOrDefaultAsync(x => x.UserId == req.UserId, cancellationToken: token);
        if (mainProjectDb is null)
        {
            mainProjectDb = new ProjectUseConstructorModelDb() { UserId = req.UserId, ProjectId = req.ProjectId, Project = projectDb };
            await context_forms.AddAsync(mainProjectDb, token);
        }
        else
        {
            mainProjectDb.Project = projectDb;
            mainProjectDb.ProjectId = req.ProjectId;
            context_forms.Update(mainProjectDb);
        }
        await context_forms.SaveChangesAsync(token);
        return ResponseBaseModel.CreateSuccess($"Проект '{projectDb.Name}' успешно установлен в роли основного/используемого");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<MainProjectViewModel>> GetCurrentMainProjectAsync(GetCurrentMainProjectRequestModel req, CancellationToken token = default)
    {
        TResponseModel<UserInfoModel[]> restUsers = await identityRepo.GetUsersOfIdentityAsync([req.UserIdentityId], token);
        if (!restUsers.Success())
            throw new Exception(restUsers.Message());

        UserInfoModel? userDb = restUsers.Response?.Single();

        TResponseModel<MainProjectViewModel> res = new();
        if (userDb is null)
        {
            res.AddError($"Пользователь #{req.UserIdentityId} не найден в БД");
            return res;
        }

        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(token);
        ProjectModelDb? project = null;
        ProjectUseConstructorModelDb? project_use = null;
        if (!await context_forms.Projects.AnyAsync(x => x.ContextName == req.ContextName && x.OwnerUserId == req.UserIdentityId, cancellationToken: token) && !await context_forms.MembersOfProjects.AnyAsync(x => x.UserId == req.UserIdentityId && x.Project!.ContextName == req.ContextName, cancellationToken: token))
        {
            project = new()
            {
                Name = "По умолчанию",
                OwnerUserId = req.UserIdentityId,
                NormalizedUpperName = "ПО УМОЛЧАНИЮ",
                ContextName = req.ContextName
            };
            await context_forms.AddAsync(project, token);
            await context_forms.SaveChangesAsync(token);

            project_use = new() { UserId = project.OwnerUserId, ProjectId = project.Id };
            await context_forms.AddAsync(project_use, token);
            await context_forms.SaveChangesAsync(token);
        }

        res.Response = project is not null
            ? MainProjectViewModel.Build(project)
            : await context_forms.ProjectsUse.Where(x => x.Project!.ContextName == req.ContextName && x.UserId == req.UserIdentityId)
            .Include(x => x.Project)
            .Select(x => new MainProjectViewModel()
            {
                Id = x.Project!.Id,
                Name = x.Project.Name,
                Description = x.Project.Description,
                IsDisabled = x.Project.IsDisabled,
                OwnerUserId = x.Project.OwnerUserId
            })
            .FirstOrDefaultAsync(cancellationToken: token);

        if (res.Response is null)
        {
            IQueryable<ProjectModelDb> members_query = context_forms
                .MembersOfProjects
                .Where(x => x.Project!.ContextName == req.ContextName)
                .Include(x => x.Project)
                .Select(x => x.Project!);

            project = await context_forms
                .Projects
                .Where(x => x.OwnerUserId == req.UserIdentityId && x.ContextName == req.ContextName)
                .Union(members_query)
                .FirstAsync(cancellationToken: token);

            project_use = new() { UserId = req.UserIdentityId, ProjectId = project.Id };
            await context_forms.ProjectsUse.AddAsync(project_use, token);
            await context_forms.SaveChangesAsync(token);

            res.Response = MainProjectViewModel.Build(project);
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CanEditProjectAsync(UserProjectModel req, CancellationToken token = default)
    {
        if (req.ProjectId < 1)
            return ResponseBaseModel.CreateError("Не указан проект");

        TResponseModel<UserInfoModel[]> call_user = await identityRepo.GetUsersOfIdentityAsync([req.UserId], token);
        UserInfoModel? author_user = call_user.Response?.Single();

        if (!call_user.Success())
            return ResponseBaseModel.Create(call_user.Messages);

        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(token);
        ProjectModelDb? project = await context_forms.Projects.FirstOrDefaultAsync(x => x.Id == req.ProjectId, cancellationToken: token);
        if (project is null)
            return ResponseBaseModel.CreateError($"Проект #{req.ProjectId} не найден в БД");

        return project.CanEdit(call_user.Response!.Single())
            ? ResponseBaseModel.CreateSuccess("Проект доступен для редактирования")
            : ResponseBaseModel.CreateError($"Проект недоступен для редактирования #{project.Id} '{project.Name}'"); ;
    }
}