using CosmeticClinicManagement.Constants;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace CosmeticClinicManagement.Data
{
    public class DefaultRolesDataSeeder(IdentityRoleManager roleManager,
        IGuidGenerator guidGenerator) : IDataSeedContributor, ITransientDependency
    {
        private readonly IdentityRoleManager _roleManager = roleManager;
        private readonly IGuidGenerator _guidGenerator = guidGenerator;
        public async Task SeedAsync(DataSeedContext context)
        {
            foreach (var roleName in DefaultRolesNames.AllRoles)
            {
                await CreateRoleIfNotExist(roleName);
            }
        }

        private async Task CreateRoleIfNotExist(string roleName)
        {
            if (await _roleManager.FindByNameAsync(roleName) == null)
            {
                var role = new IdentityRole(_guidGenerator.Create(), roleName);
                await _roleManager.CreateAsync(role);
            }
        }
    }
}
