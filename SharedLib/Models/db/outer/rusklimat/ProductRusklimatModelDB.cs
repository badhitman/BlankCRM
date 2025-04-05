////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ProductRusklimatModelDB
/// </summary>
public class ProductRusklimatModelDB : ProductRusklimatBaseModel
{
    /// <summary>
    /// Связи свойств с товаром
    /// </summary>
    public List<ProductPropertyRusklimatModelDB>? Properties { get; set; }

    /// <summary>
    /// Information
    /// </summary>
    public List<ProductInformationRusklimatModelDB>? Information { get; set; }

    /// <summary>
    /// Остатки
    /// </summary>
    public List<RemainsRusklimatModelDB>? Remains { get; set; }
}