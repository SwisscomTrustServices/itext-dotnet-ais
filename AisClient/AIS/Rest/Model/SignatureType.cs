namespace AIS.Rest.Model
{
    public class SignatureType
    {
        public static readonly SignatureType Cms = new SignatureType("urn:ietf:rfc:3369", 30000);

        public static readonly SignatureType Timestamp = new SignatureType("urn:ietf:rfc:3161", 15000);

        /*
        * URI of the signature type.
        */
        public readonly string Uri;

        /*
        * The estimated final size of the signature in bytes.
        */
        public readonly int EstimatedSignatureSizeInBytes;

        public SignatureType(string uri, int estimatedSignatureSizeInBytes)
        {
            Uri = uri;
            EstimatedSignatureSizeInBytes = estimatedSignatureSizeInBytes;
        }
    }
}