using CosmeticClinicManagement.Services.Dtos.Store;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CosmeticClinicManagement.Pages.Stores
{
    public class DetailsModel : PageModel
    {
        private readonly IStoreAppService _storeAppService;

        public StoreDto? Store { get; set; }

        public DetailsModel(IStoreAppService storeAppService)
        {
            _storeAppService = storeAppService;
        }

        public async Task OnGetAsync(Guid id)
        {
            Store = await _storeAppService.GetAsync(id);
        }
    }
}