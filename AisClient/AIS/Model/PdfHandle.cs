using AIS.Rest.Model;

namespace AIS.Model
{
    public class PdfHandle
    {
        public string InputFileName { get; set; }
        public string OutputFileName { get; set; }
        public DigestAlgorithm DigestAlgorithm { get; set; }
    }
}
