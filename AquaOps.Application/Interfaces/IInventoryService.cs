using AquaOps.Application.Common;
using AquaOps.Application.DTOs.Inventory;

namespace AquaOps.Application.Interfaces;

public interface IInventoryService
{
    Task<Result<InventoryItemDto>> ReceiveAsync(ReceiveInventoryRequest request);
    Task<Result<InventoryItemDto>> TransferAsync(TransferInventoryRequest request);
    Task<Result<InventoryItemDto>> AdjustAsync(AdjustInventoryRequest request);
    Task<Result<IReadOnlyList<InventoryItemDto>>> GetByProductAsync(Guid productId);
    Task<Result<IReadOnlyList<InventoryItemDto>>> GetByLocationAsync(Guid locationId);
    Task<Result<IReadOnlyList<InventoryItemDto>>> GetExpiringAsync(int daysThreshold = 7);
}

public interface IStorageLocationService
{
    Task<Result<IReadOnlyList<StorageLocationDto>>> GetAllAsync();
    Task<Result<StorageLocationDto>> GetByIdAsync(Guid id);
    Task<Result<StorageLocationDto>> CreateAsync(CreateStorageLocationRequest request);
    Task<Result<StorageLocationDto>> UpdateAsync(Guid id, CreateStorageLocationRequest request);
    Task<Result> DeactivateAsync(Guid id);
}

public interface ITemperatureService
{
    Task<Result<TemperatureReadingDto>> RecordAsync(CreateTemperatureReadingRequest request);
    Task<Result<IReadOnlyList<TemperatureReadingDto>>> GetByLocationAsync(Guid locationId, DateTime? from, DateTime? to);
    Task<Result<IReadOnlyList<TemperatureReadingDto>>> GetAlertsAsync();
}
