﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// Found parameter
/// </summary>
public class FoundParameterModel
{
    /// <summary>
    /// Префикс имени (опционально)
    /// </summary>
    public string? PrefixPropertyName { get; set; }

    /// <summary>
    /// Связанный PK строки базы данных (опционально)
    /// </summary>
    public int? OwnerPrimaryKey { get; set; }

    /// <summary>
    /// Данные (сериализованные)
    /// </summary>
    public string? SerializedDataJson { get; set; }

    /// <summary>
    /// Создание
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Имя тип данных (тип хранимого значения)
    /// </summary>
    public string? TypeName { get; set; }
}