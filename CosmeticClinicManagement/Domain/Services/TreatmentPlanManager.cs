using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.Interfaces;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace CosmeticClinicManagement.Domain.Services
{
    public class TreatmentPlanManager(ITreatmentPlanRepository treatmentPlanRepository,
        IGuidGenerator guidGenerator, IRawMaterialRepository rawMaterialRepository) : DomainService, ITreatmentPlanManager
    {
        private readonly ITreatmentPlanRepository _treatmentPlanRepository = treatmentPlanRepository;
        private readonly IGuidGenerator _guidGenerator = guidGenerator;
        private readonly IRawMaterialRepository _rawMaterialRepository = rawMaterialRepository;

        public async Task AddUsedMaterialToSession(Guid planId, Guid sessionId, Guid rawMaterialId, decimal quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            }

            var rawMaterial = await _rawMaterialRepository.FindAsync(rawMaterialId)
                ?? throw new InvalidOperationException("Raw material not found.");

            if (quantity > rawMaterial.Quantity)
            {
                throw new InvalidOperationException("Insufficient raw material quantity available.");
            }

            var plan = await _treatmentPlanRepository.FindAsync(planId, includeDetails: true)
                ?? throw new InvalidOperationException("Treatment plan not found.");

            plan.AddUsedMaterialToSession(sessionId, new UsedMaterial(rawMaterialId, quantity));
        }

        public async Task<Session> BookSession(Guid planId, DateTime sessionDate, List<string> notes)
        {
            TreatmentPlan plan = await _treatmentPlanRepository.FindAsync(planId, includeDetails: true)
                ?? throw new InvalidOperationException("Treatment plan not found.");

            plan.ThrowExceptionIfClosed();

            if (!plan.IsValidNewSessionDate(sessionDate))
            {
                throw new InvalidOperationException("The session date is not valid.");
            }

            var session = new Session(
                _guidGenerator.Create(),
                planId,
                sessionDate,
                notes,
                SessionStatus.Planned
            );

            plan.AddSession(session);
            return session;
        }
    }
}
