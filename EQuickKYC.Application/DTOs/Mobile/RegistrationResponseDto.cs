namespace EQuickKYC.Application.DTOs.Mobile
{
    public record RegistrationResponseDto
    {
        public Guid RegistrationId { get; init; }
        public Guid UserMasterId { get; init; }
    }
}
