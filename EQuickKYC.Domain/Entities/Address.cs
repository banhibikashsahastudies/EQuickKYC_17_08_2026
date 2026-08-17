using System;
using System.Collections.Generic;
using System.Text;

namespace EQuickKYC.Domain.Entities
{
    public class Address
    {
        public Guid? AddressId { get; set; }

        public string? Country { get; set; }

        public string? State { get; set; }

        public string? City { get; set; }

        public int? ZipCode { get; set;}
    }
}
