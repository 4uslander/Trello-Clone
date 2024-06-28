using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Trello.Application.DTOs.Board;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.CardMemberServices;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.DTOs.CardMember;
using Trello.Application.DTOs.List;
using Trello.Application.Utilities.Helper.Pagination;

namespace Trello.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardMemberController : ControllerBase
    {
        private readonly ICardMemberService _cardMemberService;
        public CardMemberController(ICardMemberService cardMemberService)
        {
            _cardMemberService = cardMemberService;
        }

        /// <summary>
        /// Creates a new card member.
        /// </summary>
        /// <param name="requestBody">The details of the card member to be created.</param>
        /// <returns>Returns the created card member details.</returns>
        /// <response code="201">If the card member is created successfully.</response>
        /// <response code="400">If the request body is invalid.</response>
        /// <response code="500">If an unexpected error occurs, returns an error message.</response>
        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<CardMemberDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCardMemberAsync([FromBody] CardMemberDTO requestBody)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _cardMemberService.CreateCardMemberAsync(requestBody);
                return Created(string.Empty, new ApiResponse<CardMemberDetail>
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
        /// Retrieves all card members with optional user name filtering and pagination support.
        /// </summary>
        /// <param name="cardId">The ID of the card to get all card members of that card.</param>
        /// <param name="query">The pagination query parameters including page index and page size.</param>
        /// <returns>Returns an action result containing a paged response of list details.</returns>
        /// <response code="200">If the retrieval is successful, returns a paged list of list details.</response>
        /// <response code="400">If the request is invalid, returns a list of validation errors.</response>
        /// <response code="500">If an unexpected error occurs, returns an error message.</response>
        [Authorize]
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(PagedApiResponse<List<CardMemberDetail>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCardMemberAsync([FromQuery] Guid cardId, [FromQuery] PagingQuery query)
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
                List<CardMemberDetail> result = await _cardMemberService.GetAllCardMemberAsync(cardId);

                var pagingResult = result.PagedItems(query.PageIndex, query.PageSize).ToList();

                var paging = new PaginationInfo
                {
                    Page = query.PageIndex,
                    Size = query.PageSize,
                };

                return Ok(new PagedApiResponse<CardMemberDetail>
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
        /// Retrieves all card members with optional user name filtering and pagination support.
        /// </summary>
        /// <param name="cardId">The ID of the card to get all card members of that card.</param>
        /// <param name="query">The pagination query parameters including page index and page size.</param>
        /// <param name="userName">The optional user name filter for lists.</param>
        /// <param name="isActive">The optional isActive filter.</param>
        /// <returns>Returns an action result containing a paged response of list details.</returns>
        /// <response code="200">If the retrieval is successful, returns a paged list of list details.</response>
        /// <response code="400">If the request is invalid, returns a list of validation errors.</response>
        /// <response code="500">If an unexpected error occurs, returns an error message.</response>
        [Authorize]
        [HttpGet("get-by-filter")]
        [ProducesResponseType(typeof(PagedApiResponse<List<CardMemberDetail>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCardMemberByFilterAsync([FromQuery] Guid cardId, [FromQuery] PagingQuery query,
            [FromQuery] string? userName, bool? isActive)
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
                List<CardMemberDetail> result = await _cardMemberService.GetCardMemberByFilterAsync(cardId, userName, isActive);

                var pagingResult = result.PagedItems(query.PageIndex, query.PageSize).ToList();

                var paging = new PaginationInfo
                {
                    Page = query.PageIndex,
                    Size = query.PageSize,
                };

                return Ok(new PagedApiResponse<CardMemberDetail>
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
        /// Changes the status of an existing card member.
        /// </summary>
        /// <param name="id">The ID of the card member whose status is to be changed.</param>
        /// <param name="isActive">The status of the card member to update.</param>
        /// <returns>Returns the updated card member details.</returns>
        /// <response code="200">If the card member status is changed successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If an unexpected error occurs, returns an error message.</response>
        [Authorize/*(Roles = "Admin")*/]
        [HttpPut("change-status/{id}")]
        [ProducesResponseType(typeof(ApiResponse<CardMemberDetail>), StatusCodes.Status200OK)]
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
                var result = await _cardMemberService.ChangeStatusAsync(id, isActive);

                return Ok(new ApiResponse<CardMemberDetail>()
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
