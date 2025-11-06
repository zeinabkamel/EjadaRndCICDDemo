using CosmeticClinicManagement.Services.Dtos.Store;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CosmeticClinicManagement.Pages.Stores
{
    public class IndexModel : PageModel
    {
        private readonly IStoreAppService _storeAppService;

        public List<StoreDto> Stores { get; set; } = new();

        public IndexModel(IStoreAppService storeAppService)
        {
            _storeAppService = storeAppService;
        }

        public async Task OnGetAsync()
        {
            Stores = await _storeAppService.GetAllStoresAsync();
        }
    }
}
