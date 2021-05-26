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
using System;
using System.IO;
using Common.Logging;

namespace CLI
{
    public class ArgumentsService
    {
        private const string SuffixDefault = "-signed-#time";

        public ArgumentsDto GetArguments(string[] args)
        {
            ArgumentsDto result = new ArgumentsDto();
            if (args.Length == 0)
            {
                ShowHelp(null);
                return null;
            }

            int argIndex = 0;

            while (argIndex < args.Length)
            {
                string currentArg = args[argIndex];

                if (currentArg.StartsWith("--"))
                {
                    currentArg = currentArg.Substring(2);
                }

                if (currentArg.StartsWith("-"))
                {
                    currentArg = currentArg.Substring(1);
                }

                switch (currentArg)
                {
                    case CliArgument.Help:
                        {
                            ShowHelp(null);
                            return null;
                        }
                    case CliArgument.Verbose:
                        {
                            result.LogLevel = LogLevel.Debug;

                            break;
                        }
                    case CliArgument.Input:
                        {
                            if (argIndex + 1 < args.Length)
                            {
                                result.InputFiles.Add(args[argIndex + 1]);
                                argIndex++;
                            }
                            else
                            {
                                ShowHelp("Input file name is missing");
                            }

                            break;
                        }
                    case CliArgument.Output:
                        {
                            if (argIndex + 1 < args.Length)
                            {
                                result.OutputFile = args[argIndex + 1];
                                argIndex++;
                            }
                            else
                            {
                                ShowHelp("Output file name is missing");
                            }

                            break;
                        }
                    case CliArgument.Suffix:
                        {
                            if (argIndex + 1 < args.Length)
                            {
                                result.Suffix = args[argIndex + 1];
                                argIndex++;
                            }
                            else
                            {
                                ShowHelp("Suffix value is missing");
                            }

                            break;
                        }
                    case CliArgument.Config:
                        {
                            if (argIndex + 1 < args.Length)
                            {
                                result.ConfigFile = args[argIndex + 1];
                                argIndex++;
                            }
                            else
                            {
                                ShowHelp("Config file name is missing. "
                                         + "If you need a sample config file, run this program with the -init argument");
                            }

                            break;
                        }
                    case CliArgument.Type:
                        {
                            if (argIndex + 1 < args.Length)
                            {
                                result.SignatureType = args[argIndex + 1];
                                argIndex++;
                            }
                            else
                            {
                                ShowHelp("Type of signature is missing");
                            }

                            break;
                        }

                }

                argIndex++;
            }
            if (result.OutputFile == null && result.Suffix == null)
            {
                result.Suffix = SuffixDefault;
            }

            result.Complete = ValidateArguments(result);
            return result;
        }

        private bool ValidateArguments(ArgumentsDto args)
        {

            if (args.InputFiles.Count == 0)
            {
                ShowHelp("Input file name is missing");
                return false;
            }

            if (args.OutputFile != null && args.Suffix != null)
            {
                ShowHelp("Both output and suffix are configured. Only one of them can be used");
                return false;
            }

            if (args.OutputFile != null && args.InputFiles.Count > 1)
            {
                ShowHelp("Cannot use output with multiple input files. Please use suffix instead");
                return false;
            }

            if (args.ConfigFile == null)
            {
                ShowHelp("Config file name is missing");
                return false;
            }

            if (args.SignatureType == null)
            {
                ShowHelp("Type of signature is missing");
                return false;
            }
            return true;
        } 

        private static void ShowHelp(string argsValidationError)
        {
            if (argsValidationError != null)
            {
                Console.WriteLine(argsValidationError);
            }

            string usageText = File.ReadAllText("usage.txt");

            Console.WriteLine(usageText);
        }
    }
}
