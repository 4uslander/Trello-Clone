using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.User;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.UserServices;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;

namespace Trello.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _boardService;
        public BoardController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        [Authorize]
        [HttpPost("create-board")]
        [ProducesResponseType(typeof(ApiResponse<BoardDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateBoardAsync(CreateBoardDTO requestBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _boardService.CreateBoardAsync(requestBody);

            return Created(string.Empty, new ApiResponse<BoardDetail>()
            {
                Code = StatusCodes.Status201Created,
                Data = result
            });
        }

        [Authorize]
        [HttpGet("get-all-board")]
        [ProducesResponseType(typeof(ApiResponse<List<BoardDetail>>), StatusCodes.Status200OK)]
        public IActionResult GetAllBoards([FromQuery] string? name)
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
            List<BoardDetail> result = _boardService.GetAllBoard(name);

            return Ok(new ApiResponse<List<BoardDetail>>
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }

        [Authorize]
        [HttpPut("update-board/{id}")]
        [ProducesResponseType(typeof(ApiResponse<BoardDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBoardAsync(Guid id, [FromForm] BoardDTO requestBody)
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
            var result = await _boardService.UpdateBoardAsync(id, requestBody);

            return Ok(new ApiResponse<BoardDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }
        [Authorize/*(Roles = "Admin")*/]
        [HttpPut("change-board/{id}")]
        [ProducesResponseType(typeof(ApiResponse<BoardDetail>), StatusCodes.Status200OK)]
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
            var result = await _boardService.ChangeStatusAsync(id);

            return Ok(new ApiResponse<BoardDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }
        [Authorize]
        [HttpPut("change-visibility/{id}")]
        [ProducesResponseType(typeof(ApiResponse<BoardDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeVisibility(Guid id)
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
            var result = await _boardService.ChangeVisibility(id);

            return Ok(new ApiResponse<BoardDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }
    }
}
