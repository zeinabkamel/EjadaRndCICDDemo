using CosmeticClinicManagement.Localization;
using Volo.Abp.Application.Services;

namespace CosmeticClinicManagement.Services;

/* Inherit your application services from this class. */
public abstract class CosmeticClinicManagementAppService : ApplicationService
{
    protected CosmeticClinicManagementAppService()
    {
        LocalizationResource = typeof(CosmeticClinicManagementResource);
    }
}