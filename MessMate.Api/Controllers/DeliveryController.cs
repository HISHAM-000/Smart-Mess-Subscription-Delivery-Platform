using MediatR;
using MessMate.Api.Models.Request;
using MessMate.Application.Common.Responses;
using MessMate.Application.Features.Delivery.Commands;
using MessMate.Application.Features.Delivery.DTOs;
using MessMate.Application.Features.Delivery.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessMate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DeliveryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{orderId}/assign")]
        [Authorize(Roles = "MessOwner")]
        public async Task<IActionResult> AssignDelivery(
           int orderId,
           AssignDeliveryRequest request,
           CancellationToken ct)
        {
            var result = await _mediator.Send(
                new AssignDeliveryCommand(orderId, request.StaffId), ct);
            return Ok(ApiResponse<AssignDeliveryResult>
                .SuccessResponse(result, result.Message));
        }

        [HttpPost("{orderId}/confirm")]
        [Authorize(Roles = "MessStaff")]
        public async Task<IActionResult> ConfirmDelivery(
           int orderId,
           ConfirmDeliveryRequest request,
           CancellationToken ct)
        {
            var result = await _mediator.Send(
                new ConfirmDeliveryCommand(orderId, request.OTP), ct);
            return Ok(ApiResponse<string?>
                .SuccessResponse(null, "Delivery confirmed successfully."));
        }

        [HttpGet("my-history")]
        [Authorize(Roles = "MessStaff")]
        public async Task<IActionResult> GetMyHistory(CancellationToken ct)
        {
            var result = await _mediator
                .Send(new GetMyDeliveryHistoryQuery(), ct);
            return Ok(ApiResponse<List<DeliveryHistoryDto>>
                .SuccessResponse(result, "Delivery history retrieved."));
        }
    }
}
