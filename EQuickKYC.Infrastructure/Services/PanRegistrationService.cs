using EQuickKYC.Application.DTOs.Pan;
using EQuickKYC.Application.Interfaces;
using EQuickKYC.Domain.Entities;
using EQuickKYC.Infrastructure.Data;
using EQuickKYC.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace EQuickKYC.Infrastructure.Services
{
    public class PanRegistrationService : IPanRegistrationService
    {
        private readonly EQuickKYCDbContext _dbContext;
        private readonly IEncryptionService _encryptionService;
        private readonly IHashService _hashService;

        public PanRegistrationService(EQuickKYCDbContext dbContext, IEncryptionService encryptionService, IHashService hashService)
        {
            _dbContext = dbContext;
            _encryptionService = encryptionService;
            _hashService = hashService;
        }

        public async Task<PanResponseDto> GetPanDetailsByPannumberAsync(string PanNo)
        {

            var hashedPan = _hashService.Hash(PanNo);

            var panDetails = await _dbContext.RegistrationMasters.FirstOrDefaultAsync(x => x.PanNoHash == hashedPan);
            //return _encryptionService.Decrypt(panDetails?.PanNo.ToUpper());
            if (panDetails == null)
                return null;

            return new PanResponseDto
            {
                Name = panDetails.Name,
                PanNo = _encryptionService.Decrypt(panDetails.PanNo),
                DOB = panDetails.DOB,
            };
        }

        public async Task<bool> RegisterPanAsync(RegistrationMaster registrationMaster, Guid userMasterId)
        {

            var registration = await _dbContext.RegistrationMasters.FirstOrDefaultAsync(x => x.UserMasterId == userMasterId);

            if (registration == null) return false;

            registration.Name = registrationMaster?.Name?.ToUpper();
            registration.PanNo = _encryptionService.Encrypt(registrationMaster.PanNo);
            registration.PanNoHash = _hashService.Hash(registrationMaster.PanNo);
            registration.DOB = registrationMaster.DOB;
            registration.UpdatedAt = DateTime.UtcNow;
            registration.status = true;

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
