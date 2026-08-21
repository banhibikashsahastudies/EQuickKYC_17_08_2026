using EQuickKYC.Application.DTOs.Bank;
using EQuickKYC.Application.Interfaces;
using EQuickKYC.Domain.Entities;
using EQuickKYC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using EQuickKYC.Application.DTOs.AddressDTO;
using EQuickKYC.Application.Common;

namespace EQuickKYC.Infrastructure.Services
{
    public class BankService : IBankService
    {
        private readonly EQuickKYCDbContext _dbContext;

        public BankService(EQuickKYCDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddBankAsync(Bank bank)
        {
            await _dbContext.Banks.AddAsync(bank);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteBankAsync(DeleteBankDto deleteBankDto)
        {
            var bank = await _dbContext.Banks.FirstOrDefaultAsync(x => x.Id == deleteBankDto.Id);
            
            //bank not found
            if(bank == null)  return false;

            //bank already deleted
            if (bank.Status == false) return true;
            
            //otherwise change bank status
            bank.Status = deleteBankDto.Status;
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public Task<List<BankResponseDto>> GetAllBankAsync()
        {
            var bankDetails = _dbContext.Banks.AsNoTracking().Select(u => new BankResponseDto
            {
                BankName = u.BankName,
                BranchName = u.BranchName,
                BranchCode = u.BranchCode,
                IFSCCode = u.IFSCCode,
                MICRCode = u.MICRCode,
                Status = u.Status,
                Url = u.Url,
                Id = u.Id,
                Address = u.Address == null
                 ? null
                 : new AddressResponseDto
                 {
                     AddressId = u.Address.AddressId,
                     State = u.Address.State,
                     City = u.Address.City,
                     Country = u.Address.Country,
                     ZipCode = u.Address.ZipCode,
                 }
            }).Where(b=>b.Status == true).ToListAsync();

            return bankDetails;
        }

        public async Task<Bank?> GetBankById(Guid? BankId)
        {
            Bank? bank = await _dbContext.Banks.Where(b=>b.Id == BankId).Where(b=>b.Status == true).Include(b=>b.Address).FirstOrDefaultAsync();
            return bank;
        }
    }
}
