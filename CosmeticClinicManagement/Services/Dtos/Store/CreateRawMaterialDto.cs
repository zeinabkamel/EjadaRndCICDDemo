using System;
using System.ComponentModel.DataAnnotations;

namespace CosmeticClinicManagement.Services.Dtos.Store
{
    public class CreateRawMaterialDto
    {
        [Required]
        public Guid StoreId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        //[StringLength(500)]
        public string Description { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]

        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(RawMaterialValidators), nameof(RawMaterialValidators.ValidateExpiryDate))]
        public DateTime ExpiryDate { get; set; }
    }

    public static class RawMaterialValidators
    {
        public static ValidationResult? ValidateExpiryDate(DateTime expiryDate, ValidationContext context)
        {
            return expiryDate <= DateTime.UtcNow
                ? new ValidationResult("Expiry date must be in the future.")
                : ValidationResult.Success;
        }
    }
}
