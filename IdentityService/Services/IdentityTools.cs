﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Security.Claims;
using IdentityLib;
using System.Text;
using SharedLib;

namespace IdentityService;

/// <summary>
/// IdentityTools
/// </summary>
public class IdentityTools(
    IEmailSender<ApplicationUser> emailSender,
    ILogger<IdentityTools> loggerRepo,
    IUserStore<ApplicationUser> userStore,
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager,
    IDbContextFactory<IdentityAppDbContext> identityDbFactory) : IIdentityTools
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteRole(string role_name)
    {
        ApplicationRole? role_db = await roleManager.FindByNameAsync(role_name);
        if (role_db is null)
            return ResponseBaseModel.CreateError($"Роль #{role_name} не найдена в БД");

        using IdentityAppDbContext identityContext = identityDbFactory.CreateDbContext();
        var users_linked =
           await (from link in identityContext.UserRoles.Where(x => x.RoleId == role_db.Id)
                  join user in identityContext.Users on link.UserId equals user.Id
                  select new { user.Id, user.Email }).ToArrayAsync();

        if (users_linked.Length != 0)
            return ResponseBaseModel.CreateError($"Роль '{role_db.Name}' нельзя удалить! Предварительно исключите из неё пользователей: {string.Join("; ", users_linked.Select(x => $"[{x.Email}]"))};");

        IdentityResult ir = await roleManager.DeleteAsync(role_db);

        if (ir.Succeeded)
            ResponseBaseModel.CreateSuccess($"Роль '{role_db.Name}' успешно удалена!");

        return new()
        {
            Messages = ir.Errors.Select(x => new ResultMessage() { TypeMessage = ResultTypesEnum.Error, Text = $"[{x.Code}: {x.Description}]" }).ToList()
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteRoleFromUser(RoleEmailModel req)
    {
        ApplicationRole? role_db = await roleManager.FindByNameAsync(req.RoleName);
        if (role_db is null)
            return ResponseBaseModel.CreateError($"Роль с именем '{req.RoleName}' не найдена в БД");

        ApplicationUser? user_db = await userManager.FindByEmailAsync(req.Email);
        if (user_db is null)
            return ResponseBaseModel.CreateError($"Пользователь `{req.Email}` не найден в БД");

        if (!await userManager.IsInRoleAsync(user_db, req.RoleName))
            return ResponseBaseModel.CreateWarning($"Роль '{req.RoleName}' у пользователя '{req.Email}' отсутствует.");

        IdentityResult ir = await userManager.RemoveFromRoleAsync(user_db, req.RoleName);

        if (ir.Succeeded)
            return ResponseBaseModel.CreateSuccess($"Пользователь '{req.Email}' исключён из роли '{req.RoleName}'");

        return new()
        {
            Messages = ir.Errors.Select(x => new ResultMessage() { TypeMessage = ResultTypesEnum.Error, Text = $"[{x.Code}: {x.Description}]" }).ToList()
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> AddRoleToUser(RoleEmailModel req)
    {
        ApplicationRole? role_db = await roleManager.FindByNameAsync(req.RoleName);
        if (role_db is null)
            return ResponseBaseModel.CreateError($"Роль с именем '{req.RoleName}' не найдена в БД");

        ApplicationUser? user_db = await userManager.FindByEmailAsync(req.Email);
        if (user_db is null)
            return ResponseBaseModel.CreateError($"Пользователь `{req.Email}` не найден в БД");

        if (await userManager.IsInRoleAsync(user_db, req.RoleName))
            return ResponseBaseModel.CreateWarning($"Роль '{req.RoleName}' у пользователя '{req.Email}' уже присутствует.");

        IdentityResult ir = await userManager.AddToRoleAsync(user_db, req.RoleName);

        if (ir.Succeeded)
            return ResponseBaseModel.CreateSuccess($"Пользователю '{req.Email}' добавлена роль '{req.RoleName}'");

        return new()
        {
            Messages = ir.Errors.Select(x => new ResultMessage() { TypeMessage = ResultTypesEnum.Error, Text = $"[{x.Code}: {x.Description}]" }).ToList()
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ResetPasswordAsync(IdentityPasswordTokenModel req)
    {
        string msg;
        ApplicationUser? user = await userManager.FindByIdAsync(req.UserId);
        if (user is null)
        {
            msg = $"Identity user ({nameof(req.UserId)}: `{req.UserId}`) не найден. error {{9D6C3816-7A39-424F-8EF1-B86732D46BD7}}";
            return (ApplicationUserResponseModel)ResponseBaseModel.CreateError(msg);
        }

        IdentityResult result = await userManager.ResetPasswordAsync(user, req.Token, req.Password);
        if (!result.Succeeded)
            return new()
            {
                Messages = result.Errors.Select(x => new ResultMessage()
                {
                    TypeMessage = ResultTypesEnum.Error,
                    Text = $"[{x.Code}: {x.Description}]"
                }).ToList()
            };

        return ResponseBaseModel.CreateSuccess("Пароль успешно сброшен");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel>> FindByEmailAsync(string email)
    {
        TResponseModel<UserInfoModel> res = new();

        ApplicationUser? user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            res.AddError($"Пользователь не найден: {email}");
            return res;
        }

        IList<Claim> claims = await userManager.GetClaimsAsync(user);

        res.Response = new()
        {
            UserId = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            AccessFailedCount = user.AccessFailedCount,
            EmailConfirmed = user.EmailConfirmed,
            LockoutEnabled = user.LockoutEnabled,
            LockoutEnd = user.LockoutEnd,
            PhoneNumber = user.PhoneNumber,
            TelegramId = user.ChatTelegramId,
            Roles = [.. (await userManager.GetRolesAsync(user))],
            Claims = claims.Select(x => new EntryAltModel() { Id = x.Type, Name = x.Value }).ToArray(),
        };

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> GenerateEmailConfirmation(SimpleUserIdentityModel req)
    {
        ApplicationUser? user = await userManager.FindByEmailAsync(req.Email);
        if (user is null)
            return ResponseBaseModel.CreateError($"Пользователь не найден: {req.Email}");

        string code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        string callbackUrl = $"{req.BaseAddress}?userId={user.Id}&code={code}";
        await emailSender.SendConfirmationLinkAsync(user, req.Email, HtmlEncoder.Default.Encode(callbackUrl));
        return ResponseBaseModel.CreateSuccess($"Письмо с ссылкой подтверждением отправлено на адрес {req.Email}. Пожалуйста, проверьте электронную почту.");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ConfirmEmailAsync(UserCodeModel req)
    {
        if (req.UserId is null || req.Code is null)
            return ResponseBaseModel.CreateError("UserId is null || Code is null. error {715DE145-87B0-48B0-9341-0A21962045BF}");

        ApplicationUser? user = await userManager.FindByIdAsync(req.UserId);
        if (user is null)
            return ResponseBaseModel.CreateError($"Ошибка загрузки пользователя с идентификатором {req.UserId}");
        else
        {
            string code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(req.Code));
            IdentityResult result = await userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded
                ? ResponseBaseModel.CreateSuccess("Благодарим вас за подтверждение вашего адреса электронной почты. Теперь вы можете авторизоваться!")
                : ResponseBaseModel.CreateError($"Ошибка подтверждения электронной почты: {string.Join(";", result.Errors.Select(x => $"[{x.Code}: {x.Description}]"))}");
        }
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> ClaimsUserFlush(string user_id)
    {
        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync();
        ApplicationUser app_user = await identityContext.Users.FirstAsync(x => x.Id == user_id);

        app_user.FirstName ??= "";
        app_user.NormalizedFirstNameUpper = app_user.FirstName.ToUpper();
        app_user.LastName ??= "";
        app_user.NormalizedLastNameUpper = app_user.LastName.ToUpper();

        TResponseModel<bool> res = new();

        string chat_tg_id = app_user.ChatTelegramId?.ToString() ?? "0";
        IdentityUserClaim<string>[] claims_db;
        int[] claims_ids;
        if (chat_tg_id != "0")
        {
            claims_db = await identityContext.UserClaims.Where(x => x.ClaimType == GlobalStaticConstants.TelegramIdClaimName && x.ClaimValue == chat_tg_id).ToArrayAsync();
            claims_ids = [.. claims_db.Where(x => x.UserId != app_user.Id).Select(x => x.Id)];
            if (claims_ids.Length != 0)
            {
                res.Response = await identityContext.UserClaims.Where(x => claims_ids.Contains(x.Id)).ExecuteDeleteAsync() != 0 || res.Response;
            }

            if (!claims_db.Any(x => x.UserId == app_user.Id))
            {
                await identityContext.AddAsync(new IdentityUserClaim<string>()
                {
                    ClaimType = GlobalStaticConstants.TelegramIdClaimName,
                    ClaimValue = app_user.ChatTelegramId.ToString(),
                    UserId = app_user.Id,
                });
                res.Response = await identityContext.SaveChangesAsync() != 0 || res.Response;
            }
        }
        else
        {
            res.Response = await identityContext
                .UserClaims
                .Where(x => x.ClaimType == GlobalStaticConstants.TelegramIdClaimName && x.UserId == app_user.Id)
                .ExecuteDeleteAsync() != 0 || res.Response;
        }

        claims_db = await identityContext.UserClaims.Where(x => x.ClaimType == ClaimTypes.GivenName && x.UserId == app_user.Id).ToArrayAsync();
        if (string.IsNullOrWhiteSpace(app_user.FirstName))
        {
            if (claims_db.Length != 0)
            {
                claims_ids = claims_db.Select(x => x.Id).ToArray();
                res.Response = await identityContext.UserClaims.Where(x => claims_ids.Contains(x.Id)).ExecuteDeleteAsync() != 0 || res.Response;
            }
        }
        else
        {
            IdentityUserClaim<string> fe;
            IOrderedEnumerable<IdentityUserClaim<string>> qo = claims_db.OrderBy(x => x.Id);

            if (claims_db.Length == 0)
            {
                await identityContext.AddAsync(new IdentityUserClaim<string>()
                {
                    ClaimType = ClaimTypes.GivenName,
                    ClaimValue = app_user.FirstName ?? "",
                    UserId = app_user.Id
                });
                res.Response = await identityContext.SaveChangesAsync() != 0 || res.Response;
            }
            else if (claims_db.Length > 1)
            {
                fe = qo.First();
                claims_ids = [.. qo.Skip(1).Select(x => x.Id)];
                res.Response = await identityContext.UserClaims.Where(x => claims_ids.Contains(x.Id)).ExecuteDeleteAsync() != 0 || res.Response;
            }
            else
            {
                fe = qo.First();
                if (fe.ClaimValue != app_user.FirstName)
                {
                    res.Response = await identityContext
                                        .UserClaims
                                        .Where(x => x.Id == fe.Id)
                                        .ExecuteUpdateAsync(set => set.SetProperty(p => p.ClaimValue, app_user.FirstName)) != 0 || res.Response;
                }
            }
        }

        claims_db = await identityContext.UserClaims.Where(x => x.ClaimType == ClaimTypes.Surname && x.UserId == app_user.Id).ToArrayAsync();
        IOrderedEnumerable<IdentityUserClaim<string>> oq = claims_db.OrderBy(x => x.Id);
        if (string.IsNullOrWhiteSpace(app_user.FirstName))
        {
            if (claims_db.Length != 0)
            {
                claims_ids = claims_db.Select(x => x.Id).ToArray();
                res.Response = await identityContext.UserClaims.Where(x => claims_ids.Contains(x.Id)).ExecuteDeleteAsync() != 0 || res.Response;
            }
        }
        else
        {
            if (claims_db.Length == 0)
            {
                await identityContext.AddAsync(new IdentityUserClaim<string>()
                {
                    ClaimType = ClaimTypes.Surname,
                    ClaimValue = app_user.LastName ?? "",
                    UserId = app_user.Id
                });
                res.Response = await identityContext.SaveChangesAsync() != 0 || res.Response;
            }
            else if (claims_db.Length > 1)
            {
                claims_ids = [.. oq.Skip(1).Select(x => x.Id)];
                res.Response = await identityContext.UserClaims.Where(x => claims_ids.Contains(x.Id)).ExecuteDeleteAsync() != 0 || res.Response;
            }
            else if (claims_db[0].ClaimValue != app_user.FirstName)
            {
                res.Response = await identityContext
                   .UserClaims
                   .Where(x => x.Id == oq.First().Id)
                   .ExecuteUpdateAsync(set => set.SetProperty(p => p.ClaimValue, app_user.LastName)) != 0 || res.Response;
            }
        }

        claims_db = await identityContext.UserClaims.Where(x => x.ClaimType == ClaimTypes.MobilePhone && x.UserId == app_user.Id).ToArrayAsync();
        IOrderedEnumerable<IdentityUserClaim<string>> ot = claims_db.OrderBy(x => x.Id);
        if (string.IsNullOrWhiteSpace(app_user.PhoneNumber))
        {
            if (claims_db.Length != 0)
            {
                claims_ids = claims_db.Select(x => x.Id).ToArray();
                res.Response = await identityContext.UserClaims.Where(x => claims_ids.Contains(x.Id)).ExecuteDeleteAsync() != 0 || res.Response;
            }
        }
        else
        {
            if (claims_db.Length == 0)
            {
                await identityContext.AddAsync(new IdentityUserClaim<string>()
                {
                    ClaimType = ClaimTypes.MobilePhone,
                    ClaimValue = app_user.PhoneNumber ?? "",
                    UserId = app_user.Id
                });
                res.Response = await identityContext.SaveChangesAsync() != 0 || res.Response;
            }
            else if (claims_db.Length > 1)
            {
                claims_ids = [.. ot.Skip(1).Select(x => x.Id)];
                res.Response = await identityContext.UserClaims.Where(x => claims_ids.Contains(x.Id)).ExecuteDeleteAsync() != 0 || res.Response;
            }
            else if (claims_db[0].ClaimValue != app_user.PhoneNumber)
            {
                res.Response = await identityContext
                    .UserClaims
                    .Where(x => x.Id == ot.First().Id)
                    .ExecuteUpdateAsync(set => set.SetProperty(p => p.ClaimValue, app_user.PhoneNumber)) != 0 || res.Response;
            }
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<RegistrationNewUserResponseModel> CreateNewUserEmailAsync(string email)
    {
        IUserEmailStore<ApplicationUser> emailStore = GetEmailStore();
        ApplicationUser user = IdentityStatic.CreateInstanceUser();
        await userStore.SetUserNameAsync(user, email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);

        IdentityResult result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
            return new() { Messages = result.Errors.Select(x => new ResultMessage() { Text = $"[{x.Code}: {x.Description}]", TypeMessage = ResultTypesEnum.Error }).ToList() };

        return new()
        {
            RequireConfirmedAccount = userManager.Options.SignIn.RequireConfirmedAccount,
            RequireConfirmedEmail = userManager.Options.SignIn.RequireConfirmedEmail,
            RequireConfirmedPhoneNumber = userManager.Options.SignIn.RequireConfirmedPhoneNumber,
            Response = user.Id,
        };
    }

    /// <inheritdoc/>
    public async Task<RegistrationNewUserResponseModel> CreateNewUserWithPasswordAsync(RegisterNewUserPasswordModel req)
    {
        ApplicationUser user = IdentityStatic.CreateInstanceUser();

        await userStore.SetUserNameAsync(user, req.Email, CancellationToken.None);
        IUserEmailStore<ApplicationUser> emailStore = GetEmailStore();
        await emailStore.SetEmailAsync(user, req.Email, CancellationToken.None);
        IdentityResult result = await userManager.CreateAsync(user, req.Password);

        if (!result.Succeeded)
            return new() { Messages = result.Errors.Select(x => new ResultMessage() { Text = $"[{x.Code}: {x.Description}]", TypeMessage = ResultTypesEnum.Error }).ToList() };

        string userId = await userManager.GetUserIdAsync(user);
        loggerRepo.LogInformation($"User #{userId} [{req.Email}] created a new account with password.");

        string code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        string callbackUrl = $"{req.BaseAddress}?userId={userId}&code={code}";

        await emailSender.SendConfirmationLinkAsync(user, req.Email, System.Text.Encodings.Web.HtmlEncoder.Default.Encode(callbackUrl));

        RegistrationNewUserResponseModel res = new()
        {
            RequireConfirmedAccount = userManager.Options.SignIn.RequireConfirmedAccount,
            RequireConfirmedEmail = userManager.Options.SignIn.RequireConfirmedEmail,
            RequireConfirmedPhoneNumber = userManager.Options.SignIn.RequireConfirmedPhoneNumber,
            Response = userId
        };
        res.AddSuccess("Регистрация выполнена.");

        if (userManager.Options.SignIn.RequireConfirmedAccount)
        {
            res.AddInfo("Требуется подтверждение учетной записи.");
            res.AddWarning("Проверьте свой E-mail .");
        }

        return res;
    }

    IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("Для пользовательского интерфейса по умолчанию требуется хранилище пользователей с поддержкой электронной почты.");
        }
        return (IUserEmailStore<ApplicationUser>)userStore;
    }
}