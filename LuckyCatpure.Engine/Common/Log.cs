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

        public static void Debug(string message)
        {
            log.Debug(message);
        }
        public static void Debug(string message, Object[] args)
        {
            log.DebugFormat(message, args);
        }
        public static void DebugException(string message, Exception e)
        {
            log.Debug(message, e);
        }

        public static void Info(string message)
        {
            log.Info(message);
        }
        public static void Info(string message, params Object[] args)
        {
            log.InfoFormat(message, args);
        }
        public static void InfoException(string message, Exception e)
        {
            log.Info(message, e);
        }

        public static void Warn(string message)
        {
            log.Warn(message);
        }
        public static void Warn(string message, params Object[] args)
        {
            log.WarnFormat(message, args);
        }
        public static void WarnException(string message, Exception e)
        {
            log.Warn(message, e);
        }

        public static void Error(string message)
        {
            log.Error(message);
        }
        public static void Error(string message, params Object[] args)
        {
            log.ErrorFormat(message, args);
        }
        public static void ErrorException(string message, Exception e)
        {
            log.Error(message, e);
        }

        public static void Fatal(string message)
        {
            log.Fatal(message);
        }
        public static void Fatal(string message, params Object[] args)
        {
            log.FatalFormat(message, args);
        }
        public static void FatalException(string message, Exception e)
        {
            log.Fatal(message, e);
        }
    }
}
