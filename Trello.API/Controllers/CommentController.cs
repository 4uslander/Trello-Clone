using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Trello.Application.DTOs.List;
using Trello.Application.Services.CommentServices;
using Trello.Application.Services.ListServices;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.DTOs.Comment;
using Trello.Application.Utilities.Helper.Pagination;

namespace Trello.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Creates a new comment.
        /// </summary>
        /// <param name="requestBody">The details of the comment to be created.</param>
        /// <returns>Returns the created comment details.</returns>
        /// <response code="201">If the comment is created successfully.</response>
        /// <response code="400">If the request body is invalid.</response>
        /// <response code="500">If an unexpected error occurs, returns an error message.</response>
        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<CommentDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCommentAsync(CommentDTO requestBody)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _commentService.CreateCommentAsync(requestBody);

                return Created(string.Empty, new ApiResponse<CommentDetail>()
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
        /// Retrieves all comments with pagination support.
        /// </summary>
        /// <param name="query">The pagination query parameters including page index and page size.</param>
        /// <param name="cardId">The Id of the card to display.</param>
        /// <returns>Returns an action result containing a paged response of comment details.</returns>
        /// <response code="200">If the retrieval is successful, returns a paged list of comment details.</response>
        /// <response code="400">If the request is invalid, returns a list of validation errors.</response>
        /// <response code="500">If an unexpected error occurs, returns an error message.</response>
        [Authorize]
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(PagedApiResponse<List<CommentDetail>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllListsAsync([FromQuery] Guid cardId, [FromQuery] PagingQuery query)
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
                List<CommentDetail> result = await _commentService.GetAllCommentAsync(cardId);

                var pagingResult = result.PagedItems(query.PageIndex, query.PageSize).ToList();
                var total = result.Count;

                var paging = new PaginationInfo
                {
                    Page = query.PageIndex,
                    Size = query.PageSize,
                };

                return Ok(new PagedApiResponse<CommentDetail>
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
        /// Updates an existing comment.
        /// </summary>
        /// <param name="id">The ID of the comment to update.</param>
        /// <param name="content">The new content for the comment.</param>
        /// <returns>Returns the updated comment content.</returns>
        /// <response code="200">If the comment is updated successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If an unexpected error occurs, returns an error message.</response>
        [Authorize]
        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(ApiResponse<CommentDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCommentAsync(Guid id, [FromForm] string content)
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
                var result = await _commentService.UpdateCommentAsync(id, content);

                return Ok(new ApiResponse<CommentDetail>()
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
        /// Changes the status of an existing comment.
        /// </summary>
        /// <param name="id">The ID of the comment whose status is to be changed.</param>
        /// <param name="isActive">The status of the comment to update.</param>
        /// <returns>Returns the updated comment status.</returns>
        /// <response code="200">If the comment status is changed successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If an unexpected error occurs, returns an error message.</response>
        [Authorize]
        [HttpPut("change-status/{id}")]
        [ProducesResponseType(typeof(ApiResponse<CommentDetail>), StatusCodes.Status200OK)]
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
                var result = await _commentService.ChangeStatusAsync(id, isActive);

                return Ok(new ApiResponse<CommentDetail>()
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
