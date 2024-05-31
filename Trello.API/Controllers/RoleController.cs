using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;
using Trello.Application.DTOs.Board;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.RoleServices;
using Trello.Application.DTOs.Role;
using Trello.Application.DTOs.BoardMember;
using System.Net;
using Trello.Application.DTOs.List;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.Pagination;
using Trello.Domain.Models;

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

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="requestBody">The details of the role to be created.</param>
        /// <returns>Returns the created role details.</returns>
        /// <response code="201">If the role is created successfully.</response>
        /// <response code="400">If the request body is invalid.</response>
        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<RoleDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateRoleAsync(RoleDTO requestBody)
        {
            try
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
            catch (ExceptionResponse ex)
            {
                return StatusCode((int)ex.StatusCode, new ApiResponse<string>
                {
                    Code = (int)ex.StatusCode,
                    Data = ex.ErrorMessage
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Data = ex.Message
                });
            }
        }

        /// <summary>
        /// Retrieves all roles, optionally filtered by name.
        /// </summary>
        /// <param name="name">The optional name filter for roles.</param>
        /// <returns>Returns a list of role details.</returns>
        /// <response code="200">If the retrieval is successful.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(PagedApiResponse<List<RoleDetail>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRolesAsync([FromQuery] PagingQuery query, [FromQuery] string? name)
        {
            try
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
                List<RoleDetail> result = await _roleService.GetAllRoleAsync(name);

                var pagingResult = result.PagedItems(query.PageIndex, query.PageSize).ToList();
                var total = result.Count;

                var paging = new PaginationInfo
                {
                    Page = query.PageIndex,
                    Size = query.PageSize,
                };

                return Ok(new PagedApiResponse<RoleDetail>
                {
                    Code = StatusCodes.Status200OK,
                    Paging = paging,
                    Data = pagingResult
                });
            }
            catch (ExceptionResponse ex)
            {
                return StatusCode((int)ex.StatusCode, new ApiResponse<string>
                {
                    Code = (int)ex.StatusCode,
                    Data = ex.ErrorMessage
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Data = ex.Message
                });
            }
        }

        /// <summary>
        /// Updates an existing role.
        /// </summary>
        /// <param name="id">The ID of the role to update.</param>
        /// <param name="requestBody">The new details for the role.</param>
        /// <returns>Returns the updated role details.</returns>
        /// <response code="200">If the role is updated successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(ApiResponse<RoleDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateRoleAsync(Guid id, [FromForm] RoleDTO requestBody)
        {
            try
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
            catch (ExceptionResponse ex)
            {
                return StatusCode((int)ex.StatusCode, new ApiResponse<string>
                {
                    Code = (int)ex.StatusCode,
                    Data = ex.ErrorMessage
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Data = ex.Message
                });
            }
        }

        /// <summary>
        /// Changes the status of an existing role.
        /// </summary>
        /// <param name="id">The ID of the role whose status is to be changed.</param>
        /// <returns>Returns the updated role details.</returns>
        /// <response code="200">If the role status is changed successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize/*(Roles = "Admin")*/]
        [HttpPut("change-status/{id}")]
        [ProducesResponseType(typeof(ApiResponse<RoleDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeStatusAsync(Guid id, bool isActive)
        {
            try
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
                var result = await _roleService.ChangeStatusAsync(id, isActive);

                return Ok(new ApiResponse<RoleDetail>()
                {
                    Code = StatusCodes.Status200OK,
                    Data = result
                });
            }
            catch (ExceptionResponse ex)
            {
                return StatusCode((int)ex.StatusCode, new ApiResponse<string>
                {
                    Code = (int)ex.StatusCode,
                    Data = ex.ErrorMessage
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Data = ex.Message
                });
            }
        }
    }
}
