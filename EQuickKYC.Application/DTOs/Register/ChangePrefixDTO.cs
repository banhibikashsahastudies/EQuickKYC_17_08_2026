using System;
using System.Collections.Generic;
using System.Text;

namespace EQuickKYC.Application.DTOs.Register
{
    public class ChangePrefixDTO
    {
        public Guid id {  get; set; }
        public string ApplicationPrefix { get; set; }
    }
}
