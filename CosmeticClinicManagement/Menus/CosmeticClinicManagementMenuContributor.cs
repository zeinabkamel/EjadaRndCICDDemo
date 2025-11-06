using CosmeticClinicManagement.Localization;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace CosmeticClinicManagement.Menus;

public class CosmeticClinicManagementMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var l = context.GetLocalizer<CosmeticClinicManagementResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                CosmeticClinicManagementMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fas fa-home",
                order: 0
            )
        );

        if (await context.IsGrantedAsync("StoreManagement") && await context.IsGrantedAsync("RawMaterialManagement"))
        {
            context.Menu.AddItem(
            new ApplicationMenuItem(
                 "InventoryManagement",
                l["Menu:InventoryManagement"],
                icon: "fas fa-shopping-cart"
            ).AddItem(
                new ApplicationMenuItem(
                    "InventoryManagement.Stores",
                    l["Menu:Stores"],
                    url: "/Stores"
                )
            )
        );
        }

        if (await context.IsGrantedAsync("PatientManagement"))
        {
            context.Menu.AddItem(
                new ApplicationMenuItem(
                    "PatientsManagement",
                    l["Menu:PatientsManagement"],
                    icon: "fa fa-users"
                ).AddItem(
                    new ApplicationMenuItem(
                        "PatientManagement.Patients",
                        l["Menu:Patients"],
                        url: "/Patients"
                    )
                )
            );
        }
        

        if (CosmeticClinicManagementModule.IsMultiTenant)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }
    }
}
