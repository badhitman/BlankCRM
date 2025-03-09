////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Metadata Kladr
/// </summary>
public class MetadataKladrModel : ResponseBaseModel
{
    /// <summary>
    /// Altnames
    /// </summary>
    public int AltnamesCount { get; set; }

    /// <summary>
    /// Names
    /// </summary>
    public int NamesCount { get; set; }

    /// <summary>
    /// Objects KLADR
    /// </summary>
    public int ObjectsCount { get; set; }

    /// <summary>
    /// Socrbases
    /// </summary>
    public int SocrbasesCount { get; set; }

    /// <summary>
    /// Streets
    /// </summary>
    public int StreetsCount { get; set; }

    /// <summary>
    /// Doma
    /// </summary>
    public int DomaCount { get; set; }

    /// <summary>
    /// RegistersJobs
    /// </summary>
    public RegisterJobTempKladrModelDB[]? RegistersJobs { get; set; }
}