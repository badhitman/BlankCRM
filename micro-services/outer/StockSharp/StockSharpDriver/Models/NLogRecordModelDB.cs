////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// NLog record
/// </summary>
[Index(nameof(RecordTime)), Index(nameof(ApplicationName)), Index(nameof(ContextPrefix)), Index(nameof(RecordLevel)), Index(nameof(Logger))]
public class NLogRecordModelDB : NLogRecordModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// ApplicationName
    /// </summary>
    public string ApplicationName { get; set; }

    /// <inheritdoc/>
    public static bool operator ==(NLogRecordModelDB L1, NLogRecordModelDB L2)
    {
        return L1.Id == L2.Id;
    }

    /// <inheritdoc/>
    public static bool operator !=(NLogRecordModelDB L1, NLogRecordModelDB L2)
    {
        return L1.Id != L2.Id;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        if (obj is NLogRecordModelDB _el)
            return Id == _el.Id;

        return false;
    }
}