namespace IronLog.MySql.Model
{
    public class MySqlLoggerOptions
    {
        public const string MySqlLoggerOption = "MySqlLogOptions";
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
    }
}
