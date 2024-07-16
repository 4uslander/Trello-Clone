using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Trello.Application.DTOs.Label;
using Trello.Application.Services.LabelServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.Pagination;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;

namespace Trello.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelService _labelService;

        public LabelController(ILabelService labelService)
        {
            _labelService = labelService;
        }


        /// <summary>
        /// Creates a new label.
        /// </summary>
        /// <param name="requestBody">The details of the label to be created.</param>
        /// <returns>Returns the created label details.</returns>
        /// <response code="201">If the label is created successfully.</response>
        /// <response code="400">If the request body is invalid.</response>
        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<LabelDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateLabelAsync([FromBody] CreateLabelDTO requestBody)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _labelService.CreateLabelAsync(requestBody);

                return Created(string.Empty, new ApiResponse<LabelDetail>()
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
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Data = ex.Message
                });
            }
        }

        /// <summary>
        /// Retrieves all label
        /// </summary>
        /// <param name="boardId">The ID of the label to get.</param>
        /// <param name="query">The pagination query parameters including page index and page size.</param>
        /// <returns>Returns a list of label details.</returns>
        /// <response code="200">If the retrieval is successful.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpGet("get-all")]
        [ProducesResponseType(typeof (PagedApiResponse<List<LabelDetail>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLabelAsync([FromQuery] Guid boardId, [FromQuery] PagingQuery query )
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

                List<LabelDetail> result = await _labelService.GetAllLabelAsync(boardId);
                var pagingResult = result.PagedItems(query.PageIndex, query.PageSize).ToList();
                var paging = new PaginationInfo
                {
                    Page = query.PageIndex,
                    Size = query.PageSize,
                };

                return Ok(new PagedApiResponse<LabelDetail>
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
                    Data = ex.Message
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
        /// Retrieves all label, optionally filtered by title.
        /// </summary>
        /// <param name="boardId">The ID of the label to get.</param>
        /// <param name="query">The pagination query parameters including page index and page size.</param>
        /// <param name="title">The optional title filter for label.</param>
        /// <param name="isActive">The optional isActive filter.</param>
        /// <returns>Returns a list of label details.</returns>
        /// <response code="200">If the retrieval is successful.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpGet("get-by-filter")]
        [ProducesResponseType(typeof(PagedApiResponse<List<LabelDetail>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLabelByFilterAsync([FromQuery] Guid boardId, [FromQuery] PagingQuery query, [FromQuery] string? name, string? color, bool? isActive)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new ApiResponse<IEnumerable<string>>{
                        Code = StatusCodes.Status400BadRequest,
                        Data = errors
                    });
                }
                List<LabelDetail> result = await _labelService.GetLabelByFilterAsync(boardId, name, color, isActive);
                var pagingResult = result.PagedItems(query.PageIndex, query.PageSize).ToList();
                var paging = new PaginationInfo
                {
                    Page = query.PageIndex,
                    Size = query.PageSize,
                };

                return Ok(new PagedApiResponse<LabelDetail>
                {
                    Code = StatusCodes.Status200OK,
                    Paging = paging,
                    Data = pagingResult
                });

            }catch (ExceptionResponse ex)
            {
                return StatusCode((int)ex.StatusCode, new ApiResponse<string>
                {
                    Code = (int)ex.StatusCode,
                    Data = ex.ErrorMessage
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string> { 
                    
                    Code = StatusCodes.Status500InternalServerError,
                    Data = ex.Message
                
                });
            }
        }


        /// <summary>
        /// Updates an existing label.
        /// </summary>
        /// <param name="id">The ID of the label to update.</param>
        /// <param name="requestBody">The new details for the label.</param>
        /// <returns>Returns the updated label details.</returns>
        /// <response code="200">If the label is updated successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(ApiResponse<LabelDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateLabelAsync(Guid id, [FromBody] UpdateLabelDTO requestBody)
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

                var result = await _labelService.UpdateLabelAsync(id, requestBody);

                return Ok(new ApiResponse<LabelDetail>()
                {
                    Code = StatusCodes.Status200OK,
                    Data = result
                });

            }catch (ExceptionResponse ex)
            {
                return StatusCode((int)ex.StatusCode, new ApiResponse<string>
                {
                    Code = (int)ex.StatusCode,
                    Data = ex.ErrorMessage
                });
            }catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Data = ex.Message

                });
            }
        }


        /// <summary>
        /// Changes the status of an existing label.
        /// </summary>
        /// <param name="id">The ID of the label whose status is to be changed.</param>
        /// <param name="isActive">The status of the label to update.</param>
        /// <returns>Returns the updated label details.</returns>
        /// <response code="200">If the label status is changed successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpPut("change-status/{id}")]
        [ProducesResponseType(typeof(ApiResponse<LabelDetail>), StatusCodes.Status200OK)]
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

                var result = await _labelService.ChangeStatusAsync(id, isActive);

                return Ok(new ApiResponse<LabelDetail>()
                {
                    Code = StatusCodes.Status200OK,
                    Data = result
                });

            }catch (ExceptionResponse ex)
            {
                return StatusCode((int)ex.StatusCode, new ApiResponse<string>
                {
                    Code = (int)ex.StatusCode,
                    Data = ex.ErrorMessage
                });
            }catch (Exception ex)
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
