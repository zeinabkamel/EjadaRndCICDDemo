using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace CosmeticClinicManagement.Pages.Patients
{
    public class EditModel(IPatientAppService patientAppService) : AbpPageModel
    {
        private readonly IPatientAppService _patientAppService = patientAppService;

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public CreateUpdatePatientDto Patient { get; set; }

        public async Task OnGetAsync(Guid Id)
        {
            var patientDto = await _patientAppService.GetAsync(Id);
            Patient = ObjectMapper.Map<PatientDto, CreateUpdatePatientDto>(patientDto);
        }

        public async Task OnPostAsync(Guid id)
        {
            ValidateModel();
            await _patientAppService.UpdateAsync(id, Patient);
        }
    }
}
