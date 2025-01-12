﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;
using Transmission.Receives.Identity;

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
            .RegisterMqListener<ClaimsUserFlushReceive, string, TResponseModel<bool>>()
            .RegisterMqListener<GetUsersIdentityByEmailReceive, string[], TResponseModel<UserInfoModel[]>>()
            .RegisterMqListener<GetUserIdentityByTelegramReceive, long[], TResponseModel<UserInfoModel[]>>()
            .RegisterMqListener<GetUsersOfIdentityReceive, string[], TResponseModel<UserInfoModel[]>>()
            .RegisterMqListener<SendEmailReceive, SendEmailRequestModel, ResponseBaseModel>()
            .RegisterMqListener<GetRoleReceive, string, TResponseModel<RoleInfoModel>>()
            .RegisterMqListener<SetLockUserReceive, IdentityBooleanModel, ResponseBaseModel>()
            .RegisterMqListener<FindUsersReceive, FindWithOwnedRequestModel, TPaginationResponseModel<UserInfoModel>>()
            .RegisterMqListener<FindRolesAsyncReceive, FindWithOwnedRequestModel, TPaginationResponseModel<RoleInfoModel>>()
            .RegisterMqListener<CateNewRoleReceive, string, ResponseBaseModel>()
            .RegisterMqListener<DeleteRoleReceive, string, ResponseBaseModel>()
            .RegisterMqListener<DeleteRoleFromUserReceive, RoleEmailModel, ResponseBaseModel>()
            .RegisterMqListener<AddRoleToUserReceive, RoleEmailModel, ResponseBaseModel>()
            .RegisterMqListener<ResetPasswordReceive, IdentityPasswordTokenModel, ResponseBaseModel>()
            .RegisterMqListener<FindUserByEmailReceive, string, TResponseModel<UserInfoModel>>()
            .RegisterMqListener<CreateNewUserReceive, string, RegistrationNewUserResponseModel>()
            .RegisterMqListener<CreateNewUserWithPasswordReceive, RegisterNewUserPasswordModel, RegistrationNewUserResponseModel>()
            .RegisterMqListener<ConfirmUserEmailCodeIdentityReceive, UserCodeModel, ResponseBaseModel>()
            .RegisterMqListener<SetRoleForUserReceive, SetRoleFoeUserRequestModel, TResponseModel<string[]>>()
            .RegisterMqListener<SelectUsersOfIdentityReceive, TPaginationRequestModel<SimpleBaseRequestModel>, TPaginationResponseModel<UserInfoModel>>()
            ;
    }
}