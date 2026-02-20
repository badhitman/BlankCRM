////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// DaichiBusinessStoreModelDB
/// </summary>
public class StoreDaichiModelDB : StoreInfoDaichiModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public bool IsDisabled { get; set; }

    /// <inheritdoc/>
    public static StoreDaichiModelDB Build(StoreInfoDaichiModel x)
    {
        return new()
        {
            NAME = x.NAME,
            XML_ID = x.XML_ID,
            IS_DEFAULT = x.IS_DEFAULT,
        };
    }
}