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

        public async Task<UserResponse> GetUser(Guid id)
        {
           if(id == Guid.Empty)
           {
                throw new ArgumentException("User Id is empty");
           }

            User? user = await _dbContext.Users.Where(user=>user.Id==id).FirstOrDefaultAsync();

            return ToUserResponse(user);
        }

        public async Task<UserResponse> UpdateUser(UpdateUserRequest request)
        {
            User? user = await _dbContext.Users
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            // User not found
            if (user == null)
                throw new Exception("User not found");

            // Update only provided fields
            if (!string.IsNullOrWhiteSpace(request.FirstName))
                user.FirstName = request.FirstName;

            if (!string.IsNullOrWhiteSpace(request.MiddleName))
                user.MiddleName = request.MiddleName;

            if (!string.IsNullOrWhiteSpace(request.LastName))
                user.LastName = request.LastName;

            if (request.Dob.HasValue)
                user.Dob = request.Dob.Value;

            if (!string.IsNullOrWhiteSpace(request.Gender))
                user.Gender = request.Gender;

            //will be updated on request
            //if (!string.IsNullOrWhiteSpace(request.Mobile))
            //    user.Mobile = request.Mobile;

            if (!string.IsNullOrWhiteSpace(request.Email))
                user.Email = request.Email;

            int rowsAffected = await _dbContext.SaveChangesAsync();

            return rowsAffected>=1?ToUserResponse(user):null;
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
