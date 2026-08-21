using EQuickKYC.Application.DTOs.Email;
using EQuickKYC.Application.Interfaces;
using EQuickKYC.Domain.Entities;
using EQuickKYC.Infrastructure.Data;
using EQuickKYC.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace EQuickKYC.Infrastructure.Services
{
    public class EmailOTPService : IEmailOTPService
    {
        private readonly EQuickKYCDbContext _dbContext;
        private readonly IEncryptionService _encryptionService;
        private readonly IHashService _hashService;

        public EmailOTPService(EQuickKYCDbContext dbContext, IEncryptionService encryptionService, IHashService hashService)
        {
            _dbContext = dbContext;
            _encryptionService = encryptionService;
            _hashService = hashService;
        }

        public async Task<EmailOtpResponseDto> SendEmailOTPAsync(string email)
        {
            var hashedEmail = _hashService.Hash(email);

            var isVerified = await _dbContext.EmailOTPs
                    .Where(x => x.HashEmail == hashedEmail)
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
                Email = _encryptionService.Encrypt(email),
                HashEmail = hashedEmail,
                OTP = _encryptionService.Encrypt(otp),
                HashOTP = _hashService.Hash(otp),
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.EmailOTPs.Add(emailOtp);
            await _dbContext.SaveChangesAsync();

            return new EmailOtpResponseDto
            {
                Email = _encryptionService.Decrypt(emailOtp.Email),
                OTP = _encryptionService.Decrypt(emailOtp.OTP)
            };
        }

        public async Task<EmailOtpResponseDto> VerifyEmailOTPAsync(string email, string otp, Guid userMasterId)
        {
            var hashedEmail = _hashService.Hash(email);
            var hashedOTP = _hashService.Hash(otp);

            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Find the latest OTP for this email
                var emailOtp = await _dbContext.EmailOTPs
                    .Where(x => x.HashEmail == hashedEmail && x.HashOTP == hashedOTP)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();

                if (emailOtp == null)
                {
                    return null;
                }

                // Check OTP
                if (emailOtp.HashOTP.ToString() != hashedOTP)
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


                emailOtp.VerifiedAt = DateTime.UtcNow;


                user.EmailOTPId = emailOtp.Id;
                user.IsEmailVerified = true;
                user.EmailVerifiedAt = DateTime.UtcNow;

                // If you have Email property in UserMaster
                // user.Email = emailOtp.Email;

                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return new EmailOtpResponseDto
                {
                    Email = _encryptionService.Decrypt(emailOtp.Email),
                    OTP = _encryptionService.Decrypt(emailOtp.OTP)
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

