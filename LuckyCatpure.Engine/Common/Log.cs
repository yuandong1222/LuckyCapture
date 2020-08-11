using System;
using System.Collections.Generic;
using System.Text;

using log4net;

namespace LuckyCatpure.Engine.Common
{
    public class Log
    {
        public static void Info(string )
        {
            ILog log = log4net.LogManager.GetLogger("Default");

        }
    }
}
