using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using CosmeticClinicManagement.Domain.InventoryManagement;
using CosmeticClinicManagement.Services.Dtos.Dashboard;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace CosmeticClinicManagement.Services.Implementation
{
    public class AdminDashboardAppService : ApplicationService
    {
        private readonly IRepository<Patient, Guid> _patientRepository;
        private readonly IRepository<TreatmentPlan, Guid> _treatmentPlanRepository;
        private readonly IRepository<Store, Guid> _storeRepository;
        private readonly IdentityUserManager _userManager;
        private readonly IIdentityUserRepository _userRepository;

        // Low stock threshold
        private const int LOW_STOCK_THRESHOLD = 10;
        private const int EXPIRY_WARNING_DAYS = 30;

        public AdminDashboardAppService(
            IRepository<Patient, Guid> patientRepository,
            IRepository<TreatmentPlan, Guid> treatmentPlanRepository,
            IRepository<Store, Guid> storeRepository,
            IdentityUserManager userManager,
            IIdentityUserRepository userRepository)
        {
            _patientRepository = patientRepository;
            _treatmentPlanRepository = treatmentPlanRepository;
            _storeRepository = storeRepository;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<AdminDashboardDto> GetDashboardDataAsync()
        {
            try
            {
                Logger.LogInformation("Starting to fetch admin dashboard data...");

                // Get basic counts
                var totalPatients = await _patientRepository.GetCountAsync();
                Logger.LogInformation($"Total patients: {totalPatients}");

                //// Count doctors
                //var allUsers = await _userManager.Users.ToListAsync();
                //var doctors = new List<IdentityUser>();
                //foreach (var user in allUsers)
                //{
                //    var roles = await _userManager.GetRolesAsync(user);
                //    if (roles.Contains("Doctor"))
                //        doctors.Add(user);
                //}
                //Logger.LogInformation($"Total doctors: {doctors.Count}");

                var allUsers = await _userRepository.GetListAsync();
                var doctors = new List<IdentityUser>();

                foreach (var user in allUsers)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Doctor"))
                        doctors.Add(user);
                }

                // Get treatment plans counts
                var activePlans = await _treatmentPlanRepository.CountAsync(tp => tp.Status == TreatmentPlanStatus.Ongoing);
                var closedPlans = await _treatmentPlanRepository.CountAsync(tp => tp.Status == TreatmentPlanStatus.Closed);
                Logger.LogInformation($"Active plans: {activePlans}, Closed plans: {closedPlans}");

                // Revenue estimation
                decimal totalRevenue = (activePlans * 400) + (closedPlans * 750);

                // Initialize collections
                var lowStockItems = new List<LowStockItemDto>();
                var expiringItems = new List<ExpiringItemDto>();
                int totalStores = 0;
                int totalRawMaterials = 0;

                // Try to get inventory stats
                try
                {
                    var storesQuery = await _storeRepository.GetQueryableAsync();
                    var stores = await storesQuery.Include(s => s.RawMaterials).ToListAsync();
                    totalStores = stores.Count;
                    Logger.LogInformation($"Total stores: {totalStores}");

                    if (stores.Any())
                    {
                        var allRawMaterials = stores
                            .Where(s => s.RawMaterials != null)
                            .SelectMany(s => s.RawMaterials.Select(rm => new { Store = s, Material = rm }))
                            .ToList();

                        totalRawMaterials = allRawMaterials.Count;
                        Logger.LogInformation($"Total raw materials: {totalRawMaterials}");

                        // Low stock items
                        lowStockItems = allRawMaterials
                            .Where(x => x.Material.Quantity <= LOW_STOCK_THRESHOLD)
                            .OrderBy(x => x.Material.Quantity)
                            .Take(10)
                            .Select(x => new LowStockItemDto
                            {
                                Id = x.Material.Id,
                                Name = x.Material.Name,
                                StoreName = x.Store.Name,
                                CurrentQuantity = x.Material.Quantity,
                                Price = x.Material.Price,
                                ExpiryDate = x.Material.ExpiryDate
                            })
                            .ToList();

                        // Expiring items
                        expiringItems = allRawMaterials
                            .Where(x => x.Material.ExpiryDate <= DateTime.Now.AddDays(EXPIRY_WARNING_DAYS)
                                     && x.Material.ExpiryDate > DateTime.Now)
                            .OrderBy(x => x.Material.ExpiryDate)
                            .Take(10)
                            .Select(x => new ExpiringItemDto
                            {
                                Id = x.Material.Id,
                                Name = x.Material.Name,
                                StoreName = x.Store.Name,
                                ExpiryDate = x.Material.ExpiryDate,
                                DaysUntilExpiry = (x.Material.ExpiryDate - DateTime.Now).Days,
                                Quantity = x.Material.Quantity
                            })
                            .ToList();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex, "Error fetching inventory data. Continuing without inventory stats.");
                }

                // Initialize doctor and patient collections
                var topDoctorsDto = new List<TopDoctorDto>();
                var recentPatientsDto = new List<RecentPatientDto>();

                try
                {
                    // Get treatment plans with sessions
                    var treatmentPlansQuery = await _treatmentPlanRepository.GetQueryableAsync();
                    var treatmentPlans = await treatmentPlansQuery
                        .Include(tp => tp.Sessions)
                        .ToListAsync();
                    Logger.LogInformation($"Total treatment plans retrieved: {treatmentPlans.Count}");

                    if (treatmentPlans.Any() && doctors.Any())
                    {
                        // Top doctors by active plans
                        var topDoctors = treatmentPlans
                            .GroupBy(tp => tp.DoctorId)
                            .Select(g => new
                            {
                                DoctorId = g.Key,
                                ActivePlansCount = g.Count(tp => tp.Status == TreatmentPlanStatus.Ongoing),
                                CompletedSessionsCount = g.Where(tp => tp.Sessions != null)
                                    .SelectMany(tp => tp.Sessions)
                                    .Count(s => s.Status == SessionStatus.Completed)
                            })
                            .OrderByDescending(x => x.ActivePlansCount)
                            .Take(5)
                            .ToList();

                        var doctorDetails = doctors
                            .Where(d => topDoctors.Any(td => td.DoctorId == d.Id))
                            .ToDictionary(d => d.Id, d => $"{d.Name} {d.Surname}");

                        topDoctorsDto = topDoctors
                            .Where(td => doctorDetails.ContainsKey(td.DoctorId))
                            .Select(td => new TopDoctorDto
                            {
                                DoctorId = td.DoctorId,
                                DoctorName = doctorDetails[td.DoctorId],
                                ActivePlansCount = td.ActivePlansCount,
                                CompletedSessionsCount = td.CompletedSessionsCount
                            })
                            .ToList();
                    }

                    // Recent patients
                    var patientsQuery = await _patientRepository.GetQueryableAsync();
                    var recentPatients = await patientsQuery
                        .OrderByDescending(p => p.CreationTime)
                        .Take(10)
                        .ToListAsync();

                    if (recentPatients.Any())
                    {
                        var doctorDetails = doctors.ToDictionary(d => d.Id, d => $"{d.Name} {d.Surname}");

                        foreach (var patient in recentPatients)
                        {
                            var patientPlans = treatmentPlans.Where(tp => tp.PatientId == patient.Id).ToList();
                            var doctorId = patientPlans.FirstOrDefault()?.DoctorId;
                            var doctorName = doctorId.HasValue && doctorDetails.ContainsKey(doctorId.Value)
                                ? doctorDetails[doctorId.Value]
                                : "Not Assigned";

                            recentPatientsDto.Add(new RecentPatientDto
                            {
                                PatientId = patient.Id,
                                PatientName = $"{patient.FirstName} {patient.LastName}",
                                RegistrationDate = patient.CreationTime,
                                Age = DateTime.Now.Year - patient.DateOfBirth.Year,
                                AssignedDoctor = doctorName
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex, "Error fetching doctor/patient analytics. Continuing with basic stats.");
                }

                var result = new AdminDashboardDto
                {
                    TotalPatients = totalPatients,
                    TotalDoctors = doctors.Count,
                    ActiveTreatmentPlans = activePlans,
                    ClosedTreatmentPlans = closedPlans,
                    TotalRevenue = totalRevenue,
                    TotalStores = totalStores,
                    TotalRawMaterials = totalRawMaterials,
                    LowStockItemsCount = lowStockItems.Count,
                    LowStockItems = lowStockItems,
                    TopDoctors = topDoctorsDto,
                    RecentPatients = recentPatientsDto,
                    ExpiringItems = expiringItems
                };

                Logger.LogInformation("Admin dashboard data fetched successfully.");
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Critical error while fetching admin dashboard data.");

                // Return empty data instead of throwing
                return new AdminDashboardDto
                {
                    TotalPatients = 0,
                    TotalDoctors = 0,
                    ActiveTreatmentPlans = 0,
                    ClosedTreatmentPlans = 0,
                    TotalRevenue = 0,
                    TotalStores = 0,
                    TotalRawMaterials = 0,
                    LowStockItemsCount = 0,
                    LowStockItems = new List<LowStockItemDto>(),
                    TopDoctors = new List<TopDoctorDto>(),
                    RecentPatients = new List<RecentPatientDto>(),
                    ExpiringItems = new List<ExpiringItemDto>()
                };
            }
        }
    }
}