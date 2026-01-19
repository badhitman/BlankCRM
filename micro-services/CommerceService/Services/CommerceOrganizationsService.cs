////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLib;
using DbcLib;

namespace CommerceService;

/// <summary>
/// Organizations
/// </summary>
public partial class CommerceImplementService : ICommerceService
{
    /// <inheritdoc/>
    public async Task<OrganizationContractorModel[]> ContractorsOrganizationsFindAsync(ContractorsOrganizationsRequestModel req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<OrganizationContractorModel> q = req.OfferFilter is null
            ? context.Contractors.Where(x => x.OfferId == null)
            : context.Contractors.Where(x => x.OfferId == null || x.OfferId == req.OfferFilter);

        if (req.OrganizationsFilter is not null && req.OrganizationsFilter.Length != 0)
            q = q.Where(x => req.OrganizationsFilter.Contains(x.OrganizationId));

        return await q.ToArrayAsync(cancellationToken: token);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OrganizationOfferContractSetAsync(TAuthRequestStandardModel<OrganizationOfferToggleModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        if (req.Payload is null)
            return ResponseBaseModel.CreateError($"{nameof(OrganizationOfferContractSetAsync)}: not set for payload");

        if (req.Payload.OfferId < 1)
            req.Payload.OfferId = null;

        if (req.Payload.SetValue)
        {
            if (await context.Contractors.AnyAsync(x => x.OrganizationId == req.Payload.OrganizationId && x.OfferId == req.Payload.OfferId, cancellationToken: token))
                return ResponseBaseModel.CreateInfo("Контракт уже установлен");

            await context.Contractors.AddAsync(new() { OfferId = req.Payload.OfferId, OrganizationId = req.Payload.OrganizationId }, token);

            try
            {
                await context.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                string msg = $"Ошибка создания контракта [{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}]";
                loggerRepo.LogError(ex, msg);
                return ResponseBaseModel.CreateError($"{msg}: {ex.Message}");
            }

            return ResponseBaseModel.CreateSuccess("Контракт создан");
        }
        else
        {
            if (!await context.Contractors.AnyAsync(x => x.OrganizationId == req.Payload.OrganizationId && x.OfferId == req.Payload.OfferId, cancellationToken: token))
                return ResponseBaseModel.CreateInfo("Контракта нет");

            return ResponseBaseModel.CreateSuccess($"Контрактов удалено: {await context.Contractors.Where(x => x.OrganizationId == req.Payload.OrganizationId && x.OfferId == req.Payload.OfferId).ExecuteDeleteAsync(cancellationToken: token)}");
        }
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<OfficeOrganizationModelDB[]>> OfficesOrganizationsReadAsync(int[] organizationsIds, CancellationToken token = default)
    {
        TResponseModel<OfficeOrganizationModelDB[]> res = new();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        res.Response = await context
            .Offices
            .Where(x => organizationsIds.Any(y => y == x.Id))
            .Include(x => x.Organization)
            .ToArrayAsync(cancellationToken: token);

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<OfficeOrganizationModelDB>> OfficeOrganizationDeleteAsync(TAuthRequestStandardModel<OfficeOrganizationDeleteRequestModel> req, CancellationToken token = default)
    {
        TResponseModel<OfficeOrganizationModelDB> res = new();
        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }

        int addressOfficeId = req.Payload.OfficeOrganizationDeleteId;
        if (addressOfficeId <= 0)
        {
            res.AddError("addressOfficeId <= 0");
            return res;
        }
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        int count = await context
            .OrdersB2B
            .CountAsync(x => context.OfficesForOrders.Any(y => y.OrderId == x.Id && y.OfficeId == addressOfficeId), cancellationToken: token);

        if (count != 0)
        {
            res.AddError($"Адрес используется в заказах: {count} шт.");
            return res;
        }

        IQueryable<OfficeOrganizationModelDB> q = context.Offices.Where(x => x.Id == addressOfficeId);
        res.Response = await q.Include(x => x.Organization).FirstAsync(cancellationToken: token);
        await q.ExecuteDeleteAsync(cancellationToken: token);
        res.AddSuccess("Команда успешно выполнена");
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OfficeOrganizationUpdateOrCreateAsync(TAuthRequestStandardModel<AddressOrganizationBaseModel> req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }

        AddressOrganizationBaseModel addressOfficeOrganization = req.Payload;
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        if (addressOfficeOrganization.Id < 1)
        {
            OfficeOrganizationModelDB add = new()
            {
                AddressUserComment = addressOfficeOrganization.AddressUserComment,
                Name = addressOfficeOrganization.Name,
                ParentId = addressOfficeOrganization.ParentId,
                Contacts = addressOfficeOrganization.Contacts,
                OrganizationId = addressOfficeOrganization.OrganizationId,
                KladrCode = addressOfficeOrganization.KladrCode,
                KladrTitle = addressOfficeOrganization.KladrTitle,
            };
            await context.AddAsync(add, token);
            await context.SaveChangesAsync(token);
            res.AddSuccess("Адрес добавлен");
            res.Response = add.Id;
            return res;
        }

        res.Response = await context.Offices
                        .Where(x => x.Id == addressOfficeOrganization.Id)
                        .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.AddressUserComment, addressOfficeOrganization.AddressUserComment)
                        .SetProperty(p => p.Name, addressOfficeOrganization.Name)
                        .SetProperty(p => p.KladrCode, addressOfficeOrganization.KladrCode)
                        .SetProperty(p => p.KladrTitle, addressOfficeOrganization.KladrTitle)
                        .SetProperty(p => p.ParentId, addressOfficeOrganization.ParentId)
                        .SetProperty(p => p.Contacts, addressOfficeOrganization.Contacts), cancellationToken: token);

        res.AddSuccess($"Обновление `{GetType().Name}` выполнено");
        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OrganizationSetLegalAsync(TAuthRequestStandardModel<OrganizationLegalModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        OrganizationLegalModel org = req.Payload;
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        int _ch = await context.Organizations
            .Where(x => x.Id == org.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow)
                .SetProperty(p => p.IsDisabled, org.IsDisabled)
                .SetProperty(p => p.Phone, org.Phone)
                .SetProperty(p => p.Email, org.Email)
                .SetProperty(p => p.Name, org.Name)
                .SetProperty(p => p.NewName, (string?)null)
                .SetProperty(p => p.KPP, org.KPP)
                .SetProperty(p => p.NewKPP, (string?)null)
                .SetProperty(p => p.OGRN, org.OGRN)
                .SetProperty(p => p.NewOGRN, (string?)null)
                .SetProperty(p => p.LegalAddress, org.LegalAddress)
                .SetProperty(p => p.NewLegalAddress, (string?)null)
                .SetProperty(p => p.INN, org.INN)
                .SetProperty(p => p.NewINN, (string?)null), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess($"Данные успешно сохранены ");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<OrganizationModelDB[]>> OrganizationsReadAsync(int[] req, CancellationToken token = default)
    {
        TResponseModel<OrganizationModelDB[]> res = new();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        res.Response = await context
            .Organizations
            .Where(x => req.Any(y => y == x.Id))
            .Include(x => x.Offices)
            .Include(x => x.Users)
            .Include(x => x.BanksDetails)
            .ToArrayAsync(cancellationToken: token);

        return res;
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<OrganizationModelDB>> OrganizationsSelectAsync(TPaginationRequestAuthModel<OrganizationsSelectRequestModel> req, CancellationToken token = default)
    {
        TResponseModel<UserInfoModel[]> res = await identityRepo.GetUsersOfIdentityAsync([req.SenderActionUserId], token);
        if (!res.Success() || res.Response?.Length != 1)
            return new();

        UserInfoModel actor = res.Response[0];
        if (req.PageSize < 10)
            req.PageSize = 10;

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<OrganizationModelDB> q = context.Organizations.AsQueryable();
        if (!actor.IsAdmin)
            q = q.Where(x => context.Units.Any(y => y.OrganizationId == x.Id && y.UserStatus > UsersOrganizationsStatusesEnum.None && y.UserPersonIdentityId == req.SenderActionUserId));

        if (req.Payload.AfterDateUpdate is not null)
            q = q.Where(x => x.LastUpdatedAtUTC >= req.Payload.AfterDateUpdate);

        if (!string.IsNullOrWhiteSpace(req.Payload.ForUserIdentityId))
            q = q.Where(x => context.Units.Any(y => y.OrganizationId == x.Id && y.UserPersonIdentityId == req.Payload.ForUserIdentityId));

        q = req.SortingDirection == DirectionsEnum.Up
            ? q.OrderBy(x => x.Name)
            : q.OrderByDescending(x => x.Name);

        IQueryable<OrganizationModelDB> pq = q
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        var extQ = pq
            .Include(x => x.Offices)
            .Include(x => x.Users)
            .Include(x => x.Contractors!)
            .ThenInclude(x => x.Offer);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = req.Payload.IncludeExternalData ? [.. await extQ.ToArrayAsync(cancellationToken: token)] : [.. await pq.ToArrayAsync(cancellationToken: token)]
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OrganizationUpdateOrCreateAsync(TAuthRequestStandardModel<OrganizationModelDB> req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }
        if (string.IsNullOrWhiteSpace(req.SenderActionUserId))
        {
            res.AddError("string.IsNullOrWhiteSpace(req.SenderActionUserId)");
            return res;
        }

        (bool IsValid, List<System.ComponentModel.DataAnnotations.ValidationResult> ValidationResults) = GlobalTools.ValidateObject(req.Payload);
        if (!IsValid)
        {
            res.Messages.InjectException(ValidationResults);
            return res;
        }
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        OrganizationModelDB? duple = default;
        UserInfoModel actor = default!;
        await Task.WhenAll([
            Task.Run(async () =>
            {
                TResponseModel<UserInfoModel[]> userFind = await identityRepo.GetUsersOfIdentityAsync([req.SenderActionUserId]);
                actor = userFind.Response!.First();
                }, token),
            Task.Run(async () => { duple = await context.Organizations.FirstOrDefaultAsync(x => x.INN == req.Payload.INN || x.OGRN == req.Payload.OGRN ); }, token)
            ]);

        if (req.Payload.Id < 1)
        {
            req.Payload.Id = 0;
            if (duple is not null)
            {
                UserOrganizationModelDB? sq = await context
                    .Units
                    .FirstOrDefaultAsync(x => x.UserPersonIdentityId == req.SenderActionUserId && x.OrganizationId == duple.Id, cancellationToken: token);

                if (!string.IsNullOrWhiteSpace(req.SenderActionUserId) && req.SenderActionUserId != GlobalStaticConstantsRoles.Roles.System && sq is not null)
                {
                    await context.AddAsync(new UserOrganizationModelDB()
                    {
                        LastUpdatedAtUTC = DateTime.UtcNow,
                        UserPersonIdentityId = req.SenderActionUserId,
                        OrganizationId = duple.Id,
                        UserStatus = UsersOrganizationsStatusesEnum.None,
                    }, token);

                    await Task.WhenAll([
                        context.SaveChangesAsync(token),
                        Task.Run(async () =>
                        {
                            await identityRepo.SendEmailAsync(new SendEmailRequestModel()
                            {
                                TextMessage = $"В компанию `{sq}` добавлен сотрудник: {actor}",
                                Email = "*",
                                Subject = "Новый сотрудник" }, false); }, token)
                        ]);

                    res.AddSuccess($"Вы добавлены как сотрудник компании, но требуется подтверждение администратором");
                }
                else
                    res.AddWarning($"Компания уже существует. Требуется подтверждение администратором");

                return res;
            }

            req.Payload.NewINN = req.Payload.INN;
            req.Payload.NewOGRN = req.Payload.OGRN;
            req.Payload.NewName = req.Payload.Name;
            req.Payload.NewLegalAddress = req.Payload.LegalAddress;
            req.Payload.NewKPP = req.Payload.KPP;
            req.Payload.LastUpdatedAtUTC = DateTime.UtcNow;

            await context.AddAsync(req.Payload, token);
            await context.SaveChangesAsync(token);
            await context.AddAsync(new UserOrganizationModelDB()
            {
                LastUpdatedAtUTC = DateTime.UtcNow,
                UserPersonIdentityId = req.SenderActionUserId,
                OrganizationId = req.Payload.Id,
                UserStatus = UsersOrganizationsStatusesEnum.None,
            }, token);
            await context.SaveChangesAsync(token);
            res.AddSuccess($"Компания создана");

            res.Response = req.Payload.Id;
        }
        else
        {
            DateTime lud = DateTime.UtcNow;
            OrganizationModelDB org_db = await context.Organizations.FirstAsync(x => x.Id == req.Payload.Id, cancellationToken: token);

            IQueryable<OrganizationModelDB> q = context
                .Organizations
                .Where(x => x.Id == org_db.Id);

            if (org_db.Name != req.Payload.Name)
                await q.ExecuteUpdateAsync(set => set.SetProperty(p => p.NewName, req.Payload.Name), cancellationToken: token);
            else if (!string.IsNullOrWhiteSpace(org_db.NewName))
                await q.ExecuteUpdateAsync(set => set.SetProperty(p => p.NewName, ""), cancellationToken: token);

            if (org_db.LegalAddress != req.Payload.LegalAddress)
                await q.ExecuteUpdateAsync(set => set.SetProperty(p => p.NewLegalAddress, req.Payload.LegalAddress), cancellationToken: token);
            else if (!string.IsNullOrWhiteSpace(org_db.NewLegalAddress))
                await q.ExecuteUpdateAsync(set => set.SetProperty(p => p.NewLegalAddress, ""), cancellationToken: token);

            if (org_db.INN != req.Payload.INN)
                await q.ExecuteUpdateAsync(set => set.SetProperty(p => p.NewINN, req.Payload.INN), cancellationToken: token);
            else if (!string.IsNullOrWhiteSpace(org_db.NewINN))
                await q.ExecuteUpdateAsync(set => set.SetProperty(p => p.NewINN, ""), cancellationToken: token);

            if (org_db.OGRN != req.Payload.OGRN)
                await q.ExecuteUpdateAsync(set => set.SetProperty(p => p.NewOGRN, req.Payload.OGRN), cancellationToken: token);
            else if (!string.IsNullOrWhiteSpace(org_db.NewOGRN))
                await q.ExecuteUpdateAsync(set => set.SetProperty(p => p.NewOGRN, ""), cancellationToken: token);

            if (org_db.KPP != req.Payload.KPP)
                await q.ExecuteUpdateAsync(set => set.SetProperty(p => p.NewKPP, req.Payload.KPP), cancellationToken: token);
            else if (!string.IsNullOrWhiteSpace(org_db.NewKPP))
                await q.ExecuteUpdateAsync(set => set.SetProperty(p => p.NewKPP, ""), cancellationToken: token);

            if (org_db.Email != req.Payload.Email)
                res.AddSuccess("Email изменён");
            if (org_db.Phone != req.Payload.Phone)
                res.AddSuccess("Phone изменён");
            if (org_db.IsDisabled != req.Payload.IsDisabled)
                res.AddSuccess(req.Payload.IsDisabled ? "Организация успешно отключена" : "Организация успешно включена");

            if (org_db.Email != req.Payload.Email || org_db.Phone != req.Payload.Phone || org_db.IsDisabled != req.Payload.IsDisabled)
                await q
                   .ExecuteUpdateAsync(set => set
                   .SetProperty(p => p.LastUpdatedAtUTC, lud)
                   .SetProperty(p => p.Phone, req.Payload.Phone)
                   .SetProperty(p => p.Email, req.Payload.Email)
                   .SetProperty(p => p.IsDisabled, req.Payload.IsDisabled)
                   .SetProperty(p => p.BankMainAccount, req.Payload.BankMainAccount), cancellationToken: token);
        }

        return res;
    }


    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OrganizationUserUpdateOrCreateAsync(TAuthRequestStandardModel<UserOrganizationModelDB> req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();

        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        if (req.Payload.Id < 1)
        {
            UserOrganizationModelDB? dl = await context.Units
                .FirstOrDefaultAsync(x => x.OrganizationId == req.Payload.OrganizationId && x.UserPersonIdentityId == req.Payload.UserPersonIdentityId, cancellationToken: token);

            if (dl is not null)
            {
                res.AddInfo($"Пользователь уже существует: {dl.UserStatus.DescriptionInfo()}");
                return res;
            }

            await context.AddAsync(req.Payload, token);
            await context.SaveChangesAsync(token);
            res.AddSuccess("Пользователь добавлен");
            res.Response = req.Payload.Id;
            return res;
        }

        res.Response = await context.Units
                        .Where(x => x.Id == req.Payload.Id)
                        .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.UserStatus, req.Payload.UserStatus)
                        .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        res.AddSuccess($"Обновление `{nameof(OrganizationUserUpdateOrCreateAsync)}` выполнено");
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<UserOrganizationModelDB[]>> UsersOrganizationsReadAsync(int[] req, CancellationToken token = default)
    {
        TResponseModel<UserOrganizationModelDB[]> res = new();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        res.Response = await context
            .Units
            .Where(x => req.Any(y => y == x.Id))
            .Include(x => x.Organization!)
            .ThenInclude(x => x.Users)
            .ToArrayAsync(cancellationToken: token);

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseStandardModel<UserOrganizationModelDB>>> UsersOrganizationsSelectAsync(TPaginationRequestAuthModel<UsersOrganizationsStatusesRequestModel> req, CancellationToken token = default)
    {
        if (req.PageSize < 10)
            req.PageSize = 10;

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<UserOrganizationModelDB> q = req.Payload.UsersOrganizationsStatusesFilter is not null && req.Payload.UsersOrganizationsStatusesFilter.Length > 0
            ? context.Units.Where(x => req.Payload.UsersOrganizationsStatusesFilter.Any(y => y == x.UserStatus))
            : context.Units.AsQueryable();

        if (req.Payload.AfterDateUpdate is not null)
            q = q.Where(x => x.LastUpdatedAtUTC >= req.Payload.AfterDateUpdate);

        if (req.Payload.OrganizationsFilter is not null && req.Payload.OrganizationsFilter.Length != 0)
        {
            q = q.Where(x => req.Payload.OrganizationsFilter.Any(y => y == x.OrganizationId));
            q = req.SortingDirection == DirectionsEnum.Up
            ? q.OrderBy(x => x.UserStatus)
            : q.OrderByDescending(x => x.UserStatus);
        }
        else
        {
            q = req.SortingDirection == DirectionsEnum.Up
            ? q.OrderBy(x => x.Organization!.Name)
            : q.OrderByDescending(x => x.Organization!.Name);
        }

        IQueryable<UserOrganizationModelDB> pq = q
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        var extQ = pq
            .Include(x => x.Organization!)
            .ThenInclude(x => x.Users);

        return new()
        {
            Response = new()
            {
                PageNum = req.PageNum,
                PageSize = req.PageSize,
                SortingDirection = req.SortingDirection,
                SortBy = req.SortBy,
                TotalRowsCount = await q.CountAsync(cancellationToken: token),
                Response = req.Payload.IncludeExternalData ? await extQ.ToListAsync(cancellationToken: token) : await pq.ToListAsync(cancellationToken: token)
            }
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> BankDetailsUpdateOrCreateAsync(TAuthRequestStandardModel<BankDetailsModelDB> req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();

        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }
        if (string.IsNullOrWhiteSpace(req.SenderActionUserId))
        {
            res.AddError("string.IsNullOrWhiteSpace(req.SenderActionUserId)");
            return res;
        }

        req.Payload.Organization = null;
        (bool IsValid, List<System.ComponentModel.DataAnnotations.ValidationResult> ValidationResults) = GlobalTools.ValidateObject(req.Payload);
        if (!IsValid)
        {
            res.Messages.InjectException(ValidationResults);
            return res;
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        BankDetailsModelDB? duple;
        UserInfoModel? actor = default;
        await Task.WhenAll([
            Task.Run(async () =>
            {
                TResponseModel<UserInfoModel[]> userFind = await identityRepo.GetUsersOfIdentityAsync([req.SenderActionUserId]);
                actor = userFind.Response?.Single();
                }, token),
            Task.Run(async () => { duple = await context.BanksDetails.Include(x => x.Organization).FirstOrDefaultAsync(x => x.Id != req.Payload.Id && x.BankBIC == req.Payload.BankBIC && x.CurrentAccount == req.Payload.CurrentAccount ); }, token)
            ]);

        if (actor is null)
        {
            res.AddError("Отказано в доступе");
            return res;
        }

        if (req.Payload.Id < 1)
        {
            req.Payload.Id = 0;
            await context.AddAsync(req.Payload, token);
            await context.SaveChangesAsync(token);

            res.Response = req.Payload.Id;
            res.AddSuccess("Банковский счёт добавлен/создан");

            if (!await context.BanksDetails.AnyAsync(x => x.OrganizationId == req.Payload.OrganizationId && x.Id != req.Payload.Id, cancellationToken: token))
                await context.Organizations
                    .Where(x => x.Id == req.Payload.OrganizationId)
                    .ExecuteUpdateAsync(set => set.SetProperty(p => p.BankMainAccount, req.Payload.Id), cancellationToken: token);
        }
        else
        {
            res.Response = await context.BanksDetails
                .Where(x => x.Id == req.Payload.Id)
                .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.CurrentAccount, req.Payload.CurrentAccount)
                .SetProperty(p => p.CorrespondentAccount, req.Payload.CorrespondentAccount)
                .SetProperty(p => p.Name, req.Payload.Name)
                .SetProperty(p => p.Description, req.Payload.Description)
                .SetProperty(p => p.BankBIC, req.Payload.BankBIC)
                .SetProperty(p => p.BankAddress, req.Payload.BankAddress)
                .SetProperty(p => p.IsDisabled, req.Payload.IsDisabled), cancellationToken: token);

            res.AddSuccess($"Банковский счёт {(res.Response > 0 ? "обновлён" : "[без изменений]")}");
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<BankDetailsModelDB>> BankDetailsForOrganizationDeleteAsync(TAuthRequestStandardModel<BankDetailsForOrganizationDeleteRequestModel> req, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(req.SenderActionUserId))
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "string.IsNullOrWhiteSpace(req.SenderActionUserId)" }] };

        if (req.Payload is null)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "req.Payload is null" }] };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        TResponseModel<BankDetailsModelDB> res = new();
        UserInfoModel? actor = default;

        await Task.WhenAll([
            Task.Run(async () =>
            {
                TResponseModel<UserInfoModel[]> userFind = await identityRepo.GetUsersOfIdentityAsync([req.SenderActionUserId]);
                actor = userFind.Response?.Single();
                }, token),
            Task.Run(async () =>
            {
                res.Response = await context.BanksDetails
                .Include(x => x.Organization)
                .FirstAsync(x => x.Id == req.Payload.BankDetailsForOrganizationId );

            }, token)
        ]);

        if (actor is null)
        {
            res.AddError("Отказано в доступе");
            return res;
        }

        await context.Organizations
            .Where(x => x.BankMainAccount == req.Payload.BankDetailsForOrganizationId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.BankMainAccount, 0), cancellationToken: token);

        res.AddInfo($"Удалено: {await context.BanksDetails.Where(x => x.Id == req.Payload.BankDetailsForOrganizationId).ExecuteDeleteAsync(cancellationToken: token)}");

        return res;
    }
}