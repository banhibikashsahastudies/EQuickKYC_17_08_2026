using EQuickKYC.Application.DTOs.Pan;
using EQuickKYC.Domain.Entities;

namespace EQuickKYC.Application.Interfaces
{
    public interface IPanRegistrationService
    {
        Task<bool> RegisterPanAsync(RegistrationMaster registrationMaster, Guid userMasterId);
        Task<PanResponseDto> GetPanDetailsByPannumberAsync(string PanNo);
    }
}
