using EQuickKYC.Application.Interfaces;
using EQuickKYC.Domain.Entities;
using EQuickKYC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EQuickKYC.Infrastructure.Services
{
    public class MobileOTPService : IMobileOTPService
    {
        private readonly EQuickKYCDbContext _dbContext;

        public MobileOTPService(EQuickKYCDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> SendMobileOTPAsync(string mobile)
        {
            var isVerified = await _dbContext.MobileOTPs
                    .Where(x => x.Mobile == mobile)
                    .FirstOrDefaultAsync();

            if (isVerified.VerifiedAt != null)
            {
                return false;
            }

            // Generate a 6-digit OTP.
            string otp = Random.Shared.Next(100000, 1000000).ToString();

            var mobileOtp = new MobileOTP
            {
                Id = Guid.NewGuid(),
                Mobile = mobile,
                OTP = otp,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.MobileOTPs.Add(mobileOtp);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VerifyMobileOTPAsync(string mobile, string otp)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var mobileOtp = await _dbContext.MobileOTPs
                    .Where(x => x.Mobile == mobile)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();

                if (mobileOtp == null)
                {
                    return false;
                }
                if (mobileOtp.OTP.ToString() != otp) return false;

                if (mobileOtp.VerifiedAt != null) return false;
                mobileOtp.VerifiedAt = DateTime.UtcNow;

                var user = new UserMaster
                {
                    Id = Guid.NewGuid(),
                    MobileOTPId = mobileOtp.Id,
                    EmailOTPId = null,
                    CreatedAt = DateTime.UtcNow,
                    IsMobileVerified = true,
                    IsEmailVerified = false,
                    MobileVerifiedAt = mobileOtp.VerifiedAt,
                    EmailVerifiedAt = null,
                    Status = true
                };

                _dbContext.UserMasters.Add(user);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}