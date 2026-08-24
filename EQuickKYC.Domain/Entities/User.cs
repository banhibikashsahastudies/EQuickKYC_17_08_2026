namespace EQuickKYC.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
        public Guid CardId { get; set; }
        public Card Card { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        //add-ons
        public bool IsDeleted { get; set; } = false;    //soft delete
        public bool status { get; set; } = true;    //active or in-active
    }
}
