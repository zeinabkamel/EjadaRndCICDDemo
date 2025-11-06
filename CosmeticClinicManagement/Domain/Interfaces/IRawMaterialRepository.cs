using CosmeticClinicManagement.Domain.InventoryManagement;
using Volo.Abp.Domain.Repositories;

namespace CosmeticClinicManagement.Domain.Interfaces
{
    public interface IRawMaterialRepository : IRepository<RawMaterial, Guid>
    {
    }
}
