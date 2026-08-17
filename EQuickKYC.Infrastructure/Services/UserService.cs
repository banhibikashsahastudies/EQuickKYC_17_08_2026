using EQuickKYC.Application.DTOs.User;
using EQuickKYC.Application.Interfaces;
using EQuickKYC.Domain.Entities;
using EQuickKYC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EQuickKYC.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly EQuickKYCDbContext _dbContext;
        public UserService(EQuickKYCDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserResponse> AddUser(AddUserRequest request)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),

                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Dob = request.Dob,
                Gender = request.Gender,

                Mobile = request.Mobile,
                Email = request.Email,

                CreatedOn = DateTime.UtcNow,

                Address = request.Address
            };

            if (request.Address != null)
            {
                request.Address.AddressId = Guid.NewGuid();
                user.AddressId = request.Address.AddressId;
            }

            _dbContext.Users.Add(user);

            //checking rows affected
            int success = await _dbContext.SaveChangesAsync();

            //atleast one row saved
            if(success>=1)
            return new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Dob = user.Dob,
                Gender = user.Gender,
                Mobile = user.Mobile,
                Email = user.Email,
                CreatedOn = user.CreatedOn,
                Address = user.Address
            };

            //save failed returning null
            return null;
        }

        public async Task<bool> DeleteUser(DeleteUserRequest request)
        {
            User? userToDelete = await _dbContext.Users.Where(u => u.Id == request.Id).FirstOrDefaultAsync();

            if (userToDelete == null)
            {
                return false;
            }

            userToDelete.IsDeleted = true;

            int rowsAffected = await _dbContext.SaveChangesAsync();

            return rowsAffected>0?true:false;
        }

        public async Task<List<UserResponse>> GetAllUser()
        {
            var users = await _dbContext.Users.Include(x => x.Address).Include(x => x.Card).ToListAsync();

            return users.Select(x => ToUserResponse(x)).ToList();
        }

        public Task<UserResponse> UpdateUser()
        {
            throw new NotImplementedException();
        }

        private UserResponse ToUserResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,

                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,

                Dob = user.Dob,
                Gender = user.Gender,

                Mobile = user.Mobile,
                Email = user.Email,

                AddressId = user.AddressId,
                Address = user.Address,

                // Don't expose these
                CreatedOn = null,
                UpdatedOn = null,
                CreatedBy = null,
                UpdatedBy = null,

                // Don't expose Card
                CardId = null,
                Card = null,

                // Don't expose soft-delete status
                IsDeleted = null
            };
        }
    }
}
