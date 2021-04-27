using System;
using System.Collections.Generic;
using System.IO;
using AIS.Common;
using iText.Kernel.Pdf;
using iText.Signatures;

namespace AIS.Sign
{
    public class PdfDocumentSigner : PdfSigner
    {

        public PdfDocumentSigner(PdfReader reader, Stream outputStream, StampingProperties properties) : base(reader, outputStream, properties)
        {
        }

        public byte[] ComputeHash(IExternalSignatureContainer externalSignatureContainer, int estimatedSize)
        {
            PdfSignature signatureDictionary = new PdfSignature();
            PdfSignatureAppearance appearance = GetSignatureAppearance();
            signatureDictionary.SetReason(appearance.GetReason());
            signatureDictionary.SetLocation(appearance.GetLocation());
            signatureDictionary.SetSignatureCreator(appearance.GetSignatureCreator());
            signatureDictionary.SetContact(appearance.GetContact());
            signatureDictionary.SetDate(new PdfDate(GetSignDate()));
            externalSignatureContainer.ModifySigningDictionary(signatureDictionary.GetPdfObject());
            cryptoDictionary = signatureDictionary;
            Dictionary<PdfName, int?> exc = new Dictionary<PdfName, int?> {[PdfName.Contents] = estimatedSize * 2 + 2};
            PreClose(exc);
            Stream dataRangeStream = GetRangeStream();
            return externalSignatureContainer.Sign(dataRangeStream);
        }

        public void SignWithAuthorizedSignature(IExternalSignatureContainer externalSignatureContainer, int estimatedSize)
        {
            Stream dataRangeStream = GetRangeStream();
            var authorizedSignature = externalSignatureContainer.Sign(dataRangeStream);
            if (estimatedSize < authorizedSignature.Length)
            {
                throw new AisClientException("Not enough space");
            }

            byte[] paddedSignature = new byte[estimatedSize];
            Array.Copy(authorizedSignature, 0, paddedSignature, 0, authorizedSignature.Length);
            PdfDictionary dict2 = new PdfDictionary();
            dict2.Put(PdfName.Contents, new PdfString(paddedSignature).SetHexWriting(true));
            Close(dict2);
            closed = true;
        }
    }
}
