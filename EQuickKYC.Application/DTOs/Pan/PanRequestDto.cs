namespace EQuickKYC.Application.DTOs.Pan
{
    public record PanRequestDto
    {
        public string Name { get; init; } = string.Empty;
        public string PanNo { get; init; } = string.Empty;
        public DateOnly DOB { get; init; }

    }
}
