using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trello.Application.DTOs.User;
using Trello.Application.Services.UserServices;
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

        //[Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<GetUserDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateEmployeeAsync(CreateUserDTO requestBody)
        {
            var result = await _userService.CreateEmployeeAsync(requestBody);

            return Created(string.Empty, new ApiResponse<GetUserDetail>()
            {
                Code = StatusCodes.Status201Created,
                Data = result
            });
        }

    }
}
