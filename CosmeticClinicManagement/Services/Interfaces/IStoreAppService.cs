using CosmeticClinicManagement.Services.Dtos.Store;
using Volo.Abp.Application.Services;

namespace CosmeticClinicManagement.Services.Interfaces
{
    public interface IStoreAppService : IApplicationService
    {
        // Store CRUD
        Task<List<StoreDto>> GetAllStoresAsync();
        Task<StoreDto> GetAsync(Guid id);
        Task<StoreDto> CreateAsync(CreateStoreDto input);
        Task<StoreDto> UpdateAsync(Guid id, UpdateStoreDto input);
        Task DeleteAsync(Guid id);

        // RawMaterial operations within a Store
        Task<List<RawMaterialDto>> GetRawMaterialsByStoreIdAsync(Guid storeId);
        Task<RawMaterialDto> CreateRawMaterialAsync(CreateRawMaterialDto input);
        Task<RawMaterialDto> UpdateRawMaterialAsync(Guid storeId, Guid rawMaterialId, UpdateRawMaterialDto input);
        Task DeleteRawMaterialAsync(Guid storeId, Guid rawMaterialId);
        Task<RawMaterialDto> GetRawMaterialAsync(Guid storeId, Guid rawMaterialId);

    }
}
