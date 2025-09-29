﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Метаданные файла
/// </summary>
public class ToolsFilesResponseModel
{
    /// <summary>
    /// FullName File
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// ScopeName File
    /// </summary>
    public required string ScopeName { get; set; }

    /// <summary>
    /// SafeScopeName
    /// </summary>
    public string SafeScopeName { get 
        {
            string _res = ScopeName;

            while (_res.StartsWith('\\') || _res.StartsWith('/'))
                _res = _res[1..];

            return _res;
        } 
    }

    /// <summary>
    /// Size File
    /// </summary>
    public required long Size { get; set; }

    /// <summary>
    /// Hash File
    /// </summary>
    public string? Hash { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{GlobalToolsStandard.SizeDataAsString(Size)} - {ScopeName}";
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is ToolsFilesResponseModel _t)
            return _t.SafeScopeName == SafeScopeName && _t.Size == Size && (_t.Hash == Hash || (string.IsNullOrWhiteSpace(_t.Hash) && string.IsNullOrWhiteSpace(Hash)));

        return false;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return $"{Size} {ScopeName}".GetHashCode();
    }
}