//using EQuickKYC.Application.DTOs.User;
//using EQuickKYC.Application.Interfaces;
//using EQuickKYC.Domain.Entities;
//using EQuickKYC.Infrastructure.Data;

//namespace EQuickKYC.Infrastructure.Services
//{
//    public class UserService_deprecated : IUserService
//    {
//        private readonly EQuickKYCDbContext _dbContext;

//        public UserService_deprecated(EQuickKYCDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public async Task<UserResponse> AddUser(AddUserRequest request)
//        {
//            var user = new User
//            {
//                Id = Guid.NewGuid(),

//                FirstName = request.FirstName,
//                MiddleName = request.MiddleName,
//                LastName = request.LastName,
//                Dob = request.Dob,
//                Gender = request.Gender,

//                Mobile = request.Mobile,
//                Email = request.Email,

//                CreatedOn = DateTime.UtcNow,

//                //Address = request.Address
//            };

//            //if (request.Address != null)
//            //{
//            //    request.Address.AddressId = Guid.NewGuid();
//            //    user.AddressId = request.Address.AddressId;
//            //}

//            _dbContext.Users.Add(user);

//            //checking rows affected
//            int success = await _dbContext.SaveChangesAsync();

//            //atleast one row saved
//            if (success >= 1)
//                return new UserResponse
//                {
//                    Id = user.Id,
//                    FirstName = user.FirstName,
//                    MiddleName = user.MiddleName,
//                    LastName = user.LastName,
//                    Dob = user.Dob,
//                    Gender = user.Gender,
//                    Mobile = user.Mobile,
//                    Email = user.Email,
//                    CreatedOn = user.CreatedOn,
//                    // Address = user.Address
//                };

//            //save failed returning null
//            return null;
//        }


//    }
//}
