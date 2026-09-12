using CRM.BLL.Service.Authentication;
using CRM.DAL.DTO.Request.Authentication;
using CRM.DAL.Models;
using CRM.PL.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRM.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly IAuthenticationService _authenticationService;
        public AccountController(IAuthenticationService authenticationService) { 
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authenticationService.RegisterAsync(request);
            if (!result.Success)
                return BadRequestResponse(result.Message, result.Errors);
            return SuccessResponse(result, result.Message);
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            var confirmed = await _authenticationService.ConfirmEmailAsync(token, userId);
            if(!confirmed)
                return BadRequestResponse("Email confirmation failed");
            return SuccessResponse("Email confirmed successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request);
            if (!result.Success)
                return UnauthorizedResponse(result.Message);
            return SuccessResponse(result.AccessToken, result.Message);
        }

        [HttpPost("sendCode")]
        public async Task<IActionResult> RequestPasswordReset(ForgotPasswordRequest request)
        {
            var result = await _authenticationService.RequestPasswordResetAsync(request);
            if (!result.Success)
                return BadRequestResponse(result.Message);

            return SuccessResponse(result, result.Message);
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> PasswordReset(ResetPasswordRequest request)
        {
            var result = await _authenticationService.PasswordResetAsync(request);
            if (!result.Success)
                return BadRequestResponse(result.Message);
            return SuccessResponse(result, result.Message);
        }
    }
}
