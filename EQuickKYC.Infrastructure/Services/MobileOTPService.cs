using EQuickKYC.Application.DTOs.Mobile;
using EQuickKYC.Application.Interfaces;
using EQuickKYC.Domain.Entities;
using EQuickKYC.Infrastructure.Data;
using EQuickKYC.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace EQuickKYC.Infrastructure.Services
{
    public class MobileOTPService : IMobileOTPService
    {
        private readonly EQuickKYCDbContext _dbContext;
        private readonly IEncryptionService _encryptionService;
        private readonly IHashService _hashService;

        public MobileOTPService(EQuickKYCDbContext dbContext, IEncryptionService encryptionService, IHashService hashService)
        {
            _dbContext = dbContext;
            _encryptionService = encryptionService;
            _hashService = hashService;
        }
        public async Task<MobileOtpResponseDto> SendMobileOTPAsync(string mobile)
        {
            var hashedMobile = _hashService.Hash(mobile);

            var isVerified = await _dbContext.MobileOTPs
                    .Where(x => x.MobileHash == hashedMobile)
                    .FirstOrDefaultAsync();

            // If there's an existing record and it was already verified, do not send a new OTP.
            if (isVerified?.VerifiedAt != null)
            {
                return null;
            }

            // Generate a 6-digit OTP.
            string otp = Random.Shared.Next(100000, 1000000).ToString();

            var mobileOtp = new MobileOTP
            {
                Id = Guid.NewGuid(),
                Mobile = _encryptionService.Encrypt(mobile),
                MobileHash = hashedMobile,
                OTP = _encryptionService.Encrypt(otp),
                OTPHash = _hashService.Hash(otp),
                CreatedAt = DateTime.UtcNow,

            };

            _dbContext.MobileOTPs.Add(mobileOtp);

            await _dbContext.SaveChangesAsync();
            return new MobileOtpResponseDto
            {
                Mobile = _encryptionService.Decrypt(mobileOtp.Mobile),
                OTP = _encryptionService.Decrypt(mobileOtp.OTP)
            };
        }

        public async Task<RegistrationResponseDto> VerifyMobileOTPAsync(string mobile, string otp, string flag)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {

                var hasedMobile = _hashService.Hash(mobile);
                var hashedOTP = _hashService.Hash(otp);

                var mobileOtp = await _dbContext.MobileOTPs
                    .Where(x => x.MobileHash == hasedMobile && x.OTPHash == hashedOTP)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();

                if (mobileOtp == null)
                {
                    return null;
                }

                if (mobileOtp.OTPHash.ToString() != hashedOTP) return null;

                if (mobileOtp.VerifiedAt != null) return null;
                mobileOtp.VerifiedAt = DateTime.UtcNow;

                var user = new UserMaster
                {
                    Id = Guid.NewGuid(),
                    MobileOTPId = mobileOtp.Id,
                    EmailOTPId = null,
                    CreatedAt = DateTime.UtcNow,
                    IsMobileVerified = true,
                    IsEmailVerified = false,
                    MobileVerifiedAt = DateTime.UtcNow,
                    EmailVerifiedAt = null,
                    Status = true,
                    Flag = flag,
                };

                var registraTionMaster = new RegistrationMaster
                {
                    Id = Guid.NewGuid(),
                    UserMasterId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    ApplicationPrefix = "APP"
                };

                _dbContext.UserMasters.Add(user);
                _dbContext.RegistrationMasters.Add(registraTionMaster);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new RegistrationResponseDto
                {
                    RegistrationId = registraTionMaster.Id,
                    UserMasterId = user.Id
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }
}