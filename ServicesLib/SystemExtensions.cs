////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.Extensions.Configuration;
using NLog;

namespace ServicesLib;

/// <inheritdoc/>
public static partial class SystemExtensions
{
    static readonly Logger logger = LogManager.GetCurrentClassLogger();

    /// <inheritdoc/>
    public static void ReadSecrets(this ConfigurationManager builder, string dirName)
    {
        string secretPath = Path.Combine("..", dirName);
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
                builder.AddJsonFile(path_load, optional: true, reloadOnChange: false);
            }
        }
        else
            logger.Debug($"секреты `{dirName}` не найдены (совсем)");
    }
}