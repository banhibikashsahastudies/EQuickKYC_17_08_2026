using EQuickKYC.Application.DTOs.Email;
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

        public async Task<EmailOtpResponseDto> SendEmailOTPAsync(string email)
        {
            var isVerified = await _dbContext.EmailOTPs
                    .Where(x => x.Email == email)
                    .FirstOrDefaultAsync();

            if (isVerified?.VerifiedAt != null)
            {
                return null;
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
            return new EmailOtpResponseDto
            {
                Email = emailOtp.Email,
                OTP = emailOtp.OTP
            };
        }

        public async Task<EmailOtpResponseDto> VerifyEmailOTPAsync(string email, string otp, Guid userMasterId)
        {
            await using var transaction =
                await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Find the latest OTP for this email
                var emailOtp = await _dbContext.EmailOTPs
                    .Where(x => x.Email == email && x.OTP == otp)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();

                if (emailOtp == null)
                {
                    return null;
                }

                // Check OTP
                if (emailOtp.OTP.ToString() != otp)
                {
                    return null;
                }

                // Don't allow the same OTP to be verified again
                if (emailOtp.VerifiedAt != null)
                {
                    return null;
                }

                // Find the EXISTING UserMaster
                var user = await _dbContext.UserMasters
                    .FirstOrDefaultAsync(x => x.Id == userMasterId);

                if (user == null)
                {
                    return null;
                }

                // Verify the email OTP
                emailOtp.VerifiedAt = DateTime.UtcNow;

                // Update existing UserMaster
                user.EmailOTPId = emailOtp.Id;
                user.IsEmailVerified = true;
                user.EmailVerifiedAt = DateTime.UtcNow;

                // If you have Email property in UserMaster
                // user.Email = emailOtp.Email;

                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return new EmailOtpResponseDto
                {
                    Email = emailOtp.Email,
                    OTP = emailOtp.OTP
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        //public async Task<EmailOtpResponseDto> VerifyEmailOTPAsync(string email, string otp, Guid userMasterId)
        //{
        //    await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        //    try
        //    {
        //        var emailOtp = await _dbContext.EmailOTPs
        //            .Where(x => x.Email == email && x.OTP == otp)
        //            .OrderByDescending(x => x.CreatedAt)
        //            .FirstOrDefaultAsync();

        //        if (emailOtp == null)
        //        {
        //            return null;
        //        }
        //        if (emailOtp.OTP.ToString() != otp) return null;

        //        if (emailOtp.VerifiedAt != null) return null;
        //        emailOtp.VerifiedAt = DateTime.UtcNow;

        //        var user = new UserMaster
        //        {
        //            Id = Guid.NewGuid(),
        //            EmailOTPId = emailOtp.Id,
        //            IsEmailVerified = true,
        //            EmailVerifiedAt = DateTime.UtcNow,
        //        };

        //        _dbContext.UserMasters.Add(user);
        //        await _dbContext.SaveChangesAsync();
        //        await transaction.CommitAsync();
        //        return new EmailOtpResponseDto
        //        {
        //            Email = emailOtp.Email,
        //            OTP = emailOtp.OTP
        //        };
        //    }
        //    catch
        //    {
        //        await transaction.RollbackAsync();
        //        throw;
        //    }
        //}
    }
}

