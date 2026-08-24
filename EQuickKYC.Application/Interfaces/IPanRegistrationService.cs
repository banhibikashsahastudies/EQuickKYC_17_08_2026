using EQuickKYC.Application.DTOs.Pan;
using EQuickKYC.Application.DTOs.Register;
using EQuickKYC.Domain.Entities;

namespace EQuickKYC.Application.Interfaces
{
    public interface IPanRegistrationService
    {
        Task<bool> RegisterPanAsync(RegistrationMaster registrationMaster, Guid userMasterId);
        Task<PanResponseDto> GetPanDetailsByPannumberAsync(string PanNo);
        Task<string> ChangePrefix(RegistrationMaster registrationMaster);
        Task<RegistrationMaster> GetUserById(Guid id);
    }
}
