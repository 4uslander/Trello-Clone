using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Trello.Application.DTOs.Board;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.UserFcmTokenServices;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.Pagination;
using Trello.Application.DTOs.UserFcmToken;
using Trello.Domain.Models;

namespace Trello.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFcmTokenController : ControllerBase
    {
        private readonly IUserFcmTokenService _userFcmTokenService;
        public UserFcmTokenController(IUserFcmTokenService userFcmTokenService)
        {
            _userFcmTokenService = userFcmTokenService;
        }

        /// <summary>
        /// Creates a new User Fcm Token with the specified details.
        /// </summary>
        /// <param name="requestBody">The details of the User Fcm Token to be created.</param>
        /// <returns>
        /// A response indicating the result of the User Fcm Token creation process. If successful, 
        /// returns a 201 Created status with the details of the created User Fcm Token.
        /// </returns>
        /// <response code="201">Returns the created User Fcm Token details.</response>
        /// <response code="400">If the request body is invalid.</response>
        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<UserFcmTokenDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateUserFcmTokenAsync([FromBody] CreateUserFcmTokenDTO requestBody)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _userFcmTokenService.CreateUserFcmTokenAsync(requestBody);
                return Created(string.Empty, new ApiResponse<UserFcmTokenDetail>
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
        /// Retrieves User Fcm Tokens
        /// </summary>
        /// <param name="query">The pagination query parameters including page index and page size.</param>
        /// <returns>Returns a list of User Fcm Token details.</returns>
        /// <response code="200">If the retrieval is successful.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(PagedApiResponse<List<UserFcmTokenDetail>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUserFcmTokenAsync([FromQuery] PagingQuery query, Guid userId)
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

                List<UserFcmTokenDetail> result = await _userFcmTokenService.GetAllUserFcmTokenAsync(userId);

                var pagingResult = result.PagedItems(query.PageIndex, query.PageSize).ToList();

                var paging = new PaginationInfo
                {
                    Page = query.PageIndex,
                    Size = query.PageSize,
                };

                return Ok(new PagedApiResponse<UserFcmTokenDetail>
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
        /// Updates an existing User Fcm Token.
        /// </summary>
        /// <param name="id">The ID of the User Fcm Token to update.</param>
        /// <param name="requestBody">The new details for the User Fcm Token.</param>
        /// <returns>Returns the updated User Fcm Token details.</returns>
        /// <response code="200">If the User Fcm Token is updated successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(ApiResponse<UserFcmTokenDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUserFcmTokenAsync(Guid id, [FromBody] UserFcmTokenDTO requestBody)
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
                var result = await _userFcmTokenService.UpdateUserFcmTokenAsync(id, requestBody);

                return Ok(new ApiResponse<UserFcmTokenDetail>()
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
        /// Changes the status of an existing User Fcm Token.
        /// </summary>
        /// <param name="id">The ID of the User Fcm Token whose status is to be changed.</param>
        /// <returns>Returns the updated User Fcm Token details.</returns>
        /// <response code="200">If the User Fcm Token status is changed successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpPut("change-status")]
        [ProducesResponseType(typeof(ApiResponse<UserFcmTokenDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeStatusAsync([FromQuery] string fcmToken, [FromQuery] Guid userId, [FromQuery] bool isActive)
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

                var result = await _userFcmTokenService.ChangeStatusAsync(fcmToken, userId, isActive);

                return Ok(new ApiResponse<UserFcmTokenDetail>
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
