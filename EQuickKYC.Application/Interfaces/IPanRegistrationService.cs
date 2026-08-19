using EQuickKYC.Domain.Entities;

namespace EQuickKYC.Application.Interfaces
{
    public interface IPanRegistrationService
    {
        Task<bool> RegisterPanAsync(RegistrationMaster registrationMaster, Guid userMasterId);
        Task<string> GetPanDetailsByPannumberAsync(string PanNo);
    }
}
