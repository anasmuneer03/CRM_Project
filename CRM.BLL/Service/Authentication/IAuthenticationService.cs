using CRM.DAL.DTO.Request.Authentication;
using CRM.DAL.DTO.Response.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Service.Authentication
{
    public interface IAuthenticationService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<bool> ConfirmEmailAsync(string token, string userId);
        Task<ForgotPasswordResponse> RequestPasswordResetAsync(ForgotPasswordRequest request);
        Task<ResetPasswordResponse> PasswordResetAsync(ResetPasswordRequest request);
        Task<LoginResponse> RefreshTokenAsync();
    }
}
