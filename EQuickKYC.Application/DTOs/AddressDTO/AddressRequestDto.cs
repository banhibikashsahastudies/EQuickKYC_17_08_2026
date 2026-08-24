namespace EQuickKYC.Application.DTOs.AddressDTO
{
    public class AddressRequestDto
    {
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public int? ZipCode { get; set; }
    }
}
