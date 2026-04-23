using MediatR;
using MessMate.Api.Models.Request;
using MessMate.Application.Common.Responses;
using MessMate.Application.Features.Orders.Commands;
using MessMate.Application.Features.Orders.DTOs;
using MessMate.Application.Features.Orders.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Cms;

namespace MessMate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("my-orders")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyOrders(CancellationToken ct)
        {
            var result = await _mediator
                .Send(new GetMyOrdersQuery(), ct);
            return Ok(ApiResponse<List<OrderDto>>
                .SuccessResponse(result, "Orders retrieved."));
        }

        [HttpGet("mess-orders")]
        [Authorize(Roles = "MessOwner, MessStaff")]
        public async Task<IActionResult> GetMessOrders(
            string? date,
            CancellationToken ct)
        {
            var result = await _mediator
                .Send(new GetMessOrdersQuery(date), ct);
            return Ok(ApiResponse<List<MessOrderDto>>
                .SuccessResponse(result, "Orders retrieved."));
        }

        [HttpPut("{orderId}/status")]
        [Authorize(Roles = "MessStaff")]
        public async Task<IActionResult> UpdateStatus(
            int orderId,
            UpdateStatusRequest request,
            CancellationToken ct)
        {
            var result = await _mediator.Send(
                new UpdateOrderStatusCommand(orderId, request.NewStatus), ct);
            return Ok(ApiResponse<string?>
                .SuccessResponse(null, "Order status updated."));
        }
    }
}
