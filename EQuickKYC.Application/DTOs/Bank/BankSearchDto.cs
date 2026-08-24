using System;
using System.Collections.Generic;
using System.Text;

namespace EQuickKYC.Application.DTOs.Bank
{
    public class BankSearchDto
    {
        public string? Name { get; set;  }
        public string? IFSC { get; set; }
        public string? BranchName {  get; set; }
        public bool? status { get; set; } = true;
    }
}
