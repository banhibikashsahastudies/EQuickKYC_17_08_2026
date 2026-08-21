namespace EQuickKYC.Application.DTOs.Bank
{
    public record ExcelUploadProgressDto
    {
        public int TotalRows { get; set; }

        public int SavedRows { get; set; }

        public int FailedRows { get; set; }

        public double Percentage { get; set; }

        public string ElapsedTime { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
    }
}
