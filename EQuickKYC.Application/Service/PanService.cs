using EQuickKYC.Application.Common;
using EQuickKYC.Application.DTOs.Pan;
using EQuickKYC.Application.Exceptions;
using EQuickKYC.Application.Interfaces;
using EQuickKYC.Domain.Entities;

namespace EQuickKYC.Application.Service
{
    public class PanService
    {
        private readonly IPanRegistrationService _panRegistrationService;

        public PanService(IPanRegistrationService panRegistrationService)
        {
            _panRegistrationService = panRegistrationService;
        }
        public async Task<Result<RegistrationMaster>> RegisterPanAsync(PanRequestDto panRequestDto, Guid userMasterId)
        {
            var panExists = await GetPanDetailsByPannumber(panRequestDto.PanNo);

            if (panExists.Success)
            {
                return Result<RegistrationMaster>.Fail("The PAN number is already registered.");
            }

            var registrationMaster = new RegistrationMaster
            {
                UserMasterId = userMasterId,
                Name = panRequestDto.Name,
                PanNo = panRequestDto.PanNo,
                DOB = panRequestDto.DOB
            };
            await _panRegistrationService.RegisterPanAsync(registrationMaster, userMasterId);

            return Result<RegistrationMaster>.Ok(registrationMaster, "PAN registration successful");
        }

        public async Task<Result<PanResponseDto>> GetPanDetailsByPannumber(string panNumber)
        {
            var panDetails = await _panRegistrationService.GetPanDetailsByPannumberAsync(panNumber);
            if (panDetails == null)
            {
                 return Result<PanResponseDto>.Fail("The PAN was not found in our database.");
                //throw new ExternalApiException(externalApi: "Test PAN Provider", message: "Simulated external API failure.", statusCode: 503);
            }
            return Result<PanResponseDto>.Ok(panDetails, "The PAN was found in our database.");
        }
    }
}