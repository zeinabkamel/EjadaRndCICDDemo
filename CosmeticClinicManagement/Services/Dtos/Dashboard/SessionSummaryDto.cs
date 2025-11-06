namespace CosmeticClinicManagement.Services.Dtos.Dashboard
{
    public class SessionSummaryDto
    {
        public Guid SessionId { get; set; }
        public string PatientName { get; set; }
        public int PatientAge { get; set; }
        public List<string> SessionNotes { get; set; }
        public string SessionStatus { get; set; }
    }
}
