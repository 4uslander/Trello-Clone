using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.List;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.ListServices;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;

namespace Trello.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        private readonly IListService _listService;
        public ListController(IListService listService)
        {
            _listService = listService;
        }

        /// <summary>
        /// Creates a new list.
        /// </summary>
        /// <param name="requestBody">The details of the list to be created.</param>
        /// <returns>Returns the created list details.</returns>
        /// <response code="201">If the list is created successfully.</response>
        /// <response code="400">If the request body is invalid.</response>
        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<ListDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateListAsync(ListDTO requestBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _listService.CreateListAsync(requestBody);

            return Created(string.Empty, new ApiResponse<ListDetail>()
            {
                Code = StatusCodes.Status201Created,
                Data = result
            });
        }

        /// <summary>
        /// Retrieves all lists, optionally filtered by name.
        /// </summary>
        /// <param name="name">The optional name filter for lists.</param>
        /// <returns>Returns a list of list details.</returns>
        /// <response code="200">If the retrieval is successful.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<ListDetail>>), StatusCodes.Status200OK)]
        public IActionResult GetAllLists([FromQuery] string? name)
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
            List<ListDetail> result = _listService.GetAllList(name);

            return Ok(new ApiResponse<List<ListDetail>>
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }

        /// <summary>
        /// Updates an existing list.
        /// </summary>
        /// <param name="id">The ID of the list to update.</param>
        /// <param name="requestBody">The new details for the list.</param>
        /// <returns>Returns the updated list details.</returns>
        /// <response code="200">If the list is updated successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ListDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateListAsync(Guid id, [FromForm] ListDTO requestBody)
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
            var result = await _listService.UpdateListAsync(id, requestBody);

            return Ok(new ApiResponse<ListDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }

        /// <summary>
        /// Changes the status of an existing list.
        /// </summary>
        /// <param name="id">The ID of the list whose status is to be changed.</param>
        /// <returns>Returns the updated list details.</returns>
        /// <response code="200">If the list status is changed successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize/*(Roles = "Admin")*/]
        [HttpPut("change-status/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ListDetail>), StatusCodes.Status200OK)]
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
            var result = await _listService.ChangeStatusAsync(id);

            return Ok(new ApiResponse<ListDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }
    }
}
