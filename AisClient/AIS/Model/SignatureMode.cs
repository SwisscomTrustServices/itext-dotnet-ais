namespace AIS.Model
{
    public class SignatureMode
    {
        public const string Timestamp = "Timestamp";

        public const string Static = "Static";

        public const string OnDemand = "On demand";

        public const string OnDemandStepUp = "On demand step up";

        public readonly string FriendlyName;

        public SignatureMode(string friendlyName)
        {
            FriendlyName = friendlyName;
        }
    }
}
