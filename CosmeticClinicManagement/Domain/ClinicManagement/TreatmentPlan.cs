using Volo.Abp.Domain.Entities.Auditing;

namespace CosmeticClinicManagement.Domain.ClinicManagement
{
    public class TreatmentPlan : FullAuditedAggregateRoot<Guid>
    {
        public Guid DoctorId { get; private set; }
        public Guid PatientId { get; private set; }
        public List<Session> Sessions { get; private set; }
        public TreatmentPlanStatus Status { get; private set; }

        protected TreatmentPlan() { }

        public TreatmentPlan(Guid Id, Guid doctorId, Guid patientId) : base(Id)
        {
            DoctorId = doctorId;
            PatientId = patientId;
            Status = TreatmentPlanStatus.Ongoing;
            //Sessions = new List<Session>();
            Sessions = [];
        }


        internal void AddSession(Session session)
        {
            Sessions.Add(session);
        }

        public void AddUsedMaterialToSession(Guid sessionId, UsedMaterial usedMaterial)
        {
            ThrowExceptionIfClosed();
            var session = GetSessionById(sessionId);
            session.AddUsedMaterial(usedMaterial);
        }

        public void RemoveSession(Guid sessionId)
        {
            ThrowExceptionIfClosed();
            var session = GetSessionById(sessionId);
            if (session.Status != SessionStatus.Planned)
            {
                throw new InvalidOperationException("Only planned sessions can be removed from the treatment plan.");
            }
            Sessions.Remove(session);
        }

        public void StartSession(Guid sessionId)
        {
            ThrowExceptionIfClosed();

            if (HasActiveSession())
            {
                throw new InvalidOperationException("There is already an active session in the treatment plan.");
            }

            var session = GetSessionById(sessionId);

            if (!IsNext(session))
            {
                throw new InvalidOperationException("Only the earliest planned session can be started.");
            }

            session.UpdateStatus(SessionStatus.InProgress);
        }

        public void CancelSession(Guid sessionId)
        {
            ThrowExceptionIfClosed();

            var session = GetSessionById(sessionId);
            session.UpdateStatus(SessionStatus.Cancelled);
        }

        public void MarkSessionAsCompleted(Guid sessionId)
        {
            ThrowExceptionIfClosed();

            var session = GetSessionById(sessionId);
            if (session.Status != SessionStatus.InProgress)
            {
                throw new InvalidOperationException("There is no active session!");
            }

            session.UpdateStatus(SessionStatus.Completed);
        }

        public void Close()
        {
            ThrowExceptionIfClosed();

            if (!Sessions.All(s => s.Status == SessionStatus.Cancelled || s.Status == SessionStatus.Completed))
            {
                throw new InvalidOperationException("You cannot close a plan with opened or planned sessions");
            }

            Status = TreatmentPlanStatus.Closed;
        }


        internal bool IsValidNewSessionDate(DateTime date)
        {
            if (HasActiveSession() && date < CurrentActiveSession().SessionDate)
            {
                return false;
            }

            if (Sessions.Any(s => s.SessionDate == date))
            {
                return false;
            }

            if (date < DateTime.Now.AddMinutes(-5))
            {
                return false;
            }

            return true;
        }

        private bool IsNext(Session session)
        {
            return session.SessionDate == Sessions.Where(s => s.Status == SessionStatus.Planned).Min(s => s.SessionDate);
        }

        private Session GetSessionById(Guid sessionId)
        {
            return Sessions.SingleOrDefault(s => s.Id == sessionId)
                ?? throw new InvalidOperationException("Session not found in the treatment plan.");
        }

        internal void ThrowExceptionIfClosed()
        {
            if (Status == TreatmentPlanStatus.Closed)
                throw new InvalidOperationException("You cannot operate on a closed plan.");
        }

        private Session CurrentActiveSession()
        {
            var session = Sessions.SingleOrDefault(s => s.Status == SessionStatus.InProgress);
            return session ?? throw new InvalidOperationException("No active session found.");
        }

        private bool HasActiveSession()
        {
            return Sessions.Any(s => s.Status == SessionStatus.InProgress);
        }
    }
}
