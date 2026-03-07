using MediatR;
using MessMate.Application.Common.Responses;
using MessMate.Application.Features.RoleApplications.Commands;
using MessMate.Application.Features.RoleApplications.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessMate.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("applications")]
        public async Task<IActionResult> GetPendingApplications()
        {
            var result = await _mediator.Send(new GetPendingApplicationsQuery());
            return Ok(result);
        }

        [HttpPost("applications/{id}/approve")]
        public async Task<IActionResult> Approve(Guid id)
        {
            var result = await _mediator.Send(
                new ApproveApplicationCommand { ApplicationId = id });

            return Ok(ApiResponse<Unit>.SuccessResponse(result,"Application approved Successfully"));
        }

        [HttpPost("applications/{id}/reject")]
        public async Task<IActionResult> Reject(Guid id)
        {
            var result = await _mediator.Send(
                new RejectApplicationCommand { ApplicationId = id });

            return Ok(ApiResponse<Unit>.SuccessResponse(result,"Application rejected"));
        }
    }
}
