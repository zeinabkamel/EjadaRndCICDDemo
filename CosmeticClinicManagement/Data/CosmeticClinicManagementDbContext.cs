using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.InventoryManagement;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace CosmeticClinicManagement.Data;

public class CosmeticClinicManagementDbContext : AbpDbContext<CosmeticClinicManagementDbContext>
{
    public CosmeticClinicManagementDbContext(DbContextOptions<CosmeticClinicManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<TreatmentPlan> TreatmentPlans { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Patient> Patients { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        builder.Entity<TreatmentPlan>(b =>
        {
            b.ToTable("AppTreatmentPlans");

            b.ConfigureByConvention();

            b.Property(x => x.DoctorId).IsRequired();
            b.Property(x => x.PatientId).IsRequired();

            b.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired();

            b.HasMany(x => x.Sessions)
                .WithOne()
                .HasForeignKey("PlanId")
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Session>(b =>
        {
            b.ToTable("AppSessions");
            b.ConfigureByConvention();

            b.Property<Guid>("PlanId");
            b.HasKey(x => x.Id);

            b.Property(x => x.SessionDate).IsRequired();
            b.Property(x => x.Status).IsRequired();
            b.OwnsMany(x => x.UsedMaterials, a =>
            {
                a.ToTable("AppSessionUsedMaterials");
                a.WithOwner().HasForeignKey("SessionId");

                a.Property(x => x.RawMaterialId).IsRequired();
                a.Property(x => x.Quantity).IsRequired();

                a.HasKey("SessionId", "RawMaterialId");
            });
        });

        builder.Entity<Store>(b =>
        {
            b.ToTable("AppStores");
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(200);
            b.HasMany(x => x.RawMaterials)
                .WithOne()
                .HasForeignKey("StoreId")
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<RawMaterial>(b =>
        {
            b.ToTable("AppRawMaterials");
            b.ConfigureByConvention();
            b.Property<Guid>("StoreId");
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).IsRequired().HasMaxLength(200);
            b.Property(x => x.Description).IsRequired().HasMaxLength(1000);
            b.Property(x => x.Quantity).IsRequired();
            b.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
            b.Property(x => x.ExpiryDate).IsRequired();
        });

        builder.Entity<Patient>(b =>
        {
            b.ToTable("AppPatients");
            b.ConfigureByConvention();
            b.Property(x => x.DateOfBirth).IsRequired();
            b.Property(x => x.PhoneNumber).HasMaxLength(500).IsRequired();
            b.Property(x => x.Email).HasMaxLength(500);
            b.Ignore(x => x.FullName);
        });
    }
}
