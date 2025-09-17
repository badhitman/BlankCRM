////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using IdentityLib;
using SharedLib;

namespace ServerLib;

/// <summary>
/// Сервис работы с аутентификацией пользователей
/// </summary>
public class UsersAuthenticateService(
    ILogger<UsersAuthenticateService> loggerRepo,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IParametersStorageTransmission StorageTransmissionRepo,
    IIdentityTransmission identityRepo,
    IHttpContextAccessor httpContextAccessor,
    IOptions<UserManageConfigModel> userManageConfig) : IUsersAuthenticateService
{
    UserManageConfigModel UserConfMan => userManageConfig.Value;

    /// <inheritdoc/>
    public async Task<IdentityResultResponseModel> TwoFactorAuthenticatorSignInAsync(string code, bool isPersistent, bool rememberClient, string? userAlias = null, CancellationToken token = default)
    {
        string authenticatorCode = code.Replace(" ", string.Empty).Replace("-", string.Empty);

        if (await signInManager.GetTwoFactorAuthenticationUserAsync() is not null)
        {
            SignInResult result = await signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, isPersistent, rememberClient);

            return new()
            {
                IsLockedOut = result.IsLockedOut,
                IsNotAllowed = result.IsNotAllowed,
                RequiresTwoFactor = result.RequiresTwoFactor,
                Succeeded = result.Succeeded,
            };
        }
        else if (!string.IsNullOrWhiteSpace(userAlias))
        {
            TResponseModel<string> checkToken = await identityRepo.CheckToken2FAAsync(new() { Token = authenticatorCode, UserAlias = userAlias }, token);
            if (checkToken.Success() && !string.IsNullOrWhiteSpace(checkToken.Response))
            {
                ApplicationUser? user = await userManager.FindByIdAsync(checkToken.Response);
                if (user is null)
                {
                    return new()
                    {
                        Succeeded = false,
                        Messages = [new() { Text = $"Пользователь {checkToken.Response} не найден", TypeMessage = MessagesTypesEnum.Error }]
                    };
                }

                bool _isLockedOut = default!, isEmailConfirmed = default!;
                await Task.WhenAll([
                    Task.Run(async () => { _isLockedOut = await userManager.IsLockedOutAsync(user); }, token),
                    Task.Run(async () => { isEmailConfirmed = await userManager.IsEmailConfirmedAsync(user); }, token)
                ]);

                if (_isLockedOut)
                    return new()
                    {
                        IsLockedOut = true,
                        Succeeded = false,
                        Messages = [new() { Text = "Пользователь заблокирован", TypeMessage = MessagesTypesEnum.Error }]
                    };

                if (!isEmailConfirmed)
                    return new()
                    {
                        IsNotAllowed = true,
                        Succeeded = false,
                        Messages = [new() { Text = "Email пользователя не подтверждён", TypeMessage = MessagesTypesEnum.Error }]
                    };

                await SignInAsync(checkToken.Response, isPersistent, token);
                return new()
                {
                    Succeeded = true,
                    Messages = [new() { TypeMessage = MessagesTypesEnum.Success, Text = "Проверка пройдена успешно" }]
                };
            }
        }

        return new()
        {
            Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Ошибка" }]
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel?>> GetTwoFactorAuthenticationUserAsync(CancellationToken token = default)
    {
        ApplicationUser? au = await signInManager.GetTwoFactorAuthenticationUserAsync();
        if (au is null)
            return new() { Messages = ResponseBaseModel.ErrorMessage("ApplicationUser is null. error {586ED8B1-1905-472E-AB1C-69AFF0A6A191}") };

        return new()
        {
            Response = UserInfoModel.Build(
            userId: au.Id,
            userName: au.UserName,
            email: au.Email,
            phoneNumber: au.PhoneNumber,
            telegramId: au.ChatTelegramId,
            emailConfirmed: au.EmailConfirmed,
            lockoutEnd: au.LockoutEnd,
            lockoutEnabled: au.LockoutEnabled,
            accessFailedCount: au.AccessFailedCount,
            firstName: au.FirstName,
            lastName: au.LastName)
        };
    }

    /// <inheritdoc/>
    public async Task<IdentityResultResponseModel> TwoFactorRecoveryCodeSignInAsync(string recoveryCode, CancellationToken token = default)
    {
        string msg;
        ApplicationUser? user = await signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user is null)
        {
            msg = "GetTwoFactorAuthenticationUser is null. error {4B4A5EDA-05E9-45BF-B283-4130394BF05E}";
            loggerRepo.LogError(msg);
            return (IdentityResultResponseModel)ResponseBaseModel.CreateError(msg);
        }
        SignInResult result = await signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

        if (result.Succeeded)
        {
            msg = $"Пользователь с идентификатором '{user.Id}' вошел в систему с кодом восстановления.";
            loggerRepo.LogInformation(msg);
            return new() { };
        }
        else if (result.IsLockedOut)
        {
            msg = $"Учетная запись пользователя #'{user.Id}' заблокирована.";
            loggerRepo.LogError(msg);
            return new()
            {
                IsLockedOut = result.IsLockedOut,
                Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = msg }],
                IsNotAllowed = result.IsNotAllowed,
                RequiresTwoFactor = result.RequiresTwoFactor,
                Succeeded = result.Succeeded,
            };
        }
        else
        {
            msg = $"Для пользователя с идентификатором введен неверный код восстановления. '{user.Id}'";
            loggerRepo.LogWarning(msg);
            return (IdentityResultResponseModel)ResponseBaseModel.CreateError(msg);
        }
    }

    /// <inheritdoc/>
    public async Task<UserLoginInfoResponseModel> GetExternalLoginInfoAsync(string? expectedXsrf = null, CancellationToken token = default)
    {
        ExternalLoginInfo? info = await signInManager.GetExternalLoginInfoAsync(expectedXsrf);

        return new()
        {
            IdentityName = info?.Principal.Identity?.Name,
            UserLoginInfoData = info is null ? null : new UserLoginInfoModel(info.LoginProvider, info.ProviderKey, info.ProviderDisplayName),
            Messages = [info is null ? new() { Text = "`ExternalLoginInfo` is null. error {89E9E6CA-C9AB-4CB5-8972-681E675381F6}", TypeMessage = MessagesTypesEnum.Error } : new() { Text = "login information, source and externally source principal for a user record", TypeMessage = MessagesTypesEnum.Success }]
        };
    }

    /// <inheritdoc/>
    public async Task<ExternalLoginSignInResponseModel> ExternalLoginSignInAsync(string loginProvider, string providerKey, string? identityName, bool isPersistent = false, bool bypassTwoFactor = true, CancellationToken token = default)
    {
        // Sign in the user with this external login provider if the user already has a login.
        SignInResult result = await signInManager.ExternalLoginSignInAsync(
            loginProvider,
            providerKey,
            isPersistent: isPersistent,
            bypassTwoFactor: bypassTwoFactor);

        ExternalLoginSignInResponseModel res = new()
        {
            IsLockedOut = result.IsLockedOut,
            IsNotAllowed = result.IsNotAllowed,
            RequiresTwoFactor = result.RequiresTwoFactor,
            Succeeded = result.Succeeded
        };
        if (result.Succeeded)
        {
            loggerRepo.LogInformation(
                "{Name} logged in with {LoginProvider} provider.",
                identityName,
                loginProvider);
            return res;
        }
        else if (result.IsLockedOut)
        {
            return res;
        }

        ExternalLoginInfo? externalLoginInfo = await signInManager.GetExternalLoginInfoAsync();
        if (externalLoginInfo is null)
        {
            res.AddError("Ошибка загрузки внешних данных для входа.");
            return res;
        }

        if (externalLoginInfo.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
        {
            res.Email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
        }
        return res;
    }

    /// <inheritdoc/>
    public async Task<RegistrationNewUserResponseModel> RegisterNewUserAsync(RegisterNewUserPasswordModel req, CancellationToken token = default)
    {
        if (!UserConfMan.UserRegistrationIsAllowed(req.Email))
            return new() { Messages = [new() { Text = $"Registration error {UserConfMan.DenyAuthorization?.Message}", TypeMessage = MessagesTypesEnum.Error }] };

        RegistrationNewUserResponseModel regUserRes = await identityRepo.CreateNewUserWithPasswordAsync(req, token);
        if (!regUserRes.Success() || string.IsNullOrWhiteSpace(regUserRes.Response) || regUserRes.RequireConfirmedEmail == true)
            return regUserRes;

        ApplicationUser? user = await userManager.FindByIdAsync(regUserRes.Response);
        if (user is null)
        {
            regUserRes.AddError("Created user not found");
            return regUserRes;
        }

        // await signInManager.SignInAsync(user, isPersistent: false);
        return regUserRes;
    }

    /// <inheritdoc/>
    public async Task<RegistrationNewUserResponseModel> ExternalRegisterNewUserAsync(string userEmail, string baseAddress, CancellationToken token = default)
    {
        ExternalLoginInfo? externalLoginInfo = await signInManager.GetExternalLoginInfoAsync();
        if (externalLoginInfo == null)
            return (RegistrationNewUserResponseModel)ResponseBaseModel.CreateError("externalLoginInfo == null. error {D991FA4A-9566-4DD4-B23A-DEB497931FF5}");

        RegistrationNewUserResponseModel regUserRes = await identityRepo.CreateNewUserAsync(userEmail, token);
        if (!regUserRes.Success() || string.IsNullOrWhiteSpace(regUserRes.Response))
            return regUserRes;

        ApplicationUser? user = await userManager.FindByIdAsync(regUserRes.Response);
        if (user is null)
        {
            regUserRes.AddError("Созданный пользователь не найден");
            return regUserRes;
        }

        IdentityResult result = await userManager.AddLoginAsync(user, externalLoginInfo);
        if (result.Succeeded)
        {
            loggerRepo.LogInformation("Пользователь создал учетную запись с помощью провайдера {Name}.", externalLoginInfo.LoginProvider);

            ResponseBaseModel genConfirm = await identityRepo.GenerateEmailConfirmationAsync(new() { BaseAddress = baseAddress, Email = userEmail }, token);
            if (!genConfirm.Success() || regUserRes.RequireConfirmedAccount == true)
            {
                regUserRes.AddRangeMessages(genConfirm.Messages);
                return regUserRes;
            }

            await signInManager.SignInAsync(user, isPersistent: false, externalLoginInfo.LoginProvider);
            return (RegistrationNewUserResponseModel)ResponseBaseModel.CreateSuccess("Вход выполнен");
        }

        return new()
        {
            Messages = result.Errors.Select(x => new ResultMessage()
            {
                TypeMessage = MessagesTypesEnum.Error,
                Text = $"[{x.Code}: {x.Description}]"
            }).ToList()
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SignInAsync(string userId, bool isPersistent, CancellationToken token = default)
    {
        ApplicationUser? user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return ResponseBaseModel.CreateError("Пользователь не найден");

        await signInManager.SignInAsync(user, isPersistent: false);
        return ResponseBaseModel.CreateSuccess("Пользователь авторизован");
    }

    /// <inheritdoc/>
    public async Task<SignInResultResponseModel> PasswordSignInAsync(string userEmail, string password, bool isPersistent, CancellationToken token = default)
    {
        if (!UserConfMan.UserAuthorizationIsAllowed(userEmail))
            return new() { Messages = [new() { Text = $"Ошибка авторизации {UserConfMan.DenyAuthorization?.Message}", TypeMessage = MessagesTypesEnum.Error }] };

        SignInResultResponseModel res = new();
        ApplicationUser? currentAppUser = await userManager.FindByEmailAsync(userEmail);
        if (currentAppUser is null)
            return new SignInResultResponseModel() { Succeeded = false, Messages = ResponseBaseModel.CreateError($"current user by email '{userEmail}' is null. error {{A19FC284-C437-4CC6-A7D2-C96FC6F6A42F}}").Messages };

        TResponseModel<bool?> globalEnable2FA = await StorageTransmissionRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.GlobalEnable2FA, token);
        if (globalEnable2FA.Response == true)
        {
            ResponseBaseModel chkUserPass = await identityRepo.CheckUserPasswordAsync(new() { Password = password, UserId = currentAppUser.Id }, token);

            if (!chkUserPass.Success())
                return new() { Messages = chkUserPass.Messages };

            TResponseModel<string> otp = await identityRepo.GenerateToken2FAAsync(currentAppUser.Id, token);
            res.RequiresTwoFactor = true;
            res.UserId = otp.Response;
            return res;
        }

        _ = await identityRepo.ClaimsUserFlushAsync(currentAppUser.Id, token);
        FlushUserRolesModel? user_flush = userManageConfig.Value.UpdatesUsersRoles?.FirstOrDefault(x => x.EmailUser.Equals(userEmail, StringComparison.OrdinalIgnoreCase));
        if (user_flush is not null)
        {
            _ = await identityRepo.TryAddRolesToUserAsync(new() { RolesNames = user_flush.SetRoles, UserId = currentAppUser.Id }, token);
        }

        SignInResult sr = await signInManager.PasswordSignInAsync(userEmail, password, isPersistent, lockoutOnFailure: true);

        if (!sr.Succeeded)
        {
            if (sr.RequiresTwoFactor)
            {
                res.AddError("Error: RequiresTwoFactor");
                TResponseModel<string> otp = await identityRepo.GenerateToken2FAAsync(currentAppUser.Id, token);
                return new() { RequiresTwoFactor = true, Messages = res.Messages, UserId = otp.Response };
            }

            if (sr.IsLockedOut)
                res.AddError("Ошибка: Учетная запись пользователя заблокирована.");

            if (res.Messages.Count == 0)
                res.AddError("user login error {7D55A217-6074-4988-8774-74F995F70D18}");

            return res;
        }

        return new()
        {
            IsLockedOut = sr.IsLockedOut,
            IsNotAllowed = sr.IsNotAllowed,
            RequiresTwoFactor = sr.RequiresTwoFactor,
            Succeeded = sr.Succeeded,
            UserId = currentAppUser.Id,
        };
    }

    /// <inheritdoc/>
    public async Task SignOutAsync(CancellationToken token = default)
    {
        if (httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true)
            await signInManager.SignOutAsync();
    }
}