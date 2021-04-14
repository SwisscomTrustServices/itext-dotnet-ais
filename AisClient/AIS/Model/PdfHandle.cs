using AIS.Model.Rest;
using AIS.Utils;

namespace AIS.Model
{
    public class PdfHandle
    {
        public string InputFileName { get; set; }
        public string OutputFileName { get; set; }
        public DigestAlgorithm DigestAlgorithm { get; set; }

        public PdfHandle()
        {
            DigestAlgorithm = DigestAlgorithm.SHA512;
        }

        public void ValidateYourself(Trace trace)
        {
            Validator.AssertValueNotEmpty(InputFileName, "The inputFromFile cannot be null or empty", trace);
            Validator.AssertValueNotEmpty(OutputFileName, "The outputToFile cannot be null or empty", trace);
            Validator.AssertValueNotNull(DigestAlgorithm, "The digest algorithm for a PDF handle cannot be NULL", trace);
        }
    }
}
