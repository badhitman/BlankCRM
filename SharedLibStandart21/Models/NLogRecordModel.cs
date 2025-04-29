////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// NLogRecordModel
/// </summary>
public class NLogRecordModel
{
    /// <inheritdoc/>
    public string? ContextPrefix { get; set; }

    /// <summary>
    /// RecordTime
    /// </summary>
    public DateTime RecordTime { get; set; }

    /// <summary>
    /// RecordLevel
    /// </summary>
    public virtual string? RecordLevel { get; set; }

    /// <inheritdoc/>
    public string? RecordMessage { get; set; }

    /// <inheritdoc/>
    public string? ExceptionMessage { get; set; }

    /// <inheritdoc/>
    public string? Logger { get; set; }

    /// <inheritdoc/>
    public string? CallSite { get; set; }

    /// <inheritdoc/>
    public string? StackTrace { get; set; }

    /// <inheritdoc/>
    public string? AllEventProperties { get; set; }
}