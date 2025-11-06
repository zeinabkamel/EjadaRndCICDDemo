using CosmeticClinicManagement.Services.Implementation;
using CosmeticClinicManagement.Services.Dtos.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace CosmeticClinicManagement.Controllers.Dashboard
{
    [Route("api/dashboard/admin")]
    public class AdminDashboardController : AbpController
    {
        private readonly AdminDashboardAppService _service;

        public AdminDashboardController(AdminDashboardAppService service)
        {
            _service = service;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<AdminDashboardDto>> GetStats()
        {
            var result = await _service.GetDashboardDataAsync();
            return Ok(result);
        }
    }
}
