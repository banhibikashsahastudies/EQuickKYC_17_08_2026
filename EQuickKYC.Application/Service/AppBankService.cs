using EQuickKYC.Application.Common;
using EQuickKYC.Application.DTOs.Bank;
using EQuickKYC.Application.Interfaces;
using EQuickKYC.Domain.Entities;

namespace EQuickKYC.Application.Service
{


    public class AppBankService
    {
        private readonly IBankService _bankService;

        public AppBankService(IBankService bankService)
        {
            _bankService = bankService;
        }

        public async Task<Result<List<BankResponseDto>>> GetAllBanks()
        {
            var response = await _bankService.GetAllBankAsync();

            if (response == null)
            {
                return new Result<List<BankResponseDto>>();
            }
            return new Result<List<BankResponseDto>>();
        }

        public async Task<Result<Guid>> AddBank(AddBankRequestDto request)
        {
            try
            {
                var address = new Address
                {
                    AddressId = Guid.NewGuid(),
                    State = request.Address?.State,
                    City = request.Address?.City,
                    ZipCode = request.Address?.ZipCode,
                };

                var bank = new Bank
                {
                    Id = Guid.NewGuid(),
                    BankName = request.BankName,
                    BranchCode = request.BranchCode,
                    IFSCCode = request.IFSCCode,
                    MICRCode = request.MICRCode,
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "Admin",
                };

                if (bank == null)
                {
                    return Result<Guid>.Fail(bank!.BankName, "Bank name can not be blank.");

                }
                int result = await _bankService.AddBankAsync(bank);

                return Result<Guid>.Ok("A new bank details successfully added.");
            }
            catch (Exception ex)
            {
                return Result<Guid>.Fail(ex.Message, "Error occurred while saving.");
            }


        }
    }
}
