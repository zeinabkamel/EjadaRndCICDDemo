using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CosmeticClinicManagement.Services.Dtos.Store;
using CosmeticClinicManagement.Services.Interfaces;

namespace CosmeticClinicManagement.Pages.Stores
{
    public class DeleteModel : PageModel
    {
        private readonly IStoreAppService _storeAppService;

        public StoreDto? Store { get; set; }

        public DeleteModel(IStoreAppService storeAppService)
        {
            _storeAppService = storeAppService;
        }

        public async Task OnGetAsync(Guid id)
        {
            Store = await _storeAppService.GetAsync(id);
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            await _storeAppService.DeleteAsync(id);
            return RedirectToPage("./Index");
        }
    }
}