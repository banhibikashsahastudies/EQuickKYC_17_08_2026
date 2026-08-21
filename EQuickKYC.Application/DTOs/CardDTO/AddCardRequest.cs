using System;
using System.Collections.Generic;
using System.Text;

namespace EQuickKYC.Application.DTOs.CardDTO
{
    public class AddCardRequest
    {
        public string? AadhaarNo { get; set; }

        public string? PanNo { get; set; }

        public string? VoterNo { get; set; }

        public string? DrivingLicenseNo { get; set; }

        public string? Otp { get; set; }
    }
}
