using System.IO;
using iText.Kernel.Pdf;
using iText.Signatures;

namespace AIS.Sign
{
    public class PdfSignatureContainer : IExternalSignatureContainer
    {
        private readonly byte[] authorizedSignature;

        public PdfSignatureContainer(byte[] authorizedSignature)
        {
            this.authorizedSignature = authorizedSignature;
        }

        public byte[] Sign(Stream data)
        {
            return authorizedSignature;
        }

        public void ModifySigningDictionary(PdfDictionary signDic)
        {
        }
    }
}
