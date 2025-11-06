using Microsoft.Extensions.Localization;
using CosmeticClinicManagement.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace CosmeticClinicManagement;

[Dependency(ReplaceServices = true)]
public class CosmeticClinicManagementBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<CosmeticClinicManagementResource> _localizer;

    public CosmeticClinicManagementBrandingProvider(IStringLocalizer<CosmeticClinicManagementResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
