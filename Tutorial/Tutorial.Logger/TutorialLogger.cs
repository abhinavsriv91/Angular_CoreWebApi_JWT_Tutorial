using log4net;
using System.IO;
using System.Reflection;
using System.Text;

namespace Tutorial.Logger
{
    /// <summary>
    /// Logger Class Implemantation. This class acts as a wrapper over log4net
    /// </summary>
    public class TutorialLogger
    {
        private static ILog _Log = null;
        private static readonly string LogConfigFile = @"Log4Net.config";

        static TutorialLogger()
        {
            GetLogManager();
        }

        private static void GetLogManager()
        {
            if (_Log == null)
            {
                SetLog4NetConfiguration();
                _Log = LogManager.GetLogger(typeof(TutorialLogger));
            }
        }

        /// <summary>
        /// Logs info passed
        /// </summary>
        /// <param name="infoMessage">string infoLogMessage</param>
        public static void LogInfo(string infoMessage)
        {
            if (_Log.IsInfoEnabled)
            {
                _Log.Info(infoMessage);
            }
        }

        /// <summary>
        /// The method logs the debug info
        /// </summary>
        /// <param name="debugMessage">string debugLogMessage</param>
        public static void LogDebug(string debugMessage)
        {
            if (_Log.IsDebugEnabled)
            {
                _Log.Debug(debugMessage);
            }
        }

        /// <summary>
        /// The method logs the debug info
        /// </summary>
        /// <param name="warningMessage">string debugLogMessage</param>
        public static void LogWarn(string warningMessage)
        {
            if (_Log.IsDebugEnabled)
            {
                _Log.Warn(warningMessage);
            }
        }

        /// <summary>
        /// The method logs the info
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        /// <param name="extraInfo"></param>
        public static void LogInfo(string userId, string message, string extraInfo)
        {
            if (_Log.IsInfoEnabled)
            {
                StringBuilder warningMessage = new StringBuilder();

                warningMessage.Append("UserId:");
                warningMessage.Append(userId);
                warningMessage.AppendLine();

                warningMessage.Append("Message:");
                warningMessage.Append(message);

                if (!string.IsNullOrWhiteSpace(extraInfo))
                {
                    warningMessage.AppendLine();
                    warningMessage.Append("Extra Information:");
                    warningMessage.Append(extraInfo);
                }

                _Log.Info(warningMessage);
            }
        }

        /// <summary>
        /// The method logs the error info
        /// </summary>
        /// <param name="errorMessage">string errorLogMessage</param>
        public static void LogError(string errorMessage)
        {
            if (_Log.IsErrorEnabled)
            {
                _Log.Error(errorMessage);
            }
        }

        /// <summary>
        /// The method logs the error with log trace
        /// </summary>
        /// <param name="User_id"></param>
        /// <param name="Log_Message"></param>
        /// <param name="Log_Trace"></param>
        public static void LogError(string User_id, string Log_Message, string Log_Trace)
        {
            if (_Log.IsErrorEnabled)
            {
                StringBuilder errorLogMessage = new StringBuilder();

                errorLogMessage.Append("UserId:");
                errorLogMessage.Append(User_id);
                errorLogMessage.AppendLine();

                errorLogMessage.Append("Error:");
                errorLogMessage.Append(Log_Message);

                if (!string.IsNullOrWhiteSpace(Log_Trace))
                {
                    errorLogMessage.AppendLine();
                    errorLogMessage.Append("Stack Trace:");
                    errorLogMessage.Append(Log_Trace);
                }

                _Log.Error(errorLogMessage);
            }
        }

        /// <summary>
        /// Set and load Log4Net.config file
        /// </summary>
        private static void SetLog4NetConfiguration()
        {
            var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            var fileInfo = new FileInfo(LogConfigFile);
            log4net.Config.XmlConfigurator.Configure(repo, fileInfo);
        }
    }
}
