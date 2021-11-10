using Google.Cloud.Firestore;

namespace EVotingSystem.Logger
{
    [FirestoreData]
    public enum LogLevel : int
    {
        Get = 0,
        Post = 1,
        Error = 2,
        Info = 3,
        None = 4
    }
}