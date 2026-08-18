namespace EQuickKYC.Application.DTOs.User
{
    public class AddUserRequest
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        //public Address? Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }
}
