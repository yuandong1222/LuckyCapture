using System;
using System.Collections.Generic;
using System.Text;

namespace LuckyCatpure.Engine.Common
{
    public class Result
    {
        public ErrorCode Code { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
