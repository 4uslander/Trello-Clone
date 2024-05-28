using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;
using Trello.Application.DTOs.Board;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.RoleServices;
using Trello.Application.DTOs.Role;
using Trello.Application.DTOs.BoardMember;

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
        [Authorize]
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<RoleDetail>>), StatusCodes.Status200OK)]
        public IActionResult GetAllRoles([FromQuery] string? name)
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
            List<RoleDetail> result = _roleService.GetAllRole(name);

            return Ok(new ApiResponse<List<RoleDetail>>
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }
        [Authorize]
        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(ApiResponse<RoleDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateRoleAsync(Guid id, [FromForm] RoleDTO requestBody)
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
            var result = await _roleService.UpdateRoleAsync(id, requestBody);

            return Ok(new ApiResponse<RoleDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }
        [Authorize/*(Roles = "Admin")*/]
        [HttpPut("change-status/{id}")]
        [ProducesResponseType(typeof(ApiResponse<RoleDetail>), StatusCodes.Status200OK)]
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
            var result = await _roleService.ChangeStatusAsync(id);

            return Ok(new ApiResponse<RoleDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }
    }
}
