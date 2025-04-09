////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////



namespace SharedLib;

/// <summary>
/// Дом
/// </summary>
public class HouseKLADRModel : RootKLADRModel
{
    /// <inheritdoc/>
    public required string KORP { get; set; }

    /// <inheritdoc/>
    public static new TResponseModel<List<HouseKLADRModel>> Build(List<FieldDescriptorBase> columns, List<object[]> rowsData)
    {
        TResponseModel<List<HouseKLADRModel>> res = new();

        foreach (System.Reflection.PropertyInfo p in typeof(HouseKLADRModel).GetProperties().Where(x => !columns.Any(x => x.FieldName.Equals(x.FieldName, StringComparison.OrdinalIgnoreCase))))
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
                CODE = r[columns.FindIndex(x => x.FieldName.Equals(nameof(CODE), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
                GNINMB = r[columns.FindIndex(x => x.FieldName.Equals(nameof(GNINMB), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
                INDEX = r[columns.FindIndex(x => x.FieldName.Equals(nameof(INDEX), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
                KORP = r[columns.FindIndex(x => x.FieldName.Equals(nameof(KORP), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
                NAME = r[columns.FindIndex(x => x.FieldName.Equals(nameof(NAME), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
                OCATD = r[columns.FindIndex(x => x.FieldName.Equals(nameof(OCATD), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
                SOCR = r[columns.FindIndex(x => x.FieldName.Equals(nameof(SOCR), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
                UNO = r[columns.FindIndex(x => x.FieldName.Equals(nameof(UNO), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
            });
        }
         
        return res;
    }
}