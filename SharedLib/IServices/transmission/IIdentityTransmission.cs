﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Identity
/// </summary>
public interface IIdentityTransmission
{
    /// <summary>
    /// Проверка 2FA токена
    /// </summary>
    public Task<TResponseModel<string>> CheckToken2FAAsync(CheckToken2FARequestModel req, CancellationToken token = default);

    /// <summary>
    /// Чтение 2fa токена (из кеша)
    /// </summary>
    public Task<TResponseModel<string>> ReadToken2FAAsync(string userId, CancellationToken token = default);

    /// <summary>
    /// Генерация (и отправка на Email++) 2fa токена
    /// </summary>
    public Task<TResponseModel<string>> GenerateToken2FAAsync(string userId, CancellationToken token = default);

    /// <summary>
    /// Извлекает связанные логины для указанного <param ref="userId"/>
    /// </summary>
    public Task<TResponseModel<IEnumerable<UserLoginInfoModel>>> GetUserLoginsAsync(string userId, CancellationToken token = default);

    /// <summary>
    /// Возвращает флаг, указывающий, действителен ли данный password для указанного userId
    /// </summary>
    /// <returns>
    /// true, если указанный password соответствует для userId, в противном случае значение false.
    /// </returns>
    public Task<ResponseBaseModel> CheckUserPasswordAsync(IdentityPasswordModel req, CancellationToken token = default);

