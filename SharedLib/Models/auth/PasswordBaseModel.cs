////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// PasswordBaseModel
/// </summary>
public class PasswordBaseModel
{
    /// <summary>
    /// Пароль пользователя для авторизации
    /// </summary>
    [StringLength(100, ErrorMessage = "Длина {0} должна быть не менее {2} и не более {1} символов.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Повтор пароля пользователя
    /// </summary>
    [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердите пароль")]
    public string ConfirmPassword { get; set; } = string.Empty;
}