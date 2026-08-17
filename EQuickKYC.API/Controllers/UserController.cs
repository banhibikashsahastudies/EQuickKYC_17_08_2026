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

        // GET: api/<ValuesController>
        [HttpGet("[action]")]
        public async Task<ActionResult<List<UserResponse>>> GetAllUsers()
        {
            return await _userService.GetAllUser();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost("[action]")]
        public async Task<ActionResult<UserResponse>> Post([FromBody] AddUserRequest? request )
        {
            if(request == null) throw new ArgumentNullException(nameof(request));

            foreach(var property in typeof(AddUserRequest).GetProperties())
            {
                if(property.GetValue(request) == null)
                {
                    throw new ArgumentException($"{property.Name} is null, please send correct data");
                }
            }

            UserResponse savedUser = await _userService.AddUser(request);
            return savedUser;
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
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
