using System.Collections.Generic;
using System.Linq;

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

        private static readonly List<ResultMessageCode> ResultMessageCodes = new List<ResultMessageCode>
        {
            InvalidPassword,
            InvalidOtp
        };
        public ResultMessageCode(string uri, string description)
        {
            Uri = uri;
            Description = description;
        }

        public static ResultMessageCode GetByUri(string uri)
        {
            return ResultMessageCodes.FirstOrDefault(rmc => rmc.Uri == uri);
        }

        private bool Equals(ResultMessageCode other)
        {
            return other?.Uri == Uri;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ResultMessageCode);
        }

        public override int GetHashCode()
        {
            return (Uri != null ? Uri.GetHashCode() : 0);
        }
    }
}