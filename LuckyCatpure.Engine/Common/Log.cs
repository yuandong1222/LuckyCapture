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

        public static Result Result(Result result)
        {
            if (result.Code == ErrorCode.OK)
            {
                Info(result.Message);
            }
            else
            {
                //We don't log exception here, since it should be logged before
                ErrorFormat("[{0}] {1}", result.Code.ToString(), result.Message);
            }
            return result;
        }

        public static void Debug(string message)
        {
            log.Debug(message);
        }
        public static void Debug(string message, Exception e)
        {
            if (e == null)
                Debug(message);
            else
                log.Debug(message, e);
        }
        public static void DebugFormat(string message, params Object[] args)
        {
            log.DebugFormat(message, args);
        }

        public static void Info(string message)
        {
            log.Info(message);
        }
        public static void Info(string message, Exception e)
        {
            if (e == null)
                Info(message);
            else
                log.Info(message, e);
        }
        public static void InfoFormat(string message, params Object[] args)
        {
            log.InfoFormat(message, args);
        }

        public static void Warn(string message)
        {
            log.Warn(message);
        }
        public static void Warn(string message, Exception e)
        {
            if (e == null)
                Warn(message);
            else
                log.Warn(message, e);
        }
        public static void WarnFormat(string message, params Object[] args)
        {
            log.WarnFormat(message, args);
        }

        public static void Error(string message)
        {
            log.Error(message);
        }
        public static void Error(string message, Exception e)
        {
            if (e == null)
                Error(message);
            else
                log.Error(message, e);
        }
        public static void ErrorFormat(string message, params Object[] args)
        {
            log.ErrorFormat(message, args);
        }

        public static void Fatal(string message)
        {
            log.Fatal(message);
        }
        public static void Fatal(string message, Exception e)
        {
            if (e == null)
                Fatal(message);
            else
                log.Fatal(message, e);
        }
        public static void FatalFormat(string message, params Object[] args)
        {
            log.FatalFormat(message, args);
        }
    }
}
