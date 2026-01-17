////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsRoutes;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Security.Claims;
using System.Net.Mail;
using Newtonsoft.Json;
using IdentityLib;
using System.Text;
using SharedLib;
using System.Text.RegularExpressions;

namespace IdentityService;

/// <summary>
/// IdentityTools
/// </summary>
public class IdentityTools(
    IServiceScopeFactory serviceScopeFactory,
    IEmailSender<ApplicationUser> emailSender,
    //IUserStore<ApplicationUser> userStore,
    //RoleManager<ApplicationRole> roleManager,
    //UserManager<ApplicationUser> userManager,
    IManualCustomCacheService memCache,
    IParametersStorageTransmission StorageTransmissionRepo,
    IMailProviderService mailRepo,
    ILogger<IdentityTools> loggerRepo,
    ITelegramTransmission tgRemoteRepo,
    IDbContextFactory<IdentityAppDbContext> identityDbFactory) : IIdentityTools
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> InitChangePhoneUserAsync(TAuthRequestStandardModel<string> req, CancellationToken token = default)
    {
        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);

        if (string.IsNullOrWhiteSpace(req.SenderActionUserId) || !await identityContext.Users.AnyAsync(x => x.Id == req.SenderActionUserId, cancellationToken: token))
            return ResponseBaseModel.CreateError("Авторство запроса не определено или пользователь не найден в БД");

        if (string.IsNullOrWhiteSpace(req.Payload))
        {
            await identityContext.Users
                .Where(x => x.Id == req.SenderActionUserId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.PhoneNumber, "")
                    .SetProperty(p => p.RequestChangePhone, "")
                    .SetProperty(p => p.PhoneNumberConfirmed, false), cancellationToken: token);

            return ResponseBaseModel.CreateSuccess("Номер телефона успешно удалён");
        }

        if (!GlobalTools.IsPhoneNumber(req.Payload))
            return ResponseBaseModel.CreateError("Телефон должен быть в формате: +79994440011 (можно без +)");

        bool _plus = req.Payload.Trim().StartsWith('+');
        req.Payload = Regex.Replace(req.Payload, @"[^\d]", "");
        if (_plus)
            req.Payload = $"+{req.Payload}";

        if (await identityContext.Users.AnyAsync(x => x.Id != req.SenderActionUserId && (x.PhoneNumber == req.Payload || x.PhoneNumber == $"+{req.Payload}"), cancellationToken: token))
            return ResponseBaseModel.CreateError("Пользователь с таким телефоном уже существует");

        await identityContext.Users
            .Where(x => x.Id == req.SenderActionUserId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.RequestChangePhone, req.Payload), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Запрос на изменение номера сформирован");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ConfirmChangePhoneUserAsync(TAuthRequestStandardModel<ChangePhoneUserRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("Ошибка запроса: Payload is null");

        if (string.IsNullOrWhiteSpace(req.SenderActionUserId))
            return ResponseBaseModel.CreateError("Авторство запроса не определено");

        TResponseModel<UserInfoModel[]> getAdminUser = await GetUsersOfIdentityAsync([req.SenderActionUserId], token);
        if (!getAdminUser.Success())
            return new() { Messages = getAdminUser.Messages };

        if (getAdminUser.Response is null || getAdminUser.Response.Length != 1)
            return ResponseBaseModel.CreateError("Пользователь (автор запроса) не найден в БД");

        if (!getAdminUser.Response[0].IsAdmin && getAdminUser.Response[0].Roles?.Contains(GlobalStaticConstantsRoles.Roles.RetailManage) != true && getAdminUser.Response[0].Roles?.Contains(GlobalStaticConstantsRoles.Roles.CommerceManager) != true)
            return ResponseBaseModel.CreateError("Недостаточно прав для операции");

        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);

        if (string.IsNullOrWhiteSpace(req.Payload.PhoneNum))
        {
            await identityContext.Users
                .Where(x => x.Id == req.Payload.UserId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.PhoneNumber, "")
                    .SetProperty(p => p.RequestChangePhone, "")
                    .SetProperty(p => p.PhoneNumberConfirmed, false), cancellationToken: token);

            return ResponseBaseModel.CreateSuccess("Номер телефона удалён");
        }

        if (!GlobalTools.IsPhoneNumber(req.Payload.PhoneNum))
            return ResponseBaseModel.CreateError("Телефон должен быть в формате: +79994440011 (можно без +)");

        bool _plus = req.Payload.PhoneNum.Trim().StartsWith('+');
        req.Payload.PhoneNum = Regex.Replace(req.Payload.PhoneNum, @"[^\d]", "");
        if (_plus)
            req.Payload.PhoneNum = $"+{req.Payload.PhoneNum}";

        if (await identityContext.Users.AnyAsync(x => x.Id != req.Payload.UserId && (x.PhoneNumber == req.Payload.PhoneNum || x.PhoneNumber == $"+{req.Payload.PhoneNum}"), cancellationToken: token))
            return ResponseBaseModel.CreateError("Пользователь с таким телефоном уже существует");

        await identityContext.Users
            .Where(x => x.Id == req.Payload.UserId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.PhoneNumber, req.Payload.PhoneNum)
                .SetProperty(p => p.RequestChangePhone, req.Payload.PhoneNum)
                .SetProperty(p => p.PhoneNumberConfirmed, true), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Номер успешно установлен");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<string>> CheckToken2FAAsync(CheckToken2FARequestModel req, CancellationToken tokenCan = default)
    {
        MemCachePrefixModel pref = new(Routes.TWOFACTOR_CONTROLLER_NAME, Routes.ALIAS_CONTROLLER_NAME);
        string? userId = await memCache.GetStringValueAsync(pref, req.UserAlias, tokenCan);
        if (string.IsNullOrWhiteSpace(userId))
            return new() { Messages = [new() { Text = "Алиас пользователя отсутствует!", TypeMessage = MessagesTypesEnum.Error }] };

        await memCache.RemoveAsync(pref, req.UserAlias, tokenCan);

        string? token = await memCache.GetStringValueAsync(new MemCachePrefixModel(Routes.TWOFACTOR_CONTROLLER_NAME, Routes.TOKEN_CONTROLLER_NAME), userId, tokenCan);
        if (string.IsNullOrWhiteSpace(token))
            return new() { Messages = [new() { Text = "Токен 2FA отсутствует!", TypeMessage = MessagesTypesEnum.Error }] };

        if (!req.Token.Equals(token))
            return new() { Messages = [new() { Text = "Токен не верный!", TypeMessage = MessagesTypesEnum.Error }] };

        await memCache.RemoveAsync(new MemCachePrefixModel(Routes.TWOFACTOR_CONTROLLER_NAME, Routes.TOKEN_CONTROLLER_NAME), userId, tokenCan);
        return new() { Response = userId, Messages = [new() { Text = "Токен верный", TypeMessage = MessagesTypesEnum.Success }] };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<string>> ReadToken2FAAsync(string userId, CancellationToken token = default)
    {
        return new() { Response = await memCache.GetStringValueAsync(new MemCachePrefixModel(Routes.TWOFACTOR_CONTROLLER_NAME, Routes.TOKEN_CONTROLLER_NAME), userId, token) };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<string>> GenerateToken2FAAsync(string userId, CancellationToken tokenCan = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser? user = await userManager.FindByIdAsync(userId); ;
        if (user is null)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = $"Пользователь #{userId} не найден" }] };

        IList<string> providers = await userManager.GetValidTwoFactorProvidersAsync(user);
        if (!providers.Contains("Email"))
            return (TResponseModel<string>)ResponseBaseModel.CreateError("Invalid 2-Step Verification Provider.");

        string token = await userManager.GenerateTwoFactorTokenAsync(user, "Email");

        if (!string.IsNullOrWhiteSpace(user.Email))
            await mailRepo.SendEmailAsync(user.Email, "Authentication 2FA token", token, token: tokenCan);

        await memCache.SetStringAsync(new MemCachePrefixModel(Routes.TWOFACTOR_CONTROLLER_NAME, Routes.TOKEN_CONTROLLER_NAME), userId, token, TimeSpan.FromMinutes(5), tokenCan);

        string aliasToken = Guid.NewGuid().ToString().Replace("-", "").Replace("{", "").Replace("}", "");
        await memCache.SetStringAsync(new MemCachePrefixModel(Routes.TWOFACTOR_CONTROLLER_NAME, Routes.ALIAS_CONTROLLER_NAME), aliasToken, userId, TimeSpan.FromMinutes(5), tokenCan);

        return new() { Response = aliasToken };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<IEnumerable<UserLoginInfoModel>>> GetUserLoginsAsync(string userId, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser? user = await userManager.FindByIdAsync(userId); ;
        if (user is null)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = $"Пользователь #{userId} не найден" }] };

        IList<UserLoginInfo> data_logins = await userManager.GetLoginsAsync(user);
        return new()
        {
            Response = data_logins.Select(x => new UserLoginInfoModel(x.LoginProvider, x.ProviderKey, x.ProviderDisplayName))
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CheckUserPasswordAsync(IdentityPasswordModel req, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser? user = await userManager.FindByIdAsync(req.UserId); ;
        if (user is null)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = $"Пользователь #{req.UserId} не найден" }] };

        string msg;
        if (!await userManager.CheckPasswordAsync(user, req.Password))
        {
            msg = "Ошибка: Неправильный пароль. error {91A2600D-5EBF-4F79-83BE-28F6FA55301C}";
            loggerRepo.LogError(msg);
            return ResponseBaseModel.CreateError(msg);
        }

        return ResponseBaseModel.CreateSuccess("Пароль проверку прошёл!");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteUserDataAsync(DeleteUserDataRequestModel req, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser? user = await userManager.FindByIdAsync(req.UserId); ;
        if (user is null)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = $"Пользователь #{req.UserId} не найден" }] };

        bool user_has_pass = await userManager.HasPasswordAsync(user);

        if (!user_has_pass || !await userManager.CheckPasswordAsync(user, req.Password))
            return ResponseBaseModel.CreateError("Ошибка изменения пароля. error {F268D35F-9697-4667-A4BA-6E57220A90EC}");

        IdentityResult result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
            return ResponseBaseModel.Create(result.Errors.Select(x => new ResultMessage() { TypeMessage = MessagesTypesEnum.Error, Text = $"[{x.Code}:{x.Description}]" }));

        return ResponseBaseModel.CreateSuccess("Данные пользователя удалены!");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<bool?>> UserHasPasswordAsync(string userId, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser? user = await userManager.FindByIdAsync(userId); ;
        if (user is null)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = $"Пользователь #{userId} не найден" }] };

        return new()
        {
            Response = await userManager.HasPasswordAsync(user)
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<bool?>> GetTwoFactorEnabledAsync(string userId, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser? user = await userManager.FindByIdAsync(userId); ;
        if (user is null)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = $"Пользователь #{userId} не найден" }] };

        return new() { Response = await userManager.GetTwoFactorEnabledAsync(user) };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetTwoFactorEnabledAsync(SetTwoFactorEnabledRequestModel req, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser? user = await userManager.FindByIdAsync(req.UserId); ;
        if (user is null)
            return ResponseBaseModel.CreateError($"Пользователь #{req.UserId} не найден");

        string msg;
        IdentityResult set2faResult = await userManager.SetTwoFactorEnabledAsync(user, req.EnabledSet);
        if (!set2faResult.Succeeded)
        {
            return ResponseBaseModel.CreateError("Произошла непредвиденная ошибка при отключении 2FA.");
        }
        msg = $"Двухфакторная аутентификация для #{req.UserId}/{user.Email} установлена в: {req.EnabledSet}";
        loggerRepo.LogInformation(msg);
        return ResponseBaseModel.CreateSuccess(msg);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ResetAuthenticatorKeyAsync(string userId, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser? user = await userManager.FindByIdAsync(userId); ;
        if (user is null)
            return ResponseBaseModel.CreateError($"Пользователь #{userId} не найден");

        IdentityResult res = await userManager.ResetAuthenticatorKeyAsync(user);

        if (!res.Succeeded)
            return ResponseBaseModel.Create(res.Errors.Select(x => new ResultMessage() { Text = $"[{x.Code}:{x.Description}]", TypeMessage = MessagesTypesEnum.Error }));

        string msg = $"Пользователь с идентификатором '{userId}' сбросил ключ приложения для аутентификации.";
        loggerRepo.LogInformation(msg);

        return ResponseBaseModel.CreateSuccess(msg);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> RemoveLoginForUserAsync(RemoveLoginRequestModel req, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser? user = await userManager.FindByIdAsync(req.UserId); ;
        if (user is null)
            return ResponseBaseModel.CreateError($"Пользователь #{req.UserId} не найден");

        IdentityResult result = await userManager.RemoveLoginAsync(user, req.LoginProvider, req.ProviderKey);
        if (!result.Succeeded)
            return new() { Messages = result.Errors.Select(x => new ResultMessage() { TypeMessage = MessagesTypesEnum.Error, Text = $"[{x.Code}:{x.Description}]" }).ToList() };

        return ResponseBaseModel.CreateSuccess("Успешно удалено");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> VerifyTwoFactorTokenAsync(VerifyTwoFactorTokenRequestModel req, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser? user = await userManager.FindByIdAsync(req.UserId); ;
        if (user is null)
            return new() { Messages = [new() { Text = $"Пользователь #{req.UserId} не найден", TypeMessage = MessagesTypesEnum.Error }] };

        bool is2faTokenValid = await userManager.VerifyTwoFactorTokenAsync(
           user, userManager.Options.Tokens.AuthenticatorTokenProvider, req.VerificationCode);

        return is2faTokenValid
            ? ResponseBaseModel.CreateSuccess("Токен действителен")
            : ResponseBaseModel.CreateError("Ошибка: код подтверждения недействителен");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int?>> CountRecoveryCodesAsync(string userId, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser? user = await userManager.FindByIdAsync(userId); ;
        if (user is null)
            return new() { Messages = [new() { Text = $"Пользователь #{userId} не найден", TypeMessage = MessagesTypesEnum.Error }] };

        return new() { Response = await userManager.CountRecoveryCodesAsync(user) };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> GenerateChangeEmailTokenAsync(GenerateChangeEmailTokenRequestModel req, CancellationToken token = default)
    {

        if (!MailAddress.TryCreate(req.NewEmail, out _))
            return ResponseBaseModel.CreateError($"Адрес e-mail `{req.NewEmail}` имеет не корректный формат");

        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser? user = await userManager.FindByIdAsync(req.UserId); ;
        if (user is null)
            return new() { Messages = [new() { Text = $"Пользователь #{req.UserId} не найден", TypeMessage = MessagesTypesEnum.Error }] };

        string code = await userManager.GenerateChangeEmailTokenAsync(user, req.NewEmail);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        string callbackUrl = $"{req.BaseAddress}?userId={user.Id}&email={req.NewEmail}&code={code}";
        await emailSender.SendConfirmationLinkAsync(user, req.NewEmail, HtmlEncoder.Default.Encode(callbackUrl));

        return ResponseBaseModel.CreateSuccess("Письмо с ссылкой для подтверждения изменения адреса почты отправлено на ваш E-mail. Пожалуйста, проверьте свою электронную почту.");

    }

    /// <inheritdoc/>
    public async Task<TResponseModel<IEnumerable<string>?>> GenerateNewTwoFactorRecoveryCodesAsync(GenerateNewTwoFactorRecoveryCodesRequestModel req, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser? user = await userManager.FindByIdAsync(req.UserId); ;
        if (user is null)
            return new() { Messages = [new() { Text = $"Пользователь #{req.UserId} не найден", TypeMessage = MessagesTypesEnum.Error }] };

        return new() { Response = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, req.Number) };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<string?>> GetAuthenticatorKeyAsync(string userId, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser? user = await userManager.FindByIdAsync(userId); ;
        if (user is null)
            return new() { Messages = [new() { Text = $"Пользователь #{userId} не найден", TypeMessage = MessagesTypesEnum.Error }] };

        string? unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(unformattedKey))
        {
            await userManager.ResetAuthenticatorKeyAsync(user);
            unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);
        }

        return new()
        {
            Response = unformattedKey
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<string?>> GeneratePasswordResetTokenAsync(string userId, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser? user = await userManager.FindByIdAsync(userId); ;
        if (user is null)
            return new() { Messages = [new() { Text = $"Пользователь #{userId} не найден", TypeMessage = MessagesTypesEnum.Error }] };

        return new()
        {
            Response = await userManager.GeneratePasswordResetTokenAsync(user)
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SendPasswordResetLinkAsync(SendPasswordResetLinkRequestModel req, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        IEmailSender<ApplicationUser> emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender<ApplicationUser>>();

        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        ApplicationUser? user = await userManager.FindByIdAsync(req.UserId); ;
        if (user is null)
            return ResponseBaseModel.CreateError($"Пользователь #{req.UserId} не найден");

        if (!MailAddress.TryCreate(user.Email, out _))
            return ResponseBaseModel.CreateError($"email `{user.Email}` имеет не корректный формат. error {{4EE55201-8367-433D-9766-ABDE15B7BC04}}");

        string code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(req.ResetToken));
        string callbackUrl = $"{req.BaseAddress}?code={code}";
        await emailSender.SendPasswordResetLinkAsync(user, user.Email, HtmlEncoder.Default.Encode(callbackUrl));

        return ResponseBaseModel.CreateSuccess("Письмо с токеном отправлено на Email");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ChangePasswordAsync(IdentityChangePasswordModel req, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        ApplicationUser? user = await userManager.FindByIdAsync(req.UserId); ;
        if (user is null)
            return ResponseBaseModel.CreateError($"Пользователь #{req.UserId} не найден");

        string msg;
        IdentityResult changePasswordResult = await userManager.ChangePasswordAsync(user, req.CurrentPassword, req.NewPassword);
        if (!changePasswordResult.Succeeded)
            return new()
            {
                Messages = [.. changePasswordResult.Errors.Select(x => new ResultMessage() { TypeMessage = MessagesTypesEnum.Error, Text = $"[{x.Code}: {x.Description}]" })],
            };

        msg = $"Пользователю [`{user.Id}`/`{user.Email}`] успешно изменён пароль.";
        loggerRepo.LogInformation(msg);
        return ResponseBaseModel.CreateSuccess(msg);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> AddPasswordAsync(IdentityPasswordModel req, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        ApplicationUser? user = await userManager.FindByIdAsync(req.UserId); ;
        if (user is null)
            return ResponseBaseModel.CreateError($"Пользователь #{req.UserId} не найден");

        IdentityResult addPasswordResult = await userManager.AddPasswordAsync(user, req.Password);
        if (!addPasswordResult.Succeeded)
        {
            return new()
            {
                Messages = [.. addPasswordResult.Errors.Select(e => new ResultMessage()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = $"[{e.Code}: {e.Description}]"
                })]
            };
        }

        return ResponseBaseModel.CreateSuccess("Пароль установлен");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<UserInfoModel>> SelectUsersOfIdentityAsync(TPaginationRequestStandardModel<SimpleBaseRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
        {
            loggerRepo.LogError("req.Payload is null");
            return new();
        }

        if (req.PageSize < 10)
            req.PageSize = 10;

        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        IQueryable<ApplicationUser> q = identityContext.Users.AsQueryable();
        if (!string.IsNullOrWhiteSpace(req.Payload.SearchQuery))
        {
            req.Payload.SearchQuery = req.Payload.SearchQuery.ToUpper();
            q = q.Where(x => (x.NormalizedEmail != null && x.NormalizedEmail.Contains(req.Payload.SearchQuery)) ||
            (x.NormalizedUserName != null && x.NormalizedUserName.Contains(req.Payload.SearchQuery)) ||
            (x.NormalizedFirstNameUpper != null && x.NormalizedFirstNameUpper.Contains(req.Payload.SearchQuery)) ||
            (x.NormalizedLastNameUpper != null && x.NormalizedLastNameUpper.Contains(req.Payload.SearchQuery)) ||
            (x.PhoneNumber != null && x.PhoneNumber.Contains(req.Payload.SearchQuery)));
        }

        return new()
        {
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortBy = req.SortBy,
            SortingDirection = req.SortingDirection,
            Response = [..await q.OrderBy(x => x.Id)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize)
            .Select(x => new UserInfoModel()
            {
                UserId = x.Id,
                AccessFailedCount = x.AccessFailedCount,
                Email = x.Email,
                EmailConfirmed = x.EmailConfirmed,
                GivenName = x.FirstName,
                LockoutEnabled = x.LockoutEnabled,
                LockoutEnd = x.LockoutEnd,
                PhoneNumber = x.PhoneNumber,
                RequestChangePhone = x.RequestChangePhone,
                Surname = x.LastName,
                TelegramId = x.ChatTelegramId,
                UserName = x.UserName ?? "",
                Patronymic = x.Patronymic,
                KladrCode = x.KladrCode,
                AddressUserComment = x.AddressUserComment,
                KladrTitle = x.KladrTitle,
            })
            .ToListAsync(cancellationToken: token)]
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel[]>> GetUsersOfIdentityAsync(string[] users_ids, CancellationToken token = default)
    {
        users_ids = [.. users_ids.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct()];
        TResponseModel<UserInfoModel[]> res = new() { Response = [] };
        if (users_ids.Length == 0)
        {
            res.AddError($"Пустой запрос > {nameof(GetUsersOfIdentityAsync)}");
            return res;
        }
        string[] find_users_ids = [.. users_ids.Where(x => x != GlobalStaticConstantsRoles.Roles.System)];
        if (find_users_ids.Length == 0)
        {
            res.Response = [.. users_ids.Select(x => UserInfoModel.BuildSystem())];
            return res;
        }

        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        ApplicationUser[] users = await identityContext
            .Users
            .Where(x => find_users_ids.Contains(x.Id))
            .ToArrayAsync(cancellationToken: token);

        var users_roles = await identityContext
           .UserRoles
           .Where(x => find_users_ids.Contains(x.UserId))
           .Select(x => new { x.RoleId, x.UserId })
           .ToArrayAsync(cancellationToken: token);

        EntryAltModel[] roles_names = users_roles.Length == 0
            ? []
            : await identityContext
                .Roles
                .Where(x => users_roles.Select(x => x.RoleId).Distinct().ToArray().Contains(x.Id))
                .Select(x => new EntryAltModel() { Id = x.Id, Name = x.Name })
                .ToArrayAsync(cancellationToken: token);

        EntryAltTagModel[] claims = await identityContext
             .UserClaims
             .Where(x => find_users_ids.Contains(x.UserId) && x.ClaimType != null && x.ClaimType != "")
             .Select(x => new EntryAltTagModel() { Id = x.UserId, Name = x.ClaimType, Tag = x.ClaimValue })
             .ToArrayAsync(cancellationToken: token);

        string[]? roles_for_user(string user_id)
        {
            return [.. roles_names
                .Where(x => users_roles.Any(y => y.UserId == user_id && y.RoleId == x.Id))
                .Select(x => x.Name!)
                .Distinct()];
        }

        UserInfoModel convert_user(ApplicationUser app_user)
        {
            return new()
            {
                GivenName = app_user.FirstName,
                Surname = app_user.LastName,
                UserId = app_user.Id,
                Patronymic = app_user.Patronymic,
                KladrCode = app_user.KladrCode,
                KladrTitle = app_user.KladrTitle,
                ExternalUserId = app_user.ExternalUserId,
                AddressUserComment = app_user.AddressUserComment,
                AccessFailedCount = app_user.AccessFailedCount,
                Email = app_user.Email,
                EmailConfirmed = app_user.EmailConfirmed,
                LockoutEnabled = app_user.LockoutEnabled,
                LockoutEnd = app_user.LockoutEnd,
                PhoneNumber = app_user.PhoneNumber,
                RequestChangePhone = app_user.RequestChangePhone,
                UserName = app_user.UserName ?? "",
                TelegramId = app_user.ChatTelegramId,
                Roles = roles_for_user(app_user.Id)?.ToList(),
                Claims = [.. claims.Where(x => x.Id == app_user.Id).Select(x => new EntryAltModel() { Id = x.Id, Name = x.Name })]
            };
        }

        res.Response = [.. users.Select(convert_user)];

        if (users_ids.Any(x => x == GlobalStaticConstantsRoles.Roles.System))
            res.Response = [.. res.Response.Union([UserInfoModel.BuildSystem()])];

        find_users_ids = [.. find_users_ids.Where(x => !res.Response.Any(y => y.UserId == x))];
        if (find_users_ids.Length != 0)
            res.AddError($"Некоторые пользователи (Identity) не найдены: {string.Join(",", find_users_ids)}");

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel[]>> GetUsersIdentityByEmailsAsync(string[] users_emails, CancellationToken token = default)
    {
        users_emails = [.. users_emails.Where(x => MailAddress.TryCreate(x, out _)).Select(x => x.ToUpper())];
        TResponseModel<UserInfoModel[]> res = new() { Response = [] };
        if (users_emails.Length == 0)
        {
            res.AddError($"Пустой запрос > {nameof(GetUsersIdentityByEmailsAsync)}");
            return res;
        }

        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        ApplicationUser[] users = await identityContext
            .Users
            .Where(x => users_emails.Contains(x.NormalizedEmail))
            .ToArrayAsync(cancellationToken: token);

        string[] find_users_ids = [.. users.Select(x => x.Id)];
        var users_roles = await identityContext
           .UserRoles
           .Where(x => find_users_ids.Contains(x.UserId))
           .Select(x => new { x.RoleId, x.UserId })
           .ToArrayAsync(cancellationToken: token);

        EntryAltModel[] roles_names = users_roles.Length == 0
            ? []
            : await identityContext
                .Roles
                .Where(x => users_roles.Select(x => x.RoleId).Distinct().ToArray().Contains(x.Id))
                .Select(x => new EntryAltModel() { Id = x.Id, Name = x.Name })
                .ToArrayAsync(cancellationToken: token);

        EntryAltTagModel[] claims = await identityContext
             .UserClaims
             .Where(x => find_users_ids.Contains(x.UserId) && x.ClaimType != null && x.ClaimType != "")
             .Select(x => new EntryAltTagModel() { Id = x.UserId, Name = x.ClaimType, Tag = x.ClaimValue })
             .ToArrayAsync(cancellationToken: token);

        string[]? roles_for_user(string user_id)
        {
            return [.. roles_names
                .Where(x => users_roles.Any(y => y.UserId == user_id && y.RoleId == x.Id))
                .Select(x => x.Name!)
                .Distinct()];
        }

        UserInfoModel convert_user(ApplicationUser app_user)
        {
            return new()
            {
                GivenName = app_user.FirstName,
                Surname = app_user.LastName,
                UserId = app_user.Id,
                AccessFailedCount = app_user.AccessFailedCount,
                Email = app_user.Email,
                EmailConfirmed = app_user.EmailConfirmed,
                LockoutEnabled = app_user.LockoutEnabled,
                LockoutEnd = app_user.LockoutEnd,
                PhoneNumber = app_user.PhoneNumber,
                RequestChangePhone = app_user.RequestChangePhone,
                UserName = app_user.UserName ?? "",
                TelegramId = app_user.ChatTelegramId,
                Roles = roles_for_user(app_user.Id)?.ToList(),
                Claims = [.. claims.Where(x => x.Id == app_user.Id).Select(x => new EntryAltModel() { Id = x.Id, Name = x.Name })]
            };
        }

        res.Response = users.Select(convert_user).ToArray();

        if (users_emails.Any(x => x == GlobalStaticConstantsRoles.Roles.System))
            res.Response = [.. res.Response.Union([UserInfoModel.BuildSystem()])];

        find_users_ids = [.. find_users_ids.Where(x => !res.Response.Any(y => y.UserId == x))];
        if (find_users_ids.Length != 0)
            res.AddWarning($"Некоторые пользователи (Identity) не найдены: {string.Join(",", find_users_ids)}");

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ChangeEmailAsync(IdentityEmailTokenModel req, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        ApplicationUser? user = await userManager.FindByIdAsync(req.UserId); ;
        if (user is null)
            return ResponseBaseModel.CreateError($"Пользователь #{req.UserId} не найден");

        string code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(req.Token));

        IdentityResult result = await userManager.ChangeEmailAsync(user, req.Email, code);
        if (!result.Succeeded)
            return ResponseBaseModel.CreateError("Ошибка при смене электронной почты.");

        IdentityResult setUserNameResult = await userManager.SetUserNameAsync(user, req.Email);

        if (!setUserNameResult.Succeeded)
            return ResponseBaseModel.CreateError("Ошибка изменения имени пользователя.");

        return ResponseBaseModel.CreateSuccess("Благодарим вас за подтверждение изменения адреса электронной почты.");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateUserDetailsAsync(IdentityDetailsModel req, CancellationToken token = default)
    {
        req.FirstName ??= "";
        req.LastName ??= "";
        req.Patronymic ??= "";

        req.FirstName = req.FirstName.Trim();
        req.LastName = req.LastName.Trim();
        req.Patronymic = req.Patronymic.Trim();

        req.AddressUserComment = req.AddressUserComment?.Trim();

        if (!string.IsNullOrWhiteSpace(req.PhoneNum) && !GlobalTools.IsPhoneNumber(req.PhoneNum))
            return ResponseBaseModel.CreateError("Телефон должен быть в формате: +79994440011 (можно без +)");

        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        ApplicationUser? user_db;

        IQueryable<ApplicationUser> _findUserQuery = identityContext
            .Users
            .Where(x => x.Id == req.UserId &&
                (x.FirstName != req.FirstName || x.LastName != req.LastName || x.Patronymic != req.Patronymic ||
                x.KladrCode != req.KladrCode || x.KladrTitle != req.KladrTitle || x.AddressUserComment != req.AddressUserComment));

        user_db = await _findUserQuery
            .FirstOrDefaultAsync(cancellationToken: token);

        if (user_db is null)
            return ResponseBaseModel.CreateInfo("Без изменений");

        IQueryable<ApplicationUser> q = identityContext
            .Users
            .Where(x => x.Id == req.UserId);

        await q
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.ExternalUserId, req.ExternalUserId)
                .SetProperty(p => p.FirstName, req.FirstName)
                .SetProperty(p => p.NormalizedFirstNameUpper, req.FirstName.ToUpper())
                .SetProperty(p => p.LastName, req.LastName)
                .SetProperty(p => p.NormalizedLastNameUpper, req.LastName.ToUpper())
                .SetProperty(p => p.Patronymic, req.Patronymic)
                .SetProperty(p => p.NormalizedPatronymicUpper, req.Patronymic.ToUpper()), cancellationToken: token);

        if (req.UpdateAddress)
        {
            await q
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.KladrTitle, req.KladrTitle)
                .SetProperty(p => p.KladrCode, req.KladrCode)
                .SetProperty(p => p.AddressUserComment, req.AddressUserComment), cancellationToken: token);
        }

        await ClaimsUserFlushAsync(user_db.Id, token);

        return ResponseBaseModel.CreateSuccess("First/Last names (and phone) update");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClaimDeleteAsync(ClaimAreaIdModel req, CancellationToken token = default)
    {
        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);

        switch (req.ClaimArea)
        {
            case ClaimAreasEnum.ForRole:
                IdentityRoleClaim<string>? claim_role_db = await identityContext.RoleClaims.FirstOrDefaultAsync(x => x.Id == req.Id, cancellationToken: token);

                if (claim_role_db is null)
                    return ResponseBaseModel.CreateWarning($"Claim #{req.Id} не найден в БД");

                identityContext.RoleClaims.Remove(claim_role_db);
                break;
            case ClaimAreasEnum.ForUser:
                IdentityUserClaim<string>? claim_user_db = await identityContext.UserClaims.FirstOrDefaultAsync(x => x.Id == req.Id, cancellationToken: token);

                if (claim_user_db is null)
                    return ResponseBaseModel.CreateError($"Claim #{req.Id} не найден в БД");

                identityContext.UserClaims.Remove(claim_user_db);
                break;
            default:
                throw new NotImplementedException("error {7F5317DC-EA89-47C3-BE2A-8A90838A113C}");
        }

        await identityContext.SaveChangesAsync(token);

        return ResponseBaseModel.CreateSuccess("Claim успешно удалён");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClaimUpdateOrCreateAsync(ClaimUpdateModel req, CancellationToken token = default)
    {
        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);

        switch (req.ClaimArea)
        {
            case ClaimAreasEnum.ForRole:
                IdentityRoleClaim<string>? claim_role_db;
                if (req.ClaimUpdate.Id < 1)
                {
                    claim_role_db = new IdentityRoleClaim<string>() { RoleId = req.ClaimUpdate.OwnerId, ClaimType = req.ClaimUpdate.ClaimType, ClaimValue = req.ClaimUpdate.ClaimValue };
                    await identityContext.RoleClaims.AddAsync(claim_role_db, token);
                }
                else
                {
                    claim_role_db = await identityContext.RoleClaims.FirstOrDefaultAsync(x => x.RoleId == req.ClaimUpdate.OwnerId, cancellationToken: token);
                    if (claim_role_db is null)
                        return ResponseBaseModel.CreateError($"Claim #{req.ClaimUpdate.OwnerId} не найден в БД");
                    else if (claim_role_db.ClaimType?.Equals(req.ClaimUpdate.ClaimType) == true && claim_role_db.ClaimValue?.Equals(req.ClaimUpdate.ClaimValue) == true)
                        return ResponseBaseModel.CreateInfo($"Claim #{req.ClaimUpdate.OwnerId} не изменён");

                    claim_role_db.ClaimType = req.ClaimUpdate.ClaimType;
                    claim_role_db.ClaimValue = req.ClaimUpdate.ClaimValue;
                    identityContext.RoleClaims.Update(claim_role_db);
                }

                break;
            case ClaimAreasEnum.ForUser:
                IdentityUserClaim<string>? claim_user_db;

                if (req.ClaimUpdate.Id < 1)
                {
                    claim_user_db = new IdentityUserClaim<string>() { UserId = req.ClaimUpdate.OwnerId, ClaimType = req.ClaimUpdate.ClaimType, ClaimValue = req.ClaimUpdate.ClaimValue };
                    await identityContext.UserClaims.AddAsync(claim_user_db, token);
                }
                else
                {
                    claim_user_db = await identityContext.UserClaims.FirstOrDefaultAsync(x => x.UserId == req.ClaimUpdate.OwnerId, cancellationToken: token);
                    if (claim_user_db is null)
                        return ResponseBaseModel.CreateError($"Claim #{req.ClaimUpdate.OwnerId} не найден в БД");
                    else if (claim_user_db.ClaimType?.Equals(req.ClaimUpdate.ClaimType) == true && claim_user_db.ClaimValue?.Equals(req.ClaimUpdate.ClaimValue) == true)
                        return ResponseBaseModel.CreateInfo($"Claim #{req.ClaimUpdate.OwnerId} не изменён");

                    claim_user_db.ClaimType = req.ClaimUpdate.ClaimType;
                    claim_user_db.ClaimValue = req.ClaimUpdate.ClaimValue;
                    identityContext.UserClaims.Update(claim_user_db);
                }

                break;
            default:
                throw new NotImplementedException("error {33A20922-0E76-421F-B2C4-109B7A420827}");
        }

        await identityContext.SaveChangesAsync(token);

        return ResponseBaseModel.CreateSuccess("Запрос успешно обработан");
    }

    /// <inheritdoc/>
    public async Task<List<ClaimBaseModel>> GetClaimsAsync(ClaimAreaOwnerModel req, CancellationToken token = default)
    {
        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);

        List<ClaimBaseModel> res = req.ClaimArea switch
        {
            ClaimAreasEnum.ForRole => await identityContext.RoleClaims.Where(x => x.RoleId == req.OwnerId).Select(x => new ClaimBaseModel() { Id = x.Id, ClaimType = x.ClaimType, ClaimValue = x.ClaimValue }).ToListAsync(cancellationToken: token),
            ClaimAreasEnum.ForUser => await identityContext.UserClaims.Where(x => x.UserId == req.OwnerId).Select(x => new ClaimBaseModel() { Id = x.Id, ClaimType = x.ClaimType, ClaimValue = x.ClaimValue }).ToListAsync(cancellationToken: token),
            _ => throw new NotImplementedException("error {61909910-B126-4204-8AE6-673E11D49BCD}")
        };

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetLockUserAsync(IdentityBooleanModel req, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        ApplicationUser? user = await userManager.FindByIdAsync(req.UserId);
        if (user is null)
            return ResponseBaseModel.CreateError($"Пользователь не найден: {req.UserId}");

        await userManager.SetLockoutEndDateAsync(user, req.Set ? DateTimeOffset.MaxValue : null);
        return ResponseBaseModel.CreateSuccess($"Пользователь успешно [{user.Email}] {(req.Set ? "заблокирован" : "разблокирован")}");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<UserInfoModel>> FindUsersAsync(FindWithOwnedRequestModel req, CancellationToken token = default)
    {
        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        IQueryable<ApplicationUser> q = identityContext.Users
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.OwnerId))
            q = q.Where(x => identityContext.UserRoles.Any(y => x.Id == y.UserId && req.OwnerId == y.RoleId));
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        if (!string.IsNullOrWhiteSpace(req.FindQuery))
        {
            string upp_query = req.FindQuery.ToUpper();
            q = q.Where(x =>
                EF.Functions.Like(x.NormalizedEmail, $"%{userManager.KeyNormalizer.NormalizeEmail(req.FindQuery)}%") ||
                EF.Functions.Like(x.NormalizedFirstNameUpper, $"%{upp_query}%") ||
                EF.Functions.Like(x.NormalizedLastNameUpper, $"%{upp_query}%") ||
                EF.Functions.Like(x.NormalizedPatronymicUpper, $"%{upp_query}%") ||
                EF.Functions.Like(x.ExternalUserId, req.FindQuery) ||
                EF.Functions.Like(x.PhoneNumber, req.FindQuery) ||
                x.Id == req.FindQuery);
        }
        int total = await q.CountAsync(cancellationToken: token);
        q = q.OrderBy(x => x.UserName).Skip(req.PageNum * req.PageSize).Take(req.PageSize);
        var users = await q
            .Select(x => new
            {
                x.Id,
                x.UserName,
                x.Email,
                x.PhoneNumber,
                x.RequestChangePhone,
                x.ChatTelegramId,
                x.EmailConfirmed,
                x.LockoutEnd,
                x.LockoutEnabled,
                x.AccessFailedCount,
                x.FirstName,
                x.LastName,
                x.Patronymic,
                x.KladrTitle,
                x.KladrCode,
                x.ExternalUserId,
                x.AddressUserComment,
            })
            .ToArrayAsync(cancellationToken: token);
        string[] users_ids = users.Select(x => x.Id).ToArray();
        var roles =
           await (from link in identityContext.UserRoles.Where(x => users_ids.Contains(x.UserId))
                  join role in identityContext.Roles on link.RoleId equals role.Id
                  select new { RoleName = role.Name, link.UserId }).ToArrayAsync(cancellationToken: token);

        var claims =
           await (from claim in identityContext.UserClaims.Where(x => users_ids.Contains(x.UserId))
                  select new { claim.ClaimValue, claim.ClaimType, claim.UserId }).ToArrayAsync(cancellationToken: token);

        IEnumerable<UserInfoModel> _res = users.Select(x => UserInfoModel
            .Build(userId: x.Id,
                   userName: x.UserName ?? "",
                   kladrTitle: x.KladrTitle,
                   kladrCode: x.KladrCode,
                   addressUserComment: x.AddressUserComment,
                   email: x.Email,
                   phoneNumber: x.PhoneNumber,
                   phoneNumberRequestChange: x.RequestChangePhone,
                   telegramId: x.ChatTelegramId,
                   emailConfirmed: x.EmailConfirmed,
                   lockoutEnd: x.LockoutEnd,
                   lockoutEnabled: x.LockoutEnabled,
                   accessFailedCount: x.AccessFailedCount,
                   firstName: x.FirstName,
                   lastName: x.LastName,
                   patronymic: x.Patronymic,
                   externalUserId: x.ExternalUserId,
                   roles: [.. roles.Where(y => y.UserId == x.Id).Select(z => z.RoleName)],
                   claims: [.. claims.Where(o => o.UserId == x.Id).Select(q => new EntryAltModel() { Id = q.ClaimType, Name = q.ClaimValue })]));

        return new()
        {
            Response = [.. _res],

            TotalRowsCount = total,
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortBy = req.SortBy,
            SortingDirection = req.SortingDirection
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ResetPasswordAsync(IdentityPasswordTokenModel req, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
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
                Messages = [.. result.Errors.Select(x => new ResultMessage()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = $"[{x.Code}: {x.Description}]"
                })]
            };

        return ResponseBaseModel.CreateSuccess("Пароль успешно сброшен");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel>> FindUserByEmailAsync(string email, CancellationToken token = default)
    {
        TResponseModel<UserInfoModel> res = new();
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
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
            UserName = user.UserName ?? "",
            AccessFailedCount = user.AccessFailedCount,
            EmailConfirmed = user.EmailConfirmed,
            LockoutEnabled = user.LockoutEnabled,
            LockoutEnd = user.LockoutEnd,
            PhoneNumber = user.PhoneNumber,
            RequestChangePhone = user.RequestChangePhone,
            TelegramId = user.ChatTelegramId,
            Roles = [.. (await userManager.GetRolesAsync(user))],
            Claims = claims.Select(x => new EntryAltModel() { Id = x.Type, Name = x.Value }).ToArray(),
        };

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> GenerateEmailConfirmationAsync(SimpleUserIdentityModel req, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        IEmailSender<ApplicationUser> emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender<ApplicationUser>>();

        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
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
    public async Task<ResponseBaseModel> ConfirmUserEmailCodeAsync(UserCodeModel req, CancellationToken token = default)
    {
        if (req.UserId is null || req.Code is null)
            return ResponseBaseModel.CreateError("UserId is null || Code is null. error {715DE145-87B0-48B0-9341-0A21962045BF}");
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
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
    public async Task<TResponseModel<bool>> ClaimsUserFlushAsync(string user_id, CancellationToken token = default)
    {
        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        ApplicationUser app_user = await identityContext.Users.FirstAsync(x => x.Id == user_id, cancellationToken: token);

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
            claims_db = await identityContext.UserClaims.Where(x => x.ClaimType == GlobalStaticConstants.TelegramIdClaimName && x.ClaimValue == chat_tg_id).ToArrayAsync(cancellationToken: token);
            claims_ids = [.. claims_db.Where(x => x.UserId != app_user.Id).Select(x => x.Id)];
            if (claims_ids.Length != 0)
            {
                res.Response = await identityContext.UserClaims.Where(x => claims_ids.Contains(x.Id)).ExecuteDeleteAsync(cancellationToken: token) != 0 || res.Response;
            }

            if (!claims_db.Any(x => x.UserId == app_user.Id))
            {
                await identityContext.AddAsync(new IdentityUserClaim<string>()
                {
                    ClaimType = GlobalStaticConstants.TelegramIdClaimName,
                    ClaimValue = app_user.ChatTelegramId.ToString(),
                    UserId = app_user.Id,
                }, token);
                res.Response = await identityContext.SaveChangesAsync(token) != 0 || res.Response;
            }
        }
        else
        {
            res.Response = await identityContext
                .UserClaims
                .Where(x => x.ClaimType == GlobalStaticConstants.TelegramIdClaimName && x.UserId == app_user.Id)
                .ExecuteDeleteAsync(cancellationToken: token) != 0 || res.Response;
        }

        claims_db = await identityContext.UserClaims.Where(x => x.ClaimType == ClaimTypes.GivenName && x.UserId == app_user.Id).ToArrayAsync(cancellationToken: token);
        if (string.IsNullOrWhiteSpace(app_user.FirstName))
        {
            if (claims_db.Length != 0)
            {
                claims_ids = claims_db.Select(x => x.Id).ToArray();
                res.Response = await identityContext.UserClaims.Where(x => claims_ids.Contains(x.Id)).ExecuteDeleteAsync(cancellationToken: token) != 0 || res.Response;
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
                }, token);
                res.Response = await identityContext.SaveChangesAsync(token) != 0 || res.Response;
            }
            else if (claims_db.Length > 1)
            {
                fe = qo.First();
                claims_ids = [.. qo.Skip(1).Select(x => x.Id)];
                res.Response = await identityContext.UserClaims.Where(x => claims_ids.Contains(x.Id)).ExecuteDeleteAsync(cancellationToken: token) != 0 || res.Response;
            }
            else
            {
                fe = qo.First();
                if (fe.ClaimValue != app_user.FirstName)
                {
                    res.Response = await identityContext
                                        .UserClaims
                                        .Where(x => x.Id == fe.Id)
                                        .ExecuteUpdateAsync(set => set.SetProperty(p => p.ClaimValue, app_user.FirstName), cancellationToken: token) != 0 || res.Response;
                }
            }
        }

        claims_db = await identityContext.UserClaims.Where(x => x.ClaimType == ClaimTypes.Surname && x.UserId == app_user.Id).ToArrayAsync(cancellationToken: token);
        IOrderedEnumerable<IdentityUserClaim<string>> oq = claims_db.OrderBy(x => x.Id);
        if (string.IsNullOrWhiteSpace(app_user.FirstName))
        {
            if (claims_db.Length != 0)
            {
                claims_ids = [.. claims_db.Select(x => x.Id)];
                res.Response = await identityContext.UserClaims.Where(x => claims_ids.Contains(x.Id)).ExecuteDeleteAsync(cancellationToken: token) != 0 || res.Response;
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
                }, token);
                res.Response = await identityContext.SaveChangesAsync(token) != 0 || res.Response;
            }
            else if (claims_db.Length > 1)
            {
                claims_ids = [.. oq.Skip(1).Select(x => x.Id)];
                res.Response = await identityContext.UserClaims.Where(x => claims_ids.Contains(x.Id)).ExecuteDeleteAsync(cancellationToken: token) != 0 || res.Response;
            }
            else if (claims_db[0].ClaimValue != app_user.FirstName)
            {
                res.Response = await identityContext
                   .UserClaims
                   .Where(x => x.Id == oq.First().Id)
                   .ExecuteUpdateAsync(set => set.SetProperty(p => p.ClaimValue, app_user.LastName), cancellationToken: token) != 0 || res.Response;
            }
        }

        claims_db = await identityContext.UserClaims.Where(x => x.ClaimType == ClaimTypes.MobilePhone && x.UserId == app_user.Id).ToArrayAsync(cancellationToken: token);
        IOrderedEnumerable<IdentityUserClaim<string>> ot = claims_db.OrderBy(x => x.Id);
        if (string.IsNullOrWhiteSpace(app_user.PhoneNumber))
        {
            if (claims_db.Length != 0)
            {
                claims_ids = claims_db.Select(x => x.Id).ToArray();
                res.Response = await identityContext.UserClaims.Where(x => claims_ids.Contains(x.Id)).ExecuteDeleteAsync(cancellationToken: token) != 0 || res.Response;
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
                }, token);
                res.Response = await identityContext.SaveChangesAsync(token) != 0 || res.Response;
            }
            else if (claims_db.Length > 1)
            {
                claims_ids = [.. ot.Skip(1).Select(x => x.Id)];
                res.Response = await identityContext.UserClaims.Where(x => claims_ids.Contains(x.Id)).ExecuteDeleteAsync(cancellationToken: token) != 0 || res.Response;
            }
            else if (claims_db[0].ClaimValue != app_user.PhoneNumber)
            {
                res.Response = await identityContext
                    .UserClaims
                    .Where(x => x.Id == ot.First().Id)
                    .ExecuteUpdateAsync(set => set.SetProperty(p => p.ClaimValue, app_user.PhoneNumber), cancellationToken: token) != 0 || res.Response;
            }
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<RegistrationNewUserResponseModel> CreateNewUserEmailAsync(string email, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using IUserStore<ApplicationUser> userStore = scope.ServiceProvider.GetRequiredService<IUserStore<ApplicationUser>>();

        using IUserEmailStore<ApplicationUser> emailStore = (IUserEmailStore<ApplicationUser>)userStore;
        ApplicationUser user = IdentityStatic.CreateInstanceUser();
        await userStore.SetUserNameAsync(user, email, token);
        await emailStore.SetEmailAsync(user, email, token);

        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        IdentityResult result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
            return new() { Messages = result.Errors.Select(x => new ResultMessage() { Text = $"[{x.Code}: {x.Description}]", TypeMessage = MessagesTypesEnum.Error }).ToList() };

        return new()
        {
            RequireConfirmedAccount = userManager.Options.SignIn.RequireConfirmedAccount,
            RequireConfirmedEmail = userManager.Options.SignIn.RequireConfirmedEmail,
            RequireConfirmedPhoneNumber = userManager.Options.SignIn.RequireConfirmedPhoneNumber,
            Response = user.Id,
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<string>> CreateUserManualAsync(TAuthRequestStandardModel<UserInfoBaseModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Ошибка запроса: Payload is null" }] };

        req.Payload.UserName = req.Payload.UserName.Trim();
        req.Payload.GivenName = req.Payload.GivenName?.Trim();
        req.Payload.Surname = req.Payload.Surname?.Trim();
        req.Payload.Patronymic = req.Payload.Patronymic?.Trim();

        TResponseModel<bool?> requiredPhoneForUser = await StorageTransmissionRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.RequiredPhoneForUser, token);
        TResponseModel<bool?> userEmailEmailGenerate = await StorageTransmissionRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.UserEmailEmailGenerate, token);

        if (requiredPhoneForUser.Response == true && (string.IsNullOrWhiteSpace(req.Payload.PhoneNumber) || !GlobalTools.IsPhoneNumber(req.Payload.PhoneNumber)))
            return new() { Messages = [new() { Text = "Телефон должен быть в формате: +79994440011 (можно без +)", TypeMessage = MessagesTypesEnum.Error }] };

        if (userEmailEmailGenerate.Response == true && string.IsNullOrWhiteSpace(req.Payload.UserName))
            req.Payload.UserName = $"{Guid.NewGuid()}@{GlobalStaticConstants.FakeHost}";

        if (!MailAddress.TryCreate(req.Payload.UserName, out _))
            return new() { Messages = [new() { Text = "Email (Username) не корректный", TypeMessage = MessagesTypesEnum.Error }] };

        if (string.IsNullOrWhiteSpace(req.SenderActionUserId))
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Автор запроса не определён" }] };

        TResponseModel<UserInfoModel[]> getAuthorUser = await GetUsersOfIdentityAsync([req.SenderActionUserId], token);
        if (!getAuthorUser.Success())
            return new() { Messages = getAuthorUser.Messages };

        if (getAuthorUser.Response is null || !getAuthorUser.Response.Any(x => x.UserId == req.SenderActionUserId))
            return new() { Messages = [new() { Text = "Автор запроса не найден в БД", TypeMessage = MessagesTypesEnum.Error }] };

        UserInfoModel author = getAuthorUser.Response.First(x => x.UserId == req.SenderActionUserId);

        if (!author.IsAdmin && author.Roles?.Contains(GlobalStaticConstantsRoles.Roles.RetailManage) != true && author.Roles?.Contains(GlobalStaticConstantsRoles.Roles.CommerceManager) != true)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Недостаточно прав для операции" }] };

        using IdentityAppDbContext ctx = await identityDbFactory.CreateDbContextAsync(token);

        if (await ctx.Users.AnyAsync(x => x.UserName == req.Payload.UserName, cancellationToken: token))
            return new() { Messages = [new() { Text = "Пользователь с таким Email (Username) уже существует", TypeMessage = MessagesTypesEnum.Error }] };

        if (!string.IsNullOrWhiteSpace(req.Payload.PhoneNumber))
        {
            bool _plus = req.Payload.PhoneNumber.Trim().StartsWith('+');
            req.Payload.PhoneNumber = Regex.Replace(req.Payload.PhoneNumber, @"[^\d]", "");
            if (_plus)
                req.Payload.PhoneNumber = $"+{req.Payload.PhoneNumber}";

            if (!GlobalTools.IsPhoneNumber(req.Payload.PhoneNumber))
                return new() { Messages = [new() { Text = "Телефон должен быть в формате: +79994440011 (можно без +)", TypeMessage = MessagesTypesEnum.Error }] };

            TResponseModel<bool?> userPhoneForUserCloneAllow = await StorageTransmissionRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.UserPhoneForUserCloneAllow, token);

            if (userPhoneForUserCloneAllow.Response != true && await ctx.Users.AnyAsync(x => x.PhoneNumber == req.Payload.PhoneNumber || x.PhoneNumber == $"+{req.Payload.PhoneNumber}", cancellationToken: token))
                return new() { Messages = [new() { Text = "Пользователь с таким телефоном уже существует", TypeMessage = MessagesTypesEnum.Error }] };
        }

        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using IUserStore<ApplicationUser> userStore = scope.ServiceProvider.GetRequiredService<IUserStore<ApplicationUser>>();

        using IUserEmailStore<ApplicationUser> emailStore = (IUserEmailStore<ApplicationUser>)userStore;
        ApplicationUser user = IdentityStatic.CreateInstanceUser();
        await userStore.SetUserNameAsync(user, req.Payload.UserName, token);
        await emailStore.SetEmailAsync(user, req.Payload.UserName, token);

        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        IdentityResult result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
            return new() { Messages = result.Errors.Select(x => new ResultMessage() { Text = $"[{x.Code}: {x.Description}]", TypeMessage = MessagesTypesEnum.Error }).ToList() };

        await ctx.Users.Where(x => x.Id == user.Id)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.FirstName, req.Payload.GivenName)
                    .SetProperty(p => p.LastName, req.Payload.Surname)
                    .SetProperty(p => p.EmailConfirmed, true)
                    .SetProperty(p => p.Patronymic, req.Payload.Patronymic), cancellationToken: token);

        if (req.Payload.GivenName != null)
            await ctx.Users.Where(x => x.Id == user.Id)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.NormalizedFirstNameUpper, req.Payload.GivenName.ToUpper()), cancellationToken: token);

        if (req.Payload.Surname != null)
            await ctx.Users.Where(x => x.Id == user.Id)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.NormalizedLastNameUpper, req.Payload.Surname.ToUpper()), cancellationToken: token);

        if (req.Payload.Patronymic != null)
            await ctx.Users.Where(x => x.Id == user.Id)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.NormalizedPatronymicUpper, req.Payload.Patronymic.ToUpper()), cancellationToken: token);

        if (!string.IsNullOrWhiteSpace(req.Payload.PhoneNumber))
        {
            await ctx.Users.Where(x => x.Id == user.Id)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.PhoneNumber, req.Payload.PhoneNumber)
                    .SetProperty(p => p.RequestChangePhone, req.Payload.PhoneNumber)
                    .SetProperty(p => p.PhoneNumberConfirmed, true), cancellationToken: token);
        }

        return new()
        {
            Response = user.Id,
            Messages = [new() { TypeMessage = MessagesTypesEnum.Success, Text = "Пользователь успешно создан" }]
        };
    }

    /// <inheritdoc/>
    public async Task<RegistrationNewUserResponseModel> CreateNewUserWithPasswordAsync(RegisterNewUserPasswordModel req, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using IUserStore<ApplicationUser> userStore = scope.ServiceProvider.GetRequiredService<IUserStore<ApplicationUser>>();
        ApplicationUser user = IdentityStatic.CreateInstanceUser();

        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        await userStore.SetUserNameAsync(user, req.Email, token);

        using IUserEmailStore<ApplicationUser> emailStore = (IUserEmailStore<ApplicationUser>)userStore;

        try
        {
            await emailStore.SetEmailAsync(user, req.Email, token);

            IdentityResult result = await userManager.CreateAsync(user, req.Password);

            if (!result.Succeeded)
                return new() { Messages = result.Errors.Select(x => new ResultMessage() { Text = $"[{x.Code}: {x.Description}]", TypeMessage = MessagesTypesEnum.Error }).ToList() };
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, "");
        }

        string userId = await userManager.GetUserIdAsync(user);
        loggerRepo.LogInformation($"User #{userId} [{req.Email}] created a new account with password.");

        string code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        string callbackUrl = $"{req.BaseAddress}?userId={userId}&code={code}";

        IEmailSender<ApplicationUser> emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender<ApplicationUser>>();
        await emailSender.SendConfirmationLinkAsync(user, req.Email, HtmlEncoder.Default.Encode(callbackUrl));

        RegistrationNewUserResponseModel res = new()
        {
            RequireConfirmedAccount = userManager.Options.SignIn.RequireConfirmedAccount,
            RequireConfirmedEmail = userManager.Options.SignIn.RequireConfirmedEmail,
            RequireConfirmedPhoneNumber = userManager.Options.SignIn.RequireConfirmedPhoneNumber,
            Response = userId
        };
        res.AddSuccess("Registration completed.");

        if (userManager.Options.SignIn.RequireConfirmedAccount)
        {
            res.AddInfo("Account verification required.");
            res.AddWarning("Check your incoming email.");
        }

        return res;
    }

    #region telegram
    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel[]>> GetUsersIdentityByTelegramAsync(long[] tg_ids, CancellationToken token = default)
    {
        tg_ids = [.. tg_ids.Where(x => x != 0)];
        TResponseModel<UserInfoModel[]> response = new() { Response = [] };
        if (tg_ids.Length == 0)
        {
            response.AddError($"Пустой запрос > {nameof(GetUsersIdentityByTelegramAsync)}");
            return response;
        }

        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        ApplicationUser[] users = await identityContext
            .Users
            .Where(x => tg_ids.Any(y => y == x.ChatTelegramId))
        .ToArrayAsync(cancellationToken: token);

        string[] users_ids = [.. users.Select(x => x.Id)];

        TResponseModel<UserInfoModel[]> res_find_users_identity = await GetUsersOfIdentityAsync(users_ids, token);
        if (!res_find_users_identity.Success())
        {
            response.AddRangeMessages(res_find_users_identity.Messages);
            return response;
        }

        if (res_find_users_identity.Response is null || res_find_users_identity.Response.Length == 0)
        {
            response.AddError("Не найдены пользователи");
            return response;
        }
        response.Response = res_find_users_identity.Response;

        tg_ids = [.. tg_ids.Where(x => !response.Response.Any(y => y.TelegramId == x))];
        if (tg_ids.Length != 0)
            response.AddInfo($"Некоторые пользователи (Telegram) не найдены: {string.Join(",", tg_ids)}");

        return response;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TelegramJoinAccountDeleteActionAsync(string userId, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        ApplicationUser? user = await userManager.FindByIdAsync(userId); ;
        if (user is null)
            return ResponseBaseModel.CreateError($"Пользователь #{userId} не найден");

        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        TelegramJoinAccountModelDb[] act = await identityContext.TelegramJoinActions
            .Where(x => x.UserIdentityId == userId)
            .ToArrayAsync(cancellationToken: token);
        if (act.Length == 0)
            return ResponseBaseModel.CreateInfo("Токена нет");
        else
        {
            identityContext.RemoveRange(act);
            int i = await identityContext.SaveChangesAsync(token);

            if (MailAddress.TryCreate(user.Email, out _))
                await mailRepo.SendEmailAsync(user.Email, "Удалён токен привязки Telegram к у/з", "Токен привязки аккаунта Telegram к учётной записи на сайте: удалён.", token: token);

            return ResponseBaseModel.CreateSuccess($"Токен удалён");
        }
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TelegramAccountRemoveIdentityJoinAsync(TAuthRequestStandardModel<TelegramAccountRemoveJoinRequestIdentityModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        ApplicationUser user = identityContext.Users.First(x => x.Id == req.Payload.UserId);
        long? tg_user_dump = user.ChatTelegramId;
        user.ChatTelegramId = null;
        identityContext.Update(user);

        if (MailAddress.TryCreate(user.Email, out _))
            _ = await mailRepo.SendEmailAsync(user.Email, "Удаление привязки Telegram к учётной записи", $"Аккаунт Telegram {tg_user_dump} отключён от вашей учётной записи на сайте", token: token);

        TResponseModel<MessageComplexIdsModel> tgCall = await tgRemoteRepo.SendTextMessageTelegramAsync(new SendTextMessageTelegramBotModel()
        {
            Message = $"Ваш Telegram аккаунт отключён от учётной записи {user.Email} с сайта {req.Payload.ClearBaseUri}",
            UserTelegramId = (await identityContext.TelegramUsers.FirstAsync(x => x.TelegramId == tg_user_dump, cancellationToken: token)).TelegramId,
            From = "уведомление",
        }, token: token);
        if (!tgCall.Success())
            loggerRepo.LogError(tgCall.Message());

        return ResponseBaseModel.CreateSuccess($"Пользователю {user.Email} удалена связь с TelegramId");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<TelegramJoinAccountModelDb>> TelegramJoinAccountCreateAsync(string userId, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        ApplicationUser? user = await userManager.FindByIdAsync(userId); ;
        if (user is null)
            return new TResponseModel<TelegramJoinAccountModelDb>() { Messages = ResponseBaseModel.CreateError($"Пользователь #{userId} не найден").Messages };//ResponseBaseModel.CreateError($"Пользователь #{userId} не найден");

        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        IQueryable<TelegramJoinAccountModelDb> actions_del = identityContext.TelegramJoinActions
            .Where(x => x.UserIdentityId == userId);

        if (await actions_del.AnyAsync(cancellationToken: token))
            identityContext.RemoveRange(actions_del);

        TelegramJoinAccountModelDb act = new()
        {
            GuidToken = Guid.NewGuid().ToString(),
            UserIdentityId = userId,
        };
        await identityContext.AddAsync(act, token);
        await identityContext.SaveChangesAsync(token);
        if (MailAddress.TryCreate(user.Email, out _))
        {
            TResponseModel<UserTelegramBaseModel> bot_username_res = await tgRemoteRepo.AboutBotAsync(token);
            UserTelegramBaseModel? bot_username = bot_username_res.Response;
            string msg = $"Создана ссылка привязки Telegram аккаунта к учётной записи сайта.<br/>";
            msg += $"Нужно подтвердить операцию через Telegram бота. Для этого нужно в TelegramBot @{bot_username?.Username} отправить токен:<br/><u><b>{act.GuidToken}</b></u><br/>Или ссылкой: <a href='https://t.me/{bot_username?.Username}?start={act.GuidToken}'>https://t.me/{bot_username?.Username}?start={act.GuidToken}</a><br/>";
            await mailRepo.SendEmailAsync(user.Email, "Статус привязки Telegram к у/з", msg, token: token);
        }

        return new() { Response = act, Messages = [new() { TypeMessage = MessagesTypesEnum.Success, Text = "Токен сформирован" }] };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TelegramJoinAccountConfirmTokenFromTelegramAsync(TelegramJoinAccountConfirmModel req, bool waitResponse = true, CancellationToken token = default)
    {
        DateTime lifeTime = DateTime.UtcNow.AddMinutes(-req.TelegramJoinAccountTokenLifetimeMinutes);

        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        TelegramJoinAccountModelDb? act = await identityContext.TelegramJoinActions
           .FirstOrDefaultAsync(x => x.CreatedAt > lifeTime && x.GuidToken == req.Token, cancellationToken: token);
        if (act is null)
            return ResponseBaseModel.CreateError("Токен не существует");

        ApplicationUser? appUserDb = await identityContext.Users.FirstOrDefaultAsync(x => x.Id == act.UserIdentityId, cancellationToken: token);
        if (appUserDb is null)
            return ResponseBaseModel.CreateError($"Пользователь (identity/{act.UserIdentityId}) для токена [{req.Token}] не найден в БД");

        //
        identityContext.Remove(act);
        await identityContext.SaveChangesAsync(token);
        //
        appUserDb.ChatTelegramId = req.TelegramId;
        identityContext.Update(appUserDb);
        await identityContext.SaveChangesAsync(token);

        await ClaimsUserFlushAsync(appUserDb.Id, token);
        string msg;

        List<ApplicationUser> other_joins = await identityContext.Users
            .Where(x => x.ChatTelegramId == req.TelegramId && x.Id != appUserDb.Id)
            .ToListAsync(cancellationToken: token);

        if (MailAddress.TryCreate(appUserDb.Email, out _))
        {
            msg = $"Аккаунт Telegram {req.TelegramId} подключился к учётной записи сайта воспользовавшись токеном из вашего профиля: <u><b>{act.GuidToken}</b></u>!<br/><br/>";

            msg += "Если это были не вы, то зайдите в профиль на сайте и удалите связь с Telegram.<br />";

            if (other_joins.Count != 0)
                msg += $"Другие привязки к этому Telegram аккаунту аннулируются: {string.Join("; ", other_joins.Select(x => x.Email))}";

            await mailRepo.SendEmailAsync(appUserDb.Email, "Подтверждение токена привязки Telegram к у/з", msg, token: token);
        }
        msg = "Токен успешно проверен, запрос на привязку вашего Telegram к учётной записи сформирован. Клиенту отправлено Email оповещение с информацией.";
        if (other_joins.Count != 0)
        {
            other_joins.ForEach(x => x.ChatTelegramId = null);
            identityContext.UpdateRange(other_joins);
            await identityContext.SaveChangesAsync(token);
        }

        TResponseModel<MessageComplexIdsModel> tgCall = await tgRemoteRepo.SendTextMessageTelegramAsync(new SendTextMessageTelegramBotModel()
        {
            Message = $"Ваш Telegram аккаунт привязан к учётной записи '{appUserDb.Email}' сайта {req.ClearBaseUri}",
            UserTelegramId = req.TelegramId,
            From = "уведомление",
        }, token: token);
        if (!tgCall.Success())
            loggerRepo.LogError(tgCall.Message());

        return ResponseBaseModel.CreateSuccess(msg);
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TelegramUserViewModel>> FindUsersTelegramAsync(SimplePaginationRequestStandardModel req, CancellationToken token = default)
    {
        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        IQueryable<TelegramUserModelDb> query = identityContext.TelegramUsers
           .AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
        {
            string find_query = req.FindQuery.ToUpper();
            query = query.Where(x =>
            EF.Functions.Like(x.NormalizedFirstNameUpper, $"%{find_query.ToUpper()}%") ||
            (x.NormalizedUserNameUpper != null && EF.Functions.Like(x.NormalizedUserNameUpper, $"%{find_query.ToUpper()}%")) ||
            (x.NormalizedLastNameUpper != null && EF.Functions.Like(x.NormalizedLastNameUpper, $"%{find_query.ToUpper()}%")));
        }

        int total = query.Count();
        query = query.OrderBy(x => x.Id).Skip(req.PageNum * req.PageSize).Take(req.PageSize);

        TelegramUserModelDb[] users_tg = await query.ToArrayAsync(cancellationToken: token);
        if (users_tg.Length == 0)
            return new() { Response = [] };

        List<long> tg_users_ids = [.. users_tg.Select(y => y.TelegramId)];

        var users_identity_data = await identityContext.Users
            .Where(x => x.ChatTelegramId.HasValue && tg_users_ids.Contains(x.ChatTelegramId.Value))
            .Select(x => new { x.Id, x.Email, x.ChatTelegramId })
            .ToArrayAsync(cancellationToken: token);

        TelegramUserViewModel? identity_get(TelegramUserModelDb ctx)
        {
            var id_data = users_identity_data.FirstOrDefault(x => x.ChatTelegramId == ctx.TelegramId);

            if (id_data is null)
                return null;

            return TelegramUserViewModel.Build(ctx, id_data.Id, id_data?.Email);
        }

#pragma warning disable CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
        return new()
        {
            Response = users_tg.Select(identity_get).Where(x => x is not null).ToList(),
            TotalRowsCount = total,
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortBy = req.SortBy,
            SortingDirection = req.SortingDirection
        };
#pragma warning restore CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TelegramAccountRemoveTelegramJoinAsync(TelegramAccountRemoveJoinRequestTelegramModel req, CancellationToken token = default)
    {
        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        ApplicationUser? userIdentityDb = await identityContext.Users.FirstOrDefaultAsync(x => x.ChatTelegramId == req.TelegramId, cancellationToken: token);
        if (userIdentityDb is null)
            return ResponseBaseModel.CreateError($"Пользователь с таким TelegramId ({req.TelegramId}) не найден в БД");

        userIdentityDb.ChatTelegramId = null;
        identityContext.Update(userIdentityDb);
        await identityContext.SaveChangesAsync(token);

        if (MailAddress.TryCreate(userIdentityDb.Email, out _))
        {
            TResponseModel<TelegramUserBaseModel> tg_user_dump = await GetTelegramUserCachedInfoAsync(req.TelegramId, token);
            await mailRepo.SendEmailAsync(userIdentityDb.Email, "Удаление привязки Telegram к учётной записи", $"Telegram аккаунт {tg_user_dump.Response} отключён от вашей учётной записи на сайте.", token: token);
        }

        TelegramUserModelDb tg_user_info = await identityContext.TelegramUsers.FirstAsync(x => x.TelegramId == req.TelegramId, cancellationToken: token);

        await identityContext
            .UserClaims
            .Where(x => x.ClaimType == GlobalStaticConstants.TelegramIdClaimName && x.ClaimValue == userIdentityDb.ChatTelegramId.ToString())
            .ExecuteDeleteAsync(cancellationToken: token);

        TResponseModel<MessageComplexIdsModel> tgCall = await tgRemoteRepo.SendTextMessageTelegramAsync(new SendTextMessageTelegramBotModel()
        {
            Message = $"Ваш Telegram аккаунт отключён от учётной записи {userIdentityDb.Email} с сайта {req.ClearBaseUri}",
            UserTelegramId = tg_user_info.TelegramId,
            From = "уведомление",
        }, token: token);
        if (!tgCall.Success())
            loggerRepo.LogError(tgCall.Message());

        return ResponseBaseModel.CreateSuccess($"Пользователю {userIdentityDb.Email} удалена связь с TelegramId ${req.TelegramId}");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<TelegramUserBaseModel>> GetTelegramUserCachedInfoAsync(long telegramId, CancellationToken token = default)
    {
        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        TResponseModel<TelegramUserBaseModel> res = new() { Response = TelegramUserBaseModel.Build(await identityContext.TelegramUsers.FirstOrDefaultAsync(x => x.TelegramId == telegramId, cancellationToken: token)) };
        if (res.Response is null)
            res.AddInfo($"Пользователь Telegram #{telegramId} не найден в кешэ БД");

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateTelegramMainUserMessageAsync(MainUserMessageModel setMainUserMessage, CancellationToken token = default)
    {
        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        TelegramUserModelDb? user_db = await identityContext.TelegramUsers.FirstOrDefaultAsync(x => x.TelegramId == setMainUserMessage.UserId, cancellationToken: token);
        if (user_db is null)
            return ResponseBaseModel.CreateError($"Пользователь Telegram #{setMainUserMessage.UserId} не найден в БД");
        if (user_db.MainTelegramMessageId == setMainUserMessage.MessageId)
            return ResponseBaseModel.CreateInfo($"Изменения {user_db} не требуются. Идентификатор `{nameof(user_db.MainTelegramMessageId)}` #{setMainUserMessage.MessageId} уже установлен");

        user_db.MainTelegramMessageId = setMainUserMessage.MessageId;
        identityContext.Update(user_db);
        await identityContext.SaveChangesAsync(token);

        return ResponseBaseModel.CreateSuccess($"Успешно. Пользователю {user_db} установлен/обновлён идентификатор `{nameof(user_db.MainTelegramMessageId)}` set:{setMainUserMessage.MessageId}");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<TelegramJoinAccountModelDb>> TelegramJoinAccountStateAsync(TelegramJoinAccountStateRequestModel req, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        ApplicationUser? user = await userManager.FindByIdAsync(req.UserId); ;
        if (user is null)
            return TResponseModel<TelegramJoinAccountModelDb>.Build(ResponseBaseModel.CreateError($"Пользователь #{req.UserId} не найден"));

        DateTime lifeTime = DateTime.UtcNow.AddMinutes(-req.TelegramJoinAccountTokenLifetimeMinutes);

        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        TelegramJoinAccountModelDb? act = await identityContext.TelegramJoinActions
            .FirstOrDefaultAsync(x => x.CreatedAt > lifeTime && x.UserIdentityId == user.Id, cancellationToken: token);

        if (act is null)
            return TResponseModel<TelegramJoinAccountModelDb>.Build(ResponseBaseModel.CreateWarning("Токена нет"));

        if (req.EmailNotify)
        {
            if (MailAddress.TryCreate(user.Email, out _))
            {
                string msg;
                TResponseModel<UserTelegramBaseModel> bot_username_res = await tgRemoteRepo.AboutBotAsync(token);
                UserTelegramBaseModel? bot_username = bot_username_res.Response;

                if (bot_username is not null)
                {
                    msg = $"Существует ссылка привязки Telegram аккаунта к учётной записи сайта действительная до {act.CreatedAt.AddMinutes(req.TelegramJoinAccountTokenLifetimeMinutes)} ({DateTime.UtcNow - lifeTime}).<br/>";
                    msg += $"Нужно подтвердить операцию через Telegram бота. Для этого нужно в TelegramBot @{bot_username.Username} отправить токен:<br/><u><b>{act.GuidToken}</b></u><br/>Или ссылкой: <a href='https://t.me/{bot_username.Username}?start={act.GuidToken}'>https://t.me/{bot_username.Username}?start={act.GuidToken}</a><br/>";
                    await mailRepo.SendEmailAsync(user.Email, "Статус привязки Telegram к у/з", msg, token: token);
                }
                else
                    loggerRepo.LogError("Ошибка уведомления в Telegram: TelegramBot unknow. error 3CD8CA38-10CD-4FA5-B882-84572BC8A295");
            }
            else
                loggerRepo.LogError($"Ошибка уведомления на Email: {user.Email} - email не валидный. error BB9E05A4-37A3-4FBB-800B-9AED947A2B3B");
        }

        TResponseModel<TelegramJoinAccountModelDb> res = new() { Response = act };
        if (req.EmailNotify)
            res.AddWarning($"Проверьте свой ящик Email. Информация вам отправлена");

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<CheckTelegramUserAuthModel>> CheckTelegramUserAsync(CheckTelegramUserHandleModel user, CancellationToken token = default)
    {
        TResponseModel<CheckTelegramUserAuthModel> res = new();
        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        TelegramUserModelDb? tgUserDb = await identityContext.TelegramUsers.FirstOrDefaultAsync(x => x.TelegramId == user.TelegramUserId, cancellationToken: token);
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        if (tgUserDb is null)
        {
            using IDbContextTransaction transaction = identityContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            ApplicationUser app_user = new()
            {
                ChatTelegramId = user.TelegramUserId,
                EmailConfirmed = true,

                Email = $"tg.{user.TelegramUserId}@{GlobalStaticConstants.FakeHost}",
                NormalizedEmail = userManager.NormalizeEmail($"tg.{user.TelegramUserId}@{GlobalStaticConstants.FakeHost}"),

                UserName = $"tg.{user.TelegramUserId}@{GlobalStaticConstants.FakeHost}",
                NormalizedUserName = userManager.NormalizeEmail($"tg.{user.TelegramUserId}@{GlobalStaticConstants.FakeHost}"),

                FirstName = user.FirstName,
                NormalizedFirstNameUpper = user.FirstName.ToUpper(),
                LastName = user.LastName,
                NormalizedLastNameUpper = user.LastName?.ToUpper(),
            };

            await identityContext.AddAsync(app_user, token);
            await identityContext.SaveChangesAsync(token);

            tgUserDb = TelegramUserModelDb.Build(user, app_user.Id);
            await identityContext.AddAsync(tgUserDb, token);
            await identityContext.SaveChangesAsync(token);

            await transaction.CommitAsync(token);
        }
        else
        {
            tgUserDb!.FirstName = user.FirstName;
            tgUserDb.NormalizedFirstNameUpper = user.FirstName.ToUpper();

            tgUserDb.LastName = user.LastName;
            tgUserDb.NormalizedLastNameUpper = user.LastName?.ToUpper();

            tgUserDb.Username = user.Username ?? "";
            tgUserDb.NormalizedUserNameUpper = user.Username?.ToUpper();

            tgUserDb.FirstName = user.FirstName;
            tgUserDb.NormalizedFirstNameUpper = user.FirstName.ToUpper();
            tgUserDb.LastName = user.LastName;
            tgUserDb.NormalizedLastNameUpper = user.LastName?.ToUpper();

            tgUserDb.TelegramId = user.TelegramUserId;
            tgUserDb.IsBot = user.IsBot;
            identityContext.Update(tgUserDb);
            await identityContext.SaveChangesAsync(token);
        }

        ApplicationUser? appUserDb = await identityContext.Users.FirstOrDefaultAsync(x => x.ChatTelegramId == user.TelegramUserId, cancellationToken: token);

        if (appUserDb is not null)
        {
            res.Response = new()
            {
                FirstName = tgUserDb.FirstName,
                LastName = tgUserDb.LastName,
                UserIdentityId = appUserDb.Id,
                TwoFactorEnabled = appUserDb.TwoFactorEnabled,
                UserEmail = appUserDb.Email,
                Username = tgUserDb.Username,
                TelegramId = user.TelegramUserId,
                MainTelegramMessageId = tgUserDb.MainTelegramMessageId,
                AccessFailedCount = appUserDb.AccessFailedCount,
                EmailConfirmed = appUserDb.EmailConfirmed,
                IsBot = user.IsBot,
                LockoutEnd = appUserDb.LockoutEnd,
                PhoneNumber = appUserDb.PhoneNumber,
                PhoneNumberConfirmed = appUserDb.PhoneNumberConfirmed,
                LockoutEnabled = appUserDb.LockoutEnabled,
            };

            if (tgUserDb.UserIdentityId != appUserDb.Id)
            {
                await identityContext
                    .TelegramUsers
                    .Where(x => x.Id == tgUserDb.Id)
                    .ExecuteUpdateAsync(set => set.SetProperty(p => p.UserIdentityId, appUserDb.Id), cancellationToken: token);
            }

            await ClaimsUserFlushAsync(appUserDb.Id, token);

        }
        else
            res.AddWarning("Пользователь Telegram не связан с учётной записью на сайте");

        return res;
    }
    #endregion

    #region roles
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TryAddRolesToUserAsync(UserRolesModel req, CancellationToken token = default)
    {
        req.RolesNames = [.. req.RolesNames
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .DistinctBy(x => x.ToLower())];

        if (req.RolesNames.Count == 0)
            return ResponseBaseModel.CreateError("Не указаны роли для добавления");

        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser? user = await userManager.FindByIdAsync(req.UserId); ;
        if (user is null)
            return ResponseBaseModel.CreateError($"Пользователь #{req.UserId} не найден");

        string[] roles_for_add_normalized = req.RolesNames.Select(r => userManager.NormalizeName(r)).ToArray();

        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);

        // роли, которые есть в БД
        string?[] roles_that_are_in_db = await identityContext.Roles
            .Where(x => roles_for_add_normalized.Contains(x.NormalizedName))
            .Select(x => x.Name)
            .ToArrayAsync(cancellationToken: token);

        // роли, которых не хватает в бд
        string[] roles_that_need_add_in_db = [.. req.RolesNames.Where(x => !roles_that_are_in_db.Any(y => y?.Equals(x, StringComparison.OrdinalIgnoreCase) == true))];

        if (roles_that_need_add_in_db.Length != 0)
        {
            loggerRepo.LogWarning($"Созданы новые роли: {JsonConvert.SerializeObject(roles_that_need_add_in_db, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            await identityContext
                .AddRangeAsync(roles_that_need_add_in_db.Select(r => new ApplicationRole() { Name = r, Title = r, NormalizedName = userManager.NormalizeName(r) }), token);
            await identityContext.SaveChangesAsync(token);
        }

        IList<string> user_roles = await userManager.GetRolesAsync(user);

        // роли, которые требуется добавить пользователю
        roles_that_need_add_in_db = [.. roles_for_add_normalized.Where(x => !user_roles.Any(y => y.Equals(x, StringComparison.OrdinalIgnoreCase) == true))];

        if (roles_that_need_add_in_db.Length != 0)
        { // добавляем пользователю ролей
            loggerRepo.LogWarning($"Добавление ролей пользователю `{req.UserId}`: {JsonConvert.SerializeObject(roles_that_need_add_in_db, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            roles_that_need_add_in_db = await identityContext
                .Roles
                .Where(x => roles_that_need_add_in_db.Contains(x.NormalizedName))
                .Select(x => x.Id)
                .ToArrayAsync(cancellationToken: token);

            await identityContext.AddRangeAsync(roles_that_need_add_in_db.Select(x => new IdentityUserRole<string>() { RoleId = x, UserId = req.UserId }), token);
            await identityContext.SaveChangesAsync(token);
        }
        return ResponseBaseModel.CreateSuccess($"Добавлено {roles_that_need_add_in_db.Length} ролей пользователю");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<string[]>> SetRoleForUserAsync(TAuthRequestStandardModel<SetRoleForUserRequestModel> req, CancellationToken token = default)
    {
        TResponseModel<string[]> res = new();
        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }

        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);

        IQueryable<ApplicationRole> q = identityContext
            .UserRoles
            .Where(x => x.UserId == req.Payload.UserIdentityId)
            .Join(identityContext.Roles, jr => jr.RoleId, r => r.Id, (jr, r) => r)
            .AsQueryable();

        ApplicationRole[] roles = await q
            .ToArrayAsync(cancellationToken: token);

        ApplicationRole? role_bd;
        if (req.Payload.Command && !roles.Any(x => x.Name?.Contains(req.Payload.RoleName, StringComparison.OrdinalIgnoreCase) == true))
        {
            role_bd = await identityContext
                .Roles
                .FirstOrDefaultAsync(x => x.NormalizedName == req.Payload.RoleName.ToUpper(), cancellationToken: token);

            if (role_bd is null)
            {
                role_bd = new ApplicationRole()
                {
                    NormalizedName = req.Payload.RoleName.ToUpper(),
                    Name = req.Payload.RoleName,
                };
                await identityContext.AddAsync(role_bd, token);
                await identityContext.SaveChangesAsync(token);
            }
            await identityContext.AddAsync(new IdentityUserRole<string>() { RoleId = role_bd.Id, UserId = req.Payload.UserIdentityId }, token);
            await identityContext.SaveChangesAsync(token);
            res.Response = [.. roles.Select(x => x.Name).Union([req.Payload.RoleName]).Where(x => !string.IsNullOrWhiteSpace(x))!];
            res.AddSuccess($"Включён в роль: {role_bd.Name}");
        }
        else if (!req.Payload.Command && roles.Any(x => x.Name?.Contains(req.Payload.RoleName, StringComparison.OrdinalIgnoreCase) == true))
        {
            role_bd = roles.First(x => x.Name?.Contains(req.Payload.RoleName, StringComparison.OrdinalIgnoreCase) == true);
            identityContext.Remove(role_bd);
            await identityContext.SaveChangesAsync(token);
            res.Response = [.. roles.Select(x => x.Name).Where(x => !string.IsNullOrWhiteSpace(x) && !x.Equals(req.Payload.RoleName, StringComparison.OrdinalIgnoreCase))!];
            res.AddSuccess($"Исключён из роли: {req.Payload.RoleName}");
        }
        else
        {
            res.AddInfo("Изменения не требуются");
            res.Response = [.. roles.Select(x => x.Name).Where(x => !string.IsNullOrWhiteSpace(x))!];
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<RoleInfoModel>> GetRoleAsync(string role_id, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using RoleManager<ApplicationRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        ApplicationRole? role_db = await roleManager.FindByIdAsync(role_id);
        if (role_db is null)
            return new() { Messages = ResponseBaseModel.ErrorMessage($"Роль #{role_id} не найдена в БД") };
        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        return new()
        {
            Response = new RoleInfoModel()
            {
                Id = role_id,
                Name = role_db.Name,
                Title = role_db.Title,
                UsersCount = await identityContext.UserRoles.CountAsync(x => x.RoleId == role_id, cancellationToken: token)
            }
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<RoleInfoModel>> FindRolesAsync(FindWithOwnedRequestModel req, CancellationToken token = default)
    {
        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        IQueryable<ApplicationRole> q = identityContext.Roles
           .AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.OwnerId))
            q = q.Where(x => identityContext.UserRoles.Any(y => x.Id == y.RoleId && req.OwnerId == y.UserId));
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using RoleManager<ApplicationRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => EF.Functions.Like(x.NormalizedName, $"%{roleManager.KeyNormalizer.NormalizeName(req.FindQuery)}%") || x.Id == req.FindQuery);

        int total = q.Count();
        q = q.OrderBy(x => x.Name).Skip(req.PageNum * req.PageSize).Take(req.PageSize);
        var roles = await
            q.Select(x => new
            {
                x.Id,
                x.Name,
                x.Title,
                UsersCount = identityContext.UserRoles.Count(z => z.RoleId == x.Id)
            })
            .ToArrayAsync(cancellationToken: token);

        return new()
        {
            Response = roles.Select(x => new RoleInfoModel() { Id = x.Id, Name = x.Name, Title = x.Title, UsersCount = x.UsersCount }).ToList(),
            TotalRowsCount = total,
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortBy = req.SortBy,
            SortingDirection = req.SortingDirection
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CreateNewRoleAsync(string role_name, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using RoleManager<ApplicationRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        role_name = role_name.Trim();
        if (string.IsNullOrEmpty(role_name))
            return ResponseBaseModel.CreateError("Не указано имя роли");
        ApplicationRole? role_db = await roleManager.FindByNameAsync(role_name);
        if (role_db is not null)
            return ResponseBaseModel.CreateWarning($"Роль '{role_db.Name}' уже существует");

        role_db = new ApplicationRole(role_name);
        IdentityResult ir = await roleManager.CreateAsync(role_db);

        if (ir.Succeeded)
            return ResponseBaseModel.CreateSuccess($"Роль '{role_name}' успешно создана");

        return new()
        {
            Messages = ir.Errors.Select(x => new ResultMessage() { TypeMessage = MessagesTypesEnum.Error, Text = $"[{x.Code}: {x.Description}]" }).ToList()
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteRoleAsync(string role_name, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using RoleManager<ApplicationRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        ApplicationRole? role_db = await roleManager.FindByNameAsync(role_name);
        if (role_db is null)
            return ResponseBaseModel.CreateError($"Роль #{role_name} не найдена в БД");

        using IdentityAppDbContext identityContext = await identityDbFactory.CreateDbContextAsync(token);
        var users_linked =
           await (from link in identityContext.UserRoles.Where(x => x.RoleId == role_db.Id)
                  join user in identityContext.Users on link.UserId equals user.Id
                  select new { user.Id, user.Email }).ToArrayAsync(cancellationToken: token);

        if (users_linked.Length != 0)
            return ResponseBaseModel.CreateError($"Роль '{role_db.Name}' нельзя удалить! Предварительно исключите из неё пользователей: {string.Join("; ", users_linked.Select(x => $"[{x.Email}]"))};");

        IdentityResult ir = await roleManager.DeleteAsync(role_db);

        if (ir.Succeeded)
            ResponseBaseModel.CreateSuccess($"Роль '{role_db.Name}' успешно удалена!");

        return new()
        {
            Messages = ir.Errors.Select(x => new ResultMessage() { TypeMessage = MessagesTypesEnum.Error, Text = $"[{x.Code}: {x.Description}]" }).ToList()
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteRoleFromUserAsync(TAuthRequestStandardModel<RoleEmailModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        using IServiceScope scope = serviceScopeFactory.CreateScope();
        using RoleManager<ApplicationRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        ApplicationRole? role_db = await roleManager.FindByNameAsync(req.Payload.RoleName);
        if (role_db is null)
            return ResponseBaseModel.CreateError($"Роль с именем '{req.Payload.RoleName}' не найдена в БД");

        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        ApplicationUser? user_db = await userManager.FindByEmailAsync(req.Payload.Email);
        if (user_db is null)
            return ResponseBaseModel.CreateError($"Пользователь `{req.Payload.Email}` не найден в БД");

        if (!await userManager.IsInRoleAsync(user_db, req.Payload.RoleName))
            return ResponseBaseModel.CreateWarning($"Роль '{req.Payload.RoleName}' у пользователя '{req.Payload.Email}' отсутствует.");

        IdentityResult ir = await userManager.RemoveFromRoleAsync(user_db, req.Payload.RoleName);

        if (ir.Succeeded)
            return ResponseBaseModel.CreateSuccess($"Пользователь '{req.Payload.Email}' исключён из роли '{req.Payload.RoleName}'");

        return new()
        {
            Messages = ir.Errors.Select(x => new ResultMessage() { TypeMessage = MessagesTypesEnum.Error, Text = $"[{x.Code}: {x.Description}]" }).ToList()
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> AddRoleToUserAsync(RoleEmailModel req, CancellationToken token = default)
    {
        using IServiceScope scope = serviceScopeFactory.CreateAsyncScope();
        using RoleManager<ApplicationRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        ApplicationRole? role_db = await roleManager.FindByNameAsync(req.RoleName);
        if (role_db is null)
            return ResponseBaseModel.CreateError($"Роль с именем '{req.RoleName}' не найдена в БД");

        using UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
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
            Messages = ir.Errors.Select(x => new ResultMessage() { TypeMessage = MessagesTypesEnum.Error, Text = $"[{x.Code}: {x.Description}]" }).ToList()
        };
    }
    #endregion
}