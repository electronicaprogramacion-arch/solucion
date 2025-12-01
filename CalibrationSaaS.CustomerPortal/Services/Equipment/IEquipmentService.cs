using CalibrationSaaS.CustomerPortal.Models.Equipment;

namespace CalibrationSaaS.CustomerPortal.Services.Equipment;

public interface IEquipmentService
{
    Task<EquipmentSearchResult> SearchEquipmentAsync(EquipmentSearchRequest request, int customerId);
    Task<EquipmentDto?> GetEquipmentAsync(int equipmentId, int customerId);
    Task<List<ManufacturerDto>> GetManufacturersAsync(int customerId);
    Task<List<EquipmentTypeDto>> GetEquipmentTypesAsync(int customerId);
    Task<byte[]> ExportEquipmentAsync(EquipmentSearchRequest request, int customerId);
}
