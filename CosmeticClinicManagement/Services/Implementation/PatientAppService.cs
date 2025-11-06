using CosmeticClinicManagement.Domain.Interfaces;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Guids;

namespace CosmeticClinicManagement.Services.Implementation
{
    [Authorize("PatientManagement")]
    public class PatientAppService : ApplicationService, IPatientAppService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientAppService(IPatientRepository patientRepository, IGuidGenerator guidGenerator)
        {
            _patientRepository = patientRepository;
        }

        public async Task<PatientDto> CreateAsync(CreateUpdatePatientDto input)
        {
            var patient = ObjectMapper.Map<CreateUpdatePatientDto, Patient>(input);
            patient = await _patientRepository.InsertAsync(patient, autoSave: true);
            return ObjectMapper.Map<Patient, PatientDto>(patient);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _patientRepository.DeleteAsync(id);
        }

        public Task<List<PatientDto>> GetAllPatientsAsync()
        {
            var patients =  _patientRepository.GetListAsync();
            return patients.ContinueWith(task => 
                ObjectMapper.Map<List<Patient>, List<PatientDto>>(task.Result)
            );
        }

        public async Task<PatientDto> GetAsync(Guid id)
        {
            PatientDto patientDto = ObjectMapper.Map<Patient, PatientDto>(await _patientRepository.GetAsync(id));
            return patientDto;
        }

        public async Task<PagedResultDto<PatientDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var patients = await _patientRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            var totalCount = await _patientRepository.GetCountAsync();
            return new PagedResultDto<PatientDto>(
                totalCount,
                ObjectMapper.Map<List<Patient>, List<PatientDto>>(patients)
            );
        }

        public async Task UpdateAsync(Guid id, CreateUpdatePatientDto input)
        {
            var patient = await _patientRepository.FindAsync(id);
            ObjectMapper.Map(input, patient);
            await _patientRepository.UpdateAsync(patient, autoSave: true);
        }
    }
}
