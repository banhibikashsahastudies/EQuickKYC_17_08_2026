using EQuickKYC.Application.Common;
using EQuickKYC.Application.DTOs.AddressDTO;
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
            return new Result<List<BankResponseDto>>() { Data = response};
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
                    Country = request.Address?.Country
                };

                var bank = new Bank
                {
                    Id = Guid.NewGuid(),
                    BankName = request.BankName,
                    BranchCode = request.BranchCode,
                    BranchName = request.BranchName,
                    IFSCCode = request.IFSCCode,
                    MICRCode = request.MICRCode,
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    Address = address,
                    Url = request.Url??"DemoURL"
                };

                if (bank == null)
                {
                    return Result<Guid>.Fail(bank!.BankName, "Bank name can not be blank.");

                }
                try
                {
                    int result = await _bankService.AddBankAsync(bank);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }

                return Result<Guid>.Ok("A new bank details successfully added.");
            }
            catch (Exception ex)
            {
                return Result<Guid>.Fail(ex.Message, "Error occurred while saving.");
            }


        }

        public async Task<Result<bool>> DeleteBank(DeleteBankDto deleteBankDto)
        {
            if (deleteBankDto == null) throw new ArgumentNullException("Delete bank request is null");

            foreach (var property in typeof(DeleteBankDto).GetProperties())
            {
                if (property.GetValue(deleteBankDto) is null)
                {
                    throw new ArgumentException($"{property} is null");
                }
            }


            bool result = await _bankService.DeleteBankAsync(deleteBankDto);

            if (!result) return Result<bool>.Fail("Either bank does not exist with that bank id or delete operation failed");

            return Result<bool>.Ok("Bank successfully deleted.");
        }

        public async Task<Result<BankResponseDto>> GetBankById(Guid? BankId)
        {
            if (BankId == null) return Result<BankResponseDto>.Fail("Bank Id sent is null");

            var bank = await _bankService.GetBankById(BankId.Value);

            if (bank == null) return Result<BankResponseDto>.Fail("No Bank with that Bank Id exists");

            var response = new BankResponseDto
            {
                Id = bank.Id,
                BankName = bank.BankName,
                BranchName = bank.BranchName,
                BranchCode = bank.BranchCode,
                IFSCCode = bank.IFSCCode,
                MICRCode = bank.MICRCode,
                Url = bank.Url,
                Status = bank.Status,
                Address = bank.Address == null ? null : new AddressResponseDto
                {
                    AddressId = bank.Address.AddressId,
                    Country = bank.Address.Country,
                    State = bank.Address.State,
                    City = bank.Address.City,
                    ZipCode = bank.Address.ZipCode
                }
            };

            return Result<BankResponseDto>.Ok(response, "Bank found");
        }

        public async Task<Result<BankResponseDto>> UpdateBank(UpdateBankRequest updateBankRequest)
        {
            Bank? bank = await _bankService.GetBankById(updateBankRequest.Id);

            if (bank == null) return Result<BankResponseDto>.Fail("Bank with this id does not exist");

            bank.BankName = updateBankRequest.BankName;
            bank.BranchName = updateBankRequest.BranchName;
            bank.IFSCCode = updateBankRequest.IFSCCode;
            bank.MICRCode = updateBankRequest.MICRCode;
            bank.Status = updateBankRequest.Status;
            bank.UpdatedAt = DateTime.UtcNow;
            bank.UpdatedBy = updateBankRequest.UpdatedBy;
            bank.BranchCode = updateBankRequest.BranchCode ?? bank.BranchCode;
            bank.Url = updateBankRequest.Url ?? bank.Url;

            await _bankService.UpdateBankAsync(bank);

            var response = new BankResponseDto
            {
                Id = bank.Id,
                BankName = bank.BankName,
                BranchName = bank.BranchName,
                BranchCode = bank.BranchCode,
                IFSCCode = bank.IFSCCode,
                MICRCode = bank.MICRCode,
                Url = bank.Url,
                Status = bank.Status
            };

            return Result<BankResponseDto>.Ok(response, "Bank details updated successfully.");
        }
    }
}
