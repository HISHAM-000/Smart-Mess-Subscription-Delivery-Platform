using MediatR;
using MessMate.Application.Common.Responses;
using MessMate.Application.Features.Menu.Commands;
using MessMate.Application.Features.Menu.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessMate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MenuController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-menu")]
        [Authorize(Roles = "MessOwner")]
        public async Task<IActionResult> CreateMenu(CreateMenuCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(ApiResponse<int>.SuccessResponse(id, "Menu created"));
        }

        [HttpPost("add-items")]
        [Authorize(Roles = "MessOwner")]
        public async Task<IActionResult> AddItem(AddMenuItemCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(ApiResponse<int>.SuccessResponse(id, "Item added"));
        }

        [HttpPut("update-items/{itemId}")]
        [Authorize(Roles = "MessOwner")]
        public async Task<IActionResult> UpdateMenuItem(
            int itemId,
            UpdateMenuItemCommand command)
        {
            var updatedCommand = command with { ItemId = itemId };

            var result = await _mediator.Send(updatedCommand);

            return Ok(ApiResponse<bool>.SuccessResponse(
                result, "Menu item updated successfully"));
        }

        [HttpDelete("delete-items/{itemId}")]
        [Authorize(Roles = "MessOwner")]
        public async Task<IActionResult> DeleteMenuItem(int itemId)
        {
            var result = await _mediator.Send(
                new DeleteMenuItemCommand(itemId));

            return Ok(ApiResponse<string?>.SuccessResponse(
                null, "Menu item deleted successfully"));
        }

        [HttpGet("get-menu/{messId}/{day}")]
        public async Task<IActionResult> GetMenu(int messId, DayOfWeek day)
        {
            var result = await _mediator.Send(
                new GetMenuByMessAndDayQuery(messId, day));

            return Ok(ApiResponse<object>.SuccessResponse(
                result, "Menu retrieved successfully"));
        }

        [HttpGet("get-today-menu/{messId}")]
        public async Task<IActionResult> GetTodayMenu(int messId)
        {
            var result = await _mediator.Send(
                new GetTodayMenuQuery(messId));

            return Ok(ApiResponse<object>.SuccessResponse(
                result, "Today's menu retrieved successfully"));
        }
    }
}
