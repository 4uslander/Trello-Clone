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


        [Authorize]
        [HttpPost("create-card")]
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

        [Authorize]
        [HttpGet("get-all-card")]
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

        [Authorize]
        [HttpPut("update-card/{id}")]
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
