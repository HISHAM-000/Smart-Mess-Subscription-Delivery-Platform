using MediatR;
using MessMate.Application.Common.Responses;
using MessMate.Application.Features.Mess.Commands;
using MessMate.Application.Features.Mess.DTOs;
using MessMate.Application.Features.Mess.Queries;
using MessMate.Application.Features.Messes.Queries;
using MessMate.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessMate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MessController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MessController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "MessOwner")]
        [HttpPost("Create-Mess")]
        public async Task<IActionResult> Create(CreateMessCommand command)
        {
            var messId = await _mediator.Send(command);

            return Ok(ApiResponse<int>.SuccessResponse(messId, "Mess created successfully"));
        }

        [Authorize(Roles = "MessOwner")]
        [HttpGet("GetMyMess")]
        public async Task<IActionResult> GetMy()
        {
            var result = await _mediator.Send(new GetMyMessQuery());
            return Ok(ApiResponse<GetMyMessResponseDto>.SuccessResponse(result, "Fetched successfully"));
        }
        
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllMessesQuery());
            return Ok(ApiResponse<List<GetAllResponseDto>>.SuccessResponse(result,
                "Fetched successfully"));
        }


        [HttpGet("GetMessById")]
        public async Task<IActionResult> GetMessById(int id)
        {
            var mess = await _mediator.Send(new GetMessByIdQuery (id));
            return Ok(ApiResponse<GetByIdResponseDto>.SuccessResponse(mess, "Fetched successfully"));
        }

        [Authorize(Roles = "MessOwner")]
        [HttpPut("UpdateMess/{id}")]
        public async Task<IActionResult> UpdateMess(int id, UpdateMessCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<Unit>.SuccessResponse(result, "Updated successfully"));
        }

        [Authorize(Roles = "MessOwner")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMess(int id)
        {
            var result = await _mediator.Send(new DeleteMessCommand { id = id });
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Deleted successfully"));
        }
    }
}
