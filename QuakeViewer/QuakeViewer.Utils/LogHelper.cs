using System;
using System.Diagnostics;
using System.Reflection;
using log4net;
using log4net.Config;

namespace QuakeViewer.Utils
{
    public static class LogHelper
    {
        private static ILog logger;

        static LogHelper()
        {
            XmlConfigurator.Configure();
            Type type = MethodBase.GetCurrentMethod().DeclaringType;
            logger = LogManager.GetLogger(type);
        }


        /// <summary>
        /// Errors the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public static void Error(object msg)
        {
            logger.Error(msg);
        }

        /// <summary>
        /// Warn the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public static void Warn(object msg)
        {
            logger.Warn(msg);
        }

        /// <summary>
        /// Errors the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static void Error(Exception ex)
        {
            logger.Error(ex);
        }

        /// <summary>
        /// Debugs the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public static void Debug(object msg)
        {
            logger.Debug(msg);
        }
    }
}