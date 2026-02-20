////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Altnames содержит сведения о соответствии кодов старых и новых наименований (обозначений домов) в случаях переподчинения 
/// и “сложного” переименования адресных объектов (когда коды записей со старым и новым наименованиями не совпадают).
/// </summary>
/// <remarks>
/// Возможные варианты “сложного” переименования:
/// улица разделилась на несколько новых улиц;
/// несколько улиц объединились в одну новую улицу;
/// населенный пункт стал улицей другого города(населенного пункта);
/// улица населенного пункта стала улицей другого города(населенного пункта).
/// В этих случаях производятся следующие действия:
/// вводятся новые объекты в файлы Kladr.dbf, Street.dbf и Doma.dbf;
/// старые объекты переводятся в разряд неактуальных;
/// в файл Altnames вводятся записи, содержащие соответствие старых и новых кодов адресных объектов.
/// </remarks>
public class AltnameKLADRModel
{
    /// <inheritdoc/>
    [Required, StringLength(19)]
    public required string OLDCODE { get; set; }

    /// <inheritdoc/>
    [Required, StringLength(19)]
    public required string NEWCODE { get; set; }

    /// <inheritdoc/>
    [Required, StringLength(1)]
    public required string LEVEL { get; set; }

    /// <inheritdoc/>
    public static TResponseModel<List<AltnameKLADRModel>> Build(List<FieldDescriptorBase> columns, List<object[]> rowsData)
    {
        TResponseModel<List<AltnameKLADRModel>> res = new();

        foreach (System.Reflection.PropertyInfo p in typeof(AltnameKLADRModel).GetProperties().Where(x => !columns.Any(x => x.FieldName.Equals(x.FieldName, StringComparison.OrdinalIgnoreCase))))
            res.AddError($"!columns.Any(x => x.FieldName.Equals(nameof({p.Name})))");

        if (rowsData.Any(x => x.Length != columns.Count))
            res.AddError("rowsData.Any(x=>x.Length != columns.Length)");

        if (!res.Success())
            return res;

        res.Response ??= [];
        foreach (object[] r in rowsData)
        {
            res.Response.Add(new()
            {
                OLDCODE = r[columns.FindIndex(x => x.FieldName.Equals(nameof(OLDCODE), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
                NEWCODE = r[columns.FindIndex(x => x.FieldName.Equals(nameof(NEWCODE), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
                LEVEL = r[columns.FindIndex(x => x.FieldName.Equals(nameof(LEVEL), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
            });
        }

        return res;
    }
}