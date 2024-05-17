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
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<GetBoardDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateBoardAsync(CreateBoardDTO requestBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _boardService.CreateBoardAsync(requestBody);

            return Created(string.Empty, new ApiResponse<GetBoardDetail>()
            {
                Code = StatusCodes.Status201Created,
                Data = result
            });
        }

        [Authorize]
        [HttpGet("all")]
        [ProducesResponseType(typeof(ApiResponse<List<GetBoardDetail>>), StatusCodes.Status200OK)]
        public IActionResult GetAllBoards([FromQuery] SearchBoardDTO searchKey)
        {
            List<GetBoardDetail> result = _boardService.GetAllBoard(searchKey);

            return Ok(new ApiResponse<List<GetBoardDetail>>
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }
    }
}
