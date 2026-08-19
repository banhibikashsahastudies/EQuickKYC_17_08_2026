namespace EQuickKYC.Domain.Entities
{
    public class EmailOTP
    {
        public Guid? Id { get; set; }
        public string OTP { get; set; }
        public string HashOTP { get; set; }
        public string Email { get; set; }
        public string HashEmail { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
    }
}
