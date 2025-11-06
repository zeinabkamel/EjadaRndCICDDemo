using Volo.Abp.Domain.Entities;

namespace CosmeticClinicManagement.Domain.InventoryManagement
{
    public class Store : BasicAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public List<RawMaterial> RawMaterials { get; private set; }

        protected Store() { }

        public Store (Guid id, string name) : base(id)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Store name cannot be empty.", nameof(name));
            }

            Name = name;
            RawMaterials = new List<RawMaterial>();
        }

        public void ChangeName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Store name cannot be empty.", nameof(newName));
            }

            Name = newName;
        }
    }
}
