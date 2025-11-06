using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CosmeticClinicManagement.Services.Dtos.Store;
using CosmeticClinicManagement.Services.Interfaces;

namespace CosmeticClinicManagement.Pages.Stores
{
    public class EditModel : PageModel
    {
        private readonly IStoreAppService _storeAppService;

        [BindProperty]
        public UpdateStoreDto Input { get; set; } = new();

        public Guid Id { get; set; }

        public EditModel(IStoreAppService storeAppService)
        {
            _storeAppService = storeAppService;
        }

        public async Task OnGetAsync(Guid id)
        {
            Id = id;
            var store = await _storeAppService.GetAsync(id);
            Input.Name = store.Name;
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid) return Page();
            await _storeAppService.UpdateAsync(id, Input);
            return RedirectToPage("/Stores/Index");
        }
    }
}