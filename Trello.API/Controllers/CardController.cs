using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trello.Application.DTOs.Card;
using Trello.Application.DTOs.List;
using Trello.Application.Services.CardServices;
using Trello.Application.Services.ListServices;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;

namespace Trello.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        /// <summary>
        /// Creates a new card.
        /// </summary>
        /// <param name="requestBody">The details of the card to be created.</param>
        /// <returns>Returns the created card details.</returns>
        /// <response code="201">If the card is created successfully.</response>
        /// <response code="400">If the request body is invalid.</response>
        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<CardDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCardAsync(CreateCardDTO requestBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _cardService.CreateCardAsync(requestBody);

            return Created(string.Empty, new ApiResponse<CardDetail>()
            {
                Code = StatusCodes.Status201Created,
                Data = result
            });
        }

        /// <summary>
        /// Retrieves all cards, optionally filtered by title.
        /// </summary>
        /// <param name="title">The optional title filter for cards.</param>
        /// <returns>Returns a list of card details.</returns>
        /// <response code="200">If the retrieval is successful.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<CardDetail>>), StatusCodes.Status200OK)]
        public IActionResult GetAllCards([FromQuery] string? title)
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
            List<CardDetail> result = _cardService.GetAllList(title);

            return Ok(new ApiResponse<List<CardDetail>>
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }

        /// <summary>
        /// Updates an existing card.
        /// </summary>
        /// <param name="id">The ID of the card to update.</param>
        /// <param name="requestBody">The new details for the card.</param>
        /// <returns>Returns the updated card details.</returns>
        /// <response code="200">If the card is updated successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize]
        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(ApiResponse<CardDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCardAsync(Guid id, [FromForm] UpdateCardDTO requestBody)
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
            var result = await _cardService.UpdateCardAsync(id, requestBody);

            return Ok(new ApiResponse<CardDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }

        /// <summary>
        /// Changes the status of an existing card.
        /// </summary>
        /// <param name="id">The ID of the card whose status is to be changed.</param>
        /// <returns>Returns the updated card details.</returns>
        /// <response code="200">If the card status is changed successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [Authorize/*(Roles = "Admin")*/]
        [HttpPut("change-status/{id}")]
        [ProducesResponseType(typeof(ApiResponse<CardDetail>), StatusCodes.Status200OK)]
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
            var result = await _cardService.ChangeStatusAsync(id);

            return Ok(new ApiResponse<CardDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }
    }
}
