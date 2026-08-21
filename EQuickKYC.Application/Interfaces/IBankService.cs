using EQuickKYC.Application.DTOs.Bank;
using EQuickKYC.Domain.Entities;

namespace EQuickKYC.Application.Interfaces
{
    public interface IBankService
    {
        Task<List<BankResponseDto>> GetAllBankAsync();
        Task<Bank?> GetBankById(Guid? BankId);
        Task<int> AddBankAsync(Bank bank);
        Task<bool> DeleteBankAsync(DeleteBankDto deleteBankDto);


    }
}
