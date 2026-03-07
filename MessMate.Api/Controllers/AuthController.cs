using MediatR;
using MessMate.Application.Common.Responses;
using MessMate.Application.Features.Auth.Commands;
using MessMate.Application.Features.Auth.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessMate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;   
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            var userId = await _mediator.Send(command);
            return Ok(ApiResponse<Guid>.SuccessResponse(userId,"User registered successfully"));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserCommand command)
        {
            var user = await _mediator.Send(command);
            return Ok(ApiResponse<LoginResponse>.SuccessResponse(user, "Login successfull"));
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult>RefreshToken(RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<LoginResponse>.SuccessResponse(result));
        }

        [HttpPost("Logout")]
        public async Task<IActionResult>Logout(LogoutCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Logout Successfull"));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public  IActionResult Try()
        {
            return Ok("Haii");
        }
    }

   
}
