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
    public async Task<TResponseModel<bool>> OrganizationOfferContractUpdateAsync(TAuthRequestModel<OrganizationOfferToggleModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        if (req.Payload is null)
        {
            return new()
            {
                Messages = [new() { Text = $"{nameof(OrganizationOfferToggleModel)}: not set for payload", TypeMessage = MessagesTypesEnum.Error }],
                Response = false,
            };
        }

        if (req.Payload.OfferId < 1)
            req.Payload.OfferId = null;

        if (req.Payload.SetValue)
        {
            if (await context.Contractors.AnyAsync(x => x.OrganizationId == req.Payload.OrganizationId && x.OfferId == req.Payload.OfferId, cancellationToken: token))
            {
                return new()
                {
                    Messages = [new() { Text = "Контракт уже установлен", TypeMessage = MessagesTypesEnum.Info }],
                    Response = false,
                };
            }
            await context.Contractors.AddAsync(new() { OfferId = req.Payload.OfferId, OrganizationId = req.Payload.OrganizationId }, token);

            try
            {
                await context.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                string msg = $"Ошибка создания контракта [{JsonConvert.SerializeObject(req)}]";
                loggerRepo.LogError(ex, msg);
                return new()
                {
                    Messages = [new() { Text = $"{msg}: {ex.Message}", TypeMessage = MessagesTypesEnum.Error }],
                    Response = false,
                };
            }

            return new()
            {
                Messages = [new() { Text = "Контракт создан", TypeMessage = MessagesTypesEnum.Success }],
                Response = true,
            };
        }
        else
        {
            if (!await context.Contractors.AnyAsync(x => x.OrganizationId == req.Payload.OrganizationId && x.OfferId == req.Payload.OfferId, cancellationToken: token))
            {
                return new()
                {
                    Messages = [new() { Text = "Контракта нет", TypeMessage = MessagesTypesEnum.Info }],
                    Response = false,
                };
            }
            return new()
            {
                Messages = [new() { Text = "Контракт удалён", TypeMessage = MessagesTypesEnum.Success }],
                Response = await context.Contractors.Where(x => x.OrganizationId == req.Payload.OrganizationId && x.OfferId == req.Payload.OfferId).ExecuteDeleteAsync(cancellationToken: token) > 0,
            };
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
    public async Task<ResponseBaseModel> OfficeOrganizationDeleteAsync(int address_id, CancellationToken token = default)
    {
        ResponseBaseModel res = new();

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        int count = await context
            .OrdersB2B
            .CountAsync(x => context.OfficesForOrders.Any(y => y.OrderId == x.Id && y.OfficeId == address_id), cancellationToken: token);

        if (count != 0)
            res.AddError($"Адрес используется в заказах: {count} шт.");

        if (!res.Success())
            return res;

        await context.Offices.Where(x => x.Id == address_id).ExecuteDeleteAsync(cancellationToken: token);
        res.AddSuccess("Команда успешно выполнена");

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OfficeOrganizationUpdateAsync(AddressOrganizationBaseModel req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        if (req.Id < 1)
        {
            OfficeOrganizationModelDB add = new()
            {
                AddressUserComment = req.AddressUserComment,
                Name = req.Name,
                ParentId = req.ParentId,
                Contacts = req.Contacts,
                OrganizationId = req.OrganizationId,
                KladrCode = req.KladrCode,
                KladrTitle = req.KladrTitle,
            };
            await context.AddAsync(add, token);
            await context.SaveChangesAsync(token);
            res.AddSuccess("Адрес добавлен");
            res.Response = add.Id;
            return res;
        }

        res.Response = await context.Offices
                        .Where(x => x.Id == req.Id)
                        .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.AddressUserComment, req.AddressUserComment)
                        .SetProperty(p => p.Name, req.Name)
                        .SetProperty(p => p.KladrCode, req.KladrCode)
                        .SetProperty(p => p.KladrTitle, req.KladrTitle)
                        .SetProperty(p => p.ParentId, req.ParentId)
                        .SetProperty(p => p.Contacts, req.Contacts), cancellationToken: token);

        res.AddSuccess($"Обновление `{GetType().Name}` выполнено");
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> OrganizationSetLegalAsync(OrganizationLegalModel org, CancellationToken token = default)
    {
        TResponseModel<bool> res = new() { Response = false };
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        OrganizationModelDB? org_db = await context.Organizations.FirstOrDefaultAsync(x => x.Id == org.Id, cancellationToken: token);
        if (org_db is null)
        {
            res.AddError("Не найдена организация");
            return res;
        }

        org_db.INN = org.INN;
        org_db.NewINN = null;

        org_db.LegalAddress = org.LegalAddress;
        org_db.NewLegalAddress = null;

        org_db.OGRN = org.OGRN;
        org_db.NewOGRN = null;

        org_db.KPP = org.KPP;
        org_db.NewKPP = null;

        org_db.Name = org.Name;
        org_db.NewName = null;

        org_db.Email = org.Email;
        org_db.Phone = org.Phone;
        org_db.IsDisabled = org.IsDisabled;
        org_db.LastUpdatedAtUTC = DateTime.UtcNow;

        context.Update(org_db);
        await context.SaveChangesAsync(token);
        res.Response = true;
        res.AddSuccess("Данные успешно сохранены");

        return res;
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
    public async Task<TPaginationResponseModel<OrganizationModelDB>> OrganizationsSelectAsync(TPaginationRequestAuthModel<OrganizationsSelectRequestModel> req, CancellationToken token = default)
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
    public async Task<TResponseModel<int>> OrganizationUpdateAsync(TAuthRequestModel<OrganizationModelDB> req, CancellationToken token = default)
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
    public async Task<TResponseModel<int>> UserOrganizationUpdateAsync(TAuthRequestModel<UserOrganizationModelDB> req, CancellationToken token = default)
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

        res.AddSuccess($"Обновление `{nameof(UserOrganizationUpdateAsync)}` выполнено");
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
    public async Task<TResponseModel<TPaginationResponseModel<UserOrganizationModelDB>>> UsersOrganizationsSelectAsync(TPaginationRequestAuthModel<UsersOrganizationsStatusesRequestModel> req, CancellationToken token = default)
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
    public async Task<TResponseModel<int>> BankDetailsUpdateAsync(TAuthRequestModel<BankDetailsModelDB> req, CancellationToken token = default)
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
    public async Task<ResponseBaseModel> BankDetailsDeleteAsync(TAuthRequestModel<int> req, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(req.SenderActionUserId))
            return ResponseBaseModel.CreateError("string.IsNullOrWhiteSpace(req.SenderActionUserId)");

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        BankDetailsModelDB? bankDb = default;
        UserInfoModel? actor = default;

        await Task.WhenAll([
            Task.Run(async () =>
            {
                TResponseModel<UserInfoModel[]> userFind = await identityRepo.GetUsersOfIdentityAsync([req.SenderActionUserId]);
                actor = userFind.Response?.Single();
                }, token),
            Task.Run(async () =>
            {
                bankDb = await context.BanksDetails
                .Include(x => x.Organization)
                .FirstAsync(x => x.Id == req.Payload );

            }, token)
        ]);

        if (actor is null || bankDb is null)
            return ResponseBaseModel.CreateError("Отказано в доступе");

        await context.Organizations
            .Where(x => x.BankMainAccount == req.Payload)
            .ExecuteUpdateAsync(set => set.SetProperty(p => p.BankMainAccount, 0), cancellationToken: token);

        return ResponseBaseModel.CreateInfo($"Удалено: {await context.BanksDetails.Where(x => x.Id == req.Payload).ExecuteDeleteAsync(cancellationToken: token)}");
    }
}