using CRM.BLL.Service.Email;
using CRM.DAL.DTO.Request.Authentication;
using CRM.DAL.DTO.Response.Authentication;
using CRM.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Service.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public AuthenticationService(UserManager<ApplicationUser> userManager,
            IEmailSender emailSender, IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user = request.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user, request.Password);

            if(!result.Succeeded)
            {
                return new RegisterResponse()
                {
                    Message = "Registration failed",
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            };

            await _userManager.AddToRoleAsync(user, "Agent");
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);

            var emailUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/account/confirmEmail?token={encodedToken}&userId={user.Id}";

            await _emailSender.SendEmailAsync(user.Email,"email confirmation",$"<h1>welcome {user.UserName}</h1> <a href ='{emailUrl}'>confirm your email</a>");

            return new RegisterResponse()
            {
                Message = "Registration successful. Please check your email to confirm your account",
                Success = true
            };
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                return new LoginResponse()
                {
                    Message = "Invalid Email",
                    Success = false,
                };
            }

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed)
            {
                return new LoginResponse()
                {
                    Message = "Email is not confirmed",
                    Success = false
                };
            }

            var isValidPass = await _userManager.CheckPasswordAsync(user, request.Password);
            if(!isValidPass)
            {
                return new LoginResponse()
                {
                    Message = "Invalid Password",
                    Success = false
                };
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "account is blocked"
                };
            }

            var refreshToken = await GenerateRefreshToken(user);
            setRefreshTokenCookies(refreshToken);

            return new LoginResponse()
            {
                Message = "Login success",
                Success = true,
                AccessToken = await GenerateAccessToken(user)
            };

        }

        public async Task<bool> ConfirmEmailAsync(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return false;

            var isConfirmed = await _userManager.ConfirmEmailAsync(user, token);
            if (isConfirmed.Succeeded)
                return true;
            return false;
        }
        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.Email,user.Email!)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(10),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<ForgotPasswordResponse> RequestPasswordResetAsync(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ForgotPasswordResponse()
                {
                    Message = "Email is not valid",
                    Success = false
                };
            }

            var random = new Random();
            var code = random.Next(1000, 9999).ToString();

            user.ResetPasswordCode = code;
            user.ResetPasswordCodeExpiry = DateTime.Now.AddMinutes(15);

            await _userManager.UpdateAsync(user);

            await _emailSender.SendEmailAsync(request.Email, "reset password", $"<p>code is {code}</p>");
            return new ForgotPasswordResponse()
            {
                Message = "code sent to your email",
                Success = true
            };
        }
        public async Task<ResetPasswordResponse> PasswordResetAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                return new ResetPasswordResponse()
                {
                    Message = "Email is not valid",
                    Success = false
                };
            }
            else if(user.ResetPasswordCode != request.Code)
            {
                return new ResetPasswordResponse()
                {
                    Message = "code is not valid",
                    Success = false
                };
            }
            else if (user.ResetPasswordCodeExpiry < DateTime.UtcNow)
            {
                return new ResetPasswordResponse()
                {
                    Message = "code is expired",
                    Success = false
                };
            }
            var isSamePassword = await _userManager.CheckPasswordAsync(user, request.NewPassword);
            if (isSamePassword)
            {
                return new ResetPasswordResponse()
                {
                    Message = "new passord must be different than old password",
                    Success = false
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetPassResult = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!resetPassResult.Succeeded)
            {
                return new ResetPasswordResponse()
                {
                    Message = "password reset failed",
                    Success = false
                };
            }

            await _emailSender.SendEmailAsync(request.Email, "reset password", "<p>your password reset successfully</p>");

            return new ResetPasswordResponse()
            {
                Message = "password reset succesfully",
                Success = true
            };
        }
        private void setRefreshTokenCookies(string refreshToken)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                "refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // true for production 
                    SameSite = SameSiteMode.None, //Strict for production
                    Expires = DateTime.UtcNow.AddDays(15)
                }
                );
        }
        private async Task<string> GenerateRefreshToken(ApplicationUser user)
        {
            var refreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(15);
            await _userManager.UpdateAsync(user);
            return refreshToken;
        }

        public async Task<LoginResponse> RefreshTokenAsync()
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            if (refreshToken is null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "invalid refresh token"
                };
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if(user is null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "no refresh token"
                };
            }
            if(user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "refresh token expired"
                };
            }

            var newRefreshToken = await GenerateRefreshToken(user);
            setRefreshTokenCookies(newRefreshToken);

            return new LoginResponse()
            {
                Message = "",
                Success = true,
                AccessToken = await GenerateAccessToken(user)
            };
        }
    }
}
