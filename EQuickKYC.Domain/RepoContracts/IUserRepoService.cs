using EQuickKYC.Domain.Entities;
using EQuickKYC.Domain.Models;

namespace EQuickKYC.Domain.RepoContracts
{
    public interface IUserRepoService
    {
        Task<User?> AddUser(User user);
        Task<User?> GetUserById(Guid id);
        Task<List<User>?> GetAllUsers();
        Task<User?> UpdateUser(User user);
        Task<bool> DeleteUser(User user);

        //pagination
        Task<UserPaginationResponseInfraDTO?> GetUsers(int page, int pageSize);
    }
}
