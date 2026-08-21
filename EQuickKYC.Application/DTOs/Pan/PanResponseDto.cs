namespace EQuickKYC.Application.DTOs.Pan
{
    public record PanResponseDto
    {
        public string Name { get; set; }
        public string PanNo { get; set; }
        public DateOnly? DOB { get; set; }
    }
}
