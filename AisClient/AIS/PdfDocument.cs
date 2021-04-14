using System;
using System.Collections.Generic;
using System.IO;
using AIS.Model;
using AIS.Model.Rest;
using AIS.Sign;
using AIS.Utils;
using iText.Kernel.Pdf;
using iText.Signatures;
using System;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using static System.String;

namespace AIS
{
    public class PdfDocumentHandler
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Base64HashToSign { get; set; }

        public DigestAlgorithm DigestAlgorithm { get; set; }

        private const string Delimiter = ";";
        private byte[] contentIn;
        private Trace trace;
        private PdfReader pdfReader;
        private PdfWriter pdfWriter;
        private MemoryStream inMemoryStream;
        private PdfDocumentSigner pdfSigner;
        private PdfDocument pdfDocument;
        private byte[] documentHash;

        public PdfDocumentHandler(byte[] input, Trace trace)
        {
            contentIn = input;
            this.trace = trace;
        }

        public void PrepareForSigning(DigestAlgorithm algorithm, SignatureType signatureType, UserData userData)
        {
            DigestAlgorithm = algorithm;
            Id = GenerateDocumentId();
            pdfDocument = new PdfDocument(new PdfReader(new MemoryStream(contentIn)));
            SignatureUtil signatureUtil = new SignatureUtil(pdfDocument);
            var hasSignature = signatureUtil.GetSignatureNames().Count > 0;
            pdfReader = new PdfReader(new MemoryStream(contentIn));
            inMemoryStream = new MemoryStream();
            pdfWriter = new PdfWriter(inMemoryStream, new WriterProperties().AddXmpMetadata().SetPdfVersion(PdfVersion.PDF_1_0));
            StampingProperties stampingProperties = new StampingProperties();
            pdfSigner = new PdfDocumentSigner(pdfReader, pdfWriter, hasSignature ? stampingProperties.UseAppendMode() : stampingProperties);
            pdfSigner.GetSignatureAppearance().SetReason(GetOptionalAttribute(userData.SignatureReason))
                .SetLocation(GetOptionalAttribute(userData.SignatureLocation)).SetContact(GetOptionalAttribute(userData.SignatureContactInfo));
            if (!userData.SignatureName.IsEmpty())
            {
                pdfSigner.SetFieldName(userData.SignatureName);
            }
            var isTimestampSignature = signatureType.Uri == SignatureType.Timestamp.Uri;
            Dictionary<PdfName, PdfObject> signatureDictionary = new Dictionary<PdfName, PdfObject>();
            signatureDictionary[PdfName.Filter] = PdfName.Adobe_PPKLite;
            signatureDictionary[PdfName.SubFilter] = isTimestampSignature ? PdfName.ETSI_RFC3161 : PdfName.ETSI_CAdES_DETACHED;
            pdfSigner.SetSignDate(isTimestampSignature? DateTime.Now : DateTime.Now.AddMinutes(3));
            PdfHashSignatureContainer hashSignatureContainer = new PdfHashSignatureContainer(DigestAlgorithm.Algorithm, new PdfDictionary(signatureDictionary));
            var hash = pdfSigner.ComputeHash(hashSignatureContainer, signatureType.EstimatedSignatureSizeInBytes);
            Base64HashToSign = Convert.ToBase64String(hash, 0, hash.Length);
        }

        public byte[] CreateSignedPdf(byte[] externalsignature, int estimatedSize, List<string> encodedCrlEntries, List<string> encodedOcspEntries)
        {
            if (pdfSigner.GetCertificationLevel() == PdfSigner.CERTIFIED_NO_CHANGES_ALLOWED)
            {
                throw new Exception($"Could not apply signature because source file contains a certification " +
                                    $"that does not allow any changes to the document with id {Id}");
            }

            //TODO - log message
            string message = "Signature size [estimated: {}" + Delimiter + "actual: {}" + Delimiter + "remaining: {}" +
                             "] - {}";
            if (estimatedSize < externalsignature.Length)
            {
                throw new Exception($"Not enough space for signature in the document with id {trace.Id}. The estimated size needs to be " +
                                    $" increased with {externalsignature.Length - estimatedSize} bytes.");
            }

            try
            {
                pdfSigner.SignWithAuthorizedSignature(new PdfSignatureContainer(externalsignature), estimatedSize);
                if (encodedOcspEntries != null || encodedCrlEntries != null)
                {
                    return ExtendDocumentWithCrlOcspMetadata(new MemoryStream(inMemoryStream.ToArray()), encodedCrlEntries,
                        encodedOcspEntries);
                }
                else
                {
                    //TODO log info
                    return inMemoryStream.ToArray();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                Close();
            }
        }

        private byte[] ExtendDocumentWithCrlOcspMetadata(MemoryStream documentStream, List<string> encodedCrlEntries, List<string> encodedOcspEntries)
        {
            throw new NotImplementedException();
        }

        private string GetOptionalAttribute(string attribute)
        {
            return attribute.IsEmpty() ? Empty : attribute;
        }
        private string GenerateDocumentId()
        {
            return "DOC-" + Guid.NewGuid();
        }

        //private byte[] MapEncodedCrl(String encodedCrl)
        //{
        //    try
        //    {
        //        MemoryStream inputStream = new MemoryStream(Convert.FromBase64String(encodedCrl));
        //        X509Crl x509Crl = new X509Crl(new CertificateList());
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    return new byte[10];
        //}

        public void Close()
        {
            CloseResource(pdfReader);
            CloseResource(pdfWriter);
            CloseResource(pdfDocument);
            CloseResource(inMemoryStream);
        }
        private void CloseResource(IDisposable resource)
        {
            try
            {
                resource?.Dispose();
            }
            catch (Exception e)
            {
               // TODO processingLogger.debug("Failed to close the resource - {}. Reason: {}", trace.getId(), e.getMessage());
            }
        }

    }
}
