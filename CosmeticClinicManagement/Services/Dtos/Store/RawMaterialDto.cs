using Volo.Abp.Application.Dtos;

namespace CosmeticClinicManagement.Services.Dtos.Store
{
    public class RawMaterialDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Guid StoreId { get; set; }
    }
}
