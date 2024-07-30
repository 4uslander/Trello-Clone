using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Trello.Application.Services.NotificationServices;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.Pagination;
using Trello.Application.DTOs.Notification;

namespace Trello.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Creates a new notification.
        /// </summary>
        /// <param name="requestBody">The details of the to do to be created.</param>
        /// <returns>Returns the created todo details.</returns>
        /// <response code="201">If the todo is created successfully.</response>
        /// <response code="400">If the request body is invalid.</response>
        /// <response code="500">If an unexpected error occurs, returns an error message.</response>
        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<NotificationDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateNotificationAsync([FromBody] NotificationDTO requestBody)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _notificationService.CreateNotificationAsync(requestBody);

                return Created(string.Empty, new ApiResponse<NotificationDetail>()
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

        ///// <summary>
        ///// Retrieves all notification, optionally filtered by title.
        ///// </summary>
        ///// <param name="listId">The ID of the list to get.</param>
        ///// <param name="query">The pagination query parameters including page index and page size.</param>
        ///// <returns>Returns a list of notification details.</returns>
        ///// <response code="200">If the retrieval is successful.</response>
        ///// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(PagedApiResponse<List<NotificationDetail>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllNotificationAsync([FromQuery] Guid userId, [FromQuery] PagingQuery query)
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

                var (result, totalCount) = await _notificationService.GetAllNotificationAsync(userId);

                var pagingResult = result.PagedItems(query.PageIndex, query.PageSize).ToList();

                var paging = new PaginationInfo
                {
                    Page = query.PageIndex,
                    Size = query.PageSize,
                };

                var response = new PagedApiResponse<NotificationDetail>
                {
                    Code = StatusCodes.Status200OK,
                    Paging = paging,
                    Data = pagingResult
                };

                response.TotalCount = totalCount; // Add total count to the response

                return Ok(response);
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
        /// Retrieves all notification, optionally filtered by title.
        /// </summary>
        /// <param name="listId">The ID of the list to get.</param>
        /// <param name="query">The pagination query parameters including page index and page size.</param>
        /// <param name="title">The optional title filter for cards.</param>
        /// <param name="isActive">The optional is Active filter for cards.</param>
        /// <returns>Returns a list of notification details.</returns>
        /// <response code="200">If the retrieval is successful.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpGet("get-by-filter")]
        [ProducesResponseType(typeof(PagedApiResponse<List<NotificationDetail>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNotificationByFilterAsync([FromQuery] Guid userId, [FromQuery] PagingQuery query, [FromQuery] string? title, bool? isRead)
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
                List<NotificationDetail> result = await _notificationService.GetNotificationByFilterAsync(userId, title, isRead);

                var pagingResult = result.PagedItems(query.PageIndex, query.PageSize).ToList();

                var paging = new PaginationInfo
                {
                    Page = query.PageIndex,
                    Size = query.PageSize,
                };

                return Ok(new PagedApiResponse<NotificationDetail>
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
        /// Retrieves total number of notifications
        /// </summary>
        /// <param name="userId">The ID of the user to get.</param>
        /// <returns>Returns a total number of notifications.</returns>
        /// <response code="200">If the retrieval is successful.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpGet("get-notification-count")]
        [ProducesResponseType(typeof(ApiResponse<NotificationDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNotificationCountAsync([FromQuery] Guid userId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                int count = await _notificationService.GetNotificationCountAsync(userId);

                return Created(string.Empty, new ApiResponse<int>()
                {
                    Code = StatusCodes.Status200OK,
                    Data = count
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
        /// Changes the status of an existing notification.
        /// </summary>
        /// <param name="id">The ID of the notification whose status is to be changed.</param>
        /// <param name="isActive">The status of the notification to update.</param>
        /// <returns>Returns the updated notification details.</returns>
        /// <response code="200">If the notification status is changed successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpPut("change-status/{id}")]
        [ProducesResponseType(typeof(ApiResponse<NotificationDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeStatusAsync(Guid id, [FromQuery] bool isRead)
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
                var result = await _notificationService.ChangeStatusAsync(id, isRead);

                return Ok(new ApiResponse<NotificationDetail>()
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
