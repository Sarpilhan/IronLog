using System;
using System.Collections.Generic;
using System.Text;

namespace IronLog.File.Model
{
    public class FileLoggerOptions
    {
        public const string FileLoggerOption = "IronLogOptions";
        public string LoggerType { get; set; }
        public string Path { get; set; }
        public string FileNameStatic { get; set; } 
        public SplitType? SplitFormat { get; set; }
        public string Layout { get; set; }
        public string DateFormat { get; set; }
    }
}
