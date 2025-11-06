using CosmeticClinicManagement.Services.Dtos.Dashboard;
using Volo.Abp.Application.Services;

namespace CosmeticClinicManagement.Services.Interfaces
{
    public interface IDoctorDashboardAppService : IApplicationService
    {
        Task<DoctorDashboardDto> GetDoctorDashboardAsync();
    }
}
