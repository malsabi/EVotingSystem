using Google.Cloud.Firestore;
namespace EVotingSystem.Logger
{
    [FirestoreData]
    public class LogEntry
    {
        #region "Properties"
        [FirestoreProperty]
        public string LogLevel { get; private set; }

        [FirestoreProperty]
        public string Title { get; private set; }

        [FirestoreProperty]
        public string Message { get; private set; }

        [FirestoreProperty]
        public string TimeStamp { get; private set; }
        #endregion

        public LogEntry()
        {
            LogLevel = string.Empty;
            Title = string.Empty;
            Message = string.Empty;
            TimeStamp = string.Empty;
        }

        public LogEntry(LogLevel LogLevel, string Title, string Message, string TimeStamp)
        {
            this.LogLevel = LogLevel.ToString();
            this.Title = Title;
            this.Message = Message;
            this.TimeStamp = TimeStamp;
        }
    }
}