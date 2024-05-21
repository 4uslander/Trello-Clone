﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.User;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.UserServices;
using static Trello.Application.Utilities.ResponseHandler.ResponseModel;

namespace Trello.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _boardService;
        public BoardController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<GetBoardDetail>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateBoardAsync(CreateBoardDTO requestBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _boardService.CreateBoardAsync(requestBody);

            return Created(string.Empty, new ApiResponse<GetBoardDetail>()
            {
                Code = StatusCodes.Status201Created,
                Data = result
            });
        }

        [Authorize]
        [HttpGet("all")]
        [ProducesResponseType(typeof(ApiResponse<List<GetBoardDetail>>), StatusCodes.Status200OK)]
        public IActionResult GetAllBoards([FromQuery] SearchBoardDTO searchKey)
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
            List<GetBoardDetail> result = _boardService.GetAllBoard(searchKey);

            return Ok(new ApiResponse<List<GetBoardDetail>>
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<GetBoardDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBoardAsync(int id, [FromForm] UpdateBoardDTO requestBody)
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
            var result = await _boardService.UpdateBoardAsync(id, requestBody);

            return Ok(new ApiResponse<GetBoardDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }
        [Authorize/*(Roles = "Admin")*/]
        [HttpDelete("ChangeStatus/{id}")]
        [ProducesResponseType(typeof(ApiResponse<GetBoardDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeStatusAsync(int id)
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
            var result = await _boardService.ChangeStatusAsync(id);

            return Ok(new ApiResponse<GetBoardDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result
            });
        }
    }
}