    /// <summary>
    /// Удалить Identity данные пользователя
    /// </summary>
    public Task<ResponseBaseModel> DeleteUserDataAsync(DeleteUserDataRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Получает флаг, указывающий, есть ли у пользователя пароль
    /// </summary>
    public Task<TResponseModel<bool?>> UserHasPasswordAsync(string userId, CancellationToken token = default);

    /// <summary>
    /// Включена ли для указанного <paramref name="userId"/> двухфакторная аутентификация.
    /// </summary>
    public Task<TResponseModel<bool?>> GetTwoFactorEnabledAsync(string userId, CancellationToken token = default);

    /// <summary>
    /// Вкл/Выкл двухфакторную аутентификацию для указанного userId
    /// </summary>
    public Task<ResponseBaseModel> SetTwoFactorEnabledAsync(SetTwoFactorEnabledRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Сбрасывает ключ аутентификации для пользователя.
    /// </summary>
    public Task<ResponseBaseModel> ResetAuthenticatorKeyAsync(string userId, CancellationToken token = default);

    /// <summary>
    /// Пытается удалить предоставленную внешнюю информацию для входа из указанного userId
    /// и возвращает флаг, указывающий, удалось ли удаление или нет
    /// </summary>
    public Task<ResponseBaseModel> RemoveLoginAsync(RemoveLoginRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Проверяет указанную двухфакторную аутентификацию VerificationCode на соответствие UserId
    /// </summary>
    public Task<ResponseBaseModel> VerifyTwoFactorTokenAsync(VerifyTwoFactorTokenRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Возвращает количество кодов восстановления, действительных для пользователя
    /// </summary>
    public Task<TResponseModel<int?>> CountRecoveryCodesAsync(string userId, CancellationToken token = default);

    /// <summary>
    /// Создает (и отправляет) токен изменения адреса электронной почты для указанного пользователя.
    /// </summary>
    public Task<ResponseBaseModel> GenerateChangeEmailTokenAsync(GenerateChangeEmailTokenRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Генерирует коды восстановления для пользователя, что делает недействительными все предыдущие коды восстановления для пользователя.
    /// </summary>
    /// <returns>Новые коды восстановления для пользователя. Примечание. Возвращенное число может быть меньше, поскольку дубликаты будут удалены.</returns>
    public Task<TResponseModel<IEnumerable<string>?>> GenerateNewTwoFactorRecoveryCodesAsync(GenerateNewTwoFactorRecoveryCodesRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Ключ аутентификации пользователя.
    /// </summary>
    public Task<TResponseModel<string?>> GetAuthenticatorKeyAsync(string userId, CancellationToken token = default);

    /// <summary>
    /// Создает токен сброса пароля для указанного <paramref name="userId"/>, используя настроенного поставщика токенов сброса пароля.
    /// Если <paramref name="userId"/> не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<TResponseModel<string?>> GeneratePasswordResetTokenAsync(string userId, CancellationToken token = default);

    /// <summary>
    /// Этот API поддерживает инфраструктуру ASP.NET Core Identity и не предназначен для использования в качестве абстракции электронной почты общего назначения.
    /// Он должен быть реализован в приложении, чтобы инфраструктура идентификации могла отправлять электронные письма для сброса пароля.
    /// </summary>
    public Task<ResponseBaseModel> SendPasswordResetLinkAsync(SendPasswordResetLinkRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Изменяет пароль пользователя после подтверждения правильности указанного currentPassword.
    /// Если userId не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    /// <remarks>Текущий пароль, который необходимо проверить перед изменением.
    /// Новый пароль, который необходимо установить для указанного userId.Пользователь, пароль которого должен быть установлен.
    /// Если не указан, то для текущего пользователя (запрос/сессия).</remarks>
    public Task<ResponseBaseModel> ChangePasswordAsync(IdentityChangePasswordModel req, CancellationToken token = default);

    /// <summary>
    /// Добавляет password к указанному userId, только если у пользователя еще нет пароля.
    /// Если userId не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public Task<ResponseBaseModel> AddPasswordAsync(IdentityPasswordModel req, CancellationToken token = default);

    /// <summary>
    /// Обновляет адрес Email, если токен действительный для пользователя.
    /// </summary>    
    public Task<ResponseBaseModel> ChangeEmailAsync(IdentityEmailTokenModel req, CancellationToken token = default);

    /// <summary>
    /// Обновить пользователю поля: FirstName и LastName
    /// </summary>
    public Task<ResponseBaseModel> UpdateUserDetailsAsync(IdentityDetailsModel req, CancellationToken token = default);

    /// <summary>
    /// Claim: Remove
    /// </summary>
    public Task<ResponseBaseModel> ClaimDeleteAsync(ClaimAreaIdModel req, CancellationToken token = default);

    /// <summary>
    /// Claim: Update or create
    /// </summary>
    public Task<ResponseBaseModel> ClaimUpdateOrCreateAsync(ClaimUpdateModel req, CancellationToken token = default);

    /// <summary>
    /// Get claims
    /// </summary>
    public Task<List<ClaimBaseModel>> GetClaimsAsync(ClaimAreaOwnerModel req, CancellationToken token = default);

    /// <summary>
    /// Установить блокировку пользователю
    /// </summary>
    public Task<ResponseBaseModel> SetLockUserAsync(IdentityBooleanModel req, CancellationToken token = default);

    /// <summary>
    /// Пользователи
    /// </summary>
    public Task<TPaginationResponseModel<UserInfoModel>> FindUsersAsync(FindWithOwnedRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Сбрасывает пароль на указанный
    /// после проверки заданного сброса пароля.
    /// </summary>
    public Task<ResponseBaseModel> ResetPasswordAsync(IdentityPasswordTokenModel req, CancellationToken token = default);

    /// <summary>
    /// Поиск пользователя по Email
    /// </summary>
    public Task<TResponseModel<UserInfoModel>> FindUserByEmailAsync(string email, CancellationToken token = default);

    /// <summary>
    /// Создает и отправляет токен подтверждения электронной почты для указанного пользователя.
    /// </summary>
    /// <remarks>
    /// Этот API поддерживает инфраструктуру ASP.NET Core Identity и не предназначен для использования в качестве абстракции электронной почты общего назначения.
    /// Он должен быть реализован в приложении, чтобы  Identityинфраструктура могла отправлять электронные письма с подтверждением.
    /// </remarks>
    public Task<ResponseBaseModel> GenerateEmailConfirmationAsync(SimpleUserIdentityModel req, CancellationToken token = default);

    /// <summary>
    /// Регистрация нового email/пользователя
    /// </summary>
    /// <remarks>
    /// Без пароля
    /// </remarks>
    public Task<RegistrationNewUserResponseModel> CreateNewUserAsync(string userEmail, CancellationToken token = default);

    /// <summary>
    /// Регистрация нового пользователя
    /// </summary>
    public Task<RegistrationNewUserResponseModel> CreateNewUserWithPasswordAsync(RegisterNewUserPasswordModel req, CancellationToken token = default);

    /// <summary>
    /// Проверяет, соответствует ли токен подтверждения электронной почты указанному пользователю.
    /// </summary>
    public Task<ResponseBaseModel> ConfirmUserEmailCodeAsync(UserCodeModel req, CancellationToken token = default);

    /// <summary>
    /// Отправка Email
    /// </summary>
    public Task<ResponseBaseModel> SendEmailAsync(SendEmailRequestModel req, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// SelectUsersOfIdentity
    /// </summary>
    public Task<TPaginationResponseModel<UserInfoModel>> SelectUsersOfIdentityAsync(TPaginationRequestStandardModel<SimpleBaseRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Получить пользователей из Identity по их идентификаторам
    /// </summary>
    public Task<TResponseModel<UserInfoModel[]>> GetUsersIdentityAsync(IEnumerable<string> ids_users, CancellationToken token = default);

    /// <summary>
    /// Получить пользователей из Identity по их Email`s
    /// </summary>
    public Task<TResponseModel<UserInfoModel[]>> GetUsersIdentityByEmailsAsync(IEnumerable<string> ids_emails, CancellationToken token = default);

    #region roles
    /// <summary>
    /// Попытка добавить роли пользователю. Если роли такой нет, то она будет создана.
    /// </summary>
    public Task<ResponseBaseModel> TryAddRolesToUserAsync(UserRolesModel req, CancellationToken token = default);

    /// <summary>
    /// Get Role (by id)
    /// </summary>
    public Task<TResponseModel<RoleInfoModel>> GetRoleAsync(string roleName, CancellationToken token = default);

    /// <summary>
    /// Роли. Если указан 'OwnerId', то поиск ограничивается ролями данного пользователя
    /// </summary>
    public Task<TPaginationResponseModel<RoleInfoModel>> FindRolesAsync(FindWithOwnedRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Создать новую роль
    /// </summary>
    public Task<ResponseBaseModel> CreateNewRoleAsync(string role_name, CancellationToken token = default);

    /// <summary>
    /// Удалить роль (если у роли нет пользователей).
    /// </summary>
    public Task<ResponseBaseModel> DeleteRoleAsync(string roleName, CancellationToken token = default);

    /// <summary>
    /// Исключить пользователя из роли (лишить пользователя роли)
    /// </summary>
    public Task<ResponseBaseModel> DeleteRoleFromUserAsync(RoleEmailModel req, CancellationToken token = default);

    /// <summary>
    /// Добавить роль пользователю (включить пользователя в роль)
    /// </summary>
    public Task<ResponseBaseModel> AddRoleToUserAsync(RoleEmailModel req, CancellationToken token = default);

    /// <summary>
    /// Получить `web config` сайта
    /// </summary>
    public Task<TResponseModel<string[]>> SetRoleForUserAsync(SetRoleForUserRequestModel req, CancellationToken token = default);
    #endregion

    #region tg
    /// <summary>
    /// Установить пользователю Claim`s[TelegramId, FirstName, LastName, PhoneNum]
    /// </summary>
    public Task<TResponseModel<bool>> ClaimsUserFlushAsync(string userIdIdentity, CancellationToken token = default);

    /// <summary>
    /// Поиск пользователей в Identity по их Telegram chat id
    /// </summary>
    public Task<TResponseModel<UserInfoModel[]>> GetUserIdentityByTelegramAsync(long[] ids_users, CancellationToken token = default);

    /// <summary>
    /// Удалить связь Telegram аккаунта с учётной записью сайта
    /// </summary>
    public Task<ResponseBaseModel> TelegramAccountRemoveIdentityJoinAsync(TelegramAccountRemoveJoinRequestIdentityModel req, CancellationToken token = default);

    /// <summary>
    /// Удалить текущую процедуру привязки Telegram аккаунта к учётной записи сайта
    /// </summary>
    public Task<ResponseBaseModel> TelegramJoinAccountDeleteActionAsync(string userId, CancellationToken token = default);

    /// <summary>
    /// Инициировать новую процедуру привязки Telegram аккаунта к учётной записи сайта
    /// </summary>
    public Task<TResponseModel<TelegramJoinAccountModelDb>> TelegramJoinAccountCreateAsync(string userId, CancellationToken token = default);

    /// <summary>
    /// Telegram пользователи (сохранённые).
    /// Все пользователи, которые когда либо писали что либо в бота - сохраняются/кэшируются в БД.
    /// </summary>
    public Task<TPaginationResponseModel<TelegramUserViewModel>> FindUsersTelegramAsync(SimplePaginationRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Проверка Telegram пользователя
    /// </summary>
    public Task<ResponseBaseModel> TelegramJoinAccountConfirmTokenAsync(TelegramJoinAccountConfirmModel req, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Удалить связь Telegram аккаунта с учётной записью сайта
    /// </summary>
    public Task<ResponseBaseModel> TelegramJoinAccountDeleteAsync(TelegramAccountRemoveJoinRequestTelegramModel req, CancellationToken token = default);

    /// <summary>
    /// Основное сообщение в чате в котором Bot ведёт диалог с пользователем.
    /// Бот может отвечать новым сообщением или редактировать своё ранее отправленное в зависимости от ситуации.
    /// </summary>
    public Task<ResponseBaseModel> UpdateTelegramMainUserMessageAsync(MainUserMessageModel setMainMessage, CancellationToken token = default);

    /// <summary>
    /// Получить данные пользователя из кэша
    /// </summary>
    public Task<TResponseModel<TelegramUserBaseModel>> GetTelegramUserAsync(long telegramUserId, CancellationToken token = default);

    /// <summary>
    /// TelegramJoinAccountState
    /// </summary>
    public Task<TResponseModel<TelegramJoinAccountModelDb>> TelegramJoinAccountStateAsync(TelegramJoinAccountStateRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Проверка пользователя (сообщение из службы TelegramBot серверной части сайта)
    /// </summary>
    public Task<TResponseModel<CheckTelegramUserAuthModel>> CheckTelegramUserAsync(CheckTelegramUserHandleModel user, CancellationToken token = default);
    #endregion
}