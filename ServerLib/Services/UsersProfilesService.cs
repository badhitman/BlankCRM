////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Net.Mail;
using IdentityLib;
using SharedLib;

namespace ServerLib;

/// <summary>
/// Сервис работы с профилями пользователей
/// </summary>
public class UsersProfilesService(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    IIdentityTransmission IdentityRepo,
    IHttpContextAccessor httpContextAccessor,
    ILogger<UsersProfilesService> LoggerRepo) : IUsersProfilesService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<IEnumerable<UserLoginInfoModel>>> GetUserLoginsAsync(string? userId = null, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new() { Messages = user.Messages };

        return await IdentityRepo.GetUserLoginsAsync(user.ApplicationUser.Id, token);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CheckUserPasswordAsync(string password, string? userId = null, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new() { Messages = user.Messages };

        return await IdentityRepo.CheckUserPasswordAsync(new() { Password = password, UserId = user.ApplicationUser.Id }, token);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteUserDataAsync(string password, string? userId = null, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new() { Messages = user.Messages };

        return await IdentityRepo.DeleteUserDataAsync(new() { Password = password, UserId = user.ApplicationUser.Id }, token);
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<bool?>> UserHasPasswordAsync(string? userId = null, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new TResponseModel<bool?>() { Messages = user.Messages };

        return await IdentityRepo.UserHasPasswordAsync(user.ApplicationUser.Id, token);
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<bool?>> GetTwoFactorEnabledAsync(string? userId = null, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new() { Messages = user.Messages };

        return await IdentityRepo.GetTwoFactorEnabledAsync(user.ApplicationUser.Id, token);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetTwoFactorEnabledAsync(bool enabled_set, string? userId = null, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new() { Messages = user.Messages };

        return await IdentityRepo.SetTwoFactorEnabledAsync(new() { UserId = user.ApplicationUser.Id, EnabledSet = enabled_set }, token);
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> IsEmailConfirmedAsync(string? userId = null, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new() { Messages = user.Messages };

        return new()
        {
            Response = user.ApplicationUser.EmailConfirmed,
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ResetAuthenticatorKeyAsync(string? userId = null, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new() { Messages = user.Messages };

        ResponseBaseModel res = await IdentityRepo.ResetAuthenticatorKeyAsync(user.ApplicationUser.Id, token);
        if (!res.Success())
            return res;

        string msg = $"Пользователь с идентификатором '{userId}' сбросил ключ приложения для аутентификации.";
        LoggerRepo.LogInformation(msg);
        await signInManager.RefreshSignInAsync(user.ApplicationUser);
        return ResponseBaseModel.CreateSuccess(msg);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> AddLoginAsync(string? userId = null, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new() { Messages = user.Messages };

        ExternalLoginInfo? info = await signInManager.GetExternalLoginInfoAsync(user.ApplicationUser.Id);
        if (info is null)
            return ResponseBaseModel.CreateError("ExternalLoginInfo is null. error {6EFD4D81-8E30-472D-8356-3CF287639792}");

        IdentityResult result = await userManager.AddLoginAsync(user.ApplicationUser, info);
        if (!result.Succeeded)
            return ResponseBaseModel.CreateError("Ошибка: внешний логин не был добавлен. Внешние логины могут быть связаны только с одной учетной записью.");

        return ResponseBaseModel.CreateSuccess("Login is added");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> RemoveLoginAsync(string loginProvider, string providerKey, string? userId = null, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new() { Messages = user.Messages };

        ResponseBaseModel res = await IdentityRepo.RemoveLoginAsync(new() { LoginProvider = loginProvider, ProviderKey = providerKey, UserId = user.ApplicationUser.Id }, token);
        if (!res.Success())
            return res;

        await signInManager.RefreshSignInAsync(user.ApplicationUser);
        return ResponseBaseModel.CreateSuccess("Успешно удалено");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> VerifyTwoFactorTokenAsync(string verificationCode, string? userId = null, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new() { Messages = user.Messages };

        return await IdentityRepo.VerifyTwoFactorTokenAsync(new() { UserId = user.ApplicationUser.Id, VerificationCode = verificationCode }, token);
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int?>> CountRecoveryCodesAsync(string? userId = null, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new() { Messages = user.Messages };

        return await IdentityRepo.CountRecoveryCodesAsync(user.ApplicationUser.Id, token);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> GenerateChangeEmailTokenAsync(string newEmail, string baseAddress, string? userId = null, CancellationToken token = default)
    {
        if (!MailAddress.TryCreate(newEmail, out _))
            return ResponseBaseModel.CreateError($"Адрес e-mail `{newEmail}` имеет не корректный формат");

        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new() { Messages = user.Messages };

        return await IdentityRepo.GenerateChangeEmailTokenAsync(new() { BaseAddress = baseAddress, NewEmail = newEmail, UserId = user.ApplicationUser.Id }, token);
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<IEnumerable<string>?>> GenerateNewTwoFactorRecoveryCodesAsync(string? userId = null, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new() { Messages = user.Messages };

        return await IdentityRepo.GenerateNewTwoFactorRecoveryCodesAsync(new() { UserId = user.ApplicationUser.Id, Number = 10 }, token);
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<string?>> GetAuthenticatorKeyAsync(string? userId = null, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new() { Messages = user.Messages };

        return await IdentityRepo.GetAuthenticatorKeyAsync(user.ApplicationUser.Id, token);
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<string?>> GeneratePasswordResetTokenAsync(string? userId = null, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new() { Messages = user.Messages };

        return await IdentityRepo.GeneratePasswordResetTokenAsync(user.ApplicationUser.Id, token);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TryAddRolesToUserAsync(IEnumerable<string> addRoles, string? userId = null, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new() { Messages = user.Messages };

        return await IdentityRepo.TryAddRolesToUserAsync(new() { RolesNames = [.. addRoles], UserId = user.ApplicationUser.Id }, token);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ChangePasswordAsync(string currentPassword, string newPassword, string? userId = null, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new() { Messages = user.Messages };

        ResponseBaseModel res = await IdentityRepo.ChangePasswordAsync(new() { CurrentPassword = currentPassword, NewPassword = newPassword, UserId = user.ApplicationUser.Id }, token);
        if (!res.Success())
            return res;

        await signInManager.RefreshSignInAsync(user.ApplicationUser);
        return ResponseBaseModel.CreateSuccess($"Пользователю [`{user.ApplicationUser.Id}`/`{user.ApplicationUser.Email}`] успешно изменён пароль.");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> AddPasswordAsync(string password, string? userId = null, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(userId, token);
        if (!user.Success() || user.ApplicationUser is null)
            return new() { Messages = user.Messages };

        ResponseBaseModel addPassRes = await IdentityRepo.AddPasswordAsync(new() { Password = password, UserId = user.ApplicationUser.Id }, token);
        if (!addPassRes.Success())
            return addPassRes;

        await signInManager.RefreshSignInAsync(user.ApplicationUser);
        return ResponseBaseModel.CreateSuccess("Пароль установлен");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ChangeEmailAsync(IdentityEmailTokenModel req, CancellationToken token = default)
    {
        ApplicationUserResponseModel user = await GetUser(req.UserId, token); ;
        if (!user.Success() || user.ApplicationUser is null)
            return ResponseBaseModel.CreateError($"Пользователь #{req.UserId} не найден");

        ResponseBaseModel changeRes = await IdentityRepo.ChangeEmailAsync(req, token);
        if (!changeRes.Success())
            return changeRes;

        await signInManager.RefreshSignInAsync(user.ApplicationUser);
        return ResponseBaseModel.CreateSuccess("Благодарим вас за подтверждение изменения адреса электронной почты.");
    }


    /// <summary>
    /// Read Identity user data.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public async Task<ApplicationUserResponseModel> GetUser(string? userId = null, CancellationToken token = default)
    {
        ApplicationUser? user;

        string msg;
        if (string.IsNullOrWhiteSpace(userId))
        {
            LoggerRepo.LogInformation($"IsAuthenticated:{httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated}");
            LoggerRepo.LogInformation($"Name:{httpContextAccessor.HttpContext?.User.Identity?.Name}");
            if (httpContextAccessor.HttpContext is not null)
                LoggerRepo.LogInformation($"Claims:{string.Join(",", httpContextAccessor.HttpContext.User.Claims.Select(x => $"[{x.ValueType}:{x.Value}]"))}");

            string? user_id = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user_id is null)
            {
                msg = "HttpContext is null (текущий пользователь) не авторизован. info D485BA3C-081C-4E2F-954D-759A181DCE78";
                return new() { Messages = [new ResultMessage() { TypeMessage = ResultTypesEnum.Info, Text = msg }] };
            }
            else
            {
                user = await userManager.FindByIdAsync(user_id);
                return new()
                {
                    ApplicationUser = user
                };
            }
        }
        user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            msg = $"Identity user ({nameof(userId)}: `{userId}`) не найден. error {{9D6C3816-7A39-424F-8EF1-B86732D46BD7}}";
            return (ApplicationUserResponseModel)ResponseBaseModel.CreateError(msg);
        }
        return new()
        {
            ApplicationUser = user
        };
    }
}