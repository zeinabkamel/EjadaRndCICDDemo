using CosmeticClinicManagement.Constants;
using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.Interfaces;
using CosmeticClinicManagement.Services.Dtos.Dashboard;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Immutable;
using Volo.Abp.Application.Services;

namespace CosmeticClinicManagement.Services.Implementation
{
    [Authorize(Roles="Doctor")]
    public class DoctorDashboardAppService(
        ITreatmentPlanRepository treatmentPlanRepository,
        IPatientRepository patientRepository
    ) : ApplicationService, IDoctorDashboardAppService
    {
        private ITreatmentPlanRepository TreatmentPlanRepository { get; } = treatmentPlanRepository;
        private IPatientRepository PatientRepository { get; } = patientRepository;

        public async Task<DoctorDashboardDto> GetDoctorDashboardAsync()
        {
            var doctorTreatmentPlans = await TreatmentPlanRepository.GetTreatmentPlansByDoctorIdAsync(CurrentUser.Id.GetValueOrDefault());
            var allSessions = doctorTreatmentPlans.SelectMany(tp => tp.Sessions).ToList();
            var patientIds = doctorTreatmentPlans.Select(tp => tp.PatientId).Distinct().ToList();
            var todaySessions = allSessions.Where(s => s.SessionDate.Day == DateTime.Now.Day).ToList();
            

            return new DoctorDashboardDto
            {
                DoctorName = $"{CurrentUser.Name} {CurrentUser.SurName}",
                CompletedSessionsCount = allSessions.Count(s => s.Status == SessionStatus.Completed),
                TodaySessionsCount = todaySessions.Count,
                PlannedSessionsCount = allSessions.Count(s => s.Status == SessionStatus.Planned),
                PatientsCount = patientIds.Count,
                TodaySessionsSummary = await GetSessionSummaries(todaySessions, patientIds, doctorTreatmentPlans)
            };
        }

        private async Task<List<SessionSummaryDto>> GetSessionSummaries(List<Session> sessions, List<Guid> patientIds, List<TreatmentPlan> treatmentPlans)
        {
            var treatmentPlanPatientIds = treatmentPlans
                .Select(tp => new { id = tp.Id, pid = tp.PatientId })
                .ToImmutableDictionary(tp => tp.id, tp => tp.pid);

            var patientsData = await PatientRepository.GetPatientNamesAndDateOfBirthAsync(patientIds);

            return [.. sessions.Select(s => new SessionSummaryDto
            {
                SessionId = s.Id,
                SessionStatus = s.Status.ToString(),
                PatientName = patientsData[treatmentPlanPatientIds[s.PlanId]].FullName,
                PatientAge = DateTime.Now.Year - patientsData[treatmentPlanPatientIds[s.PlanId]].DateOfBirth.Year,
                SessionNotes = s.Notes
            })];
        }
    }
}
