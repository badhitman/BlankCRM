﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Сервис работы с профилями пользователей
/// </summary>
public partial interface IUsersProfilesService
{
    /// <summary>
    /// Получает флаг, указывающий, есть ли у пользователя пароль.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<TResponseModel<bool?>> UserHasPassword(string? userId = null, CancellationToken token = default);

    /// <summary>
    /// Возвращает флаг, указывающий, действителен ли данный <paramref name="password"/> для указанного <paramref name="userId"/>.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<ResponseBaseModel> CheckUserPassword(string password, string? userId = null, CancellationToken token = default);

    /// <summary>
    /// Удалить Identity данные пользователя.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<ResponseBaseModel> DeleteUserData(string password, string? userId = null, CancellationToken token = default);

    /// <summary>
    /// Включена ли для указанного <paramref name="userId"/> двухфакторная аутентификация.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<TResponseModel<bool?>> GetTwoFactorEnabled(string? userId = null, CancellationToken token = default);

    /// <summary>
    /// Вкл/Выкл двухфакторную аутентификацию для указанного <paramref name="userId"/>
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<ResponseBaseModel> SetTwoFactorEnabled(bool enabled_set, string? userId = null, CancellationToken token = default);

    /// <summary>
    /// Создает токен изменения адреса электронной почты для указанного пользователя.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<ResponseBaseModel> GenerateChangeEmailToken(string userEmail, string baseAddress, string? userId = null, CancellationToken token = default);

    /// <summary>
    /// Был ли подтвержден адрес электронной почты для указанного <paramref name="userId"/>; true, если адрес электронной почты проверен/подтвержден.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<TResponseModel<bool>> IsEmailConfirmed(string? userId = null, CancellationToken token = default);

    /// <summary>
    /// Сбрасывает ключ аутентификации для пользователя.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<ResponseBaseModel> ResetAuthenticatorKey(string? userId = null, CancellationToken token = default);

    /// <summary>
    /// Извлекает связанные логины для указанного <param ref="userId"/>.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<TResponseModel<IEnumerable<UserLoginInfoModel>>> GetUserLogins(string? userId = null, CancellationToken token = default);

    /// <summary>
    /// Добавляет внешнюю <see cref="UserLoginInfoModel"/> к указанному <paramref name="userId"/>.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<ResponseBaseModel> AddLogin(string? userId = null, CancellationToken token = default);

    /// <summary>
    /// Пытается удалить предоставленную внешнюю информацию для входа из указанного <paramref name="userId"/>
    /// и возвращает флаг, указывающий, удалось ли удаление или нет.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<ResponseBaseModel> RemoveLogin(string loginProvider, string providerKey, string? userId = null, CancellationToken token = default);

    /// <summary>
    /// Проверяет указанную двухфакторную аутентификацию <paramref name="token" /> на соответствие <paramref name="userId"/>.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<ResponseBaseModel> VerifyTwoFactorToken(string token, string? userId = null, CancellationToken tokenCanc = default);

    /// <summary>
    /// Возвращает количество кодов восстановления, действительных для пользователя.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<TResponseModel<int?>> CountRecoveryCodes(string? userId = null, CancellationToken token = default);

    /// <summary>
    /// Генерирует коды восстановления для пользователя, что делает недействительными все предыдущие коды восстановления для пользователя.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<TResponseModel<IEnumerable<string>?>> GenerateNewTwoFactorRecoveryCodes(string? userId = null, CancellationToken token = default);

    /// <summary>
    /// Ключ аутентификации пользователя.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<TResponseModel<string?>> GetAuthenticatorKey(string? userId = null, CancellationToken token = default);

    /// <summary>
    /// Создает токен сброса пароля для указанного <paramref name="userId"/>, используя настроенного поставщика токенов сброса пароля.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<TResponseModel<string?>> GeneratePasswordResetToken(string? userId = null, CancellationToken token = default);

    /// <summary>
    /// Попытка добавить роль пользователю. Если роли такой нет, то она будет создана.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<ResponseBaseModel> TryAddRolesToUser(IEnumerable<string> addRoles, string? userId = null, CancellationToken token = default);

    /// <summary>
    /// Изменяет пароль пользователя после подтверждения правильности указанного <paramref name="currentPassword"/>.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    /// <param name="currentPassword">Текущий пароль, который необходимо проверить перед изменением.</param>
    /// <param name="newPassword">Новый пароль, который необходимо установить для указанного <paramref name="userId"/>.</param>
    /// <param name="userId">Пользователь, пароль которого должен быть установлен. Если не указан, то для текущего пользователя (запрос/сессия)</param>
    /// <param name="token"></param>
    public Task<ResponseBaseModel> ChangePassword(string currentPassword, string newPassword, string? userId = null, CancellationToken token = default);

    /// <summary>
    /// Добавляет <paramref name="password"/> к указанному <paramref name="userId"/>, только если у пользователя еще нет пароля.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<ResponseBaseModel> AddPassword(string password, string? userId = null, CancellationToken token = default);

    /// <summary>
    /// Обновляет адрес Email, если токен действительный для пользователя.
    /// </summary>
    /// <param name="req">Пользователь, адрес электронной почты которого необходимо обновить.Новый адрес электронной почты.Измененный токен электронной почты, который необходимо подтвердить.</param>
    /// <param name="token"></param>
    public Task<ResponseBaseModel> ChangeEmail(IdentityEmailTokenModel req, CancellationToken token = default);
}