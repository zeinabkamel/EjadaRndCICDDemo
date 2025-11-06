using CosmeticClinicManagement.Enum;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace CosmeticClinicManagement.Domain.InventoryManagement
{
    public class RawMaterial : Entity<Guid>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public Guid StoreId { get; private set; }

        protected RawMaterial() { }

        public RawMaterial(Guid id, string name, string description, int quantity, decimal price, DateTime expiryDate, Guid storeId)
            : base(id)
        {
            ValidateParameters(name, description, quantity, price, expiryDate);

            Name = name;
            Description = description;
            Quantity = quantity;
            Price = price;
            ExpiryDate = expiryDate;
            StoreId = storeId;
        }

        private static void ValidateParameters(string name, string description, int quantity, decimal price, DateTime expiryDate)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty.", nameof(name));
            }

            if (string.IsNullOrEmpty(description))
            {
               
                throw new BusinessException("CosmeticClinicManagement:RawMaterial:DescriptionEmpty")
     .WithData("Field", "Description");

            }

            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            }

            if (price <= 0)
            {
                throw new ArgumentException("Price must be greater than zero.", nameof(price));
            }

            if (expiryDate < DateTime.Now.AddDays(30))
            {
                throw new ArgumentException("Expiry date must be at least 30 days in the future.", nameof(expiryDate));
            }
        }

        public void Consume(int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to consume must be greater than zero.", nameof(amount));
            }

            if (amount > Quantity)
            {
                throw new InvalidOperationException("Insufficient quantity available to consume.");
            }

            Quantity -= amount;
        }

        public void Restock(int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to restock must be greater than zero.", nameof(amount));
            }

            Quantity += amount;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
            {
                throw new ArgumentException("Price must be positive.", nameof(newPrice));
            }

            Price = newPrice;
        }
        public void UpdateDetails(string name, string description, int quantity, decimal price, DateTime expiryDate)
        {
            ValidateParameters(name, description, quantity, price, expiryDate);

            Name = name;
            Description = description;
            Quantity = quantity;
            Price = price;
            ExpiryDate = expiryDate;
        }
    }
}
