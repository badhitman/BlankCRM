////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsRoutes;
using System.IO;

namespace SharedLib;

/// <summary>
/// Константы
/// </summary>
public static partial class GlobalStaticConstantsTransmission
{
    /// <summary>
    /// Префикс имени MQ очереди
    /// </summary>
    public static string TransmissionQueueNamePrefix { get; set; } = "Transmission.Receives";
    /// <summary>
    /// Префикс имени MQ очереди
    /// </summary>
    public static string TransmissionQueueNamePrefixMQTT { get; set; } = "MQTT.Receives";

    /// <summary>
    /// Transmission MQ queues
    /// </summary>
    public static class TransmissionQueues
    {
        #region Identity 
        /// <summary>
        /// FindUserByEmailReceive
        /// </summary>
        public readonly static string FindUserByEmailReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.FIND_ACTION_NAME}-{Routes.USER_CONTROLLER_NAME}", $"by-{Routes.EMAIL_CONTROLLER_NAME}");

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        public readonly static string RegistrationNewUserReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.USER_CONTROLLER_NAME, Routes.REGISTRATION_ACTION_NAME);

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        public readonly static string RegistrationNewUserWithPasswordReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.USER_CONTROLLER_NAME, $"{Routes.REGISTRATION_ACTION_NAME}-with-{Routes.PASSWORD_CONTROLLER_NAME}");

        /// <summary>
        /// Отразить в Claim`s мета-данные пользователя
        /// </summary>
        public readonly static string ClaimsForUserFlushReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.USER_CONTROLLER_NAME}-{Routes.CLAIMS_CONTROLLER_NAME}", Routes.FLUSH_ACTION_NAME);

        /// <summary>
        /// AddRoleToUserReceive
        /// </summary>
        public readonly static string AddRoleToUserReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.ROLE_CONTROLLER_NAME, $"{Routes.ADD_ACTION_NAME}-to-{Routes.USER_CONTROLLER_NAME}");

        /// <summary>
        /// AddPasswordToUserReceive
        /// </summary>
        public readonly static string AddPasswordToUserReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.USER_CONTROLLER_NAME, $"{Routes.ADD_ACTION_NAME}-{Routes.PASSWORD_CONTROLLER_NAME}");

        /// <summary>
        /// Проверка 2FA токена
        /// </summary>
        public readonly static string CheckToken2FAReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.TWOFACTOR_CONTROLLER_NAME}-{Routes.TOKEN_CONTROLLER_NAME}", Routes.CHECK_ACTION_NAME);

        /// <summary>
        /// Чтение 2fa токена (из кеша)
        /// </summary>
        public readonly static string ReadToken2FAReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.TWOFACTOR_CONTROLLER_NAME, $"{Routes.TOKEN_CONTROLLER_NAME}-{Routes.GET_ACTION_NAME}");

        /// <summary>
        /// Генерация (и отправка на Email++) 2fa токена
        /// </summary>
        public readonly static string GenerateToken2FAReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.TWOFACTOR_CONTROLLER_NAME}-{Routes.AUTHORIZE_CONTROLLER_NAME}", $"{Routes.GENERATE_ACTION_NAME}-{Routes.TOKEN_CONTROLLER_NAME}");

        /// <summary>
        /// Извлекает связанные логины для указанного <param ref="userId"/>
        /// </summary>
        public readonly static string GetUserLoginsReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.USER_CONTROLLER_NAME}-{Routes.LOGINS_ACTION_NAME}", Routes.GET_ACTION_NAME);

        /// <summary>
        /// Возвращает флаг, указывающий, действителен ли данный password для указанного userId
        /// </summary>
        /// <returns>
        /// true, если указанный password соответствует для userId, в противном случае значение false.
        /// </returns>
        public readonly static string CheckUserPasswordReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.USER_CONTROLLER_NAME}-{Routes.PASSWORD_CONTROLLER_NAME}", Routes.CHECK_ACTION_NAME);

        /// <summary>
        /// Удалить Identity данные пользователя
        /// </summary>
        public readonly static string DeleteUserDataReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.USER_CONTROLLER_NAME}-{Routes.DATA_CONTROLLER_NAME}", Routes.DELETE_ACTION_NAME);

        /// <summary>
        /// Получает флаг, указывающий, есть ли у пользователя пароль
        /// </summary>
        public readonly static string UserHasPasswordReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.USER_CONTROLLER_NAME, $"{Routes.HAS_ACTION_NAME}-{Routes.PASSWORD_CONTROLLER_NAME}");

        /// <summary>
        /// Включена ли для указанного userId двухфакторная аутентификация
        /// </summary>
        public readonly static string GetTwoFactorEnabledReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.TWOFACTOR_CONTROLLER_NAME, $"{Routes.ENABLED_CONTROLLER_NAME}-{Routes.GET_ACTION_NAME}");

        /// <summary>
        /// Вкл/Выкл двухфакторную аутентификацию для указанного userId
        /// </summary>
        public readonly static string SetTwoFactorEnabledReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.TWOFACTOR_CONTROLLER_NAME, $"{Routes.ENABLED_CONTROLLER_NAME}-{Routes.SET_ACTION_NAME}");

        /// <summary>
        /// Сбрасывает ключ аутентификации для пользователя.
        /// </summary>
        public readonly static string ResetAuthenticatorKeyReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.AUTHENTICATOR_CONTROLLER_NAME}-{Routes.KEY_CONTROLLER_NAME}", Routes.RESET_ACTION_NAME);

        /// <summary>
        /// Пытается удалить предоставленную внешнюю информацию для входа из указанного userId
        /// и возвращает флаг, указывающий, удалось ли удаление или нет
        /// </summary>
        public readonly static string RemoveLoginForUserReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.USER_CONTROLLER_NAME}-{Routes.LOGIN_ACTION_NAME}", Routes.DELETE_ACTION_NAME);

        /// <summary>
        /// Создает (и отправляет) токен изменения адреса электронной почты для указанного пользователя.
        /// </summary>
        public readonly static string GenerateChangeEmailTokenReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.CHANGE_ACTION_NAME}-{Routes.EMAIL_CONTROLLER_NAME}", $"{Routes.GENERATE_ACTION_NAME}-{Routes.TOKEN_CONTROLLER_NAME}");

        /// <summary>
        /// Возвращает количество кодов восстановления, действительных для пользователя
        /// </summary>
        public readonly static string CountRecoveryCodesReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.RECOVERY_CONTROLLER_NAME}-{Routes.CODES_CONTROLLER_NAME}", Routes.COUNT_ACTION_NAME);

        /// <summary>
        /// Проверяет указанную двухфакторную аутентификацию VerificationCode на соответствие UserId
        /// </summary>
        public readonly static string VerifyTwoFactorTokenReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.TWOFACTOR_CONTROLLER_NAME, $"{Routes.VERIFY_ACTION_NAME}-{Routes.TOKEN_CONTROLLER_NAME}");

        /// <summary>
        /// GenerateNewTwoFactorRecoveryCodesReceive
        /// </summary>
        public readonly static string GenerateNewTwoFactorRecoveryCodesReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.TWOFACTOR_CONTROLLER_NAME, $"{Routes.GENERATE_ACTION_NAME}-{Routes.RECOVERY_CONTROLLER_NAME}-{Routes.CODES_CONTROLLER_NAME}");

        /// <summary>
        /// Ключ аутентификации пользователя.
        /// </summary>
        public readonly static string GetAuthenticatorKeyReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.AUTHENTICATOR_CONTROLLER_NAME}-{Routes.KEY_CONTROLLER_NAME}", Routes.GET_ACTION_NAME);

        /// <summary>
        /// Создает токен сброса пароля для указанного "userId", используя настроенного поставщика токенов сброса пароля.
        /// Если "userId" не указан, то команда выполняется для текущего пользователя (запрос/сессия)
        /// </summary>
        public readonly static string GeneratePasswordResetTokenReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.PASSWORD_CONTROLLER_NAME}-{Routes.RESET_ACTION_NAME}", $"{Routes.GENERATE_ACTION_NAME}-{Routes.TOKEN_CONTROLLER_NAME}");

        /// <summary>
        /// TelegramJoinAccountStateReceive
        /// </summary>
        public readonly static string TelegramJoinAccountStateReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.TELEGRAM_CONTROLLER_NAME}-{Routes.ACCOUNT_CONTROLLER_NAME}", $"{Routes.JOIN_ACTION_NAME}-{Routes.STATE_CONTROLLER_NAME}");

        /// <summary>
        /// SendPasswordResetLinkReceive
        /// </summary>
        public readonly static string SendPasswordResetLinkReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.USER_CONTROLLER_NAME}-{Routes.PASSWORD_CONTROLLER_NAME}", $"{Routes.SEND_ACTION_NAME}-{Routes.RESET_ACTION_NAME}-{Routes.LINK_CONTROLLER_NAME}");

        /// <summary>
        /// TryAddRolesToUserReceive
        /// </summary>
        public readonly static string TryAddRolesToUserReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.ROLES_CONTROLLER_NAME, $"{Routes.TRY_ACTION_NAME}-{Routes.ADD_ACTION_NAME}-to-{Routes.USER_CONTROLLER_NAME}");

        /// <summary>
        /// ChangePasswordToUserReceive
        /// </summary>
        public readonly static string ChangePasswordToUserReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.USER_CONTROLLER_NAME, $"{Routes.CHANGE_ACTION_NAME}-{Routes.PASSWORD_CONTROLLER_NAME}");

        /// <summary>
        /// ChangeEmailReceive
        /// </summary>
        public readonly static string ChangeEmailForUserReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.EMAIL_CONTROLLER_NAME}-{Routes.USER_CONTROLLER_NAME}", Routes.CHANGE_ACTION_NAME);

        /// <summary>
        /// UpdateUserDetailsReceive
        /// </summary>
        public readonly static string UpdateUserDetailsReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.USER_CONTROLLER_NAME}-{Routes.DETAILS_CONTROLLER_NAME}", Routes.UPDATE_ACTION_NAME);

        /// <summary>
        /// ClaimDeleteReceive
        /// </summary>
        public readonly static string ClaimDeleteReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.CLAIM_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <summary>
        /// ClaimUpdateOrCreateReceive
        /// </summary>
        public readonly static string ClaimUpdateOrCreateReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.CLAIM_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <summary>
        /// GetClaimsReceive
        /// </summary>
        public readonly static string GetClaimsReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.CLAIMS_CONTROLLER_NAME, Routes.GET_ACTION_NAME);

        /// <summary>
        /// SetLockUserReceive
        /// </summary>
        public readonly static string SetLockUserReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.USER_CONTROLLER_NAME, $"{Routes.LOCK_CONFIRMATION_NAME}-{Routes.SET_ACTION_NAME}");

        /// <summary>
        /// GetRoleReceive
        /// </summary>
        public readonly static string GetRoleReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.ROLE_CONTROLLER_NAME, Routes.GET_ACTION_NAME);

        /// <summary>
        /// FindUsersReceive
        /// </summary>
        public readonly static string FindUsersReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.USERS_CONTROLLER_NAME, Routes.FIND_ACTION_NAME);

        /// <summary>
        /// FindRolesAsyncReceive
        /// </summary>
        public readonly static string FindRolesAsyncReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.ROLES_CONTROLLER_NAME, Routes.FIND_ACTION_NAME);

        /// <summary>
        /// CateNewRoleReceive
        /// </summary>
        public readonly static string CateNewRoleReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.ROLE_CONTROLLER_NAME, Routes.CREATE_ACTION_NAME);

        /// <summary>
        /// DeleteRoleReceive
        /// </summary>
        public readonly static string DeleteRoleReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.ROLE_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <summary>
        /// DeleteRoleFromUser
        /// </summary>
        public readonly static string DeleteRoleFromUserReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.ROLE_CONTROLLER_NAME, $"{Routes.DELETE_ACTION_NAME}-from-{Routes.USER_CONTROLLER_NAME}");

        /// <summary>
        /// ResetPasswordForUserReceive
        /// </summary>
        public readonly static string ResetPasswordForUserReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.USER_CONTROLLER_NAME}-{Routes.PASSWORD_CONTROLLER_NAME}", Routes.RESET_ACTION_NAME);

        /// <summary>
        /// Получить пользователей из Identity по их идентификаторам
        /// </summary>
        public readonly static string GetUsersOfIdentityReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.USERS_CONTROLLER_NAME, Routes.GET_ACTION_NAME);

        /// <summary>
        /// Получить пользователей из Identity по их Email
        /// </summary>
        public readonly static string GetUsersOfIdentityByEmailReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.USERS_CONTROLLER_NAME, $"{Routes.GET_ACTION_NAME}-by-{Routes.EMAIL_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string SelectUsersOfIdentityReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.USERS_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string SetRoleForUserOfIdentityReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.USER_CONTROLLER_NAME}_{Routes.ROLE_CONTROLLER_NAME}", Routes.SET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetUsersOfIdentityByTelegramIdsReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.USERS_CONTROLLER_NAME, $"{Routes.GET_ACTION_NAME}-by-{Routes.TELEGRAM_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string ConfirmUserEmailCodeIdentityReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.USER_CONTROLLER_NAME}-{Routes.CONFIRM_ACTION_NAME}", $"{Routes.EMAIL_CONTROLLER_NAME}-{Routes.CODE_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string GenerateEmailConfirmationIdentityReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.EMAIL_CONTROLLER_NAME}-{Routes.CONFIRM_ACTION_NAME}", $"{Routes.SEND_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string GetTelegramUserReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.IDENTITY_CONTROLLER_NAME}-{Routes.TELEGRAM_CONTROLLER_NAME}", $"{Routes.USER_CONTROLLER_NAME}-{Routes.CACHE_CONTROLLER_NAME}", Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string FindUsersTelegramReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.USER_CONTROLLER_NAME}-{Routes.TELEGRAM_CONTROLLER_NAME}", Routes.FIND_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string TelegramJoinAccountDeleteActionReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.ACCOUNT_CONTROLLER_NAME}-{Routes.JOIN_ACTION_NAME}", $"{Routes.DELETE_ACTION_NAME}-{Routes.FLOW_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string TelegramAccountRemoveIdentityJoinReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.TELEGRAM_CONTROLLER_NAME}-{Routes.ACCOUNT_CONTROLLER_NAME}", $"{Routes.DELETE_ACTION_NAME}-{Routes.JOIN_ACTION_NAME}-of-{Routes.IDENTITY_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string GetUsersIdentityByTelegramReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, Routes.USERS_CONTROLLER_NAME, $"{Routes.GET_ACTION_NAME}-by-{Routes.TELEGRAM_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string TelegramJoinAccountCreateReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.IDENTITY_CONTROLLER_NAME, $"{Routes.TELEGRAM_CONTROLLER_NAME}-{Routes.ACCOUNT_CONTROLLER_NAME}", $"{Routes.JOIN_ACTION_NAME}-{Routes.CREATE_ACTION_NAME}");
        // 

        /// <inheritdoc/>
        public readonly static string TelegramJoinAccountConfirmReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.IDENTITY_CONTROLLER_NAME}-{Routes.TELEGRAM_CONTROLLER_NAME}", $"{Routes.JOIN_ACTION_NAME}-{Routes.ACCOUNT_CONTROLLER_NAME}", Routes.CONFIRM_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string TelegramJoinAccountDeleteReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.IDENTITY_CONTROLLER_NAME}-{Routes.TELEGRAM_CONTROLLER_NAME}", $"{Routes.JOIN_ACTION_NAME}-{Routes.ACCOUNT_CONTROLLER_NAME}", Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string UpdateTelegramMainUserMessageReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.IDENTITY_CONTROLLER_NAME}-{Routes.TELEGRAM_CONTROLLER_NAME}", $"{Routes.MAIN_CONTROLLER_NAME}-{Routes.MESSAGE_CONTROLLER_NAME}", Routes.UPDATE_ACTION_NAME);
        #endregion

        #region Web
        /// <inheritdoc/>
        public readonly static string SendEmailReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.EMAIL_CONTROLLER_NAME, Routes.OUTGOING_CONTROLLER_NAME, Routes.SEND_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string CheckTelegramUserReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.TELEGRAM_CONTROLLER_NAME}-{Routes.USER_CONTROLLER_NAME}", Routes.CHECK_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetWebConfigReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.WEB_CONTROLLER_NAME, Routes.CONFIGURATION_CONTROLLER_NAME, Routes.READ_ACTION_NAME);
        #endregion

        #region TelegramBot
        /// <inheritdoc/>
        public readonly static string GetBotUsernameReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TELEGRAM_CONTROLLER_NAME, Routes.NAME_CONTROLLER_NAME, Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string SendTextMessageTelegramReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TELEGRAM_CONTROLLER_NAME, $"{Routes.TEXT_CONTROLLER_NAME}_{Routes.MESSAGE_CONTROLLER_NAME}", Routes.SEND_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ForwardTextMessageTelegramReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TELEGRAM_CONTROLLER_NAME, $"{Routes.TEXT_CONTROLLER_NAME}_{Routes.MESSAGE_CONTROLLER_NAME}", Routes.FORWARD_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ReadFileTelegramReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TELEGRAM_CONTROLLER_NAME, Routes.FILE_CONTROLLER_NAME, Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ChatsReadTelegramReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TELEGRAM_CONTROLLER_NAME, Routes.CHATS_CONTROLLER_NAME, Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ChatReadTelegramReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TELEGRAM_CONTROLLER_NAME, Routes.CHAT_CONTROLLER_NAME, Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ChatsFindForUserTelegramReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TELEGRAM_CONTROLLER_NAME, $"{Routes.CHATS_CONTROLLER_NAME}-for-{Routes.USERS_CONTROLLER_NAME}", Routes.FIND_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string MessagesChatsSelectTelegramReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TELEGRAM_CONTROLLER_NAME, $"{Routes.MESSAGES_CONTROLLER_NAME}-of-{Routes.CHATS_CONTROLLER_NAME}", Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string UserTelegramPermissionUpdateReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TELEGRAM_CONTROLLER_NAME, $"{Routes.USER_CONTROLLER_NAME}-{Routes.PERMISSIONS_CONTROLLER_NAME}", Routes.SET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string UsersReadTelegramReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TELEGRAM_CONTROLLER_NAME, Routes.USERS_CONTROLLER_NAME, Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ChatsSelectTelegramReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TELEGRAM_CONTROLLER_NAME, Routes.CHATS_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ErrorsForChatsSelectTelegramReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TELEGRAM_CONTROLLER_NAME, $"{Routes.ERRORS_CONTROLLER_NAME}-of-{Routes.CHATS_CONTROLLER_NAME}", Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string SetWebConfigTelegramReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TELEGRAM_CONTROLLER_NAME, $"{Routes.WEB_CONTROLLER_NAME}_{Routes.CONFIGURATION_CONTROLLER_NAME}", $"{Routes.SET_ACTION_NAME}-of-{Routes.TELEGRAM_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string GetBotTokenTelegramReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TELEGRAM_CONTROLLER_NAME, $"{Routes.TOKEN_CONTROLLER_NAME}_{Routes.CONFIGURATION_CONTROLLER_NAME}", Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string SendWappiMessageReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.WAPPI_CONTROLLER_NAME, Routes.MESSAGE_CONTROLLER_NAME, Routes.SEND_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string SetWebConfigHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TELEGRAM_CONTROLLER_NAME, $"{Routes.WEB_CONTROLLER_NAME}_{Routes.CONFIGURATION_CONTROLLER_NAME}", $"{Routes.SET_ACTION_NAME}-of-{Routes.HELPDESK_CONTROLLER_NAME}");
        #endregion

        #region Constructor
        #region projects
        /// <inheritdoc/>
        public readonly static string ProjectsReadReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.PROJECTS_CONTROLLER_NAME, Routes.DATA_CONTROLLER_NAME, Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ProjectsForUserReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.PROJECTS_CONTROLLER_NAME, Routes.USER_CONTROLLER_NAME, $"{Routes.GET_ACTION_NAME}_{Routes.ROWS_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string ProjectCreateReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.PROJECT_CONTROLLER_NAME, Routes.CREATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string SetMarkerDeleteProjectReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.PROJECT_CONTROLLER_NAME, $"{Routes.MARK_ACTION_NAME}_{Routes.DELETE_ACTION_NAME}", Routes.SET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetMembersOfProjectReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.PROJECT_CONTROLLER_NAME, Routes.MEMBERS_CONTROLLER_NAME, Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string DeleteMembersFromProjectReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.PROJECT_CONTROLLER_NAME, Routes.MEMBERS_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string CanEditProjectReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.PROJECT_CONTROLLER_NAME, Routes.MEMBER_CONTROLLER_NAME, $"{Routes.ALLOW_ACTION_NAME}_{Routes.EDIT_ACTION_NAME}_{Routes.CHECK_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string AddMembersToProjectReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.PROJECT_CONTROLLER_NAME, Routes.MEMBERS_CONTROLLER_NAME, Routes.ADD_ACTION_NAME);
        #endregion

        #region directories
        /// <inheritdoc/>
        public readonly static string ReadDirectoriesReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DIRECTORIES_CONTROLLER_NAME, Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string UpdateOrCreateDirectoryReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DIRECTORY_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetDirectoriesReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DIRECTORIES_CONTROLLER_NAME, Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string DeleteDirectoryReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DIRECTORY_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetElementsOfDirectoryReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DIRECTORY_CONTROLLER_NAME, Routes.ELEMENTS_CONTROLLER_NAME, Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string CreateElementForDirectoryReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DIRECTORY_CONTROLLER_NAME, Routes.ELEMENT_CONTROLLER_NAME, Routes.CREATE_ACTION_NAME);
        #endregion

        #region forms
        /// <inheritdoc/>
        public readonly static string SelectFormsReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.FORMS_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetFormReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.FORM_CONTROLLER_NAME, Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string FormUpdateOrCreateReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.FORM_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string FormDeleteReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.FORM_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string FieldFormMoveReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.FORM_CONTROLLER_NAME, Routes.FIELD_CONTROLLER_NAME, Routes.MOVE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string FieldDirectoryFormMoveReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.FORM_CONTROLLER_NAME, $"{Routes.FIELD_CONTROLLER_NAME}_{Routes.DIRECTORY_CONTROLLER_NAME}", Routes.MOVE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string FormFieldUpdateOrCreateReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.FORM_CONTROLLER_NAME, Routes.FIELD_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string FormFieldDeleteReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.FORM_CONTROLLER_NAME, Routes.FIELD_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string FormFieldDirectoryUpdateOrCreateReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.FORM_CONTROLLER_NAME, $"{Routes.FIELD_CONTROLLER_NAME}_{Routes.DIRECTORY_CONTROLLER_NAME}", Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string FormFieldDirectoryDeleteReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.FORM_CONTROLLER_NAME, $"{Routes.FIELD_CONTROLLER_NAME}_{Routes.DIRECTORY_CONTROLLER_NAME}", Routes.DELETE_ACTION_NAME);
        #endregion

        /// <inheritdoc/>
        public readonly static string GetSessionDocumentDataReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.SESSION_CONTROLLER_NAME, Routes.DOCUMENT_CONTROLLER_NAME, $"{Routes.DATA_CONTROLLER_NAME}_{Routes.GET_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string SetValueFieldSessionDocumentDataReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.SESSION_CONTROLLER_NAME, Routes.DOCUMENT_CONTROLLER_NAME, Routes.DATA_CONTROLLER_NAME, Routes.VALUE_CONTROLLER_NAME, Routes.FIELD_CONTROLLER_NAME, Routes.SET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string SetDoneSessionDocumentDataReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.SESSION_CONTROLLER_NAME, Routes.DOCUMENT_CONTROLLER_NAME, Routes.DATA_CONTROLLER_NAME, Routes.STATUS_CONTROLLER_NAME, Routes.DONE_ACTION_NAME, Routes.SET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string DeleteValuesFieldsByGroupSessionDocumentDataByRowNumReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.SESSION_CONTROLLER_NAME, Routes.DOCUMENT_CONTROLLER_NAME, Routes.DATA_CONTROLLER_NAME, Routes.ROW_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string CheckAndNormalizeSortIndexForElementsOfDirectoryReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DIRECTORY_CONTROLLER_NAME, Routes.CHECK_ACTION_NAME, Routes.NORMALIZE_ACTION_NAME, Routes.SORT_ACTION_NAME, Routes.INDEX_CONTROLLER_NAME, Routes.ELEMENTS_CONTROLLER_NAME);

        /// <inheritdoc/>
        public readonly static string UpdateOrCreateDocumentSchemeReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DOCUMENT_CONTROLLER_NAME, Routes.SCHEME_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string RequestDocumentsSchemesReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DOCUMENTS_CONTROLLER_NAME, Routes.SCHEMES_CONTROLLER_NAME, Routes.REQUEST_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetDocumentSchemeReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DOCUMENTS_CONTROLLER_NAME, Routes.SCHEMES_CONTROLLER_NAME, Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string DeleteDocumentSchemeReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DOCUMENT_CONTROLLER_NAME, Routes.SCHEME_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string CreateOrUpdateTabOfDocumentSchemeReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DOCUMENT_CONTROLLER_NAME, $"{Routes.SCHEME_CONTROLLER_NAME}_{Routes.TAB_CONTROLLER_NAME}", Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string MoveTabOfDocumentSchemeReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DOCUMENT_CONTROLLER_NAME, $"{Routes.SCHEME_CONTROLLER_NAME}_{Routes.TAB_CONTROLLER_NAME}", Routes.MOVE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetTabOfDocumentSchemeReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DOCUMENT_CONTROLLER_NAME, $"{Routes.SCHEME_CONTROLLER_NAME}_{Routes.TAB_CONTROLLER_NAME}", Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string DeleteTabOfDocumentSchemeReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DOCUMENT_CONTROLLER_NAME, $"{Routes.SCHEME_CONTROLLER_NAME}_{Routes.TAB_CONTROLLER_NAME}", Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetTabDocumentSchemeJoinFormReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DOCUMENT_CONTROLLER_NAME, $"{Routes.SCHEME_CONTROLLER_NAME}_{Routes.TAB_CONTROLLER_NAME}", Routes.JOIN_ACTION_NAME, Routes.FORM_CONTROLLER_NAME, Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string CreateOrUpdateTabDocumentSchemeJoinFormReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DOCUMENT_CONTROLLER_NAME, $"{Routes.SCHEME_CONTROLLER_NAME}_{Routes.TAB_CONTROLLER_NAME}", Routes.JOIN_ACTION_NAME, Routes.FORM_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string MoveTabDocumentSchemeJoinFormReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DOCUMENT_CONTROLLER_NAME, $"{Routes.SCHEME_CONTROLLER_NAME}_{Routes.TAB_CONTROLLER_NAME}", Routes.JOIN_ACTION_NAME, Routes.FORM_CONTROLLER_NAME, Routes.MOVE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string DeleteTabDocumentSchemeJoinFormReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DOCUMENT_CONTROLLER_NAME, $"{Routes.SCHEME_CONTROLLER_NAME}_{Routes.TAB_CONTROLLER_NAME}", Routes.JOIN_ACTION_NAME, Routes.FORM_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string SaveSessionFormReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.SESSION_CONTROLLER_NAME, $"{Routes.DOCUMENT_CONTROLLER_NAME}_{Routes.FORM_CONTROLLER_NAME}", Routes.SAVE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string SetStatusSessionDocumentReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.SESSION_CONTROLLER_NAME, Routes.DOCUMENT_CONTROLLER_NAME, Routes.STATUS_CONTROLLER_NAME, Routes.SET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetSessionDocumentReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.SESSION_CONTROLLER_NAME, Routes.DOCUMENT_CONTROLLER_NAME, Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string AddRowToTableReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.SESSION_CONTROLLER_NAME, Routes.DOCUMENT_CONTROLLER_NAME, Routes.ROW_CONTROLLER_NAME, Routes.ADD_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string UpdateOrCreateSessionDocumentReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.SESSION_CONTROLLER_NAME, Routes.DOCUMENT_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string RequestSessionsDocumentsReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.SESSIONS_CONTROLLER_NAME, Routes.DOCUMENTS_CONTROLLER_NAME, Routes.REQUEST_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string FindSessionsDocumentsByFormFieldNameReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.SESSIONS_CONTROLLER_NAME, Routes.DOCUMENTS_CONTROLLER_NAME, Routes.FIND_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ClearValuesForFieldNameReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.SESSION_CONTROLLER_NAME, Routes.DOCUMENT_CONTROLLER_NAME, Routes.FIELDS_CONTROLLER_NAME, Routes.NAME_CONTROLLER_NAME, Routes.VALUES_CONTROLLER_NAME, Routes.CLEAR_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string DeleteSessionDocumentReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.SESSION_CONTROLLER_NAME, Routes.DOCUMENT_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string UpdateElementOfDirectoryReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DIRECTORY_CONTROLLER_NAME, Routes.ELEMENT_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetElementOfDirectoryReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DIRECTORY_CONTROLLER_NAME, Routes.ELEMENT_CONTROLLER_NAME, Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string DeleteElementFromDirectoryReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DIRECTORY_CONTROLLER_NAME, Routes.ELEMENT_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string UpMoveElementOfDirectoryReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DIRECTORY_CONTROLLER_NAME, Routes.ELEMENT_CONTROLLER_NAME, Routes.MOVE_ACTION_NAME, Routes.UP_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string DownMoveElementOfDirectoryReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DIRECTORY_CONTROLLER_NAME, Routes.ELEMENT_CONTROLLER_NAME, Routes.MOVE_ACTION_NAME, Routes.DOWN_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetDirectoryReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DIRECTORY_CONTROLLER_NAME, Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string SetProjectAsMainReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.PROJECT_CONTROLLER_NAME, Routes.USER_CONTROLLER_NAME, Routes.MAIN_CONTROLLER_NAME, Routes.SET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetCurrentMainProjectReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.PROJECT_CONTROLLER_NAME, Routes.MAIN_CONTROLLER_NAME, Routes.USER_CONTROLLER_NAME, Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string UpdateProjectReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.PROJECT_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);
        #endregion

        #region Commerce
        /// <inheritdoc/>
        public readonly static string ReadFileCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.FILE_CONTROLLER_NAME, Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string StatusChangeOrderByHelpDeskDocumentIdReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, $"{Routes.STATUS_CONTROLLER_NAME}-for-{Routes.ORDER_CONTROLLER_NAME}", $"{Routes.CHANGE_ACTION_NAME}-{Routes.UPDATE_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string NomenclaturesReadCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.NOMENCLATURES_CONTROLLER_NAME, Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string CalendarsSchedulesReadCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.WORKSCHEDULES_CONTROLLER_NAME}-{Routes.CALENDARS_CONTROLLER_NAME}", Routes.READ_ACTION_NAME);

        /// <summary>
        /// WorkSchedulesReadCommerceReceive
        /// </summary>
        public readonly static string WeeklySchedulesReadCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.WORKSCHEDULES_CONTROLLER_NAME}-{Routes.WEEKLY_CONTROLLER_NAME}", Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string NomenclaturesSelectCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.NOMENCLATURES_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string WorksSchedulesFindCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.WORKSCHEDULES_CONTROLLER_NAME, Routes.FIND_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string CalendarsSchedulesSelectCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.WORKSCHEDULES_CONTROLLER_NAME}-{Routes.CALENDARS_CONTROLLER_NAME}", Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string WeeklySchedulesSelectCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.WORKSCHEDULES_CONTROLLER_NAME}-{Routes.WEEKLY_CONTROLLER_NAME}", Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OrdersSelectCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.ORDERS_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string WarehousesSelectCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.WAREHOUSE_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OffersRegistersSelectCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.OFFERS_CONTROLLER_NAME}-{Routes.REGISTERS_CONTROLLER_NAME}", Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OrdersByIssuesGetReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.ORDERS_CONTROLLER_NAME}-by-{Routes.ISSUES_CONTROLLER_NAME}", Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OrdersAttendancesByIssuesGetReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.ORDERS_CONTROLLER_NAME}-of-{Routes.ATTENDANCES_CONTROLLER_NAME}-by-{Routes.ISSUES_CONTROLLER_NAME}", Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OrdersAttendancesStatusesChangeByHelpDeskDocumentIdReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.ORDERS_CONTROLLER_NAME}-of-{Routes.ATTENDANCES_CONTROLLER_NAME}-by-{Routes.ISSUES_CONTROLLER_NAME}", $"{Routes.CHANGE_ACTION_NAME}-{Routes.STATUS_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string OrdersReadCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.ORDERS_CONTROLLER_NAME, Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OrderReportGetCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.ORDER_CONTROLLER_NAME}-{Routes.REPORT_CONTROLLER_NAME}", Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string PriceFullFileGetCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.PRICE_CONTROLLER_NAME}-{Routes.FULL_CONTROLLER_NAME}", Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string WarehousesDocumentsReadCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.WAREHOUSE_CONTROLLER_NAME}-{Routes.DOCUMENTS_CONTROLLER_NAME}", Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OrganizationsSelectCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.ORGANIZATIONS_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OrganizationsUsersSelectCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.ORGANIZATIONS_CONTROLLER_NAME}-{Routes.USERS_CONTROLLER_NAME}", Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string BankDetailsUpdateCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.ORGANIZATIONS_CONTROLLER_NAME}-{Routes.BANK_CONTROLLER_NAME}-{Routes.DETAILS_CONTROLLER_NAME}", Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string BankDetailsDeleteCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.ORGANIZATIONS_CONTROLLER_NAME}-{Routes.BANK_CONTROLLER_NAME}-{Routes.DETAILS_CONTROLLER_NAME}", Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OrganizationsFindCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.ORGANIZATIONS_CONTROLLER_NAME, Routes.FIND_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OrganizationsReadCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.ORGANIZATION_CONTROLLER_NAME, Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OrganizationsUsersReadCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.ORGANIZATIONS_CONTROLLER_NAME}-{Routes.USERS_CONTROLLER_NAME}", Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OfficesOrganizationsReadCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.OFFICES_CONTROLLER_NAME, Routes.ORGANIZATIONS_CONTROLLER_NAME, Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OrganizationUpdateOrCreateCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.ORGANIZATION_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OrganizationOfferContractUpdateOrCreateCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.ORGANIZATION_CONTROLLER_NAME}-{Routes.OFFER_CONTROLLER_NAME}-{Routes.CONTRACT_CONTROLLER_NAME}", Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OrganizationUserUpdateOrCreateCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.ORGANIZATION_CONTROLLER_NAME}-{Routes.USER_CONTROLLER_NAME}", Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OrganizationSetLegalCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.ORGANIZATIONS_CONTROLLER_NAME, Routes.LEGAL_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OfficeOrganizationUpdateCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.ORGANIZATIONS_CONTROLLER_NAME, Routes.OFFICE_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <summary>
        /// Обновление номенклатуры
        /// </summary>
        public readonly static string NomenclatureUpdateCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.NOMENCLATURE_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string CalendarScheduleUpdateCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.WORKSCHEDULE_CONTROLLER_NAME}-{Routes.CALENDAR_CONTROLLER_NAME}", Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string CreateAttendanceRecordsCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.ATTENDANCES_CONTROLLER_NAME}-{Routes.RECORDS_CONTROLLER_NAME}", Routes.CREATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string RecordsAttendancesSelectCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.ATTENDANCES_CONTROLLER_NAME}-{Routes.RECORDS_CONTROLLER_NAME}", Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string AttendanceRecordDeleteCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.ATTENDANCE_CONTROLLER_NAME}-{Routes.RECORD_CONTROLLER_NAME}", Routes.DELETE_ACTION_NAME);

        /// <summary>
        /// Attendance Update
        /// </summary>
        public readonly static string AttendanceUpdateCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.ATTENDANCE_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <summary>
        /// WorkSchedule Update
        /// </summary>
        public readonly static string WeeklyScheduleUpdateCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, $"{Routes.WORKSCHEDULES_CONTROLLER_NAME}-{Routes.WEEKLY_CONTROLLER_NAME}", Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OrderUpdateCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.ORDER_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string WarehouseDocumentUpdateCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.WAREHOUSE_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <summary>
        /// Прикрепить файл к заказу (счёт, акт и т.п.)
        /// </summary>
        public readonly static string AttachmentAddToOrderCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.ORDER_CONTROLLER_NAME, Routes.ATTACHMENT_CONTROLLER_NAME, Routes.ADD_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string AttachmentDeleteFromOrderCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.ORDER_CONTROLLER_NAME, Routes.ATTACHMENT_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string RowForOrderUpdateCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.ORDER_CONTROLLER_NAME, Routes.ROW_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string RowForWarehouseDocumentUpdateCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.WAREHOUSE_CONTROLLER_NAME, Routes.ROW_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string PaymentDocumentUpdateCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.ORDER_CONTROLLER_NAME, Routes.PAYMENT_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string PaymentDocumentDeleteCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.ORDER_CONTROLLER_NAME, Routes.PAYMENT_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string RowsDeleteFromOrderCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.ORDER_CONTROLLER_NAME, Routes.ROWS_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string RowsDeleteFromWarehouseDocumentCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.WAREHOUSE_CONTROLLER_NAME, Routes.ROWS_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OfferUpdateCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.GOODS_CONTROLLER_NAME, Routes.OFFER_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OfferSelectCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.GOODS_CONTROLLER_NAME, Routes.OFFER_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OfferReadCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.GOODS_CONTROLLER_NAME, Routes.OFFER_CONTROLLER_NAME, Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string PricesRulesGetForOfferCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.PRICE_CONTROLLER_NAME, Routes.OFFER_CONTROLLER_NAME, Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string PriceRuleUpdateCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.PRICE_CONTROLLER_NAME, Routes.OFFER_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string OfficeOrganizationDeleteCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.ORGANIZATIONS_CONTROLLER_NAME, Routes.OFFICE_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <summary>
        /// Удалить оффер
        /// </summary>
        public readonly static string OfferDeleteCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.OFFER_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string PriceRuleDeleteCommerceReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.COMMERCE_CONTROLLER_NAME, Routes.PRICE_CONTROLLER_NAME, Routes.OFFER_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);
        #endregion

        #region Bank`s
        /// <inheritdoc/>
        public readonly static string BankConnectionCreateOrUpdateReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.BANK_CONTROLLER_NAME, Routes.CONNECTION_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string AccountTBankCreateOrUpdateReceive = Path.Combine(TransmissionQueueNamePrefix, $"T-{Routes.BANK_CONTROLLER_NAME}", Routes.ACCOUNT_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string CustomerBankCreateOrUpdateReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.BANK_CONTROLLER_NAME, Routes.CUSTOMER_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string BankTransferCreateOrUpdateReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.BANK_CONTROLLER_NAME, Routes.TRANSFER_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ConnectionsBanksSelectReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.BANK_CONTROLLER_NAME, Routes.CONNECTIONS_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string AccountsTBankSelectReceive = Path.Combine(TransmissionQueueNamePrefix, $"T-{Routes.BANK_CONTROLLER_NAME}", Routes.ACCOUNTS_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string CustomersBanksSelectReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.BANK_CONTROLLER_NAME, Routes.CUSTOMERS_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string BanksTransfersSelectReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.BANK_CONTROLLER_NAME, Routes.TRANSFERS_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetTBankConnectionAccountsReceive = Path.Combine(TransmissionQueueNamePrefix, $"T-{Routes.BANK_CONTROLLER_NAME}", Routes.CONNECTION_CONTROLLER_NAME, Routes.ACCOUNTS_CONTROLLER_NAME, Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string BankAccountCheckReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.BANK_CONTROLLER_NAME, Routes.ACCOUNT_CONTROLLER_NAME, Routes.CHECK_ACTION_NAME, Routes.ROUTE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string IncomingTBankMerchantPaymentReceive = Path.Combine(TransmissionQueueNamePrefix, nameof(MerchantsBanksEnum.TBankCash), Routes.MERCHANT_CONTROLLER_NAME, $"{Routes.INCOMING_CONTROLLER_NAME}-{Routes.PAYMENT_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string BindCustomerTBankReceive = Path.Combine(TransmissionQueueNamePrefix, nameof(MerchantsBanksEnum.TBankCash), Routes.MERCHANT_CONTROLLER_NAME, $"{Routes.CUSTOMER_CONTROLLER_NAME}-{Routes.BIND_ACTION_NAME}");
        #endregion

        #region HelpDesk
        /// <inheritdoc/>
        public readonly static string RubricForIssuesUpdateReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, $"{Routes.ISSUE_CONTROLLER_NAME}-{Routes.RUBRIC_CONTROLLER_NAME}", Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ArticleUpdateHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, Routes.ARTICLE_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string RubricsForArticleSetReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, $"{Routes.RUBRICS_CONTROLLER_NAME}-for-{Routes.ARTICLES_CONTROLLER_NAME}", Routes.SET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string RubricsForIssuesListHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, $"{Routes.RUBRIC_CONTROLLER_NAME}-for-{Routes.ISSUE_CONTROLLER_NAME}", Routes.LIST_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string RubricForIssuesMoveHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, $"{Routes.RUBRIC_CONTROLLER_NAME}-for-{Routes.ISSUE_CONTROLLER_NAME}", Routes.MOVE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string RubricForIssuesReadHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, $"{Routes.RUBRIC_CONTROLLER_NAME}-for-{Routes.ISSUE_CONTROLLER_NAME}", Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string RubricsForIssuesGetHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, $"{Routes.RUBRIC_CONTROLLER_NAME}-for-{Routes.ISSUE_CONTROLLER_NAME}", Routes.GET_ACTION_NAME);


        /// <inheritdoc/>
        public readonly static string IssueUpdateHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, Routes.ISSUE_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string IssuesSelectHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, Routes.ISSUE_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ArticlesSelectHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, Routes.ARTICLES_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ArticlesReadReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, Routes.ARTICLES_CONTROLLER_NAME, Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ConsoleIssuesSelectHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, $"{Routes.ISSUE_CONTROLLER_NAME}-{Routes.CONSOLE_CONTROLLER_NAME}", Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string StatusChangeIssueHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, $"{Routes.STATUS_CONTROLLER_NAME}-for-{Routes.ISSUE_CONTROLLER_NAME}", Routes.CHANGE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string PulseIssuePushHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, Routes.ISSUE_CONTROLLER_NAME, Routes.PULSE_CONTROLLER_NAME, Routes.PUSH_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string IssuesGetHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, $"{Routes.ISSUES_CONTROLLER_NAME}-for-{Routes.USER_CONTROLLER_NAME}", Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string IncomingTelegramMessageHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, $"{Routes.ISSUE_CONTROLLER_NAME}-for-{Routes.TELEGRAM_CONTROLLER_NAME}", $"{Routes.MESSAGE_CONTROLLER_NAME}-{Routes.INCOMING_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string SubscribeIssueUpdateHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, $"{Routes.SUBSCRIBE_CONTROLLER_NAME}-{Routes.ISSUE_CONTROLLER_NAME}", Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string SubscribesIssueListHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, $"{Routes.SUBSCRIBE_CONTROLLER_NAME}-{Routes.ISSUE_CONTROLLER_NAME}", Routes.LIST_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string PulseJournalHelpDeskSelectReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, $"{Routes.PULSE_CONTROLLER_NAME}-{Routes.JOURNAL_CONTROLLER_NAME}", Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ExecuterIssueUpdateHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, $"{Routes.EXECUTER_CONTROLLER_NAME}-{Routes.ISSUE_CONTROLLER_NAME}", Routes.UPDATE_ACTION_NAME);


        /// <inheritdoc/>
        public readonly static string MessageOfIssueUpdateHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, $"{Routes.ISSUE_CONTROLLER_NAME}-{Routes.MESSAGE_CONTROLLER_NAME}", Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string MessageOfIssueVoteHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, $"{Routes.ISSUE_CONTROLLER_NAME}-{Routes.MESSAGE_CONTROLLER_NAME}", Routes.VOTE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string MessagesOfIssueListHelpDeskReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HELPDESK_CONTROLLER_NAME, $"{Routes.ISSUE_CONTROLLER_NAME}-{Routes.MESSAGE_CONTROLLER_NAME}", Routes.LIST_ACTION_NAME);
        #endregion

        #region Storage
        /// <inheritdoc/>
        public readonly static string SetWebConfigStorageReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TELEGRAM_CONTROLLER_NAME, $"{Routes.WEB_CONTROLLER_NAME}_{Routes.CONFIGURATION_CONTROLLER_NAME}", $"{Routes.SET_ACTION_NAME}-of-{Routes.STORAGE_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string LogsSelectStorageReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TOOLS_CONTROLLER_NAME, Routes.LOGS_ACTION_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GoToPageForRowLogsReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TOOLS_CONTROLLER_NAME, $"{Routes.LOGS_ACTION_NAME}-{Routes.PAGE_CONTROLLER_NAME}", $"{Routes.GOTO_ACTION_NAME}-for-{Routes.RECORD_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string MetadataLogsReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.TOOLS_CONTROLLER_NAME, Routes.LOGS_ACTION_NAME, Routes.METADATA_CONTROLLER_NAME);

        /// <inheritdoc/>
        public readonly static string FilesSelectReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.STORAGE_CONTROLLER_NAME, Routes.FILES_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string TagsSelectReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.STORAGE_CONTROLLER_NAME, Routes.TAGS_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string TagSetReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.STORAGE_CONTROLLER_NAME, Routes.TAG_CONTROLLER_NAME, Routes.SET_ACTION_NAME);

        /// <summary>
        /// Получить сводку (метаданные) по пространствам хранилища
        /// </summary>
        /// <remarks>
        /// Общий размер и количество группируется по AppName
        /// </remarks>
        public readonly static string FilesAreaGetMetadataReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.STORAGE_CONTROLLER_NAME, $"{Routes.FILES_CONTROLLER_NAME}-{Routes.AREAS_CONTROLLER_NAME}", $"{Routes.METADATA_CONTROLLER_NAME}-{Routes.CALCULATE_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string SaveCloudParameterReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.CLOUD_CONTROLLER_NAME, Routes.PROPERTY_CONTROLLER_NAME, Routes.SET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string DeleteCloudParameterReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.CLOUD_CONTROLLER_NAME, Routes.PROPERTY_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string SaveFileReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.CLOUD_CONTROLLER_NAME, Routes.FILE_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ReadFileReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.CLOUD_CONTROLLER_NAME, Routes.FILE_CONTROLLER_NAME, Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ReadCloudParameterReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.CLOUD_CONTROLLER_NAME, Routes.PROPERTY_CONTROLLER_NAME, Routes.READ_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ReadCloudParametersReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.CLOUD_CONTROLLER_NAME, Routes.PROPERTY_CONTROLLER_NAME, $"{Routes.READ_ACTION_NAME}-{Routes.LIST_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string FindCloudParameterReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.CLOUD_CONTROLLER_NAME, Routes.PROPERTY_CONTROLLER_NAME, Routes.FIND_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string FindCloudParametersObjectsReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.CLOUD_CONTROLLER_NAME, $"{Routes.PROPERTY_CONTROLLER_NAME}-as-{Routes.OBJECTS_CONTROLLER_NAME}", Routes.FIND_ACTION_NAME);
        #endregion

        #region Outer
        /// <inheritdoc/>
        public readonly static string LeftoversGetBreezReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.BREEZ_CONTROLLER_NAME}-{Routes.LEFTOVERS_CONTROLLER_NAME}", Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetBrandsBreezReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.BREEZ_CONTROLLER_NAME}-{Routes.BRANDS_CONTROLLER_NAME}", $"{Routes.GET_ACTION_NAME}-{Routes.CATALOG_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string GetCategoriesBreezReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.BREEZ_CONTROLLER_NAME}-{Routes.CATEGORIES_CONTROLLER_NAME}", $"{Routes.GET_ACTION_NAME}-{Routes.CATALOG_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string GetProductsBreezReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.BREEZ_CONTROLLER_NAME}-{Routes.PRODUCTS_CONTROLLER_NAME}", $"{Routes.GET_ACTION_NAME}-{Routes.CATALOG_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string GetTechCategoryBreezReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.BREEZ_CONTROLLER_NAME, $"{Routes.TECH_CONTROLLER_NAME}-{Routes.CATEGORY_CONTROLLER_NAME}", Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetTechProductBreezReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.BREEZ_CONTROLLER_NAME, $"{Routes.TECH_CONTROLLER_NAME}-{Routes.PRODUCT_CONTROLLER_NAME}", Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string DownloadAndSaveBreezReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.BREEZ_CONTROLLER_NAME}-{Routes.SYNCHRONIZATION_CONTROLLER_NAME}", $"{Routes.DOWNLOAD_ACTION_NAME}-{Routes.DATA_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string ProductUpdateBreezReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.BREEZ_CONTROLLER_NAME}-{Routes.SYNCHRONIZATION_CONTROLLER_NAME}", $"{Routes.PRODUCT_CONTROLLER_NAME}-{Routes.UPDATE_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string CategoryUpdateBreezReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.BREEZ_CONTROLLER_NAME}-{Routes.SYNCHRONIZATION_CONTROLLER_NAME}", $"{Routes.CATEGORY_CONTROLLER_NAME}-{Routes.UPDATE_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string TechCategoryUpdateBreezReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.BREEZ_CONTROLLER_NAME}-{Routes.SYNCHRONIZATION_CONTROLLER_NAME}", $"{Routes.TECH_CONTROLLER_NAME}-{Routes.CATEGORY_CONTROLLER_NAME}-{Routes.UPDATE_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string TechProductUpdateBreezReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.BREEZ_CONTROLLER_NAME}-{Routes.SYNCHRONIZATION_CONTROLLER_NAME}", $"{Routes.TECH_CONTROLLER_NAME}-{Routes.PRODUCT_CONTROLLER_NAME}-{Routes.UPDATE_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string HealthCheckBreezReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.BREEZ_CONTROLLER_NAME, $"{Routes.HEALTH_CONTROLLER_NAME}-{Routes.CHECK_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string ProductsSelectBreezReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.BREEZ_CONTROLLER_NAME, Routes.PRODUCTS_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string DownloadAndSaveDaichiReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.DAICHI_CONTROLLER_NAME}-{Routes.SYNCHRONIZATION_CONTROLLER_NAME}", $"{Routes.DOWNLOAD_ACTION_NAME}-{Routes.DATA_CONTROLLER_NAME}");
        /// <inheritdoc/>
        public readonly static string ParameterUpdateDaichiReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.DAICHI_CONTROLLER_NAME}-{Routes.SYNCHRONIZATION_CONTROLLER_NAME}", $"{Routes.PROPERTY_CONTROLLER_NAME}-{Routes.UPDATE_ACTION_NAME}");
        /// <inheritdoc/>
        public readonly static string ProductUpdateDaichiReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.DAICHI_CONTROLLER_NAME}-{Routes.SYNCHRONIZATION_CONTROLLER_NAME}", $"{Routes.PRODUCT_CONTROLLER_NAME}-{Routes.UPDATE_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string ProductsGetDaichiReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.DAICHI_CONTROLLER_NAME}-{Routes.API_CONTROLLER_NAME}", Routes.PRODUCTS_CONTROLLER_NAME, Routes.GET_ACTION_NAME);
        /// <inheritdoc/>
        public readonly static string ProductsParamsGetDaichiReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.DAICHI_CONTROLLER_NAME}-{Routes.API_CONTROLLER_NAME}", $"{Routes.PRODUCTS_CONTROLLER_NAME}-{Routes.PARAMETERS_CONTROLLER_NAME}-{Routes.GET_ACTION_NAME}");
        /// <inheritdoc/>
        public readonly static string StoresGetDaichiReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.DAICHI_CONTROLLER_NAME}-{Routes.API_CONTROLLER_NAME}", $"{Routes.STORES_CONTROLLER_NAME}-{Routes.GET_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string ProductsSelectDaichiReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DAICHI_CONTROLLER_NAME, Routes.PRODUCTS_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string HealthCheckDaichiReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.DAICHI_CONTROLLER_NAME, $"{Routes.HEALTH_CONTROLLER_NAME}-{Routes.CHECK_ACTION_NAME}");


        /// <inheritdoc/>
        public readonly static string DownloadAndSaveRusklimatReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.RUSKLIMAT_CONTROLLER_NAME}-{Routes.SYNCHRONIZATION_CONTROLLER_NAME}", $"{Routes.DOWNLOAD_ACTION_NAME}-{Routes.DATA_CONTROLLER_NAME}");
        /// <inheritdoc/>
        public readonly static string ProductUpdateRusklimatReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.RUSKLIMAT_CONTROLLER_NAME}-{Routes.SYNCHRONIZATION_CONTROLLER_NAME}", $"{Routes.PRODUCT_CONTROLLER_NAME}-{Routes.UPDATE_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string ProductsSelectRusklimatReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.RUSKLIMAT_CONTROLLER_NAME, Routes.PRODUCTS_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetUnitsRusklimatReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.RUSKLIMAT_CONTROLLER_NAME}-{Routes.API_CONTROLLER_NAME}", $"{Routes.UNITS_CONTROLLER_NAME}-{Routes.GET_ACTION_NAME}");
        /// <inheritdoc/>
        public readonly static string GetCategoriesRusklimatReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.RUSKLIMAT_CONTROLLER_NAME}-{Routes.API_CONTROLLER_NAME}", $"{Routes.CATEGORIES_CONTROLLER_NAME}-{Routes.GET_ACTION_NAME}");
        /// <inheritdoc/>
        public readonly static string GetPropertiesRusklimatReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.RUSKLIMAT_CONTROLLER_NAME}-{Routes.API_CONTROLLER_NAME}", $"{Routes.PROPERTIES_CONTROLLER_NAME}-{Routes.GET_ACTION_NAME}");
        /// <inheritdoc/>
        public readonly static string GetProductsRusklimatReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.RUSKLIMAT_CONTROLLER_NAME}-{Routes.API_CONTROLLER_NAME}", $"{Routes.PRODUCTS_CONTROLLER_NAME}-{Routes.GET_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string HealthCheckRusklimatReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.RUSKLIMAT_CONTROLLER_NAME, $"{Routes.HEALTH_CONTROLLER_NAME}-{Routes.CHECK_ACTION_NAME}");


        /// <inheritdoc/>
        public readonly static string DownloadAndSaveHaierProffReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.HAIERPROFF_CONTROLLER_NAME}-{Routes.SYNCHRONIZATION_CONTROLLER_NAME}", $"{Routes.DOWNLOAD_ACTION_NAME}-{Routes.DATA_CONTROLLER_NAME}");
        /// <inheritdoc/>
        public readonly static string ProductsFeedGetHaierProffReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.HAIERPROFF_CONTROLLER_NAME}-{Routes.RSS_CONTROLLER_NAME}", $"{Routes.PRODUCTS_CONTROLLER_NAME}-{Routes.GET_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string ProductsSelectHaierProffReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HAIERPROFF_CONTROLLER_NAME, Routes.PRODUCTS_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string HealthCheckHaierProffReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.HAIERPROFF_CONTROLLER_NAME, $"{Routes.HEALTH_CONTROLLER_NAME}-{Routes.CHECK_ACTION_NAME}");
        #endregion

        #region kladr
        /// <inheritdoc/>
        public readonly static string KladrNavigationGetObjectReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.KLADR_CONTROLLER_NAME}-{Routes.NAVIGATION_CONTROLLER_NAME}", $"{Routes.GET_ACTION_NAME}-{Routes.DATA_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string KladrNavigationListReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.KLADR_CONTROLLER_NAME}-{Routes.NAVIGATION_CONTROLLER_NAME}", Routes.LIST_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string KladrNavigationSelectReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.KLADR_CONTROLLER_NAME}-{Routes.NAVIGATION_CONTROLLER_NAME}", Routes.SELECT_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string KladrNavigationChildsContainsReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.KLADR_CONTROLLER_NAME}-{Routes.NAVIGATION_CONTROLLER_NAME}", $"{Routes.CHILDS_CONTROLLER_NAME}-{Routes.CONTAINS_ACTION_NAME}");


        /// <inheritdoc/>
        public readonly static string KladrNavigationFindReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.KLADR_CONTROLLER_NAME}-{Routes.NAVIGATION_CONTROLLER_NAME}", Routes.FIND_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ClearTempKladrReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.KLADR_CONTROLLER_NAME}-{Routes.TEMP_CONTROLLER_NAME}", Routes.CLEAR_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetMetadataKladrReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.KLADR_CONTROLLER_NAME, Routes.METADATA_CONTROLLER_NAME, Routes.CALCULATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string UploadPartTempKladrReceive = Path.Combine(TransmissionQueueNamePrefix, Routes.KLADR_CONTROLLER_NAME, Routes.TEMP_CONTROLLER_NAME, $"{Routes.UPLOAD_ACTION_NAME}-{Routes.PART_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string FlushTempKladrReceive = Path.Combine(TransmissionQueueNamePrefix, $"{Routes.KLADR_CONTROLLER_NAME}-{Routes.TEMP_CONTROLLER_NAME}", Routes.FLUSH_ACTION_NAME);
        #endregion

        #region events/notifies
        /// <inheritdoc/>
        public readonly static string ValuesChangedStockSharpNotifyReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, $"~{Routes.EVENT_CONTROLLER_NAME}-{Routes.NOTIFY_ACTION_NAME}", Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.VALUES_CONTROLLER_NAME}-{Routes.CHANGE_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string TelegramBotStartingStockSharpNotifyReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, $"~{Routes.EVENT_CONTROLLER_NAME}-{Routes.NOTIFY_ACTION_NAME}", Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.TELEGRAM_CONTROLLER_NAME}-{Routes.BOT_CONTROLLER_NAME}-{Routes.START_ACTION_NAME}");//TelegramBotStarting

        /// <inheritdoc/>
        public readonly static string UpdateConnectionStockSharpNotifyReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, $"~{Routes.EVENT_CONTROLLER_NAME}-{Routes.NOTIFY_ACTION_NAME}", Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.CONNECTION_CONTROLLER_NAME}-{Routes.STATE_CONTROLLER_NAME}-{Routes.UPDATE_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string ToastClientShowStockSharpNotifyReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, $"~{Routes.EVENT_CONTROLLER_NAME}-{Routes.NOTIFY_ACTION_NAME}", Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.CLIENT_CONTROLLER_NAME}-{Routes.TOAST_CONTROLLER_NAME}", Routes.SHOW_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string DashboardTradeUpdateStockSharpNotifyReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, $"~{Routes.EVENT_CONTROLLER_NAME}-{Routes.NOTIFY_ACTION_NAME}", Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.DASHBOARD_CONTROLLER_NAME}-{Routes.TRADE_CONTROLLER_NAME}", Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string PortfolioReceivedStockSharpNotifyReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, $"~{Routes.EVENT_CONTROLLER_NAME}-{Routes.NOTIFY_ACTION_NAME}", Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.PORTFOLIO_CONTROLLER_NAME}-{Routes.RECEIVED_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string PositionReceivedStockSharpNotifyReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, $"~{Routes.EVENT_CONTROLLER_NAME}-{Routes.NOTIFY_ACTION_NAME}", Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.POSITION_CONTROLLER_NAME}-{Routes.RECEIVED_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string OrderReceivedStockSharpNotifyReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, $"~{Routes.EVENT_CONTROLLER_NAME}-{Routes.NOTIFY_ACTION_NAME}", Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.ORDER_CONTROLLER_NAME}-{Routes.RECEIVED_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string InstrumentReceivedStockSharpNotifyReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, $"~{Routes.EVENT_CONTROLLER_NAME}-{Routes.NOTIFY_ACTION_NAME}", Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.INSTRUMENT_CONTROLLER_NAME}-{Routes.RECEIVED_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string BoardReceivedStockSharpNotifyReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, $"~{Routes.EVENT_CONTROLLER_NAME}-{Routes.NOTIFY_ACTION_NAME}", Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.BOARD_CONTROLLER_NAME}-{Routes.RECEIVED_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string OwnTradeReceivedStockSharpNotifyReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, $"~{Routes.EVENT_CONTROLLER_NAME}-{Routes.NOTIFY_ACTION_NAME}", Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.OWNTRADE_CONTROLLER_NAME}-{Routes.RECEIVED_ACTION_NAME}");
        #endregion

        #region stock-sharp
        /// <inheritdoc/>
        public readonly static string TradeInstrumentStrategyStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.TRADE_CONTROLLER_NAME}-{Routes.INSTRUMENT_CONTROLLER_NAME}-{Routes.STRATEGY_CONTROLLER_NAME}");

        /// <inheritdoc/>
        public readonly static string DisconnectStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.CONNECTION_CONTROLLER_NAME, Routes.CLOSE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string TerminateStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.CONNECTION_CONTROLLER_NAME, Routes.TERMINATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string ConnectStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.CONNECTION_CONTROLLER_NAME, Routes.OPEN_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string AboutConnectStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.CONNECTION_CONTROLLER_NAME, Routes.ABOUT_CONTROLLER_NAME);

        /// <inheritdoc/>
        public readonly static string ClearCashFlowsStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.CASH_CONTROLLER_NAME}-{Routes.FLOW_CONTROLLER_NAME}`s", Routes.CLEAR_ACTION_NAME);

        /// <summary>
        /// Получить профили по их идентификаторам
        /// </summary>
        /// <returns>Если идентификаторы не установлены, тогда возвращаются все</returns>
        public readonly static string GetPortfoliosStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.PORTFOLIOS_CONTROLLER_NAME, $"{Routes.GET_ACTION_NAME}-{Routes.LIST_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string CashFlowListStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.CASH_CONTROLLER_NAME}-{Routes.FLOW_CONTROLLER_NAME}", $"{Routes.GET_ACTION_NAME}-{Routes.LIST_ACTION_NAME}");

        /// <inheritdoc/>
        public readonly static string CashFlowDeleteStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.CASH_CONTROLLER_NAME}-{Routes.FLOW_CONTROLLER_NAME}", Routes.DELETE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string CashFlowUpdateStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.CASH_CONTROLLER_NAME}-{Routes.FLOW_CONTROLLER_NAME}", Routes.UPDATE_ACTION_NAME);

        /// <summary>
        /// Получить площадки бирж по их идентификаторам
        /// </summary>
        /// <returns>Если идентификаторы не установлены, тогда возвращаются все</returns>
        public readonly static string GetBoardsStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.BOARDS_CONTROLLER_NAME, $"{Routes.GET_ACTION_NAME}-{Routes.LIST_ACTION_NAME}");

        /// <summary>
        /// Поиск Board`s, подходящие под запрос
        /// </summary>
        public readonly static string FindBoardsStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.BOARDS_CONTROLLER_NAME, $"{Routes.FIND_ACTION_NAME}-{Routes.ELEMENTS_CONTROLLER_NAME}");

        /// <summary>
        /// Получить биржи по их идентификаторам
        /// </summary>
        /// <returns>Если идентификаторы не установлены, тогда возвращаются все</returns>
        public readonly static string GetExchangesStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.EXCHANGES_CONTROLLER_NAME, $"{Routes.GET_ACTION_NAME}-{Routes.LIST_ACTION_NAME}");

        /// <summary>
        /// Получить инструменты по их идентификаторам
        /// </summary>
        /// <returns>Если идентификаторы не установлены, тогда возвращаются все</returns>
        public readonly static string GetInstrumentsStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.INSTRUMENTS_CONTROLLER_NAME, $"{Routes.GET_ACTION_NAME}-{Routes.LIST_ACTION_NAME}");

        /// <summary>
        /// UpdateInstrumentStockSharpReceive
        /// </summary>
        public readonly static string UpdateInstrumentStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.INSTRUMENT_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <summary>
        /// Получить инструменты по их идентификаторам
        /// </summary>
        /// <returns>Если идентификаторы не установлены, тогда возвращаются все</returns>
        public readonly static string GetMarkersForInstrumentsStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.MARKERS_CONTROLLER_NAME}-for-{Routes.INSTRUMENT_CONTROLLER_NAME}", $"{Routes.GET_ACTION_NAME}-{Routes.LIST_ACTION_NAME}");

        /// <summary>
        /// InstrumentFavoriteToggleStockSharpReceive
        /// </summary>
        public readonly static string InstrumentFavoriteToggleStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.INSTRUMENTS_CONTROLLER_NAME, $"{Routes.TOGGLE_ACTION_NAME}-{Routes.FAVORITE_CONTROLLER_NAME}");

        /// <summary>
        /// OrderRegisterStockSharpReceive
        /// </summary>
        public readonly static string OrderRegisterStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.INSTRUMENTS_CONTROLLER_NAME, Routes.ORDER_CONTROLLER_NAME, Routes.REGISTRATION_ACTION_NAME);

        /// <summary>
        /// StrategyStartStockSharpDriverReceive
        /// </summary>
        public readonly static string StrategyStartStockSharpDriverReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.STRATEGY_CONTROLLER_NAME, Routes.SIMPLE_CONTROLLER_NAME, Routes.START_ACTION_NAME);

        /// <summary>
        /// StrategyStartStockSharpDriverReceive
        /// </summary>
        public readonly static string StrategyStopStockSharpDriverReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.STRATEGY_CONTROLLER_NAME, Routes.SIMPLE_CONTROLLER_NAME, Routes.STOP_ACTION_NAME);

        /// <summary>
        /// ShiftCurveStockSharpDriverReceive
        /// </summary>
        public readonly static string ShiftCurveStockSharpDriverReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.STRATEGY_CONTROLLER_NAME, Routes.CURVE_CONTROLLER_NAME, Routes.SHIFT_ACTION_NAME);

        /// <summary>
        /// LimitsStrategiesUpdateStockSharpDriverReceive
        /// </summary>
        public readonly static string LimitsStrategiesUpdateStockSharpDriverReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.STRATEGY_CONTROLLER_NAME, Routes.LIMITS_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <summary>
        /// InitialLoadStockSharpDriverReceive
        /// </summary>
        public readonly static string InitialLoadStockSharpDriverReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.STRATEGY_CONTROLLER_NAME, Routes.INITIAL_ACTION_NAME, Routes.LOAD_ACTION_NAME);

        /// <summary>
        /// ResetStrategyStockSharpDriverReceive
        /// </summary>
        public readonly static string ResetStrategyStockSharpDriverReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.STRATEGY_CONTROLLER_NAME, Routes.RESET_ACTION_NAME);

        /// <summary>
        /// ResetAllStrategiesStockSharpDriverReceive
        /// </summary>
        public readonly static string ResetAllStrategiesStockSharpDriverReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.STRATEGIES_CONTROLLER_NAME}-ALL", Routes.RESET_ACTION_NAME);

        /// <summary>
        /// Добавить/обновить адаптер
        /// </summary>
        public readonly static string UpdateOrCreateAdapterStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.ADAPTER_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <summary>
        /// Удалить адаптер
        /// </summary>
        public readonly static string DeleteAdapterStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.ADAPTER_CONTROLLER_NAME, Routes.DELETE_ACTION_NAME);

        /// <summary>
        /// Подбор адаптеров
        /// </summary>
        public readonly static string AdaptersSelectStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.ADAPTERS_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <summary>
        /// Подбор ордеров/заявок
        /// </summary>
        public readonly static string OrdersSelectStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.ORDERS_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <summary>
        /// Generate regular CashFlow`s
        /// </summary>
        public readonly static string GenerateRegularCashFlowsStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.REGULARCASHFLOWS_CONTROLLER_NAME, Routes.GENERATE_ACTION_NAME);

        /// <summary>
        /// Подбор MyTrade`s
        /// </summary>
        public readonly static string MyTradesSelectStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.MY_CONTROLLER_NAME}-{Routes.TRADES_CONTROLLER_NAME}", Routes.SELECT_ACTION_NAME);

        /// <summary>
        /// AdaptersGetStockSharpReceive
        /// </summary>
        public readonly static string AdaptersGetStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.ADAPTERS_CONTROLLER_NAME, $"{Routes.GET_ACTION_NAME}-{Routes.LIST_ACTION_NAME}");

        /// <summary>
        /// AboutDatabasesStockSharpReceive
        /// </summary>
        public readonly static string AboutDatabasesStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.DATABASES_CONTROLLER_NAME, Routes.ABOUT_CONTROLLER_NAME);

        /// <summary>
        /// InstrumentsSelectStockSharpReceive
        /// </summary>
        public readonly static string InstrumentsSelectStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.INSTRUMENTS_CONTROLLER_NAME, Routes.SELECT_ACTION_NAME);

        /// <summary>
        /// InstrumentsReadTradeStockSharpReceive
        /// </summary>
        public readonly static string InstrumentsReadTradeStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.INSTRUMENTS_CONTROLLER_NAME, $"{Routes.READ_ACTION_NAME}-of-{Routes.TRADE_CONTROLLER_NAME}");

        /// <summary>
        /// SetMarkersForInstrumentStockSharpReceive
        /// </summary>
        public readonly static string SetMarkersForInstrumentStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.MARKERS_CONTROLLER_NAME}-{Routes.INSTRUMENTS_CONTROLLER_NAME}", Routes.SET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string SaveBoardStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.TRADE_CONTROLLER_NAME}-{Routes.BOARD_CONTROLLER_NAME}", Routes.SAVE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string SaveExchangeStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.TRADE_CONTROLLER_NAME}-{Routes.EXCHANGE_CONTROLLER_NAME}", Routes.SAVE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string SaveInstrumentStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.TRADE_CONTROLLER_NAME}-{Routes.INSTRUMENT_CONTROLLER_NAME}", Routes.SAVE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string SaveOrderStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.TRADE_CONTROLLER_NAME}-{Routes.ORDER_CONTROLLER_NAME}", Routes.SAVE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string SavePortfolioStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.TRADE_CONTROLLER_NAME}-{Routes.PORTFOLIO_CONTROLLER_NAME}", Routes.SAVE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string SaveTradeStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.MY_CONTROLLER_NAME}-{Routes.TRADE_CONTROLLER_NAME}", Routes.SAVE_ACTION_NAME);

        /// <summary>
        /// Получить заказы по их идентификаторам
        /// </summary>
        /// <returns>Если идентификаторы не установлены, тогда возвращаются все</returns>
        public readonly static string GetOrdersStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.ORDERS_CONTROLLER_NAME, Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string InstrumentRubricUpdateStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, Routes.INSTRUMENT_CONTROLLER_NAME, Routes.RUBRIC_CONTROLLER_NAME, Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string RubricsInstrumentUpdateStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.RUBRICS_CONTROLLER_NAME}-for-{Routes.INSTRUMENT_CONTROLLER_NAME}", Routes.UPDATE_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetRubricsForInstrumentStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.RUBRICS_CONTROLLER_NAME}-for-{Routes.INSTRUMENT_CONTROLLER_NAME}", Routes.GET_ACTION_NAME);

        /// <inheritdoc/>
        public readonly static string GetInstrumentsForRubricStockSharpReceive = Path.Combine(TransmissionQueueNamePrefixMQTT, Routes.STOCKSHARP_CONTROLLER_NAME, $"{Routes.INSTRUMENTS_CONTROLLER_NAME}-for-{Routes.RUBRIC_CONTROLLER_NAME}", Routes.GET_ACTION_NAME);
        #endregion
    }
}