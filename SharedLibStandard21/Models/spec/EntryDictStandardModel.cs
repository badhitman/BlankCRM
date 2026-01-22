////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLib;

/// <summary>
/// Простой вещественный тип (id:int;name:string) + полезная нагрузка через Tag словарь
/// </summary>
public class EntryDictStandardModel : EntryStandardModel
{
    /// <summary>
    /// Дополнительная полезная нагрузка ответа
    /// </summary>
    [NotMapped]
    public Dictionary<string, object>? Tag { get; set; }
}