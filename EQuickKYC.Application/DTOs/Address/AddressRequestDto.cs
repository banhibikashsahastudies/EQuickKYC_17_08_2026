namespace EQuickKYC.Application.DTOs.Address
{
    public class AddressRequestDto
    {
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public int? ZipCode { get; set; }
    }
}
