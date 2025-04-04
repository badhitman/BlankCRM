﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Связь свойства с товаром
/// </summary>
public class ProductPropertyRusklimatModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Product
    /// </summary>
    public ProductRusklimatModelDB? Product { get; set; }
    /// <summary>
    /// Product
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Property
    /// </summary>
    public PropertyRusklimatModelDB? Property { get; set; }
    /// <summary>
    /// Property
    /// </summary>
    public int PropertyId { get; set; }
}