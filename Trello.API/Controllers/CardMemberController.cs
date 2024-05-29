using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Trello.Application.DTOs.Board;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.CardMemberServices;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.DTOs.CardMember;

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

        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<CardMemberDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCardMemberAsync(CardMemberDTO requestBody)
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
    }
}
