////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// WarehouseDocumentRecord
/// </summary>
public record WarehouseDocumentRecord(int WarehouseId, int WritingOffWarehouseId, bool IsDisabled);