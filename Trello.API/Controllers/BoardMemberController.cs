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
