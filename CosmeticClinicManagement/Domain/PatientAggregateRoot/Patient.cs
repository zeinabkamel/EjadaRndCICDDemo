using Volo.Abp.Domain.Entities.Auditing;

namespace CosmeticClinicManagement.Domain.PatientAggregateRoot
{
    public class Patient : FullAuditedAggregateRoot<Guid>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string FullName => $"{FirstName} {LastName}";

        private Patient() { }

        public Patient(Guid id, string firstName, string lastName, DateTime dateOfBirth, string email, string phoneNumber) : base(id)
        {
            CheckParameters(firstName, lastName, dateOfBirth, email, phoneNumber);
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        private static void CheckParameters(string firstName, string lastName, DateTime dateOfBirth, string email, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));
            }
            
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty or null.", nameof(email));
            }
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("Phone number cannot be null or empty.", nameof(phoneNumber));
            }
        }
    }
}
