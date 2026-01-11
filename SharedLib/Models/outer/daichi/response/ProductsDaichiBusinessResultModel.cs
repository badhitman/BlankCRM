////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;

namespace SharedLib;

/// <summary>
/// ProductsDaichiBusinessResultModel
/// </summary>
public class ProductsDaichiBusinessResultModel
{
    /// <inheritdoc/>
    public JObject? Result { get; set; }

    /// <inheritdoc/>
    public double Time { get; set; }

    /// <inheritdoc/>
    public ConcurrentDictionary<string, Exception>? Exceptions { get; set; }

    List<ProductDaichiModel>? _getProducts;
    /// <inheritdoc/>
    public List<ProductDaichiModel>? GetProducts
    {
        get
        {
            if (_getProducts is not null)
                return _getProducts;

            Exceptions = null;
            JProperty[]? tokens = [.. Result?.Properties().Where(x => !x.Name.Equals("_count", StringComparison.OrdinalIgnoreCase))!];

            if (tokens is null)
                return null;

            try
            {
                List<ProductDaichiModel> _pr = [];

                foreach (ProductDaichiModel v in tokens.Select(GetProduct).Where(x => x is not null)!)
                    _pr.Add(v);

                _getProducts = _pr;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return _getProducts;
        }
    }

    ProductDaichiModel? GetProduct(JProperty entry)
    {
        try
        {
            ProductDaichiModel res = entry.First().ToObject<ProductDaichiModel>()!;
            res.KeyIndex = int.Parse(entry.Name);
            return res;
        }
        catch (Exception ex)
        {
            Exceptions ??= [];
            Exceptions.TryAdd(entry.Name, new Exception(entry.First().ToString(), ex));
            return null;
        }
    }
}