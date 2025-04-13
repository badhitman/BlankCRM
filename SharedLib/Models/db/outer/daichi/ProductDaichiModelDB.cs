////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// ProductDaichiModelDB
/// </summary>
[Index(nameof(CreatedAt)), Index(nameof(UpdatedAt))]
public class ProductDaichiModelDB : ProductDaichiBaseModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public ParamsProductDaichiModelDB? Params { get; set; }

    /// <inheritdoc/>
    public AvailabilityProductsDaichiModelDB? StoreAvailability { get; set; }

    /// <inheritdoc/>
    public List<PriceProductDaichiModelDB>? Prices { get; set; }

    /// <summary>
    /// Дата первого появления
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }


    /// <inheritdoc/>
    public static ProductDaichiModelDB Build(ProductDaichiModel x, List<StoreDaichiModelDB> storesDb)
    {
        ProductDaichiModelDB res = new()
        {
            XML_ID = x.XML_ID,
            NAME = x.NAME,
            KeyIndex = x.KeyIndex,
        };
        res.Params = new ParamsProductDaichiModelDB()
        {
            ATTR_L_GOODGROUP = x.PARAMS.ATTR_L_GOODGROUP,
            ATTR_L_GOODTYPE = x.PARAMS.ATTR_L_GOODTYPE,
            ATTR_L_IN_UNIT_TYPE = x.PARAMS.ATTR_L_IN_UNIT_TYPE,
            ATTR_L_SERIA = x.PARAMS.ATTR_L_SERIA,
            ATTR_RUS_NAME_AX = x.PARAMS.ATTR_RUS_NAME_AX,
            BRAND = x.PARAMS?.BRAND,
            Product = res
        };

        res.Prices = [];
        if (!string.IsNullOrWhiteSpace(x.PRICES.BASE?.NAME))
            res.Prices.Add(new PriceProductDaichiModelDB()
            {
                NAME = x.PRICES.BASE.NAME,
                XML_ID = x.PRICES.BASE.XML_ID,
                CURRENCY = x.PRICES.BASE.CURRENCY,
                Product = res,
                PRICE = x.PRICES.BASE.PRICE,
            });
        if (!string.IsNullOrWhiteSpace(x.PRICES.mprc?.NAME))
            res.Prices.Add(new PriceProductDaichiModelDB()
            {
                NAME = x.PRICES.mprc.NAME,
                XML_ID = x.PRICES.mprc.XML_ID,
                CURRENCY = x.PRICES.mprc.CURRENCY,
                Product = res,
                PRICE = x.PRICES.mprc.PRICE,
            });

        res.StoreAvailability = new()
        {
            StoreId = storesDb.First(y => y.XML_ID == x.STORE.XML_ID).Id,
            Product = res,
            STORE_AMOUNT = x.STORE.STORE_AMOUNT,
            DELIVERY_AMOUNT = x.STORE.DELIVERY_AMOUNT,

            DELIVERY_HIDE_MORE_LIMIT = x.STORE.DELIVERY_PARAMS_INFO?.HIDE_MORE_LIMIT,
            DELIVERY_LIMIT = x.STORE.DELIVERY_PARAMS_INFO?.LIMIT,

            STORE_HIDE_MORE_LIMIT = x.STORE.STORE_PARAMS_INFO?.HIDE_MORE_LIMIT,
            STORE_LIMIT = x.STORE.STORE_PARAMS_INFO?.LIMIT,
        };

        return res;
    }

    /// <inheritdoc/>
    public void SetLive()
    {
        Prices?.ForEach(pp => { pp.Product = this; });

        if (StoreAvailability is not null)
            StoreAvailability.Product = this;
        if (Params is not null)
            Params.Product = this;
    }
}