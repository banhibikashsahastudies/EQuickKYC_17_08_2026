using EQuickKYC.Application.Common;
using EQuickKYC.Application.DTOs.Pan;
using EQuickKYC.Application.DTOs.Register;
using EQuickKYC.Application.Exceptions;
using EQuickKYC.Application.Interfaces;
using EQuickKYC.Domain.Entities;
using System.Security.AccessControl;

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

        public async Task<Result<string>> ChangePrefix(ChangePrefixDTO changePrefixDTO)
        {
            if (changePrefixDTO == null) return Result<string>.Fail("Prefix change dto is null, send correct request data") ;

            foreach (var property in typeof(ChangePrefixDTO).GetProperties()) 
            { 
                if(property.GetValue(changePrefixDTO) == null)
                {
                    return Result<string>.Fail($"Value of {property} is null, send correct data.");
                }
            }

            var response = await _panRegistrationService.GetUserById(changePrefixDTO.id);
            if (response == null) return Result<string>.Fail("No user with this id exists");

            response.ApplicationPrefix = changePrefixDTO.ApplicationPrefix;
            var prefix = await _panRegistrationService.ChangePrefix(response);

            return Result<string>.Ok(data:prefix,"Changed prefix successfully.", totalCount:1);
        } 
    }
}