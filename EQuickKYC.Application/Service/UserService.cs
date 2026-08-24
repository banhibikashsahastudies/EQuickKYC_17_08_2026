using EQuickKYC.Application.DTOs.User;
using EQuickKYC.Application.Interfaces;
using EQuickKYC.Domain.Entities;
using EQuickKYC.Domain.RepoContracts;

namespace EQuickKYC.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepoService _userRepoService;

        public UserService(IUserRepoService userRepoService)
        {
            _userRepoService = userRepoService;
        }

        public async Task<UserResponse> AddUser(AddUserRequest request)
        {
            Card card = new Card()
            {
                CardId = Guid.NewGuid(),
                AadhaarNo = request.CardRequest.AadhaarNo,
                PanNo = request.CardRequest.PanNo,
                VoterNo = request.CardRequest.VoterNo,
                DrivingLicenseNo = request.CardRequest.DrivingLicenseNo
            };

            Address address = new Address()
            {
                AddressId = Guid.NewGuid(),
                Country = request.AddressRequest.Country,
                State = request.AddressRequest.State,
                City = request.AddressRequest.City,
                ZipCode = request.AddressRequest.ZipCode
            };

            var user = new User
            {
                Id = Guid.NewGuid(),

                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Dob = request.Dob,
                Gender = request.Gender,
                AddressId = address.AddressId,
                CardId = card.CardId,
                Mobile = request.Mobile,
                Email = request.Email,
                CreatedOn = DateTime.UtcNow,
                Card = card,
                Address = address
            };

            User? updatedUser = await _userRepoService.AddUser(user);

            return ToUserResponse(updatedUser);
        }

        public async Task<bool> DeleteUser(DeleteUserRequest request)
        {
            //check user
            User? user = await _userRepoService.GetUserById(request.Id);

            if (user == null)
                return false;

            //soft
            user.IsDeleted = true;

            User? deletedUser = await _userRepoService.UpdateUser(user);

            //if deleted user is null
            if(deletedUser == null)
                return false;

            return deletedUser.IsDeleted;
        }

        public async Task<List<UserResponse>> GetAllUser()
        {
            List<User>? usersList = await _userRepoService.GetAllUsers();
            List<UserResponse>? userResponseList = usersList.Select(u => ToUserResponse(u)).ToList();

            return userResponseList;
        }

        public async Task<UserResponse> GetUser(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("User Id is empty");
            }

            User? user = await _userRepoService.GetUserById(id);

            return ToUserResponse(user);
        }

        public async Task<UserPaginationResponse> GetUsers(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<UserResponse> UpdateUser(UpdateUserRequest request)
        {
            User? user = await _userRepoService.GetUserById(request.Id);

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

            User? updatedUser = await _userRepoService.UpdateUser(user);
            return ToUserResponse(updatedUser);
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
