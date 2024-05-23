using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.List;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.ListServices;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;

namespace Trello.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        private readonly IListService _listService;
        public ListController(IListService listService)
        {
            _listService = listService;
        }

        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<ListDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateListAsync(CreateListDTO requestBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _listService.CreateListAsync(requestBody);

            return Created(string.Empty, new ApiResponse<ListDetail>()
            {
                Code = StatusCodes.Status201Created,
                Data = result
            });
        }

        [Authorize]
        [HttpGet("all")]
        [ProducesResponseType(typeof(ApiResponse<List<ListDetail>>), StatusCodes.Status200OK)]
        public IActionResult GetAllLists([FromQuery] string? name)
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
            List<ListDetail> result = _listService.GetAllList(name);

            return Ok(new ApiResponse<List<ListDetail>>
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ListDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateListAsync(Guid id, [FromForm] UpdateListDTO requestBody)
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
            var result = await _listService.UpdateListAsync(id, requestBody);

            return Ok(new ApiResponse<ListDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }

        [Authorize/*(Roles = "Admin")*/]
        [HttpDelete("ChangeStatus/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ListDetail>), StatusCodes.Status200OK)]
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
            var result = await _listService.ChangeStatusAsync(id);

            return Ok(new ApiResponse<ListDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }
    }
}
