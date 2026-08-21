namespace EQuickKYC.Application.DTOs.Bank
{
    public record UpdateBankRequestDto
    {
        public Guid Id { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string? BranchCode { get; set; }
        public string IFSCCode { get; set; }
        public string MICRCode { get; set; }
        public string? Url { get; set; }
        public Guid? AddressId { get; set; }

        //public Address? Address { get; set; }
        public bool Status { get; set; }
    }
}
