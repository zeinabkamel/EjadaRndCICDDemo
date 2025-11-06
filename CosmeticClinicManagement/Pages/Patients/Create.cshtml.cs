using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace CosmeticClinicManagement.Pages.Patients
{
    public class CreateModel(IPatientAppService patientAppService) : AbpPageModel
    {
        private readonly IPatientAppService _patientAppService = patientAppService;

        [BindProperty]
        public CreateUpdatePatientDto Patient { get; set; } = new CreateUpdatePatientDto();
        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        {
            ValidateModel();
            await _patientAppService.CreateAsync(Patient);
        }

    }
}
