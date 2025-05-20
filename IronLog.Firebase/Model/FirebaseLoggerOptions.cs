namespace IronLog.Firebase.Model
{
    public class FirebaseLoggerOptions
    {
        public const string FirebaseLoggerOption = "FirebaseLogOptions";
        public string BaseUrl { get; set; }
        public string LogPath { get; set; }
        public string AuthToken { get; set; }
    }
}
