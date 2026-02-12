////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// FileGoodsConfigModelDB
/// </summary>
[Index(nameof(OwnerId), nameof(OwnerTypeName), IsUnique = true), Index(nameof(IsGallery))]
public class FileGoodsConfigModelDB : EntrySwitchableModel
{
    /// <summary>
    /// владелец
    /// </summary>
    public required int OwnerId { get; set; }
    /// <summary>
    /// владелец
    /// </summary>
    public required int OwnerTypeName { get; set; }

    /// <summary>
    /// Описание/примечание для объекта
    /// </summary>
    public string? ShortDescription { get; set; }

    /// <summary>
    /// Описание/примечание для объекта
    /// </summary>
    public string? FullDescription { get; set; }

    /// <summary>
    /// IsGallery
    /// </summary>
    public bool IsGallery { get; set; }
}
