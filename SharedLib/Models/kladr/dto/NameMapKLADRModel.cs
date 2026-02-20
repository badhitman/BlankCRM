////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <inheritdoc/>
[Index(nameof(SHNAME)), Index(nameof(SCNAME))]
public class NameMapKLADRModel : BaseKladrScopedModel
{
    /// <inheritdoc/>
    [Required, StringLength(40)]
    public required string SHNAME { get; set; }

    /// <inheritdoc/>
    [Required, StringLength(10)]
    public required string SCNAME { get; set; }

    /// <inheritdoc/>
    public static TResponseModel<List<NameMapKLADRModel>> Build(List<FieldDescriptorBase> columns, List<object[]> rowsData)
    {
        TResponseModel<List<NameMapKLADRModel>> res = new();

        foreach (System.Reflection.PropertyInfo p in typeof(NameMapKLADRModel).GetProperties().Where(x => !columns.Any(x => x.FieldName.Equals(x.FieldName, StringComparison.OrdinalIgnoreCase))))
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
                NAME = r[columns.FindIndex(x => x.FieldName.Equals(nameof(NAME), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
                SCNAME = r[columns.FindIndex(x => x.FieldName.Equals(nameof(SCNAME), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
                SHNAME = r[columns.FindIndex(x => x.FieldName.Equals(nameof(SHNAME), StringComparison.OrdinalIgnoreCase))].ToString() ?? "",
            });
        }

        return res;
    }
}