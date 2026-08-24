namespace EQuickKYC.Application.DTOs.Bank
{
    public class BankUploadResponseDto
    {
        public int TotalRows { get; set; }
        public int SavedRows { get; set; }
        public int FailedRows { get; set; }
        public string TimeTaken { get; set; } = string.Empty;
    }
}
