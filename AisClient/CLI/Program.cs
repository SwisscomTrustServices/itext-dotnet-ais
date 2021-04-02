using System;
using System.Collections.Generic;
using System.IO;
using AIS;
using AIS.Model;
using AIS.Rest;
using AIS.Rest.Model;
using Newtonsoft.Json;

namespace CLI
{
    class Cli
    {
        private const string PARAM_INPUT = "input";
        private const string PARAM_OUTPUT = "output";
        private const string PARAM_SUFFIX = "suffix";
        private const string PARAM_CONFIG = "config";
        private const string PARAM_TYPE = "type";
        private const string PARAM_HELP = "help";
        private const string PARAM_VERBOSE1 = "v";
        private const string PARAM_VERBOSE2 = "vv";
        private const string PARAM_VERBOSE3 = "vvv";

        private const string SEPARATOR =
            "--------------------------------------------------------------------------------";

        private const string SUFFIX_DEFAULT = "-signed-#time";

        private const string TYPE_STATIC = "static";
        private const string TYPE_ON_DEMAND = "ondemand";
        private const string TYPE_ON_DEMAND_STEP_UP = "ondemand-stepup";
        private const string TYPE_TIMESTAMP = "timestamp";

        //private static ClientVersionProvider versionProvider;
        private static bool continueExecution;
        private static string startFolder;

        private static readonly List<string> InputFileList = new List<string>();
        private static string outputFile;
        private static string suffix;
        private static string configFile;
        private static string type;
        private static int verboseLevel;

        static void Main(string[] args)
        {
            startFolder = Environment.CurrentDirectory;

            ParseArguments(args);
            if (!continueExecution)
            {
                return;
            }

            Console.WriteLine(SEPARATOR);
            Console.WriteLine("Starting with following parameters:");
            Console.WriteLine("Config            : " + configFile);
            Console.WriteLine("Input file(s)     : " + string.Join(", ", InputFileList));
            Console.WriteLine("Output file       : " + outputFile);
            Console.WriteLine("Suffix            : " + suffix);
            Console.WriteLine("Type of signature : " + type);
            Console.WriteLine("Verbose level     : " + verboseLevel);
            Console.WriteLine(SEPARATOR);

            IRestClient restClient = new RestClient();

            IAisClient aisClient = new AisClient();

            AisConfiguration aisConfiguration = DeserializeAisConfig();
            UserData userData = new UserData(aisConfiguration);

            List<PdfHandle> documentsToSign = SetDocumentsToSign();

            SignatureResult result;
            switch (type)
            {
                case TYPE_STATIC:
                {
                    result = aisClient.SignWithStaticCertificate(documentsToSign, userData);
                    break;
                }
                case TYPE_ON_DEMAND:
                {
                    result = aisClient.SignWithOnDemandCertificate(documentsToSign, userData);
                    break;
                }
                case TYPE_ON_DEMAND_STEP_UP:
                {
                    result = aisClient.SignWithOnDemandCertificateAndStepUp(documentsToSign, userData);
                    break;
                }
                case TYPE_TIMESTAMP:
                {
                    result = aisClient.Timestamp(documentsToSign, userData);
                    break;
                }
                default:
                {
                    throw new ArgumentException("Invalid type: " + type);
                }
            }

            Console.WriteLine(SEPARATOR);
            Console.WriteLine("Final result: " + result);
            Console.WriteLine(SEPARATOR);
        }

        private static void ParseArguments(string[] args)
        {
            if (args.Length == 0)
            {
                ShowHelp(null);
                return;
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
                    case PARAM_HELP:
                        {
                            ShowHelp(null);
                            return;
                        }
                    case PARAM_VERBOSE1:
                        {
                            if (verboseLevel < 1)
                            {
                                verboseLevel = 1;
                            }

                            break;
                        }
                    case PARAM_VERBOSE2:
                        {
                            if (verboseLevel < 2)
                            {
                                verboseLevel = 2;
                            }

                            break;
                        }
                    case PARAM_VERBOSE3:
                        {
                            if (verboseLevel < 3)
                            {
                                verboseLevel = 3;
                            }

                            break;
                        }
                    case PARAM_INPUT:
                        {
                            if (argIndex + 1 < args.Length)
                            {
                                InputFileList.Add(args[argIndex + 1]);
                                argIndex++;
                            }
                            else
                            {
                                ShowHelp("Input file name is missing");
                            }

                            break;
                        }
                    case PARAM_OUTPUT:
                        {
                            if (argIndex + 1 < args.Length)
                            {
                                outputFile = args[argIndex + 1];
                                argIndex++;
                            }
                            else
                            {
                                ShowHelp("Output file name is missing");
                            }

                            break;
                        }
                    case PARAM_SUFFIX:
                        {
                            if (argIndex + 1 < args.Length)
                            {
                                suffix = args[argIndex + 1];
                                argIndex++;
                            }
                            else
                            {
                                ShowHelp("Suffix value is missing");
                            }

                            break;
                        }
                    case PARAM_CONFIG:
                        {
                            if (argIndex + 1 < args.Length)
                            {
                                configFile = args[argIndex + 1];
                                argIndex++;
                            }
                            else
                            {
                                ShowHelp("Config file name is missing. "
                                         + "If you need a sample config file, run this program with the -init argument");
                            }

                            break;
                        }
                    case PARAM_TYPE:
                        {
                            if (argIndex + 1 < args.Length)
                            {
                                type = args[argIndex + 1];
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

            if (InputFileList.Count == 0)
            {
                ShowHelp("Input file name is missing");
                return;
            }

            if (outputFile != null && suffix != null)
            {
                ShowHelp("Both output and suffix are configured. Only one of them can be used");
                return;
            }

            if (outputFile == null && suffix == null)
            {
                suffix = SUFFIX_DEFAULT;
            }

            if (outputFile != null && InputFileList.Count > 1)
            {
                ShowHelp("Cannot use output with multiple input files. Please use suffix instead");
                return;
            }

            if (configFile == null)
            {
                configFile = "config.properties";
            }

            if (type == null)
            {
                ShowHelp("Type of signature is missing");
                return;
            }

            continueExecution = true;

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

        private static List<PdfHandle> SetDocumentsToSign()
        {
            List<PdfHandle> documentsToSign = new List<PdfHandle>();

            foreach (var inputFile in InputFileList)
            {
                PdfHandle document = new PdfHandle
                {
                    InputFileName = inputFile,
                    OutputFileName = outputFile ?? GenerateOutputFileName(inputFile, suffix)
                };
                documentsToSign.Add(document);
            }

            return documentsToSign;
        }

        private static string GenerateOutputFileName(string inputFileName, string suffix)
        {
            string finalSuffix = suffix.Replace("#time",
                TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Now).ToString("yyyyMMdd-HHmmss"));

            int lastDotIndex = inputFileName.LastIndexOf('.');

            if (lastDotIndex < 0)
            {
                return inputFileName + finalSuffix;
            }

            return inputFileName.Substring(0, lastDotIndex) + finalSuffix + inputFileName.Substring(lastDotIndex);
        }

        private static AisConfiguration DeserializeAisConfig()
        {
            string aisConfig = File.ReadAllText("AisConfig.Json");

            return JsonConvert.DeserializeObject<AisConfiguration>(aisConfig);
        }
    }
}
