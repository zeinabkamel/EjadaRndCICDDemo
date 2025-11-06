using Volo.Abp.Identity;

namespace CosmeticClinicManagement.Constants
{
    public class DefaultRolesNames
    {
        public static readonly string Admin = "Admin";
        public static readonly string Receptionist = "Receptionist";
        public static readonly string Doctor = "Doctor";

        public static readonly string[] AllRoles = [
            Admin,
            Receptionist,
            Doctor
        ];
    }
}
