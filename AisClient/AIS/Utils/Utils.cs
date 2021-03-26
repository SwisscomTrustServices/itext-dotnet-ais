using System;
using System.IO;

namespace AIS.Utils
{
    public class Utils
    {
        public static void CopyFileFromClasspathToDisk(string sourceFile, string destFile)
        {
            try
            {
                File.Copy(sourceFile, destFile, false);

            } catch (IOException e)
            {
                // throw new AisClientException("Failed to create the file: [" + outputFile + "]");
            }
        }

        public static string CopyFileFromClasspathToString(string inputFile)
        {
            try
            {
                StreamReader reader = new StreamReader(Path.Combine(Environment.CurrentDirectory, inputFile));
                string text = reader.ReadToEnd();
                reader.Close();
                return text;

            } catch (IOException e)
            {
                // throw new AisClientException("Failed to copy the file: [" + inputFile + "] to string");
            }

            return null;
        }
    }
}
