using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Net;
using Trello.Application.DTOs.BoardMember;
using Trello.Application.DTOs.CardLabel;
using Trello.Application.DTOs.CardMember;
using Trello.Application.Services.CardLabelServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.Pagination;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;

namespace Trello.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardLabelController : ControllerBase
    {
        private readonly ICardLabelService _cardLabelService;
        public CardLabelController(ICardLabelService cardLabelService)
        {
            _cardLabelService = cardLabelService;
        }



        /// <summary>
        /// Creates a new card label.
        /// </summary>
        /// <param name="requestBody">The details of the card label to be created.</param>
        /// <returns>Returns the created card label details.</returns>
        /// <response code="201">If the card label is created successfully.</response>
        /// <response code="400">If the request body is invalid.</response>
        /// <response code="500">If an unexpected error occurs, returns an error message.</response>
        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<CardLabelDetail>) , StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCardLabelAsync([FromBody] CardLabelDTO requestBody)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _cardLabelService.CreateCardLabelAsync(requestBody);
                return Created(string.Empty, new ApiResponse<CardLabelDetail>
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
        /// Retrieves all card label with optional filtering and pagination support.
        /// </summary>
        /// <param name="cardId">The ID of the card to get all card members of that card.</param>
        /// <param name="query">The pagination query parameters including page index and page size.</param>
        /// <returns>Returns an action result containing a paged response of list details.</returns>
        /// <response code="200">If the retrieval is successful, returns a paged list of list details.</response>
        /// <response code="400">If the request is invalid, returns a list of validation errors.</response>
        /// <response code="500">If an unexpected error occurs, returns an error message.</response>
        [Authorize]
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<CardLabelDetail>>), StatusCodes.Status200OK)] 
        public async Task<IActionResult> GetAllCardLabelAsync([FromQuery] Guid cardId, [FromQuery] PagingQuery query)
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
                List<CardLabelDetail> result = await _cardLabelService.GetAllCardLabelAsync(cardId);

                var pagingResult = result.PagedItems(query.PageIndex, query.PageSize).ToList();

                var paging = new PaginationInfo
                {
                    Page = query.PageIndex,
                    Size = query.PageSize,
                };

                return Ok(new PagedApiResponse<CardLabelDetail>
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
        /// Retrieves all card label with optional label name filtering and pagination support.
        /// </summary>
        /// <param name="cardId">The ID of the card to get all card label of that card.</param>
        /// <param name="query">The pagination query parameters including page index and page size.</param>
        /// <param name="labelName">The optional label name filter for lists.</param>
        /// <param name="isActive">The optional isActive filter.</param>
        /// <returns>Returns an action result containing a paged response of list details.</returns>
        /// <response code="200">If the retrieval is successful, returns a paged list of list details.</response>
        /// <response code="400">If the request is invalid, returns a list of validation errors.</response>
        /// <response code="500">If an unexpected error occurs, returns an error message.</response>
        [Authorize]
        [HttpGet("get-by-filter")]
        [ProducesResponseType(typeof(PagedApiResponse<List<CardLabelDetail>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCardLabelByFilterAsync([FromQuery] Guid cardId, [FromQuery] PagingQuery query, [FromQuery] string? labelName, bool? isActive)
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

                List<CardLabelDetail> result = await _cardLabelService.GetCardLabelByFilterAsync(cardId, labelName, isActive);
                var pagingResult = result.PagedItems(query.PageIndex, query.PageSize).ToList();
                var paging = new PaginationInfo
                {
                    Page = query.PageIndex,
                    Size = query.PageSize,
                };

                return Ok(new PagedApiResponse<CardLabelDetail>
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
            catch(Exception ex) {
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
        /// <param name="id">The ID of the card label whose status is to be changed.</param>
        /// <param name="isActive">The status of the card label to update.</param>
        /// <returns>Returns the updated card label details.</returns>
        /// <response code="200">If the card label status is changed successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If an unexpected error occurs, returns an error message.</response>
        [Authorize]
        [HttpPut("change-status/{id}")]
        [ProducesResponseType(typeof(ApiResponse<CardLabelDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeStatusCardLabelAsync(Guid id, [FromQuery] bool isActive)
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
                var result = await _cardLabelService.ChangeStatusCardLabelAsync(id, isActive);
                return Ok(new ApiResponse<CardLabelDetail>()
                {
                    Code = StatusCodes.Status200OK,
                    Data = result
                });
            }
            catch( ExceptionResponse ex)
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
        /// Updates an existing board member.
        /// </summary>
        /// <param name="id">The ID of the board member to update.</param>
        /// <param name="requestBody">The new details for the board member.</param>
        /// <returns>Returns the updated board member details.</returns>
        /// <response code="200">If the board member is updated successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If an unexpected error occurs, returns an error message.</response>
        [Authorize]
        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(ApiResponse<CardLabelDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCardLabelAsync(Guid id, [FromBody] UpdateCardLabelDTO color)
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

                var result = await _cardLabelService.UpdateCardLabelAsync(id, color);
                return Ok(new ApiResponse<CardLabelDetail>()
                {
                    Code = StatusCodes.Status200OK,
                    Data = result
                });
            }
            catch( ExceptionResponse ex )
            {
                return StatusCode((int)ex.StatusCode, new ApiResponse<string>
                {
                    Code = (int)ex.StatusCode,
                    Data = ex.ErrorMessage
                });
            }
            catch(Exception ex)
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
