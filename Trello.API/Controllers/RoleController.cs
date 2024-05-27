using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;
using Trello.Application.DTOs.Board;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.RoleServices;
using Trello.Application.DTOs.Role;

namespace Trello.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<RoleDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateRoleAsync(RoleDTO requestBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _roleService.CreateRoleAsync(requestBody);

            return Created(string.Empty, new ApiResponse<RoleDetail>()
            {
                Code = StatusCodes.Status201Created,
                Data = result
            });
        }
    }
}
