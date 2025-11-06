namespace CosmeticClinicManagement.Services.Dtos.Dashboard
{
    public class AdminDashboardDto
    {
        public long TotalPatients { get; set; }
        public long TotalDoctors { get; set; }
        public long ActiveTreatmentPlans { get; set; }
        public long ClosedTreatmentPlans { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalStores { get; set; }
        public int TotalRawMaterials { get; set; }
        public int LowStockItemsCount { get; set; }
        public List<LowStockItemDto> LowStockItems { get; set; }
        public List<TopDoctorDto> TopDoctors { get; set; }
        public List<RecentPatientDto> RecentPatients { get; set; }
        public List<ExpiringItemDto> ExpiringItems { get; set; }
    }

    public class LowStockItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string StoreName { get; set; }
        public int CurrentQuantity { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class TopDoctorDto
    {
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int ActivePlansCount { get; set; }
        public int CompletedSessionsCount { get; set; }
    }

    public class RecentPatientDto
    {
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int Age { get; set; }
        public string AssignedDoctor { get; set; }
    }

    public class ExpiringItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string StoreName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int DaysUntilExpiry { get; set; }
        public int Quantity { get; set; }
    }
}
