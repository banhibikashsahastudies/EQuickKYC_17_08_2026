namespace EQuickKYC.Domain.Entities
{
    public class Bank
    {
        public Guid Id { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string IFSCCode { get; set; }
        public string MICRCode { get; set; }
        public string Url { get; set; }
        public Guid AddressId { get; set; }
        public Address? Address { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
