////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using DocumentFormat.OpenXml.Spreadsheet;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// [remote]: Identity
/// </summary>
public class IdentityTransmission(IRabbitClient rabbitClient) : IIdentityTransmission
{
    /// <inheritdoc/>
    public async Task<TResponseModel<string>> CheckToken2FA(CheckToken2FARequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<string>>(GlobalStaticConstants.TransmissionQueues.CheckToken2FAReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<string>> ReadToken2FA(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<string>>(GlobalStaticConstants.TransmissionQueues.ReadToken2FAReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<string>> GenerateToken2FA(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<string>>(GlobalStaticConstants.TransmissionQueues.GenerateToken2FAReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<IEnumerable<UserLoginInfoModel>>> GetUserLogins(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<IEnumerable<UserLoginInfoModel>>>(GlobalStaticConstants.TransmissionQueues.GetUserLoginsReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CheckUserPassword(IdentityPasswordModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.CheckUserPasswordReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteUserData(DeleteUserDataRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteUserDataReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool?>> UserHasPassword(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool?>>(GlobalStaticConstants.TransmissionQueues.UserHasPasswordReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool?>> GetTwoFactorEnabled(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool?>>(GlobalStaticConstants.TransmissionQueues.GetTwoFactorEnabledReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetTwoFactorEnabled(SetTwoFactorEnabledRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.SetTwoFactorEnabledReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ResetAuthenticatorKey(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.ResetAuthenticatorKeyReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> RemoveLogin(RemoveLoginRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.RemoveLoginForUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> VerifyTwoFactorToken(VerifyTwoFactorTokenRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.VerifyTwoFactorTokenReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int?>> CountRecoveryCodes(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int?>>(GlobalStaticConstants.TransmissionQueues.CountRecoveryCodesReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> GenerateChangeEmailToken(GenerateChangeEmailTokenRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.GenerateChangeEmailTokenReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<IEnumerable<string>?>> GenerateNewTwoFactorRecoveryCodes(GenerateNewTwoFactorRecoveryCodesRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<IEnumerable<string>?>>(GlobalStaticConstants.TransmissionQueues.GenerateNewTwoFactorRecoveryCodesReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<string?>> GetAuthenticatorKey(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<string?>>(GlobalStaticConstants.TransmissionQueues.GetAuthenticatorKeyReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<string?>> GeneratePasswordResetTokenAsync(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<string?>>(GlobalStaticConstants.TransmissionQueues.GeneratePasswordResetTokenReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SendPasswordResetLinkAsync(SendPasswordResetLinkRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.SendPasswordResetLinkReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ChangePassword(IdentityChangePasswordModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.ChangePasswordToUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> AddPassword(IdentityPasswordModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.AddPasswordToUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ChangeEmailAsync(IdentityEmailTokenModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.ChangeEmailForUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateUserDetails(IdentityDetailsModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.UpdateUserDetailsReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClaimDelete(ClaimAreaIdModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.ClaimDeleteReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClaimUpdateOrCreate(ClaimUpdateModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.ClaimUpdateOrCreateReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<ClaimBaseModel>> GetClaims(ClaimAreaOwnerModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<ClaimBaseModel>>(GlobalStaticConstants.TransmissionQueues.GetClaimsReceive, req, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetLockUser(IdentityBooleanModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.SetLockUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<UserInfoModel>> FindUsersAsync(FindWithOwnedRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<UserInfoModel>>(GlobalStaticConstants.TransmissionQueues.FindUsersReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ResetPassword(IdentityPasswordTokenModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.ResetPasswordForUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel>> FindUserByEmail(string email, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<UserInfoModel>>(GlobalStaticConstants.TransmissionQueues.FindUserByEmailReceive, email, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> GenerateEmailConfirmation(SimpleUserIdentityModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.GenerateEmailConfirmationIdentityReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<RegistrationNewUserResponseModel> CreateNewUser(string userEmail, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<RegistrationNewUserResponseModel>(GlobalStaticConstants.TransmissionQueues.RegistrationNewUserReceive, userEmail, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<RegistrationNewUserResponseModel> CreateNewUserWithPassword(RegisterNewUserPasswordModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<RegistrationNewUserResponseModel>(GlobalStaticConstants.TransmissionQueues.RegistrationNewUserWithPasswordReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ConfirmUserEmailCode(UserCodeModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.ConfirmUserEmailCodeIdentityReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SendEmail(SendEmailRequestModel req, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.SendEmailReceive, req, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<UserInfoModel>> SelectUsersOfIdentity(TPaginationRequestModel<SimpleBaseRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<UserInfoModel>>(GlobalStaticConstants.TransmissionQueues.SelectUsersOfIdentityReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel[]>> GetUsersIdentity(IEnumerable<string> ids_users, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<UserInfoModel[]>>(GlobalStaticConstants.TransmissionQueues.GetUsersOfIdentityReceive, ids_users, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> ClaimsUserFlush(string userIdIdentity, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.ClaimsForUserFlushReceive, userIdIdentity, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel[]>> GetUsersIdentityByEmails(IEnumerable<string> ids_emails, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<UserInfoModel[]>>(GlobalStaticConstants.TransmissionQueues.GetUsersOfIdentityByEmailReceive, ids_emails, token: token) ?? new();

    #region tg
    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel[]>> GetUserIdentityByTelegram(long[] ids_users, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<UserInfoModel[]>>(GlobalStaticConstants.TransmissionQueues.GetUsersOfIdentityByTelegramIdsReceive, ids_users, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel[]>> GetUsersIdentityByTelegram(List<long> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<UserInfoModel[]>>(GlobalStaticConstants.TransmissionQueues.GetUsersIdentityByTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TelegramAccountRemoveIdentityJoin(TelegramAccountRemoveJoinRequestIdentityModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.TelegramAccountRemoveIdentityJoinReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TelegramJoinAccountDeleteAction(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.TelegramJoinAccountDeleteActionReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TelegramJoinAccountModelDb>> TelegramJoinAccountCreate(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TelegramJoinAccountModelDb>>(GlobalStaticConstants.TransmissionQueues.TelegramJoinAccountCreateReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<TelegramUserViewModel>> FindUsersTelegram(FindRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<TelegramUserViewModel>>(GlobalStaticConstants.TransmissionQueues.FindUsersTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TelegramJoinAccountConfirmToken(TelegramJoinAccountConfirmModel req, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.TelegramJoinAccountConfirmReceive, req, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TelegramJoinAccountDelete(TelegramAccountRemoveJoinRequestTelegramModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.TelegramJoinAccountDeleteReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateTelegramMainUserMessage(MainUserMessageModel setMainMessage, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.UpdateTelegramMainUserMessageReceive, setMainMessage, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TelegramUserBaseModel>> GetTelegramUser(long telegramUserId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TelegramUserBaseModel>>(GlobalStaticConstants.TransmissionQueues.GetTelegramUserReceive, telegramUserId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TelegramJoinAccountModelDb>> TelegramJoinAccountState(TelegramJoinAccountStateRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TelegramJoinAccountModelDb>>(GlobalStaticConstants.TransmissionQueues.TelegramJoinAccountStateReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<CheckTelegramUserAuthModel>> CheckTelegramUser(CheckTelegramUserHandleModel user, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<CheckTelegramUserAuthModel>>(GlobalStaticConstants.TransmissionQueues.CheckTelegramUserReceive, user, token: token) ?? new();
    #endregion

    #region roles
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TryAddRolesToUser(UserRolesModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.TryAddRolesToUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<RoleInfoModel>> GetRole(string roleName, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<RoleInfoModel>>(GlobalStaticConstants.TransmissionQueues.GetRoleReceive, roleName, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RoleInfoModel>> FindRolesAsync(FindWithOwnedRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<RoleInfoModel>>(GlobalStaticConstants.TransmissionQueues.FindRolesAsyncReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CreateNewRole(string roleName, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.CateNewRoleReceive, roleName, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteRole(string roleName, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteRoleReceive, roleName, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteRoleFromUser(RoleEmailModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteRoleFromUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> AddRoleToUser(RoleEmailModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.AddRoleToUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<string[]>> SetRoleForUser(SetRoleForUserRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<string[]>>(GlobalStaticConstants.TransmissionQueues.SetRoleForUserOfIdentityReceive, req, token: token) ?? new();
    #endregion
}