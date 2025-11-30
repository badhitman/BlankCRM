////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Служба доставки
/// </summary>
[Index(nameof(IsDisabled))]
public class DeliveryServiceRetailModelDB : EntryUpdatedModel
{
    /// <inheritdoc/>
    public bool IsDisabled { get; set; }

    /// <inheritdoc/>
    public int SortIndex { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is DeliveryServiceRetailModelDB deliveryOther)
            return deliveryOther.IsDisabled == IsDisabled && deliveryOther.Name == Name && deliveryOther.Description == Description;

        return false;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(IsDisabled, Id, Name, Description);
    }
}