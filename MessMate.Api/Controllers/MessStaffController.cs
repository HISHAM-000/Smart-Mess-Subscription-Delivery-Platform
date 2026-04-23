using MediatR;
using MessMate.Application.Common.Responses;
using MessMate.Application.Features.MessStaff.Commands;
using MessMate.Application.Features.MessStaff.DTOs;
using MessMate.Application.Features.MessStaff.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessMate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessStaffController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MessStaffController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add-staff")]
        public async Task<IActionResult> AddStaff(AddStaffCommand command)
        {
            var staffId = await _mediator.Send(command);
            return Ok(ApiResponse<int>.SuccessResponse(
                staffId, "Staff member added successfully."));
        }

        [HttpGet("get-staff")]
        [Authorize(Roles = "MessOwner")]
        public async Task<IActionResult> GetMyStaff(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetMyStaffQuery(), ct);

            return Ok(ApiResponse<List<StaffDto>>.SuccessResponse(
                result, "Staff retrieved."));
        }

        [HttpDelete("delete-staff/{staffId}")]
        [Authorize(Roles = "MessOwner")]
        public async Task<IActionResult> DeleteStaff(
            int staffId,
            CancellationToken ct)
        {
            var result = await _mediator
                .Send(new DeleteStaffCommand(staffId), ct);
            return Ok(ApiResponse<string?>
                .SuccessResponse(null, "Staff member removed successfully."));
        }
    }
}
