using Volo.Abp.Domain.Entities.Auditing;

namespace CosmeticClinicManagement.Domain.ClinicManagement
{
    public class Session : FullAuditedEntity<Guid>
    {
        public DateTime SessionDate { get; private set; }
        public List<UsedMaterial> UsedMaterials { get; private set; }
        public List<string> Notes { get; private set; }
        public SessionStatus Status { get; private set; }
        public Guid PlanId { get; private set; }

        protected Session() { }

        public Session(Guid Id, Guid planId, DateTime sessionDate, List<string> notes, SessionStatus status) : base(Id)
        {
            if (sessionDate < DateTime.Now)
            {
                throw new ArgumentException("Session date cannot be in the past.");
            }

            if (status == SessionStatus.Completed || status == SessionStatus.Cancelled)
            {
                throw new ArgumentException("New sessions cannot have a 'Completed' or 'Cancelled' status.");
            }

            SessionDate = sessionDate;
            UsedMaterials = [];
            Notes = notes;
            Status = status;
            PlanId = planId;
        }

        public void AddUsedMaterial(UsedMaterial usedMaterial)
        {
            if (!IsInProgress())
            {
                throw new InvalidOperationException("Cannot add used materials to an inactive session.");
            }

            var existingMaterial = UsedMaterials.FirstOrDefault(um => um.RawMaterialId == usedMaterial.RawMaterialId);
            if (existingMaterial != null)
            {
                existingMaterial.AddQuantity(usedMaterial.Quantity);
            }
            else
            {
                UsedMaterials.Add(usedMaterial);
            }
        }

        public void AddNote(string note)
        {
            if (!IsOpen())
            {
                throw new InvalidOperationException("Cannot add notes to a closed session.");
            }

            Notes.Add(note);
        }

        public void UpdateStatus(SessionStatus newStatus)
        {
            if (newStatus < Status)
            {
                throw new InvalidOperationException("Cannot revert to a previous status.");
            }

            Status = newStatus;
        }

        private bool IsInProgress()
        {
            return Status == SessionStatus.InProgress;
        }

        private bool IsOpen()
        {
            return Status == SessionStatus.Planned || Status == SessionStatus.InProgress;
        }
    }
}
