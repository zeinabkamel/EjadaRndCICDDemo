using System.ComponentModel.DataAnnotations;

namespace CosmeticClinicManagement.Services.Dtos.Store
{
    public class CreateStoreDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;
    }
}
