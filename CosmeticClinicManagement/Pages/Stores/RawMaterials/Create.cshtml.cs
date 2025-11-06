using CosmeticClinicManagement.Localization;
using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Dtos.Store;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;
using Volo.Abp;

namespace CosmeticClinicManagement.Pages.Stores.RawMaterials
{
    public class CreateModel : PageModel
    {
        private readonly IStoreAppService _storeAppService;
         private readonly IStringLocalizer<CosmeticClinicManagementResource> _L;
        public CreateModel(IStoreAppService storeAppService,
                     IStringLocalizer<CosmeticClinicManagementResource> localizer)
        {
            _storeAppService = storeAppService;
            _L = localizer;
        }

        [BindProperty]
        public CreateRawMaterialDto Input { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public Guid StoreId { get; set; }

        public CreateModel(IStoreAppService storeAppService)
        {
            _storeAppService = storeAppService;
        }

        public void OnGet(Guid storeId)
        {
            StoreId = storeId;
            Input.StoreId = storeId;
            Input.ExpiryDate = DateTime.Now.AddMonths(6);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                await _storeAppService.CreateRawMaterialAsync(Input);
                return RedirectToPage("/Stores/Details", new { id = Input.StoreId });
            }
            catch (BusinessException ex)
            {
                var localized = _L[ex.Code].Value;
                var message = !string.IsNullOrWhiteSpace(localized) && localized != ex.Code
                                ? localized
                                : (string.IsNullOrWhiteSpace(ex.Message) ? ex.Code : ex.Message);

                var field = ex.Data?["Field"]?.ToString();
                if (!string.IsNullOrWhiteSpace(field))
                    ModelState.AddModelError($"Input.{field}", message);
                else
                    ModelState.AddModelError(string.Empty, message);

                return Page();


            }
        }
    }
}