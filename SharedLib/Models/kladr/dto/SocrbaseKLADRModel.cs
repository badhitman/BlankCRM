////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <inheritdoc/>
public class SocrbaseKLADRModel
{
    /// <inheritdoc/>
    [Required, StringLength(5)]
    public required string LEVEL { get; set; }

    /// <inheritdoc/>
    [Required, StringLength(10)]
    public required string SCNAME { get; set; }

    /// <inheritdoc/>
    [Required, StringLength(29)]
    public required string SOCRNAME { get; set; }

    /// <inheritdoc/>
    [Required, StringLength(3)]
    public required string KOD_T_ST { get; set; }

    /// <inheritdoc/>
    public static TResponseModel<List<SocrbaseKLADRModel>> Build(List<FieldDescriptorBase> columns, List<object[]> rowsData)
    {
        TResponseModel<List<SocrbaseKLADRModel>> res = new();

        foreach (System.Reflection.PropertyInfo p in typeof(SocrbaseKLADRModel).GetProperties().Where(x => !columns.Any(x => x.FieldName.Equals(x.FieldName, StringComparison.OrdinalIgnoreCase))))
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
                KOD_T_ST = r[columns.FindIndex(x => x.FieldName.Equals(nameof(KOD_T_ST), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
                LEVEL = r[columns.FindIndex(x => x.FieldName.Equals(nameof(LEVEL), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
                SCNAME = r[columns.FindIndex(x => x.FieldName.Equals(nameof(SCNAME), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
                SOCRNAME = r[columns.FindIndex(x => x.FieldName.Equals(nameof(SOCRNAME), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
            });
        }

        return res;
    }
}