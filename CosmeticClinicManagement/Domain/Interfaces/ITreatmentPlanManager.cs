using CosmeticClinicManagement.Domain.ClinicManagement;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace CosmeticClinicManagement.Domain.Interfaces
{
    public interface ITreatmentPlanManager : IDomainService
    {
        Task<Session> BookSession(Guid planId, DateTime sessionDate, List<string> notes);

        [UnitOfWork]
        Task AddUsedMaterialToSession(Guid planId, Guid sessionId, Guid rawMaterialId, decimal quantity);
    }
}
