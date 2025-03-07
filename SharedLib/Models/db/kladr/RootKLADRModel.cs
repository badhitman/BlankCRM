////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <inheritdoc/>
public class RootKLADRModelDB : BaseKladrModel
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
}