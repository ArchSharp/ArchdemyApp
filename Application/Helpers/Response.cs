using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class Response
    {
        public int code { get; set; }
        public bool? Success { get; set; }
        public string Message { get; set; }
        public object? ExtraInfo { get; set; }
    }
    public class SuccessResponse<T> : Response
    {
        public SuccessResponse()
        {
            Success = true;
        }
        public T? Data { get; set; }
    }
    public class ErrorResponse<T> : Response
    {
        public T Error { get; set; }
    }
}
