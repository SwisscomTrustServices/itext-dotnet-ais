namespace AIS.Model
{
    public class SignatureResponse
    {
        public byte[] SignedContent { get; set; }
        public SignatureResult SignatureResult { get; set; }

    }
}
