using CosmeticClinicManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace CosmeticClinicManagement.Authorization
{
    public class InventoryManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup("InventoryManagement",
                LocalizableString.Create<CosmeticClinicManagementResource>("InventoryManagement"));

            var storePermission = myGroup.AddPermission("StoreManagement",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:StoreManagement"));

            storePermission.AddChild("StoreManagement.Create",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:CreateStore"));

            storePermission.AddChild("StoreManagement.Edit",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:EditStore"));

            storePermission.AddChild("StoreManagement.Delete",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:DeleteStore"));

            storePermission.AddChild("StoreManagement.View",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:ViewStore"));


            var rawMaterialPermission = myGroup.AddPermission("RawMaterialManagement",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:RawMaterialManagement"));

            rawMaterialPermission.AddChild("RawMaterialManagement.Create",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:CreateRawMaterial"));

            rawMaterialPermission.AddChild("RawMaterialManagement.Edit",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:EditRawMaterial"));

            rawMaterialPermission.AddChild("RawMaterialManagement.Delete",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:DeleteRawMaterial"));

            rawMaterialPermission.AddChild("RawMaterialManagement.View",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:ViewRawMaterial"));

        }
    }
}
