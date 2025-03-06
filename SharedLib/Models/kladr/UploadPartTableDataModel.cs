////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Загрузка порции данных
/// </summary>
public class UploadPartTableDataModel
{
    /// <summary>
    /// Columns
    /// </summary>
    public required FieldDescriptorBase[] Columns { get; set; }

    /// <summary>
    /// RowsData
    /// </summary>
    public required List<object[]> RowsData { get; set; }

    /// <summary>
    /// Имя таблицы
    /// </summary>
    public required string TableName { get; set; }
    
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
    public HouseKLADRModel[]? HousesPart { get; set; }
    */
}