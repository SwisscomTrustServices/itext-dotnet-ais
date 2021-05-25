using System.Collections.Generic;
using Common.Logging;

namespace CLI
{
    public class ArgumentsDto
    {
        public List<string> InputFiles { get; set; }

        public string OutputFile { get; set; }

        public string Suffix { get; set; }

        public string ConfigFile { get; set; }

        public string SignatureType { get; set; }

        public LogLevel LogLevel { get; set; }

        public bool Complete { get; set; }

        public ArgumentsDto()
        {
            LogLevel = LogLevel.Info;
            InputFiles = new List<string>();
        }
    }
}
