using EQuickKYC.Application.DTOs.AddressDTO;
using EQuickKYC.Application.DTOs.CardDTO;

namespace EQuickKYC.Application.DTOs.User
{
    public class AddUserRequest
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public AddCardRequest CardRequest { get; set; }
        public AddAddressRequest AddressRequest { get; set; }
    }
}
