namespace AIS.Model
{
    public class SignatureMode
    {
        public const string TIMESTAMP = "Timestamp";

        public const string STATIC = "Static";

        public const string ON_DEMAND = "On demand";

        public const string ON_DEMAND_STEP_UP = "On demand step up";

        public string FriendlyName;

        public SignatureMode(string friendlyName)
        {
            FriendlyName = friendlyName;
        }
    }
}
