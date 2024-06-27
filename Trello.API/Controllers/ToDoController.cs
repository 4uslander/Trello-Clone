using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Trello.Application.DTOs.List;
using Trello.Application.Services.ListServices;
using Trello.Application.Services.ToDoServices;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.DTOs.ToDo;
using Trello.Application.DTOs.Card;
using Trello.Application.Utilities.Helper.Pagination;

namespace Trello.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;
        public ToDoController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        /// <summary>
        /// Creates a new todo list.
        /// </summary>
        /// <param name="requestBody">The details of the to do to be created.</param>
        /// <returns>Returns the created todo details.</returns>
        /// <response code="201">If the todo is created successfully.</response>
        /// <response code="400">If the request body is invalid.</response>
        /// <response code="500">If an unexpected error occurs, returns an error message.</response>
        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<ToDoDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateToDoListAsync(ToDoDTO requestBody)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _toDoService.CreateToDoListAsync(requestBody);

                return Created(string.Empty, new ApiResponse<ToDoDetail>()
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
        /// Retrieves all todo list, optionally filtered by title.
        /// </summary>
        /// <param name="listId">The ID of the list to get.</param>
        /// <param name="query">The pagination query parameters including page index and page size.</param>
        /// <returns>Returns a list of todo list details.</returns>
        /// <response code="200">If the retrieval is successful.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(PagedApiResponse<List<ToDoDetail>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllToDoListAsync([FromQuery] Guid cardId, [FromQuery] PagingQuery query)
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
                List<ToDoDetail> result = await _toDoService.GetAllToDoListAsync(cardId);

                var pagingResult = result.PagedItems(query.PageIndex, query.PageSize).ToList();

                var paging = new PaginationInfo
                {
                    Page = query.PageIndex,
                    Size = query.PageSize,
                };

                return Ok(new PagedApiResponse<ToDoDetail>
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
        /// Retrieves all todo list, optionally filtered by title.
        /// </summary>
        /// <param name="listId">The ID of the list to get.</param>
        /// <param name="query">The pagination query parameters including page index and page size.</param>
        /// <param name="title">The optional title filter for cards.</param>
        /// <param name="isActive">The optional is Active filter for cards.</param>
        /// <returns>Returns a list of todo list details.</returns>
        /// <response code="200">If the retrieval is successful.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpGet("get-by-filter")]
        [ProducesResponseType(typeof(PagedApiResponse<List<ToDoDetail>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetToDoListByFilterAsync([FromQuery] Guid cardId, [FromQuery] PagingQuery query, [FromQuery] string? title, bool? isActive)
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
                List<ToDoDetail> result = await _toDoService.GetToDoListByFilterAsync(cardId, title, isActive);

                var pagingResult = result.PagedItems(query.PageIndex, query.PageSize).ToList();

                var paging = new PaginationInfo
                {
                    Page = query.PageIndex,
                    Size = query.PageSize,
                };

                return Ok(new PagedApiResponse<ToDoDetail>
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
        /// Updates an existing todo list.
        /// </summary>
        /// <param name="id">The ID of the todo list to update.</param>
        /// <param name="title">The new title for the todo list.</param>
        /// <returns>Returns the updated todo list details.</returns>
        /// <response code="200">If the card is updated successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ToDoDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateToDoListAsync(Guid id, [FromForm] string title)
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
                var result = await _toDoService.UpdateToDoListAsync(id, title);

                return Ok(new ApiResponse<ToDoDetail>()
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
        /// Changes the status of an existing todo list.
        /// </summary>
        /// <param name="id">The ID of the todo list whose status is to be changed.</param>
        /// <param name="isActive">The status of the todo list to update.</param>
        /// <returns>Returns the updated todo list details.</returns>
        /// <response code="200">If the todo list status is changed successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize/*(Roles = "Admin")*/]
        [HttpPut("change-status/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ToDoDetail>), StatusCodes.Status200OK)]
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
                var result = await _toDoService.ChangeStatusAsync(id, isActive);

                return Ok(new ApiResponse<ToDoDetail>()
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
