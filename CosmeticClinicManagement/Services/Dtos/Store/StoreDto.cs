using Volo.Abp.Application.Dtos;

namespace CosmeticClinicManagement.Services.Dtos.Store
{
    public class StoreDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public List<RawMaterialDto> RawMaterials { get; set; } = new();
    }
}
