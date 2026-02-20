////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <inheritdoc/>
public class RootKLADRModel : BaseKladrScopedModel
{
    /// <inheritdoc/>
    [Required, StringLength(10)]
    public required string SOCR { get; set; }

    /// <inheritdoc/>
    [Required, StringLength(6)]
    public required string INDEX { get; set; }

    /// <inheritdoc/>
    [Required, StringLength(4)]
    public required string GNINMB { get; set; }

    /// <inheritdoc/>
    [Required, StringLength(4)]
    public required string UNO { get; set; }

    /// <inheritdoc/>
    [Required, StringLength(11)]
    public required string OCATD { get; set; }

    /// <inheritdoc/>
    public static TResponseModel<List<RootKLADRModel>> Build(List<FieldDescriptorBase> columns, List<object[]> rowsData)
    {
        TResponseModel<List<RootKLADRModel>> res = new();

        foreach (System.Reflection.PropertyInfo p in typeof(RootKLADRModel).GetProperties().Where(x => !columns.Any(x => x.FieldName.Equals(x.FieldName, StringComparison.OrdinalIgnoreCase))))
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
                NAME = r[columns.FindIndex(x => x.FieldName.Equals(nameof(NAME), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
                OCATD = r[columns.FindIndex(x => x.FieldName.Equals(nameof(OCATD), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
                SOCR = r[columns.FindIndex(x => x.FieldName.Equals(nameof(SOCR), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
                UNO = r[columns.FindIndex(x => x.FieldName.Equals(nameof(UNO), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
            });
        }

        return res;
    }
}