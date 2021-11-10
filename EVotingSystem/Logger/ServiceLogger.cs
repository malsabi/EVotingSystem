using EVotingSystem.DataBase;
using System;

namespace EVotingSystem.Logger
{
    public static class ServiceLogger
    {
        #region "Fields"
        private static FireStoreManager FireStore;
        private static object LogLock;
        #endregion

        static ServiceLogger()
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
        public static void Log(LogType LogType, LogLevel LogLevel, string Title, string Message)
        {
            lock (LogLock)
            {
                LogEntry Entry = null;
                if (LogType.Equals(LogType.Admin))
                {
                    Entry = new LogEntry(LogLevel, Title, Message, DateTime.Now.ToString());
                    FireStore.LogAdminEntry(Entry);
                }
                else
                {
                    Entry = new LogEntry(LogLevel, Title, Message, DateTime.Now.ToString());
                    FireStore.LogStudentEntry(Entry);
                }
            }
        }
        #endregion
    }
}