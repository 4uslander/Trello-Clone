using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Trello.Application.DTOs.CardActivity;
using Trello.Application.DTOs.Comment;
using Trello.Application.Services.CardActivityServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.Pagination;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;

namespace Trello.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardActivityController : ControllerBase
    {
        private readonly ICardActivityService _cardActivityService;

        public CardActivityController(ICardActivityService cardActivityService)
        {
            _cardActivityService = cardActivityService;
        }

        /// <summary>
        /// Creates a new card activity.
        /// </summary>
        /// <param name="requestBody">The details of the card activity to be created.</param>
        /// <returns>Returns the created card activity details.</returns>
        /// <response code="201">If the comment is created successfully.</response>
        /// <response code="400">If the request body is invalid.</response>
        /// <response code="500">If an unexpected error occurs, returns an error message.</response>
        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<CardActivityDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCardActivityAsync([FromBody] CreateCardActivityDTO requestBody)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _cardActivityService.CreateCardActivityAsync(requestBody);

                return Created(string.Empty, new ApiResponse<CardActivityDetail>() {
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
        /// Retrieves all card activity with pagination support.
        /// </summary>
        /// <param name="query">The pagination query parameters including page index and page size.</param>
        /// <param name="cardId">The Id of the card to display.</param>
        /// <returns>Returns an action result containing a paged response of card activity details.</returns>
        /// <response code="200">If the retrieval is successful, returns a paged list of card activity details.</response>
        /// <response code="400">If the request is invalid, returns a list of validation errors.</response>
        /// <response code="500">If an unexpected error occurs, returns an error message.</response>
        [Authorize]
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(PagedApiResponse<List<CardActivityDetail>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCardActivityAsync([FromQuery] Guid cardId, [FromQuery] PagingQuery query)
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
                List<CardActivityDetail> result = await _cardActivityService.GetCardActivityDetailAsync(cardId);

                var pagingResult = result.PagedItems(query.PageIndex, query.PageSize).ToList();
                var total = result.Count;

                var paging = new PaginationInfo
                {
                    Page = query.PageIndex,
                    Size = query.PageSize,
                };

                return Ok(new PagedApiResponse<CardActivityDetail>
                {
                    Code = StatusCodes.Status200OK,
                    Paging = paging,
                    Data = pagingResult
                });
            }
            catch(ExceptionResponse ex)
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
