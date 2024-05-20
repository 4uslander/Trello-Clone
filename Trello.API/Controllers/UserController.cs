using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Trello.Application.DTOs.User;
using Trello.Application.Services.UserServices;
using Trello.Application.Utilities.ErrorHandler;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;
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

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<GetUserDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateEmployeeAsync(CreateUserDTO requestBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.CreateUserAsync(requestBody);

            return Created(string.Empty, new ApiResponse<GetUserDetail>()
            {
                Code = StatusCodes.Status201Created,
                Data = result
            });
        }
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginAsync(LoginDTO loginRequest)
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
        [HttpGet("all")]
        [ProducesResponseType(typeof(ApiResponse<List<GetUserDetail>>), StatusCodes.Status200OK)]
        public IActionResult GetAllUsers([FromQuery] SearchUserDTO searchKey)
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
            List<GetUserDetail> users = _userService.GetAllUser(searchKey);

            return Ok(new ApiResponse<List<GetUserDetail>>
            {
                Code = StatusCodes.Status200OK,
                Data = users
            });
        }
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserLoginAsync(int id)
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
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<GetUserDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromForm] UpdateUserDTO requestBody)
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

            return Ok(new ApiResponse<GetUserDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }

        [Authorize/*(Roles = "Admin")*/]
        [HttpDelete("ChangeStatus/{id}")]
        [ProducesResponseType(typeof(ApiResponse<GetUserDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeStatusAsync(int id)
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

            return Ok(new ApiResponse<GetUserDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }

        [HttpGet("signin-google")]
        public IActionResult SignInWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleResponse", "User");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.LOGIN_FIELD, "Google authentication failed");

            var token = await _userService.HandleGoogleLoginAsync(authenticateResult.Principal);
            return Ok(new { Token = token });
        }

        [HttpGet("signout")]
        public async Task<IActionResult> SignOut()
        {
            await _userService.SignOutAsync();
            return Ok();
        }
    }

}
