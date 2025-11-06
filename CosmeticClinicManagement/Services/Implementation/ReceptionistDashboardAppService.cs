using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using CosmeticClinicManagement.Services.Dtos.Dashboard;
using CosmeticClinicManagement.Domain.Interfaces;
using Volo.Abp.Identity;

namespace CosmeticClinicManagement.Services.Implementation
{
    public class ReceptionistDashboardAppService : ApplicationService
    {
        private readonly ITreatmentPlanRepository _treatmentPlanRepository;
        private readonly IRepository<Patient, Guid> _patientRepository;
        private readonly IIdentityUserRepository _userRepository;

        public ReceptionistDashboardAppService(
            ITreatmentPlanRepository treatmentPlanRepository,
            IRepository<Patient, Guid> patientRepository,
            IIdentityUserRepository userRepository)
        {
            _treatmentPlanRepository = treatmentPlanRepository;
            _patientRepository = patientRepository;
            _userRepository = userRepository;
        }

        public async Task<ReceptionistDashboardDto> GetDashboardDataAsync()
        {
            var treatmentPlans = await _treatmentPlanRepository.GetListWithDetailsAsync();
            var patients = await _patientRepository.GetListAsync();

            var allSessions = treatmentPlans
                .Where(tp => tp.Sessions != null)
                .SelectMany(tp => tp.Sessions)
                .ToList();

            var upcomingSessions = allSessions
                .Where(s => s.SessionDate > DateTime.Now && s.Status == SessionStatus.Planned)
                .OrderBy(s => s.SessionDate)
                .Take(10)
                .ToList();

            var recentCompletedSessions = allSessions
                .Where(s => s.Status == SessionStatus.Completed)
                .OrderByDescending(s => s.SessionDate)
                .Take(10)
                .ToList();

            var completedCount = allSessions.Count(s => s.Status == SessionStatus.Completed);
            var cancelledCount = allSessions.Count(s => s.Status == SessionStatus.Cancelled);

            // Get patient and doctor data
            var patientIds = treatmentPlans.Select(tp => tp.PatientId).Distinct().ToList();
            var doctorIds = treatmentPlans.Select(tp => tp.DoctorId).Distinct().ToList();

            var patientsDict = patients.ToDictionary(p => p.Id, p => p);
            var doctors = await _userRepository.GetListAsync();
            var doctorsDict = doctors.Where(d => doctorIds.Contains(d.Id))
                .ToDictionary(d => d.Id, d => $"{d.Name} {d.Surname}");

            // Map treatment plans for easy lookup
            var treatmentPlansDict = treatmentPlans.ToDictionary(tp => tp.Id, tp => tp);

            return new ReceptionistDashboardDto
            {
                TotalPatients = patients.Count,
                TotalTreatmentPlans = treatmentPlans.Count,
                UpcomingSessions = upcomingSessions.Count,
                CompletedSessions = completedCount,
                CancelledSessions = cancelledCount,
                UpcomingSessionsList = MapToUpcomingSessionDetails(upcomingSessions, treatmentPlansDict, patientsDict, doctorsDict),
                RecentCompletedSessions = MapToRecentSessionDetails(recentCompletedSessions, treatmentPlansDict, patientsDict, doctorsDict)
            };
        }

        private List<UpcomingSessionDetailDto> MapToUpcomingSessionDetails(
            List<Session> sessions,
            Dictionary<Guid, TreatmentPlan> treatmentPlans,
            Dictionary<Guid, Patient> patients,
            Dictionary<Guid, string> doctors)
        {
            return sessions.Select(s =>
            {
                var plan = treatmentPlans[s.PlanId];
                var patient = patients[plan.PatientId];
                var doctorName = doctors.ContainsKey(plan.DoctorId) ? doctors[plan.DoctorId] : "Unknown";

                return new UpcomingSessionDetailDto
                {
                    SessionId = s.Id,
                    PatientName = $"{patient.FirstName} {patient.LastName}",
                    PatientAge = DateTime.Now.Year - patient.DateOfBirth.Year,
                    DoctorName = doctorName,
                    SessionDate = s.SessionDate,
                    Status = s.Status.ToString(),
                };
            }).ToList();
        }

        private List<RecentSessionDetailDto> MapToRecentSessionDetails(
            List<Session> sessions,
            Dictionary<Guid, TreatmentPlan> treatmentPlans,
            Dictionary<Guid, Patient> patients,
            Dictionary<Guid, string> doctors)
        {
            return sessions.Select(s =>
            {
                var plan = treatmentPlans[s.PlanId];
                var patient = patients[plan.PatientId];
                var doctorName = doctors.ContainsKey(plan.DoctorId) ? doctors[plan.DoctorId] : "Unknown";

                return new RecentSessionDetailDto
                {
                    SessionId = s.Id,
                    PatientName = $"{patient.FirstName} {patient.LastName}",
                    DoctorName = doctorName,
                    SessionDate = s.SessionDate,
                    Status = s.Status.ToString(),
                };
            }).ToList();
        }
    }
}