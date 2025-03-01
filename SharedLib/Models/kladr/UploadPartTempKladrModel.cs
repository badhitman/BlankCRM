////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Загрузка порции данных КЛАДР 4.0
/// </summary>
public class UploadPartTempKladrModel
{
    /// <summary>
    /// Columns
    /// </summary>
    public FieldDescriptorBase[]? Columns { get; set; }

    /// <summary>
    /// RowsData
    /// </summary>
    public List<object[]>? RowsData { get; set; }
    /*
    /// <summary>
    /// Altnames
    /// </summary>
    public AltnameKLADRModel[]? AltnamesPart { get; set; }

    /// <summary>
    /// Names
    /// </summary>
    public NameMapKLADRModel[]? NamesPart { get; set; }

    /// <summary>
    /// Objects KLADR
    /// </summary>
    public ObjectKLADRModel[]? ObjectsPart { get; set; }

    /// <summary>
    /// Socrbases
    /// </summary>
    public SocrbaseKLADRModel[]? SocrbasesPart { get; set; }

    /// <summary>
    /// Streets
    /// </summary>
    public RootKLADRModel[]? StreetsPart { get; set; }

    /// <summary>
    /// Houses
    /// </summary>
    public HouseKLADRModel[]? HousesPart { get; set; }*/
}