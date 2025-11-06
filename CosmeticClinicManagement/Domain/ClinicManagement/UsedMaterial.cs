using Volo.Abp.Domain.Values;

namespace CosmeticClinicManagement.Domain.ClinicManagement
{
    public class UsedMaterial : ValueObject
    {
        public Guid RawMaterialId { get; private set; }
        public decimal Quantity { get; private set; }

        protected UsedMaterial() { }

        public UsedMaterial(Guid rawMaterialId, decimal quantity)
        {
            RawMaterialId = rawMaterialId;
            Quantity = quantity;
        }

        public void AddQuantity(decimal additionalQuantity)
        {
            if (additionalQuantity <= 0)
            {
                throw new ArgumentException("Additional quantity must be greater than zero.");
            }
            Quantity += additionalQuantity;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return RawMaterialId;
            yield return Quantity;
        }
    }
}
