////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;

namespace SharedLib;

/// <summary>
/// Метаданные логов: доступные типы данных и общая статистика по ним
/// </summary>
public class LogsMetadataResponseModel : PeriodDatesTimesModel
{
    /// <inheritdoc/>
    public Dictionary<string, int> LevelsAvailable { get; set; }

    /// <inheritdoc/>
    public Dictionary<string, int> ApplicationsAvailable { get; set; }

    /// <inheritdoc/>
    public Dictionary<string, int> ContextsPrefixesAvailable { get; set; }

    /// <inheritdoc/>
    public Dictionary<string, int> LoggersAvailable { get; set; }
}