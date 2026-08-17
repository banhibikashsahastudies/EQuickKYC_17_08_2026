using EQuickKYC.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace EQuickKYC.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> AddUser(AddUserRequest request);
    }
}
