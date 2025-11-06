namespace CosmeticClinicManagement.Services.Dtos.Dashboard
{
    public class DoctorDashboardDto
    {
        public string DoctorName { get; set; }
        public int CompletedSessionsCount { get; set; }
        public int TodaySessionsCount { get; set; }
        public int PlannedSessionsCount { get; set; }
        public int PatientsCount { get; set; }
        public List<SessionSummaryDto> TodaySessionsSummary { get; set; }
    }
}
