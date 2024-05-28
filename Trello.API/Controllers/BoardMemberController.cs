using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.BoardMember;
using Trello.Application.DTOs.List;
using Trello.Application.Services.BoardMemberServices;
using Trello.Application.Services.BoardServices;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;

namespace Trello.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardMemberController : ControllerBase
    {
        private readonly IBoardMemberService _boardMemberService;
        public BoardMemberController(IBoardMemberService boardMemberService)
        {
            _boardMemberService = boardMemberService;
        }

        /// <summary>
        /// Creates a new board member.
        /// </summary>
        /// <param name="requestBody">The details of the board member to be created.</param>
        /// <returns>Returns the created board member details.</returns>
        /// <response code="201">If the board member is created successfully.</response>
        /// <response code="400">If the request body is invalid.</response>
        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<BoardMemberDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateBoardMemberAsync(CreateBoardMemberDTO requestBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _boardMemberService.CreateBoardMemberAsync(requestBody);

            return Created(string.Empty, new ApiResponse<BoardMemberDetail>()
            {
                Code = StatusCodes.Status201Created,
                Data = result
            });
        }

        /// <summary>
        /// Retrieves all board members, optionally filtered by name.
        /// </summary>
        /// <param name="name">The optional name filter for board members.</param>
        /// <returns>Returns a list of board member details.</returns>
        /// <response code="200">If the retrieval is successful.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<BoardMemberDetail>>), StatusCodes.Status200OK)]
        public IActionResult GetAllBoardMembers([FromQuery] string? name)
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
            List<BoardMemberDetail> result = _boardMemberService.GetAllBoardMember(name);

            return Ok(new ApiResponse<List<BoardMemberDetail>>
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }

        /// <summary>
        /// Updates an existing board member.
        /// </summary>
        /// <param name="id">The ID of the board member to update.</param>
        /// <param name="requestBody">The new details for the board member.</param>
        /// <returns>Returns the updated board member details.</returns>
        /// <response code="200">If the board member is updated successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(ApiResponse<BoardMemberDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBoardMemberAsync(Guid id, [FromForm] BoardMemberDTO requestBody)
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
            var result = await _boardMemberService.UpdateBoardMemberAsync(id, requestBody);

            return Ok(new ApiResponse<BoardMemberDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }

        /// <summary>
        /// Changes the status of an existing board member.
        /// </summary>
        /// <param name="id">The ID of the board member whose status is to be changed.</param>
        /// <returns>Returns the updated board member details.</returns>
        /// <response code="200">If the board member status is changed successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize/*(Roles = "Admin")*/]
        [HttpPut("change-status/{id}")]
        [ProducesResponseType(typeof(ApiResponse<BoardMemberDetail>), StatusCodes.Status200OK)]
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
            var result = await _boardMemberService.ChangeStatusAsync(id);

            return Ok(new ApiResponse<BoardMemberDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }
    }
}
