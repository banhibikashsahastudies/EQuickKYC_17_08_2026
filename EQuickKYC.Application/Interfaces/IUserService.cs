using EQuickKYC.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace EQuickKYC.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> AddUser(AddUserRequest request);

        Task<List<UserResponse>> GetAllUser();

        Task<UserResponse> GetUser(Guid id);

        Task<UserResponse> UpdateUser(UpdateUserRequest request);

        Task<bool> DeleteUser(DeleteUserRequest request);
    }
}
