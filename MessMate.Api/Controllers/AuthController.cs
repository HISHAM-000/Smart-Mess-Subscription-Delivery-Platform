using MediatR;
using MessMate.Api.Services;
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
        private readonly CookieService _cookieService;

        public AuthController(IMediator mediator, CookieService cookieService)
        {
            _mediator = mediator;
            _cookieService = cookieService;
        }

        [HttpPost("register-customer")]
        public async Task<IActionResult> RegisteUser(RegisterUserCommand command)
        {
            var userId = await _mediator.Send(command);
            return Ok(ApiResponse<int>.SuccessResponse(userId, "User registered successfully"));
        }
        [HttpPost("register-mess-owner")]
        public async Task<IActionResult> RegisterMess(RegisterMessCommand command)
        {
            var userId = await _mediator.Send(command);
            return Ok(ApiResponse<int>.SuccessResponse(userId, "Mess Owner registered successfully"));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserCommand command)
        {
            var user = await _mediator.Send(command);
            _cookieService.SetAuthCookies(Response, user.AccessToken, user.RefreshToken);
            return Ok(ApiResponse<LoginResponse>.SuccessResponse(user, "Login successfull"));
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            _cookieService.SetAuthCookies(Response, result.AccessToken, result.RefreshToken);
            return Ok(ApiResponse<LoginResponse>.SuccessResponse(result));
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(LogoutCommand command)
        {
            var result = await _mediator.Send(command);
            _cookieService.ClearAuthCookies(Response);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Logout Successfull"));
        }

    }


}
