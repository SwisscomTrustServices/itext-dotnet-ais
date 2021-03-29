namespace AIS.Rest.Model
{
    public class DigestAlgorithm
    {

        public static readonly DigestAlgorithm SHA256 = new DigestAlgorithm("SHA-256", "http://www.w3.org/2001/04/xmlenc#sha256");

        public static readonly DigestAlgorithm SHA384 = new DigestAlgorithm("SHA-384", "http://www.w3.org/2001/04/xmldsig-more#sha384");

        public static readonly DigestAlgorithm SHA512 = new DigestAlgorithm("SHA-512", "http://www.w3.org/2001/04/xmlenc#sha512");

        /*
        * Name of the algorithm (to be used with security provider).
         */
        public readonly string Algorithm;

        /*
        * Uri of the algorithm (to be used in the AIS API).
        */
        public readonly string Uri;

        public DigestAlgorithm(string algorithm, string uri)
        {
            Algorithm = algorithm;
            Uri = uri;
        }
    }
}
