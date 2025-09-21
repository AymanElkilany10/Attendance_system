using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.ViewModels
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponse() { }

        public ApiResponse(int code, string status, string message, T data)
        {
            Code = code;
            Status = status;
            Message = message;
            Data = data;
        }

        public static ApiResponse<T> Success(T data, string message = "Request successful", int code = 200)
        {
            return new ApiResponse<T>(code, "Success", message, data);
        }

        public static ApiResponse<T> Fail(string message = "Something went wrong", int code = 400)
        {
            return new ApiResponse<T>(code, "Error", message, default);
        }
    }

}
