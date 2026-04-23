using MediatR;
using MessMate.Application.Common.Responses;
using MessMate.Application.Features.Applications.DTOs;
using MessMate.Application.Features.Applications.Queries;
using MessMate.Application.Features.Auth.Commands;
using MessMate.Application.Features.Orders.Commands;
using MessMate.Domain.Entities;
using MessMate.Domain.Enums;
using MessMate.Domain.Interfaces.Contracts;
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
        private readonly IServiceScopeFactory _scopeFactory;
        public AdminController(IMediator mediator, IServiceScopeFactory scopeFactor)
        {
            _mediator = mediator;
            _scopeFactory = scopeFactor;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("trigger-order-generation")]
        public async Task<IActionResult> TriggerOrderGeneration(CancellationToken ct)
        {
            var count = await _mediator.Send(new GenerateOrdersCommand(), ct);

            return Ok(ApiResponse<string?>.SuccessResponse(
                null, $"{count} orders generated for today."));
        }

    }
}
