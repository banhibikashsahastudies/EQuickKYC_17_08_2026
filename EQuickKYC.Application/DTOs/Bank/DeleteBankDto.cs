using System;
using System.Collections.Generic;
using System.Text;

namespace EQuickKYC.Application.DTOs.Bank
{
    public class DeleteBankDto
    {
        public Guid Id { get; set; }

        public bool Status { get; set; }
    }
}
