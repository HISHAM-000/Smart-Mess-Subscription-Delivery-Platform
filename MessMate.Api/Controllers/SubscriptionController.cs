using MediatR;
using MessMate.Application.Common.Responses;
using MessMate.Application.Features.Subscriptions.Commands;
using MessMate.Application.Features.Subscriptions.DTOs;
using MessMate.Application.Features.Subscriptions.Queries;
using MessMate.Domain.Entities;
using MessMate.Domain.Enums;
using MessMate.Domain.Interfaces.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace MessMate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubscriptionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SubscriptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("plans")]
        [Authorize(Roles = "MessOwner")]
        public async Task<IActionResult> CreatePlan(CreateSubscriptionPlanCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(ApiResponse<int>.SuccessResponse(id, "Plan created successfully."));
        }

        [HttpGet("plans/{messId}")]
        public async Task<IActionResult> GetPlans(int messId)
        {
            var result = await _mediator.Send(new GetPlansByMessQuery(messId));
            return Ok(ApiResponse<List<SubscriptionPlanDto>>.SuccessResponse(result, "Plans retrieved."));
        }

        [HttpDelete("plans/{planId}")]
        [Authorize(Roles = "MessOwner")]
        public async Task<IActionResult> DeletePlan(int planId)
        {
            var result = await _mediator.Send(new DeleteSubscriptionPlanCommand(planId));
            return Ok(ApiResponse<string?>.SuccessResponse(null, "Plan deleted successfully."));
        }

        [HttpPost("enroll")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Enroll(EnrollSubscriptionCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(ApiResponse<int>.SuccessResponse(id, "Enrolled successfully."));
        }

        [HttpGet("my-Subscription")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMySubscriptions()
        {
            var result = await _mediator.Send(new GetMySubscriptionsQuery());
            return Ok(ApiResponse<List<MySubscriptionDto>>.SuccessResponse(result, "Subscriptions retrieved."));
        }

        [HttpPost("{subscriptionId}/pause")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Pause( int subscriptionId, PauseSubscriptionCommand command)
        {
            var result = await _mediator.Send(
                command with { SubscriptionId = subscriptionId });
            return Ok(ApiResponse<string?>.SuccessResponse(null, "Subscription paused."));
        }

        [HttpPost("{subscriptionId}/cancel")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Cancel(int subscriptionId)
        {
            var result = await _mediator.Send(
                new CancelSubscriptionCommand(subscriptionId));
            return Ok(ApiResponse<string?>.SuccessResponse(null, "Subscription cancelled."));
        }

        [HttpPost("skip/{orderId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> SkipMeal(int orderId)
        {
            var result = await _mediator.Send(new SkipMealCommand(orderId));
            return Ok(ApiResponse<SkipMealResult>.SuccessResponse(result, result.Message));
        }

        [HttpGet("my-plans")]
        [Authorize(Roles = "MessOwner")]
        public async Task<IActionResult> GetMyPlans()
        {
            var result = await _mediator.Send(new GetMyPlansQuery());
            return Ok(ApiResponse<List<SubscriptionPlanDto>>
                .SuccessResponse(result, "Plans retrieved successfully."));
        }

        [HttpGet("skips/my")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMySkips()
        {
            var result = await _mediator.Send(new GetMySkipsQuery());
            return Ok(ApiResponse<List<MealSkipDto>>.SuccessResponse(result, "Skip history retrieved."));
        }

        [Authorize(Roles = "MessOwner")]
        [HttpPut("update-plan/{planId}")]
        public async Task<IActionResult> UpdatePlan(int planId, UpdateSubscriptionPlanCommand command)
        {
            var updatedCommand = command with { PlanId = planId };
            var result = await _mediator.Send(updatedCommand);
            return Ok(ApiResponse<bool>.SuccessResponse(
                result, "Updated Successfully"));
        }


    }
}
