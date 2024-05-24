using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trello.Application.DTOs.User;
using Trello.Application.Services.UserServices;
using Trello.Application.Utilities.ErrorHandler;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;

namespace Trello.API.Controllers
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

        [HttpPost("register-user")]
        [ProducesResponseType(typeof(ApiResponse<UserDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateEmployeeAsync(CreateUserDTO requestBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.CreateUserAsync(requestBody);

            return Created(string.Empty, new ApiResponse<UserDetail>()
            {
                Code = StatusCodes.Status201Created,
                Data = result
            });
        }
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginAsync(UserLoginDTO loginRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<IEnumerable<string>>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Data = errors
                });
            }


            var token = await _userService.LoginAsync(loginRequest);

            return Ok(new LoginResponse<string>
            {
                Code = StatusCodes.Status200OK,
                Bearer = token
            });
        }
        [Authorize]
        [HttpGet("get-all-user")]
        [ProducesResponseType(typeof(ApiResponse<List<UserDetail>>), StatusCodes.Status200OK)]
        public IActionResult GetAllUsers([FromQuery] string? email, string? name, string? gender)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<IEnumerable<string>>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Data = errors
                });
            }
            List<UserDetail> users = _userService.GetAllUser(email, name, gender);

            return Ok(new ApiResponse<List<UserDetail>>
            {
                Code = StatusCodes.Status200OK,
                Data = users
            });
        }
        [Authorize]
        [HttpGet("get-user/{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserLoginAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<IEnumerable<string>>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Data = errors
                });
            }
            var result = await _userService.GetUserLoginAsync(id);

            return Ok(new ApiResponse<object>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }
        [Authorize]
        [HttpPut("update-user/{id}")]
        [ProducesResponseType(typeof(ApiResponse<UserDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUserAsync(Guid id, [FromForm] UpdateUserDTO requestBody)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<IEnumerable<string>>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Data = errors
                });
            }
            var result = await _userService.UpdateUserAsync(id, requestBody);

            return Ok(new ApiResponse<UserDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }

        [Authorize/*(Roles = "Admin")*/]
        [HttpPut("change-status/{id}")]
        [ProducesResponseType(typeof(ApiResponse<UserDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeStatusAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<IEnumerable<string>>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Data = errors
                });
            }
            var result = await _userService.ChangeStatusAsync(id);

            return Ok(new ApiResponse<UserDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }
    }

}
