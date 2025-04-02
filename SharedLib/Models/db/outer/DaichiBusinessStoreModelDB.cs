////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// DaichiBusinessStoreModelDB
/// </summary>
[Index(nameof(LoadedDateTime))]
public class DaichiBusinessStoreModelDB : StoreInfoDaichiModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// LoadedDateTime
    /// </summary>
    public DateTime LoadedDateTime { get; set; }

    /// <inheritdoc/>
    public static DaichiBusinessStoreModelDB Build(StoreInfoDaichiModel x)
    {
        return new()
        {
            LoadedDateTime = DateTime.UtcNow,
            IS_DEFAULT = x.IS_DEFAULT,
            NAME = x.NAME,
            XML_ID = x.XML_ID,
        };
    }
}