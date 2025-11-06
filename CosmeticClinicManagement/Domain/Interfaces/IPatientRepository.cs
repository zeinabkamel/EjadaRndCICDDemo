using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using Volo.Abp.Domain.Repositories;

namespace CosmeticClinicManagement.Domain.Interfaces
{
    public interface IPatientRepository : IRepository<Patient, Guid>
    {
        Task<List<Patient>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting);
        Task<Dictionary<Guid, (string FullName, DateTime DateOfBirth)>> GetPatientNamesAndDateOfBirthAsync(List<Guid> ids);
    }
}
