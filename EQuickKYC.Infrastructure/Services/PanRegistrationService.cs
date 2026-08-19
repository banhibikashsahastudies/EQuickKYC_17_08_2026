using EQuickKYC.Application.Interfaces;
using EQuickKYC.Domain.Entities;
using EQuickKYC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EQuickKYC.Infrastructure.Services
{
    public class PanRegistrationService : IPanRegistrationService
    {
        private readonly EQuickKYCDbContext _dbContext;

        public PanRegistrationService(EQuickKYCDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GetPanDetailsByPannumberAsync(string PanNo)
        {
            var panDetails = await _dbContext.RegistrationMasters.FirstOrDefaultAsync(x => x.PanNo == PanNo);
            return panDetails?.PanNo;
        }

        public async Task<bool> RegisterPanAsync(RegistrationMaster registrationMaster, Guid userMasterId)
        {

            var registration = await _dbContext.RegistrationMasters
                .FirstOrDefaultAsync(x => x.UserMasterId == userMasterId);

            if (registration == null)
            {
                return false;
            }

            registration.Name = registrationMaster.Name;
            registration.PanNo = registrationMaster.PanNo.ToUpper();
            registration.DOB = registrationMaster.DOB;
            registration.UpdatedAt = DateTime.UtcNow;
            registration.status = true;
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
