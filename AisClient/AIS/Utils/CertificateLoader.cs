using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using BouncyCastleX509Certificate = Org.BouncyCastle.X509.X509Certificate;

namespace AIS.Utils
{
    public class CertificateLoader
    {
        public X509Certificate2 LoadCertificate(CertificateConfiguration certificateConfiguration)
        {
            return BuildCertificate(certificateConfiguration);
        }

        private X509Certificate2 BuildCertificate(CertificateConfiguration certificateConfiguration)
        {
            var builder = new Pkcs12StoreBuilder();
            builder.SetUseDerEncoding(true);
            var store = builder.Build();

            var certEntry = new X509CertificateEntry(ReadCertificate(certificateConfiguration));
            store.SetCertificateEntry("", certEntry);
            store.SetKeyEntry("", new AsymmetricKeyEntry(ReadPrivateKey(certificateConfiguration)), new[] { certEntry });

            byte[] data;
            using (var ms = new MemoryStream())
            {
                store.Save(ms, Array.Empty<char>(), new SecureRandom());
                data = ms.ToArray();
            }

            return new X509Certificate2(data);
        }
        private BouncyCastleX509Certificate ReadCertificate(CertificateConfiguration certificateConfiguration)
        {
            var x509CertificateParser = new X509CertificateParser();
            return x509CertificateParser.ReadCertificate(File.ReadAllBytes(certificateConfiguration.CertificateFile));
        }

        private AsymmetricKeyParameter ReadPrivateKey(CertificateConfiguration certificateConfiguration)
        {

            using (TextReader privateKeyTextReader = new StringReader(File.ReadAllText(certificateConfiguration.PrivateKeyFile)))
            {
                var reader = new PemReader(privateKeyTextReader, new PasswordFinder(certificateConfiguration.Password));
                var readKeyPair = (AsymmetricCipherKeyPair)reader.ReadObject();
                return readKeyPair.Private;
            }
        }
    }
}
