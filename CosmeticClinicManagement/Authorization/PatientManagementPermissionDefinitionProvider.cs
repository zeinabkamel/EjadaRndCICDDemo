using CosmeticClinicManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.PermissionManagement;

namespace CosmeticClinicManagement.Authorization
{
    public class PatientManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup("PatientManagement",
                LocalizableString.Create<CosmeticClinicManagementResource>("PatientManagement"));

            var permission = myGroup.AddPermission("PatientManagement",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:PatientManagement"));

            permission.AddChild("PatientManagement.Create",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:CreatePatient"));

            permission.AddChild("PatientManagement.Edit",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:EditPatient"));

            permission.AddChild("PatientManagement.Delete",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:DeletePatient"));

            permission.AddChild("PatientManagement.View",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:ViewPatient"));

        }
    }
}
