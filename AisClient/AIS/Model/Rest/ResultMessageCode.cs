namespace AIS.Model.Rest
{
    public class ResultMessageCode
    {
        public static readonly ResultMessageCode InvalidPassword = new ResultMessageCode("urn:swisscom:names:sas:1.0:status:InvalidPassword",
            "User entered an invalid password, as part of the Step Up phase");

        public static readonly ResultMessageCode InvalidOtp = new ResultMessageCode("urn:swisscom:names:sas:1.0:status:InvalidOtp",
            "User entered an invalid OTP, as part of the Step Up phase");

        public readonly string Uri;

        public readonly string Description;

        public ResultMessageCode(string uri, string description)
        {
            Uri = uri;
            Description = description;
        }
    }
}