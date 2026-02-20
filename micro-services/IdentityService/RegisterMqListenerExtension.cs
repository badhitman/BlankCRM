////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using IdentityService.Services.Receives.users;
using Transmission.Receives.Identity;
using Transmission.Receives.web;
using SharedLib;

namespace IdentityService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection IdentityRegisterMqListeners(this IServiceCollection services)
    {
        return services
            .RegisterListenerRabbitMQ<ClaimsUserFlushReceive, string, TResponseModel<bool>>()
            .RegisterListenerRabbitMQ<GetUsersIdentityByEmailReceive, string[], TResponseModel<UserInfoModel[]?>>()
            .RegisterListenerRabbitMQ<GetUserIdentityByTelegramReceive, long[], TResponseModel<UserInfoModel[]?>>()
            .RegisterListenerRabbitMQ<GetUsersOfIdentityReceive, string[], TResponseModel<UserInfoModel[]?>>()
            .RegisterListenerRabbitMQ<SendEmailReceive, SendEmailRequestModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<ReadToken2FAReceive, string, TResponseModel<string?>>()
            .RegisterListenerRabbitMQ<CheckToken2FAReceive, CheckToken2FARequestModel, TResponseModel<string>>()
            .RegisterListenerRabbitMQ<GenerateOTPFor2StepVerificationReceive, string, TResponseModel<string>>()
            .RegisterListenerRabbitMQ<GetUserLoginsReceive, string, TResponseModel<IEnumerable<UserLoginInfoModel>>>()
            .RegisterListenerRabbitMQ<CheckUserPasswordReceive, IdentityPasswordModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<AddPasswordForUserReceive, IdentityPasswordModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<CreateUserManualReceive, TAuthRequestStandardModel<UserInfoBaseModel>, TResponseModel<string>>()
            .RegisterListenerRabbitMQ<UserHasPasswordReceive, string, TResponseModel<bool?>>()
            .RegisterListenerRabbitMQ<GetTwoFactorEnabledReceive, string, TResponseModel<bool?>>()
            .RegisterListenerRabbitMQ<SetTwoFactorEnabledReceive, SetTwoFactorEnabledRequestModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<RemoveLoginReceive, RemoveLoginRequestModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<GetRoleReceive, string, TResponseModel<RoleInfoModel>>()
            .RegisterListenerRabbitMQ<VerifyTwoFactorTokenReceive, VerifyTwoFactorTokenRequestModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<CountRecoveryCodesReceive, string, TResponseModel<int?>>()
            .RegisterListenerRabbitMQ<GenerateNewTwoFactorRecoveryCodesReceive, GenerateNewTwoFactorRecoveryCodesRequestModel, TResponseModel<IEnumerable<string>?>>()
            .RegisterListenerRabbitMQ<GetAuthenticatorKeyReceive, string, TResponseModel<string?>>()
            .RegisterListenerRabbitMQ<GeneratePasswordResetTokenReceive, string, TResponseModel<string?>>()
            .RegisterListenerRabbitMQ<CheckTelegramUserReceive, CheckTelegramUserHandleModel, TResponseModel<CheckTelegramUserAuthModel>>()
            .RegisterListenerRabbitMQ<GetUsersIdentityByTelegramReceive, long[], TResponseModel<UserInfoModel[]>>()
            .RegisterListenerRabbitMQ<TelegramAccountRemoveIdentityJoinReceive, TAuthRequestStandardModel<TelegramAccountRemoveJoinRequestIdentityModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<TelegramJoinAccountCreateReceive, string, TResponseModel<TelegramJoinAccountModelDb>>()
            .RegisterListenerRabbitMQ<FindUsersTelegramReceive, SimplePaginationRequestStandardModel, TPaginationResponseStandardModel<TelegramUserViewModel>>()
            .RegisterListenerRabbitMQ<TelegramJoinAccountStateReceive, TelegramJoinAccountStateRequestModel, TResponseModel<TelegramJoinAccountModelDb>>()
            .RegisterListenerRabbitMQ<ClaimUpdateOrCreateReceive, ClaimUpdateModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<ClaimDeleteReceive, ClaimAreaIdModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<TelegramJoinAccountDeleteActionReceive, string, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<SendPasswordResetLinkReceive, SendPasswordResetLinkRequestModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<TryAddRolesToUserReceive, UserRolesModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<ChangePasswordForUserReceive, IdentityChangePasswordModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<ConfirmChangePhoneUserReceive, TAuthRequestStandardModel<ChangePhoneUserRequestModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<InitChangePhoneUserReceive, TAuthRequestStandardModel<string>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<ChangeEmailForUserReceive, IdentityEmailTokenModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<UpdateUserDetailsReceive, TAuthRequestStandardModel<IdentityDetailsModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<GetClaimsReceive, ClaimAreaOwnerModel, List<ClaimBaseModel>>()
            .RegisterListenerRabbitMQ<SetLockUserReceive, TAuthRequestStandardModel<IdentityBooleanModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<FindUsersReceive, FindWithOwnedRequestModel, TPaginationResponseStandardModel<UserInfoModel>>()
            .RegisterListenerRabbitMQ<FindRolesAsyncReceive, FindWithOwnedRequestModel, TPaginationResponseStandardModel<RoleInfoModel>>()
            .RegisterListenerRabbitMQ<CreateNewRoleReceive, string, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<TelegramJoinAccountConfirmReceive, TelegramJoinAccountConfirmModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<TelegramJoinAccountDeleteReceive, TelegramAccountRemoveJoinRequestTelegramModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<UpdateTelegramMainUserMessageReceive, MainUserMessageModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<GetTelegramUserReceive, long, TResponseModel<TelegramUserBaseModel?>>()
            .RegisterListenerRabbitMQ<DeleteRoleReceive, string, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<DeleteRoleFromUserReceive, TAuthRequestStandardModel<RoleEmailModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<AddRoleToUserReceive, TAuthRequestStandardModel<RoleEmailModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<ResetPasswordReceive, IdentityPasswordTokenModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<FindUserByEmailReceive, string, TResponseModel<UserInfoModel>>()
            .RegisterListenerRabbitMQ<CreateNewUserReceive, string, RegistrationNewUserResponseModel>()
            .RegisterListenerRabbitMQ<CreateNewUserWithPasswordReceive, RegisterNewUserPasswordModel, RegistrationNewUserResponseModel>()
            .RegisterListenerRabbitMQ<ConfirmUserEmailCodeIdentityReceive, UserCodeModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<SetRoleForUserReceive, TAuthRequestStandardModel<SetRoleForUserRequestModel>, TResponseModel<string[]>>()
            .RegisterListenerRabbitMQ<SelectUsersOfIdentityReceive, TPaginationRequestStandardModel<SimpleBaseRequestModel>, TPaginationResponseStandardModel<UserInfoModel>>()
            ;
    }
}