////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Меню (основное)
/// </summary>
public class NavMainMenuModel
{
    /// <inheritdoc/>
    public event ThemeChangeHandler? Notify;

    /// <summary>
    /// Элементы меню (верхнее)
    /// </summary>
    public NavItemModel[]? TopNavMenuItems { get; set; }

    /// <summary>
    /// Элементы меню (слева)
    /// </summary>
    public NavItemModel[]? NavMenuItems { get; set; }

    /// <summary>
    /// Элементы меню (нижнего/второстепенного/дополнительного)
    /// </summary>
    /// <remarks>
    /// Для отладки. Подключение второго меню, которое было создано генератором
    /// </remarks>
    public NavItemModel[]? BottomNavMenuItems { get; set; }

    /// <inheritdoc/>
    public void ThemeChangeHandle(bool darkMode)
    {
        if (Notify is not null)
            Notify(darkMode);
    }
}
/// <summary>
/// ThemeChangeHandler
/// </summary>
public delegate void ThemeChangeHandler(bool darkMode);