////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Склад в документе. на который осуществляется доставка или с которого производится отгрузка
/// </summary>
public partial class ДокументТоварСклад : Склад
{
    /// <remarks/>
    public decimal Количество { get; set; }
}