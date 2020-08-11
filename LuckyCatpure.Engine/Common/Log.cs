using System;
using System.Collections.Generic;
using System.Text;

using log4net;

namespace LuckyCatpure.Engine.Common
{
    public class Log
    {
        static ILog log;

        static Log()
        {
            log4net.Config.XmlConfigurator.Configure();
            log = log4net.LogManager.GetLogger("");
        }

        public static void Info(string message)
        {
            log.Info(message);
        }
        public static void Info(string message, Object[] args)
        {
            log.InfoFormat(message, args);
        }
        public static void InfoException(string message, Exception e)
        {
            log.Info(message, e);
        }
    }
}
