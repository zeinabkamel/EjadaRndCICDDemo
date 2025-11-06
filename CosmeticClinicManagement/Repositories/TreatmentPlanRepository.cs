using CosmeticClinicManagement.Data;
using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace CosmeticClinicManagement.Repositories
{
    public class TreatmentPlanRepository(IDbContextProvider<CosmeticClinicManagementDbContext> dbContextProvider)
        : EfCoreRepository<CosmeticClinicManagementDbContext, TreatmentPlan, Guid>(dbContextProvider),
        ITreatmentPlanRepository
    {
        public async Task<List<TreatmentPlan>> GetTreatmentPlansByDoctorIdAsync(Guid doctorId)
        {
            var query = await GetQueryableAsync();
            return await query
                .Include(tp => tp.Sessions)
                .Where(tp => tp.DoctorId == doctorId)
                .ToListAsync();
        }

        
        public async Task<List<TreatmentPlan>> GetListWithDetailsAsync()
        {
            var query = await GetQueryableAsync();
            return await query
                .Include(tp => tp.Sessions)
                .ToListAsync();
        }
    }
}
