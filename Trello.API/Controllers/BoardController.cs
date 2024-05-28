using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.User;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.UserServices;
using Trello.Application.Utilities.Helper.CheckNullProperties;
using Trello.Application.Utilities.Helper.Pagination;
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

        /// <summary>
        /// Creates a new board.
        /// </summary>
        /// <param name="requestBody">The details of the board to be created.</param>
        /// <returns>Returns the created board details.</returns>
        /// <response code="201">If the board is created successfully.</response>
        /// <response code="400">If the request body is invalid.</response>
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

        /// <summary>
        /// Retrieves all boards, optionally filtered by name.
        /// </summary>
        /// <param name="name">The optional name filter for boards.</param>
        /// <returns>Returns a list of board details.</returns>
        /// <response code="200">If the retrieval is successful.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpGet("get-all-board")]
        [ProducesResponseType(typeof(PagedApiResponse<List<BoardDetail>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBoards([FromQuery] PagingQuery query, [FromQuery] string? name)
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

            var pagingResult = result.PagedItems(query.PageIndex, query.PageSize).ToList();
            var total = name.AreAllPropertiesNull() ? await _boardService.GetTotalBoardAsync()
                                                         : result.Count;

            var paging = new PaginationInfo
            {
                Page = query.PageIndex,
                Size = query.PageSize,
                Total = total
            };
            return Ok(new PagedApiResponse<BoardDetail>()
            {
                Code = StatusCodes.Status200OK,
                Paging = paging,
                Data = pagingResult
            });
        }

        /// <summary>
        /// Updates an existing board.
        /// </summary>
        /// <param name="id">The ID of the board to update.</param>
        /// <param name="requestBody">The new details for the board.</param>
        /// <returns>Returns the updated board details.</returns>
        /// <response code="200">If the board is updated successfully.</response>
        /// <response code="400">If the request is invalid.</response>
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

        /// <summary>
        /// Changes the status of an existing board.
        /// </summary>
        /// <param name="id">The ID of the board whose status is to be changed.</param>
        /// <returns>Returns the updated board details.</returns>
        /// <response code="200">If the board status is changed successfully.</response>
        /// <response code="400">If the request is invalid.</response>
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

        /// <summary>
        /// Changes the visibility of an existing board.
        /// </summary>
        /// <param name="id">The ID of the board whose visibility is to be changed.</param>
        /// <returns>Returns the updated board details.</returns>
        /// <response code="200">If the board visibility is changed successfully.</response>
        /// <response code="400">If the request is invalid.</response>
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
