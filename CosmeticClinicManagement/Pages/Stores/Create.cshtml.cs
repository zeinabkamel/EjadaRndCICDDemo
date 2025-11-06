using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CosmeticClinicManagement.Services.Dtos.Store;
using CosmeticClinicManagement.Services.Interfaces;

namespace CosmeticClinicManagement.Pages.Stores
{
    public class CreateModel : PageModel
    {
        private readonly IStoreAppService _storeAppService;

        [BindProperty]
        public CreateStoreDto Input { get; set; } = new();

        public CreateModel(IStoreAppService storeAppService)
        {
            _storeAppService = storeAppService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _storeAppService.CreateAsync(Input);
            return RedirectToPage("./Index");
        }
    }
}