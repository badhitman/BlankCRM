////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Заказ (документ)
/// </summary>
public class OrderDocumentModelDB : OrderDocumentBaseModelDB
{
    /// <summary>
    /// Адреса доставки
    /// </summary>
    public List<TabOfficeForOrderModelDb>? OfficesTabs { get; set; }

    /// <summary>
    /// Подготовить объект заказа для записи в БД
    /// </summary>
    public void PrepareForSave()
    {
        Organization = null;
        OfficesTabs?.ForEach(x =>
        {
            x.Office = null;
            x.Rows?.ForEach(y =>
            {
                y.Id = 0;
                y.Amount = y.Quantity * y.Offer!.Price;
                y.Order = this;
                y.Nomenclature = null;
                y.Offer = null;
                y.Version = Guid.NewGuid();
            });
        });
    }

    /// <summary>
    /// Сумма заказа всего
    /// </summary>
    public decimal TotalSumForRows()
    {
        if (OfficesTabs is null || OfficesTabs.Count == 0 || OfficesTabs.Any(x => x.Rows is null) || OfficesTabs.Any(x => x.Rows is null || x.Rows.Any(z => z.Offer is null)))
            return 0;

        return OfficesTabs.Sum(x => x.Rows!.Sum(y => y.Quantity * y.Offer!.Price));
    }

    /// <inheritdoc/>
    public static OrderDocumentModelDB NewEmpty(string authorIdentityUserId)
    {
        return new() { AuthorIdentityUserId = authorIdentityUserId, Name = "Новый" };
    }
}