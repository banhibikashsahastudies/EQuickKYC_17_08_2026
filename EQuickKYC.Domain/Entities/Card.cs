namespace EQuickKYC.Domain.Entities
{
    public class Card
    {
        public Guid? CardId { get; set; }

        public string? AadhaarNo {  get; set; }

        public string? PanNo { get; set; }

        public string? VoterNo { get; set; }

        public string? DrivingLicenseNo { get; set; }
    }
}
