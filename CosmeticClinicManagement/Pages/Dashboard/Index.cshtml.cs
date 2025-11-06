using CosmeticClinicManagement.Services.Implementation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Users;

namespace CosmeticClinicManagement.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly ICurrentUser _currentUser;
        private readonly AdminDashboardAppService _adminService;
        private readonly DoctorDashboardAppService _doctorService;
        private readonly ReceptionistDashboardAppService _receptionistService;

        public string Role { get; set; } = "";
        public string UserName { get; set; } = "";

        public IndexModel(
            ICurrentUser currentUser,
            AdminDashboardAppService adminService,
            DoctorDashboardAppService doctorService,
            ReceptionistDashboardAppService receptionistService)
        {
            _currentUser = currentUser;
            _adminService = adminService;
            _doctorService = doctorService;
            _receptionistService = receptionistService;
        }

        public async Task OnGetAsync()
        {
            UserName = $"{_currentUser.Name} {_currentUser.SurName}";

            if (_currentUser.Roles.Contains("admin"))
                Role = "Admin";
            else if (_currentUser.Roles.Contains("Doctor"))
                Role = "Doctor";
            else if (_currentUser.Roles.Contains("Receptionist"))
                Role = "Receptionist";
            else
                Role = "Unknown";
        }
    }
}
