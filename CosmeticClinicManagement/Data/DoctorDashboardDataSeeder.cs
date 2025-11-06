using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.Interfaces;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace CosmeticClinicManagement.Data
{
    public class DoctorDashboardDataSeeder(IdentityUserManager userManager,
        IPatientRepository patientRepository,
        ITreatmentPlanRepository treatmentPlanRepository,
        IGuidGenerator guidGenerator) : IDataSeedContributor, ITransientDependency
    {
        private readonly IdentityUserManager _userManager = userManager;
        private readonly IPatientRepository _patientRepository = patientRepository;
        private readonly ITreatmentPlanRepository _treatmentPlanRepository = treatmentPlanRepository;
        private readonly IGuidGenerator _guidGenerator = guidGenerator;
        public async Task SeedAsync(DataSeedContext context)
        {
            var doctorUser = await SeedDoctorIfNotExist();
            var patients = await SeedPatientsIfNotExist();
            var treatmentPlans = await SeedTreatmentPlansIfNotExist(doctorUser, patients);
            await SeedSessionsIfNotExist(treatmentPlans);
        }


        private async Task<List<TreatmentPlan>> SeedTreatmentPlansIfNotExist(IdentityUser doctorUser, List<Patient> patients)
        {
            var dbTreatmentPlans = await _treatmentPlanRepository.GetTreatmentPlansByDoctorIdAsync(doctorUser.Id);
            
            List<TreatmentPlan> treatmentPlans = [];

            foreach (var patient in patients)
            {
                if (dbTreatmentPlans.Any(tp => tp.PatientId == patient.Id))
                {
                    continue;
                }

                var treatmentPlan = new TreatmentPlan(
                    _guidGenerator.Create(),
                    doctorUser.Id,
                    patient.Id
                );
                
                treatmentPlans.Add(treatmentPlan);
            }

            await _treatmentPlanRepository.InsertManyAsync(treatmentPlans);

            return await _treatmentPlanRepository.GetTreatmentPlansByDoctorIdAsync(doctorUser.Id);
        }

        private async Task SeedSessionsIfNotExist(List<TreatmentPlan> treatmentPlans)
        {
            foreach (var plan in treatmentPlans)
            {
                if (plan.Sessions.Any(s => s.SessionDate.Day == DateTime.Now.Day))
                {
                    foreach (var session in plan.Sessions)
                    {
                        if (session.SessionDate < DateTime.Now && session.Status != SessionStatus.Completed)
                        {
                            session.UpdateStatus(SessionStatus.Completed);
                        }
                    }

                    continue;
                }

                if (plan.Sessions.Count > 0)
                {
                    foreach (var session in plan.Sessions)
                    {
                        session.UpdateStatus(SessionStatus.Completed);
                    }
                }

                var session1 = new Session(
                    _guidGenerator.Create(),
                    plan.Id,
                    DateTime.Now.AddHours(5),
                    ["Initial consultation and assessment."],
                    SessionStatus.Planned
                );
                var session2 = new Session(
                    _guidGenerator.Create(),
                    plan.Id,
                    DateTime.Now.AddHours(6),
                    ["First treatment session.", "Applied laser therapy."],
                    SessionStatus.Planned
                );
                plan.AddSession(session1);
                plan.AddSession(session2);
            }

            await _treatmentPlanRepository.UpdateManyAsync(treatmentPlans);
        }

        private async Task<List<Patient>> SeedPatientsIfNotExist()
        {
            if (await _patientRepository.GetCountAsync() > 0)
            {
                return await _patientRepository.GetListAsync();
            }

            var patient1 = new Patient(
                _guidGenerator.Create(),
                "John",
                "Doe",
                new DateTime(2000, 9, 15),
                "j@d.com",
                "1234567890"
            );

            var patient2 = new Patient(
                _guidGenerator.Create(),
                "Hossam",
                "Hassan",
                new DateTime(1999, 7, 10),
                "h@h.com",
                "1234568890"
            );

            await _patientRepository.InsertManyAsync([patient1, patient2]);

            return [patient1, patient2];
        }

        private async Task<IdentityUser> SeedDoctorIfNotExist()
        {
            var doctorUser = await _userManager.FindByNameAsync("doctor");
            if (doctorUser == null)
            {
                doctorUser = new IdentityUser(
                    _guidGenerator.Create(),
                    "doctor",
                    "d@s.com"
                );

                doctorUser.SetProperty("Name", "Osama");
                doctorUser.SetProperty("SurName", "Ibrahim");

                await _userManager.CreateAsync(doctorUser, "Doctor@123");
                await _userManager.AddToRoleAsync(doctorUser, "Doctor");
            }

            
            return doctorUser;
        }
    }
}
