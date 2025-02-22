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
}