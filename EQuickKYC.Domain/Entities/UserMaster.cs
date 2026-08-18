namespace EQuickKYC.Domain.Entities
{
    public class UserMaster
    {
        public Guid Id { get; set; }
        public Guid MobileOTPId { get; set; }
        public Guid? EmailOTPId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? MobileVerifiedAt { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }
        public bool IsMobileVerified { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool Status { get; set; }
    }
}
