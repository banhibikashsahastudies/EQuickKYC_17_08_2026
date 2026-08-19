namespace EQuickKYC.Application.DTOs.Address
{
    public record AddressResponseDto
    {
        public Guid? AddressId { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public int? ZipCode { get; set; }
    }
}
