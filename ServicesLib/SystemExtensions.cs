////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog;
using SharedLib;

namespace ServicesLib;

/// <inheritdoc/>
public static partial class SystemExtensions
{
    static readonly Logger logger = LogManager.GetCurrentClassLogger();

    /// <inheritdoc/>
    public static void ReadSecrets(this ConfigurationManager confManager, string secretDirName)
    {
        string secretPath = Path.Combine("..", secretDirName);
        DirectoryInfo di = new(secretPath);
        string lastDI = di.FullName;
        for (int i = 0; i < 5 && !di.Exists; i++)
        {
            logger.Debug($"файл секретов не найден (продолжение следует...): {di.FullName}");
            secretPath = Path.Combine("..", secretPath);
            di = new(secretPath);

            if (lastDI.Equals(di.FullName))
                break;

            lastDI = di.FullName;
        }

        if (Directory.Exists(secretPath))
        {
            foreach (string secret in Directory.GetFiles(secretPath, $"*.json"))
            {
                string path_load = Path.GetFullPath(secret);
                logger.Debug($"!secret load: {path_load}");
                confManager.AddJsonFile(path_load, optional: true, reloadOnChange: false);
            }
        }
        else
            logger.Debug($"секреты `{secretDirName}` не найдены (совсем)");
    }

    /// <inheritdoc/>
    public static string ReadBaseOptions(this ConfigurationManager confManager, string[] args, string dotnetEnvironment, string prefixSecrets = "")
    {
        logger.Warn($"init main: {dotnetEnvironment}");
        string _modePrefix = Environment.GetEnvironmentVariable(nameof(GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix)) ?? "";
        if (!string.IsNullOrWhiteSpace(_modePrefix) && !GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix.EndsWith(_modePrefix))
        {
            GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix += _modePrefix.Trim();
            GlobalStaticConstantsTransmission.TransmissionQueueNamePrefixMQTT += _modePrefix.Trim();
        }
        logger.Warn($"{nameof(GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix)}: {Environment.GetEnvironmentVariable(nameof(GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix))}");

        string currentDirName = Directory.GetCurrentDirectory();

        confManager.SetBasePath(currentDirName);
        string path_load = Path.Combine(currentDirName, "appsettings.json");
        if (Path.Exists(path_load))
            confManager.AddJsonFile(path_load, optional: true, reloadOnChange: false);

        path_load = Path.Combine(currentDirName, $"appsettings.{dotnetEnvironment}.json");
        if (Path.Exists(path_load))
            confManager.AddJsonFile(path_load, optional: true, reloadOnChange: false);
        else
            logger.Debug($"отсутствует: {path_load}");


        confManager.ReadSecrets($"secrets{prefixSecrets}");
        if (!string.IsNullOrWhiteSpace(_modePrefix))
            confManager.ReadSecrets($"secrets{prefixSecrets}{_modePrefix}");


        confManager.AddEnvironmentVariables();
        confManager.AddCommandLine(args);
        return _modePrefix;
    }
}