using EQuickKYC.Application.Interfaces;
using EQuickKYC.Domain.Entities;
using EQuickKYC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EQuickKYC.Infrastructure.Services
{
    public class EmailOTPService : IEmailOTPService
    {
        private readonly EQuickKYCDbContext _dbContext;

        public EmailOTPService(EQuickKYCDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> SendEmailOTPAsync(string email)
        {
            var isVerified = await _dbContext.MobileOTPs
                    .Where(x => x.Mobile == email)
                    .FirstOrDefaultAsync();

            if (isVerified.VerifiedAt != null)
            {
                return false;
            }

            // Generate a 6-digit OTP.
            string otp = Random.Shared.Next(100000, 1000000).ToString();

            var emailOtp = new EmailOTP
            {
                Id = Guid.NewGuid(),
                Email = email,
                OTP = otp,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.EmailOTPs.Add(emailOtp);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VerifyEmailOTPAsync(string email, string otp)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var emailOtp = await _dbContext.EmailOTPs
                    .Where(x => x.Email == email)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();

                if (emailOtp == null)
                {
                    return false;
                }
                if (emailOtp.OTP.ToString() != otp) return false;

                if (emailOtp.VerifiedAt != null) return false;
                emailOtp.VerifiedAt = DateTime.UtcNow;

                var user = new UserMaster
                {
                    Id = Guid.NewGuid(),
                    EmailOTPId = emailOtp.Id,
                    CreatedAt = DateTime.UtcNow,
                    IsMobileVerified = true,
                    IsEmailVerified = false,
                    MobileVerifiedAt = emailOtp.VerifiedAt,
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
}
