using CosmeticClinicManagement.Services.Dtos.Dashboard;
using CosmeticClinicManagement.Services.Implementation;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace CosmeticClinicManagement.Controllers.Dashboard
{
    [Route("api/app/receptionist-dashboard")]

    public class ReceptionistDashboardController : AbpController
    {
        private readonly ReceptionistDashboardAppService _service;

        public ReceptionistDashboardController(ReceptionistDashboardAppService service)
        {
            _service = service;
        }

        [HttpGet("summary")]
        public async Task<ReceptionistDashboardDto> GetSummaryAsync()
        {
            return await _service.GetDashboardDataAsync();
        }
    }
}
