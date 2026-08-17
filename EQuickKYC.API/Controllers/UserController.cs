using EQuickKYC.Application.DTOs.User;
using EQuickKYC.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EQuickKYC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<UserController>
        [HttpGet("[action]")]
        public async Task<ActionResult<List<UserResponse>>> GetAllUsers()
        {
            return await _userService.GetAllUser();
        }

        // GET api/<UserController>/5
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<UserResponse>> GetUserById(Guid id)
        {
            UserResponse user = await _userService.GetUser(id);

            if (user == null) throw new Exception("user not found");

            return Ok(user);
        }

        // POST api/<UserController>
        [HttpPost("[action]")]
        public async Task<ActionResult<UserResponse>> Post([FromBody] AddUserRequest? request )
        {
            if(request == null) throw new ArgumentNullException(nameof(request));

            //check all fields
            foreach (var property in typeof(AddUserRequest).GetProperties())
            {
                if(property.GetValue(request) == null)
                {
                    throw new ArgumentException($"{property.Name} is null, please send correct data");
                }
            }

            UserResponse savedUser = await _userService.AddUser(request);
            return Ok(savedUser);
        }

        // PUT api/<UserController>/5
        [HttpPut("[action]")]
        public async Task<ActionResult<UserResponse>> UpdateUser([FromBody] UpdateUserRequest request)
        {
            if (request == null || request.Id == Guid.Empty) throw new ArgumentNullException("Update user request is null or User Id is empty");

            UserResponse? updatedUser = await _userService.UpdateUser(request);

            if (updatedUser == null) throw new Exception("Failed to update user");

            return updatedUser;
        }

        // DELETE api/<UserController>/5
        [HttpDelete("[action]")]
        public async Task<ActionResult<bool>> Delete(DeleteUserRequest request)
        {
            if(request == null) throw new ArgumentNullException(nameof(request));

            foreach (var property in typeof(DeleteUserRequest).GetProperties())
            {
                if (property.GetValue(request) == null)
                {
                    throw new ArgumentException($"{property.Name} is null, please send correct data");
                }
            }

            return await _userService.DeleteUser(request);
        }
    }
}
