namespace CosmeticClinicManagement.Services.Dtos.Dashboard
{
    public class ReceptionistDashboardDto
    {
        public int TotalPatients { get; set; }
        public int TotalTreatmentPlans { get; set; }
        public int UpcomingSessions { get; set; }
        public int CompletedSessions { get; set; }
        public int CancelledSessions { get; set; }
        public List<UpcomingSessionDetailDto> UpcomingSessionsList { get; set; }
        public List<RecentSessionDetailDto> RecentCompletedSessions { get; set; }
    }

    public class UpcomingSessionDetailDto
    {
        public Guid SessionId { get; set; }
        public string PatientName { get; set; }
        public int PatientAge { get; set; }
        public string DoctorName { get; set; }
        public DateTime SessionDate { get; set; }
        public string Status { get; set; }
    }

    public class RecentSessionDetailDto
    {
        public Guid SessionId { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime SessionDate { get; set; }
        public string Status { get; set; }
    }
}