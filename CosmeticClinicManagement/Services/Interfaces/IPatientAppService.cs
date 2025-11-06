using CosmeticClinicManagement.Services.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace CosmeticClinicManagement.Services.Interfaces
{
    public interface IPatientAppService : IApplicationService
    {
        Task<PagedResultDto<PatientDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<PatientDto> GetAsync(Guid id);
        Task<PatientDto> CreateAsync(CreateUpdatePatientDto input);
        Task UpdateAsync(Guid id, CreateUpdatePatientDto input);
        Task DeleteAsync(Guid id);

        Task<List<PatientDto>> GetAllPatientsAsync();
    }
}
