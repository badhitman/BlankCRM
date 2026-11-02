using IdentityLib;
using Microsoft.AspNetCore.Identity;
using SharedLib;

namespace ServerLib;

/// <summary>
/// Инфраструктура ASP.NET Core Identity (не предназначено для использования в качестве абстракции электронной почты общего назначения).
/// Отправка электронных писем с подтверждением и сбросом пароля.
/// This API supports the ASP.NET Core Identity infrastructure and is not intended to be used as a general purpose email abstraction. It should be implemented by the application so the Identity infrastructure can send confirmation and password reset emails.
/// </summary>
public sealed class IdentityEmailSender(IMailProviderService emailSender) : IEmailSender<ApplicationUser>
{
    /// <inheritdoc/>
    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
        emailSender.SendEmailAsync(email, "Confirm your email address", $"Please verify your account <a href='{confirmationLink}'>by clicking on the link</a>.");
    /// <inheritdoc/>

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
        emailSender.SendEmailAsync(email, "Password reset", $"To reset your password - <a href='{resetLink}'>clicking on the link</a>.");
    /// <inheritdoc/>

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
        emailSender.SendEmailAsync(email, "Password reset", $"Please reset your password using the following code: {resetCode}");
}