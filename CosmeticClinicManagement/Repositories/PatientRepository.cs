using CosmeticClinicManagement.Data;
using CosmeticClinicManagement.Domain.Interfaces;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CosmeticClinicManagement.Repositories
{
    public class PatientRepository(IDbContextProvider<CosmeticClinicManagementDbContext> dbContextProvider) : EfCoreRepository<CosmeticClinicManagementDbContext, Patient, Guid>(dbContextProvider), IPatientRepository
    {
        public async Task<List<Patient>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting)
        {
            var query = await GetQueryableAsync();
            return await query
                .OrderBy(sorting ?? nameof(Patient.FirstName))
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<Dictionary<Guid, (string FullName, DateTime DateOfBirth)>> GetPatientNamesAndDateOfBirthAsync(List<Guid> ids)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(p => ids.Contains(p.Id))
                .ToDictionaryAsync(
                    p => p.Id,
                    p => ($"{p.FirstName} {p.LastName}", p.DateOfBirth)
                );
        }
    }
}
