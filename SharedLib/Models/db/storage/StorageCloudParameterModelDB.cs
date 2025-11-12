////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <inheritdoc/>
[Index(nameof(TypeName))]
[Index(nameof(CreatedAt))]
[Index(nameof(PrefixPropertyName), nameof(OwnerPrimaryKey))]
[Index(nameof(ApplicationName), nameof(PropertyName))]
public class StorageCloudParameterModelDB : StorageCloudParameterViewModel
{
    
}