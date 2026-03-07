using MediatR;
using MessMate.Application.Common.Responses;
using MessMate.Application.Features.Applications.Commands;
using MessMate.Application.Features.RoleApplications.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessMate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ApplicationsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize]
        [HttpPost("Apply-MessOwner")]
        public async Task<IActionResult> ApplyMessOwner()
        {
            var result = await _mediator.Send(new ApplyMessOwnerCommand());
            return Ok(ApiResponse<Unit>.SuccessResponse(result, "Application submitted successfully"));
        }

        [HttpPost("Apply-DeliveryPartner")]
        public async Task<IActionResult> ApplyDeliveyPartner()
        {
            var result = await _mediator.Send(new ApplyDeliveryPartnerCommand());
            return Ok(ApiResponse<Unit>.SuccessResponse(result, "Application submitted successfully"));
        }
    }
}
