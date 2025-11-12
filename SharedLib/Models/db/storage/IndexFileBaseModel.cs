////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// IndexFileBaseModel
/// </summary>
public class IndexFileBaseModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// StoreFile
    /// </summary>
    public StorageFileModelDB? StoreFile { get; set; }

    /// <summary>
    /// StoreFile
    /// </summary>
    public int StoreFileId { get; set; }
}