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

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="requestBody">The details of the user to register.</param>
        /// <returns>Returns the registered user's details.</returns>
        /// <response code="201">If the user is created successfully.</response>
        /// <response code="400">If the request body is invalid.</response>
        [HttpPost("register")]
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

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="loginRequest">The login details of the user.</param>
        /// <returns>Returns a JWT token if the login is successful.</returns>
        /// <response code="200">If the login is successful.</response>
        /// <response code="400">If the request body is invalid.</response>
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

        /// <summary>
        /// Retrieves all users, optionally filtered by email, name, or gender.
        /// </summary>
        /// <param name="email">The optional email filter.</param>
        /// <param name="name">The optional name filter.</param>
        /// <param name="gender">The optional gender filter.</param>
        /// <returns>Returns a list of user details.</returns>
        /// <response code="200">If the retrieval is successful.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpGet("get-all")]
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

        /// <summary>
        /// Retrieves the details of a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>Returns the user's details.</returns>
        /// <response code="200">If the retrieval is successful.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpGet("get/{id}")]
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

        /// <summary>
        /// Updates the details of an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="requestBody">The new details of the user.</param>
        /// <returns>Returns the updated user details.</returns>
        /// <response code="200">If the update is successful.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpPut("update/{id}")]
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

        /// <summary>
        /// Changes the status of an existing user.
        /// </summary>
        /// <param name="id">The ID of the user whose status is to be changed.</param>
        /// <returns>Returns the updated user details.</returns>
        /// <response code="200">If the status change is successful.</response>
        /// <response code="400">If the request is invalid.</response>
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
