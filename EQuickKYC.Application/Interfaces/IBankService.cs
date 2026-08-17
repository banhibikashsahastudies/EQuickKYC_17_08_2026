using EQuickKYC.Application.DTOs.Bank;
using EQuickKYC.Domain.Entities;

namespace EQuickKYC.Application.Interfaces
{
    public interface IBankService
    {
        Task<List<BankResponseDto>> GetAllBankAsync();
        Task<int> AddBankAsync(Bank bank);
    }
}
