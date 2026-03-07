using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Common.Responses
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public T? Data { get; private set; }
        public IEnumerable<string>? Errors { get; private set; }
        public string? TraceId { get; private set; }

        private ApiResponse(bool isSuccess, string message, T? data, IEnumerable<string>? errors, string? traceId)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            Errors = errors;
            TraceId = traceId;
        }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Request successful")
        {
            return new ApiResponse<T>(true, message, data, null,null);
        }

        public static ApiResponse<T> FailureResponse(string message, IEnumerable<string>? errors = null, string? traceId = null)
        {
            return new ApiResponse<T>(false, message, default, errors, traceId);
        }
    }
}
