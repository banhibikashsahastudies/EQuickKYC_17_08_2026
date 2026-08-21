using EQuickKYC.Domain.Entities;
using EQuickKYC.Domain.Models;
using EQuickKYC.Domain.RepoContracts;
using EQuickKYC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EQuickKYC.Infrastructure.Services
{
    public class UserRepoService : IUserRepoService
    {
        public readonly EQuickKYCDbContext _dbContext;

        public UserRepoService(EQuickKYCDbContext dbConetxt)
        {
            _dbContext = dbConetxt;
        }

        public async Task<User?> AddUser(User user)
        {
            _dbContext.Users.Add(user);

            //checking rows affected
            int success = await _dbContext.SaveChangesAsync();

            if (success >= 1)
                return user;

            //save failed returning null
            return null;
        }

        public async Task<bool> DeleteUser(User user)
        {
            //User? userToDelete = await _dbContext.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();

            //if (userToDelete == null)
            //{
            //    return false;
            //}

            //userToDelete.IsDeleted = true;

            //int rowsAffected = await _dbContext.SaveChangesAsync();

            //return rowsAffected > 0 ? true : false;

            return false;
        }

        public async Task<List<User>?> GetAllUsers()
        {
            return await _dbContext.Users.Select(u=>u).ToListAsync();
        }

        public async Task<User?> GetUserById(Guid id)
        {
            User? user = await _dbContext.Users.AsNoTracking().Where(user => user.Id == id).FirstOrDefaultAsync();

            return user;
        }

        public async Task<UserPaginationResponseInfraDTO?> GetUsers(int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 100;

            var query = _dbContext.Users.AsNoTracking().Where(x => !x.IsDeleted).OrderBy(x => x.CreatedOn);

            var totalRecords = await query.CountAsync();

            var users = await query.Skip((page - 1) * pageSize).Take(pageSize).Select(x => x).ToListAsync();

            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return new UserPaginationResponseInfraDTO
            {
                Users = users,
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                HasPreviousPage = page > 1,
                HasNextPage = page < totalPages
            };
        }

        public async Task<User?> UpdateUser(User user)
        {
            _dbContext.Users.Update(user);
            int row = await _dbContext.SaveChangesAsync();

            return user;
        }
    }
}
