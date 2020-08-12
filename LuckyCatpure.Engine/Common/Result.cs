using System;
using System.Collections.Generic;
using System.Text;

namespace LuckyCatpure.Engine.Common
{
    public class Result
    {
        public Result()
        {
            Code = ErrorCode.OK;
        }
        public Result(ErrorCode code)
        {
            Code = code;
        }
        public Result(string message)
        {
            Code = ErrorCode.OK;
            Message = message;
        }
        public Result(ErrorCode code, string message)
        {
            Code = code;
            Message = message;
        }
        public Result(ErrorCode code, string message, Exception exception)
        {
            Code = code;
            Message = message;
            Exception = exception;
        }

        public ErrorCode Code { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
