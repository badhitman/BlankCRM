////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// [remote]: Identity
/// </summary>
public class IdentityTransmission(IRabbitClient rabbitClient) : IIdentityTransmission
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> InitChangePhoneUserAsync(TAuthRequestModel<string> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.InitChangePhoneUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ConfirmChangePhoneUserAsync(TAuthRequestModel<ChangePhoneUserRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ConfirmChangePhoneUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<string>> CheckToken2FAAsync(CheckToken2FARequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<string>>(GlobalStaticConstantsTransmission.TransmissionQueues.CheckToken2FAReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<string>> ReadToken2FAAsync(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<string>>(GlobalStaticConstantsTransmission.TransmissionQueues.ReadToken2FAReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<string>> GenerateToken2FAAsync(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<string>>(GlobalStaticConstantsTransmission.TransmissionQueues.GenerateToken2FAReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<IEnumerable<UserLoginInfoModel>>> GetUserLoginsAsync(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<IEnumerable<UserLoginInfoModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetUserLoginsReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CheckUserPasswordAsync(IdentityPasswordModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.CheckUserPasswordReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteUserDataAsync(DeleteUserDataRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteUserDataReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool?>> UserHasPasswordAsync(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool?>>(GlobalStaticConstantsTransmission.TransmissionQueues.UserHasPasswordReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool?>> GetTwoFactorEnabledAsync(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool?>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetTwoFactorEnabledReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetTwoFactorEnabledAsync(SetTwoFactorEnabledRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SetTwoFactorEnabledReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ResetAuthenticatorKeyAsync(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ResetAuthenticatorKeyReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> RemoveLoginForUserAsync(RemoveLoginRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.RemoveLoginForUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> VerifyTwoFactorTokenAsync(VerifyTwoFactorTokenRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.VerifyTwoFactorTokenReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int?>> CountRecoveryCodesAsync(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int?>>(GlobalStaticConstantsTransmission.TransmissionQueues.CountRecoveryCodesReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> GenerateChangeEmailTokenAsync(GenerateChangeEmailTokenRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.GenerateChangeEmailTokenReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<IEnumerable<string>?>> GenerateNewTwoFactorRecoveryCodesAsync(GenerateNewTwoFactorRecoveryCodesRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<IEnumerable<string>?>>(GlobalStaticConstantsTransmission.TransmissionQueues.GenerateNewTwoFactorRecoveryCodesReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<string?>> GetAuthenticatorKeyAsync(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<string?>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetAuthenticatorKeyReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<string?>> GeneratePasswordResetTokenAsync(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<string?>>(GlobalStaticConstantsTransmission.TransmissionQueues.GeneratePasswordResetTokenReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SendPasswordResetLinkAsync(SendPasswordResetLinkRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SendPasswordResetLinkReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ChangePasswordAsync(IdentityChangePasswordModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ChangePasswordToUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> AddPasswordAsync(IdentityPasswordModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.AddPasswordToUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ChangeEmailAsync(IdentityEmailTokenModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ChangeEmailForUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateUserDetailsAsync(IdentityDetailsModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.UpdateUserDetailsReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClaimDeleteAsync(ClaimAreaIdModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ClaimDeleteReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClaimUpdateOrCreateAsync(ClaimUpdateModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ClaimUpdateOrCreateReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<ClaimBaseModel>> GetClaimsAsync(ClaimAreaOwnerModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<ClaimBaseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetClaimsReceive, req, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetLockUserAsync(IdentityBooleanModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SetLockUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<UserInfoModel>> FindUsersAsync(FindWithOwnedRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<UserInfoModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.FindUsersReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ResetPasswordAsync(IdentityPasswordTokenModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ResetPasswordForUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel>> FindUserByEmailAsync(string email, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<UserInfoModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.FindUserByEmailReceive, email, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> GenerateEmailConfirmationAsync(SimpleUserIdentityModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.GenerateEmailConfirmationIdentityReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<RegistrationNewUserResponseModel> CreateNewUserEmailAsync(string userEmail, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<RegistrationNewUserResponseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.RegistrationNewUserReceive, userEmail, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<RegistrationNewUserResponseModel> CreateNewUserWithPasswordAsync(RegisterNewUserPasswordModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<RegistrationNewUserResponseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.RegistrationNewUserWithPasswordReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ConfirmUserEmailCodeAsync(UserCodeModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ConfirmUserEmailCodeIdentityReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SendEmailAsync(SendEmailRequestModel req, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SendEmailReceive, req, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<UserInfoModel>> SelectUsersOfIdentityAsync(TPaginationRequestStandardModel<SimpleBaseRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<UserInfoModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.SelectUsersOfIdentityReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel[]>> GetUsersOfIdentityAsync(string[] ids_users, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<UserInfoModel[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetUsersOfIdentityReceive, ids_users, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> ClaimsUserFlushAsync(string userIdIdentity, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.ClaimsForUserFlushReceive, userIdIdentity, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel[]>> GetUsersIdentityByEmailsAsync(string[] ids_emails, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<UserInfoModel[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetUsersOfIdentityByEmailReceive, ids_emails, token: token) ?? new();

    #region tg
    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel[]>> GetUsersIdentityByTelegramAsync(long[] ids_users, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<UserInfoModel[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetUsersOfIdentityByTelegramIdsReceive, ids_users, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel[]>> GetUsersIdentityByTelegramAsync(List<long> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<UserInfoModel[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetUsersIdentityByTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TelegramAccountRemoveIdentityJoinAsync(TelegramAccountRemoveJoinRequestIdentityModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.TelegramAccountRemoveIdentityJoinReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TelegramJoinAccountDeleteActionAsync(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.TelegramJoinAccountDeleteActionReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TelegramJoinAccountModelDb>> TelegramJoinAccountCreateAsync(string userId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TelegramJoinAccountModelDb>>(GlobalStaticConstantsTransmission.TransmissionQueues.TelegramJoinAccountCreateReceive, userId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<TelegramUserViewModel>> FindUsersTelegramAsync(SimplePaginationRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<TelegramUserViewModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.FindUsersTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TelegramJoinAccountConfirmTokenFromTelegramAsync(TelegramJoinAccountConfirmModel req, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.TelegramJoinAccountConfirmReceive, req, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TelegramAccountRemoveTelegramJoinAsync(TelegramAccountRemoveJoinRequestTelegramModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.TelegramJoinAccountDeleteReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateTelegramMainUserMessageAsync(MainUserMessageModel setMainMessage, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.UpdateTelegramMainUserMessageReceive, setMainMessage, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TelegramUserBaseModel>> GetTelegramUserCachedInfoAsync(long telegramUserId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TelegramUserBaseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetTelegramUserReceive, telegramUserId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TelegramJoinAccountModelDb>> TelegramJoinAccountStateAsync(TelegramJoinAccountStateRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TelegramJoinAccountModelDb>>(GlobalStaticConstantsTransmission.TransmissionQueues.TelegramJoinAccountStateReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<CheckTelegramUserAuthModel>> CheckTelegramUserAsync(CheckTelegramUserHandleModel user, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<CheckTelegramUserAuthModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.CheckTelegramUserReceive, user, token: token) ?? new();
    #endregion

    #region roles
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TryAddRolesToUserAsync(UserRolesModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.TryAddRolesToUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<RoleInfoModel>> GetRoleAsync(string roleName, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<RoleInfoModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetRoleReceive, roleName, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RoleInfoModel>> FindRolesAsync(FindWithOwnedRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<RoleInfoModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.FindRolesAsyncReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CreateNewRoleAsync(string roleName, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.CateNewRoleReceive, roleName, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteRoleAsync(string roleName, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteRoleReceive, roleName, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteRoleFromUserAsync(RoleEmailModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteRoleFromUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> AddRoleToUserAsync(RoleEmailModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.AddRoleToUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<string[]>> SetRoleForUserAsync(SetRoleForUserRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<string[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.SetRoleForUserOfIdentityReceive, req, token: token) ?? new();
#endregion
}