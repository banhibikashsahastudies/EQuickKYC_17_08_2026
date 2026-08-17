using System;
using System.Collections.Generic;
using System.Text;

namespace EQuickKYC.Application.DTOs.User
{
    public class DeleteUserRequest
    {
        public Guid Id { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
