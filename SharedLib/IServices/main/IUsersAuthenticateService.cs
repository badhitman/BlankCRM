////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Сервис работы с аутентификацией пользователей
/// </summary>
public interface IUsersAuthenticateService
{
    /// <summary>
    /// Проверяет код входа из приложения проверки подлинности, а также создает и подписывает пользователя в виде асинхронной операции.
    /// </summary>
    /// <param name="code">Код двухфакторной аутентификации для проверки.</param>
    /// <param name="isPersistent">Флаг, указывающий, должен ли файл cookie для входа сохраняться после закрытия браузера.</param>
    /// <param name="rememberClient">Флаг, указывающий, следует ли запомнить текущий браузер, подавляя все дальнейшие запросы двухфакторной аутентификации.</param>
    /// <param name="userAlias">User alias</param>
    /// <param name="token"></param>
    public Task<IdentityResultResponseModel> TwoFactorAuthenticatorSignInAsync(string code, bool isPersistent, bool rememberClient, string? userAlias = null, CancellationToken token = default);

    /// <summary>
    /// Войти в учётную запись пользователя
    /// </summary>
    public Task<ResponseBaseModel> SignInAsync(string userId, bool isPersistent, CancellationToken token = default);

    /// <summary>
    /// Войти в учётную запись пользователя
    /// </summary>
    public Task<SignInResultResponseModel> PasswordSignInAsync(string userEmail, string password, bool isPersistent, CancellationToken token = default);

    /// <summary>
    /// Регистрация нового пользователя
    /// </summary>
    /// <param name="req">Email + Пароль + Адрес сайта/домена (для формирования ссылки подтверждения)</param>
    /// <param name="token"></param>
    public Task<RegistrationNewUserResponseModel> RegisterNewUserAsync(RegisterNewUserPasswordModel req, CancellationToken token = default);

    /// <summary>
    /// [External] Регистрация нового пользователя
    /// </summary>
    /// <param name="userEmail">Email</param>
    /// <param name="baseAddress">Адрес сайта/домена (для формирования ссылки подтверждения)</param>
    /// <param name="token"></param>
    public Task<RegistrationNewUserResponseModel> ExternalRegisterNewUserAsync(string userEmail, string baseAddress, CancellationToken token = default);

    /// <summary>
    /// Получает информацию о внешнем входе для текущего входа в виде асинхронной операции.
    /// Gets the external login information for the current login, as an asynchronous operation.
    /// </summary>
    public Task<UserLoginInfoResponseModel> GetExternalLoginInfoAsync(string? expectedXsrf = null, CancellationToken token = default);

    /// <summary>
    /// Вход пользователя через ранее зарегистрированный сторонний логин в виде асинхронной операции.
    /// </summary>
    public Task<ExternalLoginSignInResponseModel> ExternalLoginSignInAsync(string loginProvider, string providerKey, string? identityName, bool isPersistent = false, bool bypassTwoFactor = true, CancellationToken token = default);

    /// <summary>
    /// Получает информацию о пользователе для текущего входа в систему с двухфакторной аутентификацией.
    /// </summary>
    public Task<TResponseModel<UserInfoModel?>> GetTwoFactorAuthenticationUserAsync(CancellationToken token = default);

    /// <summary>
    /// Вход пользователя без двухфакторной аутентификации с использованием двухфакторного кода восстановления.
    /// </summary>
    /// <param name="recoveryCode">Двухфакторный код восстановления.</param>
    /// <param name="token"></param>
    public Task<IdentityResultResponseModel> TwoFactorRecoveryCodeSignInAsync(string recoveryCode, CancellationToken token = default);

    /// <summary>
    /// Выводит текущего пользователя из приложения.
    /// </summary>
    public Task SignOutAsync(CancellationToken token = default);
}