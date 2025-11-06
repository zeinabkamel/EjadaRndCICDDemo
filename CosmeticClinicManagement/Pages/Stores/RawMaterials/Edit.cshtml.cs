using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CosmeticClinicManagement.Services.Dtos.Store;
using CosmeticClinicManagement.Services.Interfaces;

namespace CosmeticClinicManagement.Pages.Stores.RawMaterials
{
    public class EditModel : PageModel
    {
        private readonly IStoreAppService _storeAppService;

        [BindProperty]
        public UpdateRawMaterialDto Input { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }  // raw material ID

        [BindProperty(SupportsGet = true)]
        public Guid StoreId { get; set; }

        public EditModel(IStoreAppService storeAppService)
        {
            _storeAppService = storeAppService;
        }

        public async Task<IActionResult> OnGetAsync(Guid id, Guid storeId)
        {
            Id = id;
            StoreId = storeId;

            var raw = await _storeAppService.GetRawMaterialAsync(storeId, id);
            if (raw == null)
            {
                return NotFound();
            }

            Input.Name = raw.Name;
            Input.Description = raw.Description;
            Input.Quantity = raw.Quantity;
            Input.Price = raw.Price;
            Input.ExpiryDate = raw.ExpiryDate;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _storeAppService.UpdateRawMaterialAsync(StoreId, Id, Input);
            return RedirectToPage("/Stores/Details", new { id = StoreId });
        }
    }
}
