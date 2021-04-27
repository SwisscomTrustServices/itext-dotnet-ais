using System.IO;
using iText.Kernel.Pdf;
using iText.Signatures;

namespace AIS.Sign
{
    public class PdfHashSignatureContainer : IExternalSignatureContainer
    {
        private readonly string hashAlgorithm;
        private PdfDictionary signatureDictionary;

        public PdfHashSignatureContainer(string hashAlgorithm, PdfDictionary signatureDictionary)
        {
            this.hashAlgorithm = hashAlgorithm;
            this.signatureDictionary = signatureDictionary;
        }

        public byte[] Sign(Stream data)
        {
            return DigestAlgorithms.Digest(data, hashAlgorithm);
        }

        public void ModifySigningDictionary(PdfDictionary signDic)
        {
            signDic.PutAll(signatureDictionary);
        }
    }
}
