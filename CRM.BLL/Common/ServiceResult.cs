using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Common
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public bool IsNotFound { get; set; }

        public static ServiceResult<T> Ok(T data, string message = "Success") =>
            new() { Success = true, Message = message, Data = data };

        public static ServiceResult<T> Fail(string message, List<string>? errors = null) =>
            new() { Success = false, Message = message, Errors = errors };

        public static ServiceResult<T> NotFound(string message) =>
            new() { Success = false, Message = message, IsNotFound = true };
    }
}
