using System;
using System.Diagnostics;
using System.Reflection;
using log4net;
using log4net.Config;

namespace QuakeViewer.Utils
{
    public static class LogHelper
    {
        private static ILog loger;

        static LogHelper()
        {
            XmlConfigurator.Configure();
            Type type = MethodBase.GetCurrentMethod().DeclaringType;
            loger = LogManager.GetLogger(type);
        }

        public static void Debug(string content)
        {
            loger.Debug(content);
        }

        public static void Info(string content)
        {
            loger.Info(content);
        }

        public static void Warn(string content)
        {
            loger.Warn(content);
        }

        public static void Error(string content)
        {
            loger.Error(content);
        }

        public static void Fatal(string content)
        {
            loger.Error(content);
        }

    }
}