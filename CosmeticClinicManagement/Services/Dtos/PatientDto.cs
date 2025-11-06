using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace CosmeticClinicManagement.Services.Dtos
{
    public class PatientDto : EntityDto<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public class CreateUpdatePatientDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}


