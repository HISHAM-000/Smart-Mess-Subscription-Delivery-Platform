using MediatR;
using MessMate.Application.Common.Responses;
using MessMate.Application.Features.Applications.Commands;
using MessMate.Application.Features.Applications.DTOs;
using MessMate.Application.Features.Applications.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessMate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ApplicationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ApplicationController(IMediator mediator)
        {
            _mediator = mediator; 
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("pending-messes")]
        public async Task<IActionResult> GetPendingMesses()
        {
            var result = await _mediator.Send(new GetPendingMessesQuery());
            return Ok(ApiResponse<List<PendingMessDto>>.SuccessResponse(result, "Pending messes retrieved."));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("approve-mess/{messId}")]
        public async Task<IActionResult> ApproveMess(int messId)
        {
            var result = await _mediator.Send(new ApproveMessCommand(messId));
            return Ok(ApiResponse<string?>.SuccessResponse(null, "Mess approved successfully."));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("reject-owner/{userId}")]
        public async Task<IActionResult> RejectOwner(int userId,RejectRequest request)
        {
            var result = await _mediator.Send(new RejectOwnerCommand(userId, request.Reason));
            return Ok(ApiResponse<string?>.SuccessResponse(null,
                "Owner rejected successfully."));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("reject-Mess/{messId}")]
        public async Task<IActionResult> RejectMess(int messId,RejectRequest request)
        {
            var result = await _mediator.Send(new RejectMessCommand(messId, request.Reason));
            return Ok(ApiResponse<string?>.SuccessResponse(null,
                "Mess rejected successfully."));
        }

        [Authorize(Roles = "MessOwner")]
        [HttpPost("resubmit-owner")]
        public async Task<IActionResult> ResubmitOwner(ResubmitOwnerCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<int>.SuccessResponse(result,
                "Resubmitted Successfully"));
        }


    }
}
