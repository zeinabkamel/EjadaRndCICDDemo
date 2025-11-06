using CosmeticClinicManagement.Services.Interfaces;
using CosmeticClinicManagement.Domain.InventoryManagement;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using CosmeticClinicManagement.Services.Dtos.Store;
using Volo.Abp;
using Microsoft.AspNetCore.Authorization;

namespace CosmeticClinicManagement.Services
{
    [Authorize("StoreManagement")]
    [Authorize("RawMaterialManagement")]
    public class StoreAppService : ApplicationService, IStoreAppService
    {
        private readonly IRepository<Store, Guid> _storeRepository;

        public StoreAppService(IRepository<Store, Guid> storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public async Task<List<StoreDto>> GetAllStoresAsync()
        {
            var q = await _storeRepository.GetQueryableAsync();
            var stores = await q.Include(s => s.RawMaterials).ToListAsync();

            return stores.Select(MapToStoreDto).ToList();
        }

        public async Task<StoreDto> GetAsync(Guid id)
        {
            var q = await _storeRepository.GetQueryableAsync();
            var store = await q.Include(s => s.RawMaterials)
                               .FirstOrDefaultAsync(s => s.Id == id)
                        ?? throw new UserFriendlyException("Store not found.");
            return MapToStoreDto(store);
        }

        public async Task<StoreDto> CreateAsync(CreateStoreDto input)
        {
            var store = new Store(Guid.NewGuid(), input.Name);
            await _storeRepository.InsertAsync(store, autoSave: true);
            return MapToStoreDto(store);
        }

        public async Task<StoreDto> UpdateAsync(Guid id, UpdateStoreDto input)
        {
            var store = await _storeRepository.GetAsync(id);
            store.ChangeName(input.Name);
            await _storeRepository.UpdateAsync(store, autoSave: true);
            return MapToStoreDto(store);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _storeRepository.DeleteAsync(id, autoSave: true);
        }

        // ---------- RawMaterial-related methods (aggregate-based) ----------

        public async Task<List<RawMaterialDto>> GetRawMaterialsByStoreIdAsync(Guid storeId)
        {
            var q = await _storeRepository.GetQueryableAsync();
            var store = await q.Include(s => s.RawMaterials)
                               .FirstOrDefaultAsync(s => s.Id == storeId)
                        ?? throw new UserFriendlyException("Store not found.");

            return store.RawMaterials.Select(MapToRawDto).ToList();
        }

        public async Task<RawMaterialDto> GetRawMaterialAsync(Guid storeId, Guid rawMaterialId)
        {
            var q = await _storeRepository.GetQueryableAsync();
            var store = await q.Include(s => s.RawMaterials)
                               .FirstOrDefaultAsync(s => s.Id == storeId)
                        ?? throw new UserFriendlyException("Store not found.");

            var raw = store.RawMaterials.FirstOrDefault(r => r.Id == rawMaterialId)
                      ?? throw new UserFriendlyException("Raw material not found.");

            return MapToRawDto(raw);
        }

        public async Task<RawMaterialDto> CreateRawMaterialAsync(CreateRawMaterialDto input)
        {
            var q = await _storeRepository.GetQueryableAsync();
            var store = await q.Include(s => s.RawMaterials)
                               .FirstOrDefaultAsync(s => s.Id == input.StoreId)
                        ?? throw new UserFriendlyException("Store not found.");

            var raw = new RawMaterial(Guid.NewGuid(), input.Name, input.Description, input.Quantity, input.Price, input.ExpiryDate, store.Id);
            store.RawMaterials.Add(raw);

            await _storeRepository.UpdateAsync(store, autoSave: true);
            return MapToRawDto(raw);
        }

        public async Task<RawMaterialDto> UpdateRawMaterialAsync(Guid storeId, Guid rawMaterialId, UpdateRawMaterialDto input)
        {
            var q = await _storeRepository.GetQueryableAsync();
            var store = await q.Include(s => s.RawMaterials)
                               .FirstOrDefaultAsync(s => s.Id == storeId)
                        ?? throw new UserFriendlyException("Store not found.");

            var raw = store.RawMaterials.FirstOrDefault(r => r.Id == rawMaterialId)
                      ?? throw new UserFriendlyException("Raw material not found.");

            raw.UpdateDetails(input.Name, input.Description, input.Quantity, input.Price, input.ExpiryDate);
            await _storeRepository.UpdateAsync(store, autoSave: true);
            return MapToRawDto(raw);
        }

        public async Task DeleteRawMaterialAsync(Guid storeId, Guid rawMaterialId)
        {
            var q = await _storeRepository.GetQueryableAsync();
            var store = await q.Include(s => s.RawMaterials)
                               .FirstOrDefaultAsync(s => s.Id == storeId)
                        ?? throw new UserFriendlyException("Store not found.");

            var raw = store.RawMaterials.FirstOrDefault(r => r.Id == rawMaterialId)
                      ?? throw new UserFriendlyException("Raw material not found.");

            store.RawMaterials.Remove(raw);
            await _storeRepository.UpdateAsync(store, autoSave: true);
        }

        // ---------- Mapping ----------
        private static StoreDto MapToStoreDto(Store s) =>
            new StoreDto
            {
                Id = s.Id,
                Name = s.Name,
                RawMaterials = s.RawMaterials?.Select(MapToRawDto).ToList() ?? new List<RawMaterialDto>()
            };

        private static RawMaterialDto MapToRawDto(RawMaterial r) =>
            new RawMaterialDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Quantity = r.Quantity,
                Price = r.Price,
                ExpiryDate = r.ExpiryDate,
                StoreId = r.StoreId
            };
    }
}
