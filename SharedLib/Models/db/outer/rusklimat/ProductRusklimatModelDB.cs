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
    public RemainsRusklimatModelDB? Remains { get; set; }
    /// <summary>
    /// Остатки
    /// </summary>
    public int RemainsId { get; set; }

    /// <inheritdoc/>
    public static ProductRusklimatModelDB Build(ProductRusklimatModel x, PropertyRusklimatModelDB[] data)
    {
        ProductRusklimatModelDB res = new()
        {
            Id = x.Id,
            Name = x.Name,
            Brand = x.Brand,
            CategoryId = x.CategoryId,
            ClientPrice = x.ClientPrice,
            Description = x.Description,
            Exclusive = x.Exclusive,
            InternetPrice = x.InternetPrice,
            NSCode = x.NSCode,
            Price = x.Price,
            VendorCode = x.VendorCode,
        };

        if (x.PropertiesV2 is not null && x.PropertiesV2.Count != 0)
            res.Properties = [.. x.PropertiesV2.Select(x => ProductPropertyRusklimatModelDB.Build(x, res, data))];

        res.Information = [];

        if (x.Instructions is not null && x.Instructions.Count != 0)
            res.Information.AddRange(x.Instructions.Select(y => new ProductInformationRusklimatModelDB()
            {
                Name = y,
                TypeInfo = nameof(x.Instructions),
                Product = res,
            }));

        if (x.Analog is not null && x.Analog.Count != 0)
            res.Information.AddRange(x.Analog.Select(y => new ProductInformationRusklimatModelDB()
            {
                Name = y,
                TypeInfo = nameof(x.Analog),
                Product = res,
            }));

        if (x.Barcode is not null && x.Barcode.Count != 0)
            res.Information.AddRange(x.Barcode.Select(y => new ProductInformationRusklimatModelDB()
            {
                Name = y,
                TypeInfo = nameof(x.Barcode),
                Product = res,
            }));

        if (x.Certificates is not null && x.Certificates.Count != 0)
            res.Information.AddRange(x.Certificates.Select(y => new ProductInformationRusklimatModelDB()
            {
                Name = y,
                TypeInfo = nameof(x.Certificates),
                Product = res,
            }));

        if (x.Video is not null && x.Video.Count != 0)
            res.Information.AddRange(x.Video.Select(y => new ProductInformationRusklimatModelDB()
            {
                Name = y,
                TypeInfo = nameof(x.Video),
                Product = res,
            }));

        if (x.RelatedProducts is not null && x.RelatedProducts.Count != 0)
            res.Information.AddRange(x.RelatedProducts.Select(y => new ProductInformationRusklimatModelDB()
            {
                Name = y,
                TypeInfo = nameof(x.RelatedProducts),
                Product = res,
            }));

        if (x.PromoMaterials is not null && x.PromoMaterials.Count != 0)
            res.Information.AddRange(x.PromoMaterials.Select(y => new ProductInformationRusklimatModelDB()
            {
                Name = y,
                TypeInfo = nameof(x.PromoMaterials),
                Product = res,
            }));

        if (x.Pictures is not null && x.Pictures.Count != 0)
            res.Information.AddRange(x.Pictures.Select(y => new ProductInformationRusklimatModelDB()
            {
                Name = y,
                TypeInfo = nameof(x.Pictures),
                Product = res,
            }));

        if (x.Drawing is not null && x.Drawing.Count != 0)
            res.Information.AddRange(x.Drawing.Select(y => new ProductInformationRusklimatModelDB()
            {
                Name = y,
                TypeInfo = nameof(x.Drawing),
                Product = res,
            }));

        if (x.Remains is not null)
        {
            res.Remains = new()
            {
                Total = x.Remains.Total,
                Product = res,
            };
            if (x.Remains.Warehouses is not null)
            {
                res.Remains.WarehousesRemains = [];
                foreach (KeyValuePair<string, string> rw in x.Remains.Warehouses)
                    res.Remains.WarehousesRemains.Add(new() { Name = rw.Key, RemainValue = rw.Value, Parent = res.Remains });

            }
        }
        return res;
    }
}