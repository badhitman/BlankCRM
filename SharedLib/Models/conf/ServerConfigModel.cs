////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Серверные конфигурации
/// </summary>
public class ServerConfigModel
{
    /// <summary>
    /// Конфигурация web сервера
    /// </summary>
    public BackendConfigModel KestrelConfig { get; set; } = new BackendConfigModel();

    /// <summary>
    /// Конфигурация reCaptcha
    /// </summary>
    public ReCaptchaConfigClientModel? ReCaptchaConfig { get; set; }

    /// <summary>
    /// Стили для вставки в сайт
    /// </summary>
    public StyleInjectionModel[]? StylesInject { get; set; }

    /// <summary>
    /// Скрипты для вставки в сайт
    /// </summary>
    public ScriptInjectionModel[]? ScriptsInject { get; set; }

    /// <summary>
    /// Скрыть выбор языка
    /// </summary>
    public bool HideLanguageSelector { get; set; }

    /// <summary>
    /// Скрыть навигацию для входа/авторизации
    /// </summary>
    public bool HideAuthArea { get; set; }
}

/// <summary>
/// StyleInjectionModel
/// </summary>
public class StyleInjectionModel()
{
    /// <inheritdoc/>
    public required string Href { get; set; }
}

/// <summary>
/// ScriptInjectionModel
/// </summary>
public class ScriptInjectionModel()
{
    /// <inheritdoc/>
    public required string Src { get; set; }
}