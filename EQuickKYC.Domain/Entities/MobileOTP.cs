namespace EQuickKYC.Domain.Entities
{
    public class MobileOTP
    {
        public Guid Id { get; set; }
        public string OTP { get; set; }
        public string Mobile { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
    }
}
