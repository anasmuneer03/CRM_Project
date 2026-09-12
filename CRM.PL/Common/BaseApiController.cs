using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.PL.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected IActionResult SuccessResponse<T>(T data, string message = "Success") =>
        Ok(ApiResponse<T>.Ok(data, message));

        protected IActionResult SuccessResponse(string message = "Success") =>
            Ok(ApiResponse.Ok(message));

        protected IActionResult CreatedResponse<T>(string actionName, object routeValues, T data, string message = "Created") =>
            CreatedAtAction(actionName, routeValues, ApiResponse<T>.Ok(data, message));

        protected IActionResult NotFoundResponse(string message = "Resource not found") =>
            NotFound(ApiResponse.Fail(message));

        protected IActionResult BadRequestResponse(string message, List<string>? errors = null) =>
            BadRequest(ApiResponse.Fail(message, errors));

        protected IActionResult UnauthorizedResponse(string message = "Invalid credentials") =>
        Unauthorized(ApiResponse.Fail(message));
    }
}
