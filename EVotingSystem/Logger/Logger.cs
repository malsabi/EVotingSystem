using EVotingSystem.DataBase;
using System;

namespace EVotingSystem.Logger
{
    public static class Logger
    {
        #region "Fields"
        private static FireStoreManager FireStore;
        private static object LogLock;
        #endregion

        static Logger()
        {
            Initialize();
        }

        #region "Private Static Methods"
        private static void Initialize()
        {
            //Initialize the fire store manager.
            FireStore = new FireStoreManager();
            //Initialize the log locker to prevent multiple access.
            LogLock = new object();
        }
        #endregion

        #region "Public Static Methods"
        public static void Log(LogType LogType, LogLevel LogLevel, string Message)
        {
            lock (LogLock)
            {
                LogEntry Entry = null;
                if (LogType.Equals(LogType.Admin))
                {
                    Entry = new LogEntry(LogLevel, Message, DateTime.Now);
                    //Add "Entry" in AdminLog
                }
                else
                {
                    Entry = new LogEntry(LogLevel, Message, DateTime.Now);
                    //Add "Entry" in StudentLog
                }
            }
        }
        #endregion
    }
}