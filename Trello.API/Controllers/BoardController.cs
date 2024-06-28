using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Xml.Linq;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.User;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.UserServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.CheckNullProperties;
using Trello.Application.Utilities.Helper.Pagination;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
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
        /// Creates a new board with the specified details.
        /// </summary>
        /// <param name="requestBody">The details of the board to be created.</param>
        /// <returns>
        /// A response indicating the result of the board creation process. If successful, 
        /// returns a 201 Created status with the details of the created board.
        /// </returns>
        /// <response code="201">Returns the created board details.</response>
        /// <response code="400">If the request body is invalid.</response>
        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<BoardDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateBoardAsync([FromBody] BoardDTO requestBody)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _boardService.CreateBoardAsync(requestBody);
                return Created(string.Empty, new ApiResponse<BoardDetail>
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
        /// Retrieves boards
        /// </summary>
        /// <returns>Returns a list of board details.</returns>
        /// <response code="200">If the retrieval is successful.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpGet("gets")]
        [ProducesResponseType(typeof(PagedApiResponse<List<BoardDetail>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBoardAsync([FromQuery] PagingQuery query)
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

                List<BoardDetail> result = await _boardService.GetBoardAsync();

                var pagingResult = result.PagedItems(query.PageIndex, query.PageSize).ToList();

                var paging = new PaginationInfo
                {
                    Page = query.PageIndex,
                    Size = query.PageSize,
                };

                return Ok(new PagedApiResponse<BoardDetail>
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
        /// Retrieves all boards, optionally filtered by name.
        /// </summary>
        /// <param name="name">The optional name filter for boards.</param>
        /// <param name="isPublic">The optional isPublic filter.</param>
        /// <param name="isActive">The optional isActive filter.</param>
        /// <returns>Returns a list of board details.</returns>
        /// <response code="200">If the retrieval is successful.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpGet("get-by-filter")]
        [ProducesResponseType(typeof(PagedApiResponse<List<BoardDetail>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBoardByFilterAsync([FromQuery] PagingQuery query, [FromQuery] string? name, bool? isPublic, bool? isActive)
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

                List<BoardDetail> result = await _boardService.GetBoardByFilterAsync(name, isPublic, isActive);

                var pagingResult = result.PagedItems(query.PageIndex, query.PageSize).ToList();

                var paging = new PaginationInfo
                {
                    Page = query.PageIndex,
                    Size = query.PageSize,
                };

                return Ok(new PagedApiResponse<BoardDetail>
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
        /// Updates an existing board.
        /// </summary>
        /// <param name="id">The ID of the board to update.</param>
        /// <param name="requestBody">The new details for the board.</param>
        /// <returns>Returns the updated board details.</returns>
        /// <response code="200">If the board is updated successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(ApiResponse<BoardDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBoardAsync(Guid id, [FromBody] BoardDTO requestBody)
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
                var result = await _boardService.UpdateBoardAsync(id, requestBody);

                return Ok(new ApiResponse<BoardDetail>()
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
        /// Changes the status of an existing board.
        /// </summary>
        /// <param name="id">The ID of the board whose status is to be changed.</param>
        /// <returns>Returns the updated board details.</returns>
        /// <response code="200">If the board status is changed successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize/*(Roles = "Admin")*/]
        [HttpPut("change-status/{id}")]
        [ProducesResponseType(typeof(ApiResponse<BoardDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeStatusAsync(Guid id, [FromQuery] bool isActive)
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
                var result = await _boardService.ChangeStatusAsync(id, isActive);

                return Ok(new ApiResponse<BoardDetail>()
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
        /// Changes the visibility of an existing board.
        /// </summary>
        /// <param name="id">The ID of the board whose visibility is to be changed.</param>
        /// <returns>Returns the updated board details.</returns>
        /// <response code="200">If the board visibility is changed successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpPut("change-visibility/{id}")]
        [ProducesResponseType(typeof(ApiResponse<BoardDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeVisibilityAsync(Guid id, [FromQuery] bool isPublic)
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
                var result = await _boardService.ChangeVisibilityAsync(id, isPublic);

                return Ok(new ApiResponse<BoardDetail>()
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
