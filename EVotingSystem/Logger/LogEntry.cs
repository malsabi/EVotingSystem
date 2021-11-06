using System;
using Google.Cloud.Firestore;
namespace EVotingSystem.Logger
{
    [FirestoreData]
    public class LogEntry
    {
        [FirestoreProperty]
        public LogLevel LogLevel { get; private set; }

        [FirestoreProperty]
        public string Message { get; private set; }

        [FirestoreProperty]
        public DateTime TimeStamp { get; private set; }

        public LogEntry(LogLevel LogLevel, string Message, DateTime TimeStamp)
        {
            this.LogLevel = LogLevel;
            this.Message = Message;
            this.TimeStamp = TimeStamp;
        }
    }
}