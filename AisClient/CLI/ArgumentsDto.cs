/*
 * Copyright 2021 Swisscom Trust Services (Schweiz) AG
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
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
