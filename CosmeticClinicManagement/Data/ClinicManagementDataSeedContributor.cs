using CosmeticClinicManagement.Domain.InventoryManagement;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace CosmeticClinicManagement.Data
{
    public class ClinicManagementDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Store, Guid> _storeRepository;

        public ClinicManagementDataSeedContributor(IRepository<Store, Guid> storeRepository)
        {
            _storeRepository = storeRepository ?? throw new ArgumentNullException(nameof(storeRepository));
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _storeRepository.GetCountAsync() > 0)
            {
                return; // Already seeded
            }

            var defaultStore = new Store(
                Guid.NewGuid(),
                "Main Clinic Store");

            // Add raw materials directly to the store aggregate
            defaultStore.RawMaterials.Add(new RawMaterial(
                Guid.NewGuid(),
                "Hyaluronic Acid",
                "Used for skin hydration treatments",
                100,
                49.99m,
                DateTime.Now.AddYears(1),
                defaultStore.Id));

            defaultStore.RawMaterials.Add(new RawMaterial(
                Guid.NewGuid(),
                "Vitamin C Serum",
                "Used for skin brightening treatments",
                150,
                39.99m,
                DateTime.Now.AddYears(1),
                defaultStore.Id));

            defaultStore.RawMaterials.Add(new RawMaterial(
                Guid.NewGuid(),
                "Retinol Cream",
                "Used for anti-aging treatments",
                200,
                59.99m,
                DateTime.Now.AddYears(1),
                defaultStore.Id));

            await _storeRepository.InsertAsync(defaultStore, autoSave: true);
        }
    }
}
