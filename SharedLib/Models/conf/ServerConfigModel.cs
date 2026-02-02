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
    public InjectionBaseModel[]? StylesInject { get; set; }

    /// <summary>
    /// Скрипты для вставки в сайт
    /// </summary>
    public InjectionBaseModel[]? ScriptsInject { get; set; }

    /// <summary>
    /// Рекомендуемый курсы конвертаций/переводов
    /// </summary>
    public WalletTypeConversionExchangeModel[]? ExchangeRateConversions { get; set; }

    /// <summary>
    /// Скрыть выбор языка
    /// </summary>
    public bool HideLanguageSelector { get; set; }

    /// <summary>
    /// WebChatEnable
    /// </summary>
    public bool WebChatEnable { get; set; }

    /// <summary>
    /// Скрыть навигацию для входа/авторизации
    /// </summary>
    public bool HideAuthArea { get; set; }

    /// <summary>
    /// Папки (или файлы) для отображения в инструментах
    /// </summary>
    public string[]? DirectoriesReadList { get; set; }
}