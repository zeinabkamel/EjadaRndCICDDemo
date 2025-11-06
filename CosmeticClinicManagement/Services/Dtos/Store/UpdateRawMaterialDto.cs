using System;
using System.ComponentModel.DataAnnotations;


namespace CosmeticClinicManagement.Services.Dtos.Store
{
    public class UpdateRawMaterialDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [StringLength(500)]
        public string Description { get; set; } = null!;

        [Range(1, 1000000)]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(RawMaterialValidators), nameof(RawMaterialValidators.ValidateExpiryDate))]
        public DateTime ExpiryDate { get; set; }
    }
}
