namespace AIS.Model.Rest
{
    public class ResultMinorCode
    {
        public static readonly ResultMinorCode AuthenticationFailed = new ResultMinorCode("http://ais.swisscom.ch/1.0/resultminor/AuthenticationFailed",
            "Request authentication failed. For example, the customer used an unknown certificate.");

        public static readonly ResultMinorCode CantServeTimely = new ResultMinorCode("http://ais.swisscom.ch/1.0/resultminor/CantServeTimely",
            "The request could not be processed on time. The subsystem might be overloaded.");

        public static readonly ResultMinorCode InsufficientData = new ResultMinorCode("http://ais.swisscom.ch/1.0/resultminor/InsufficientData",
            "The request could not be completed, because some information is missing.");

        public static readonly ResultMinorCode ServiceInactive = new ResultMinorCode("http://ais.swisscom.ch/1.0/resultminor/ServiceInactive",
            "The requested service is inactive (or not defined at all).");

        public static readonly ResultMinorCode SignatureError = new ResultMinorCode("http://ais.swisscom.ch/1.0/resultminor/SignatureError",
            "An error occurred, while creating a signature.");

        public static readonly ResultMinorCode SerialNumberMismatch = new ResultMinorCode("http://ais.swisscom.ch/1.1/resultminor/subsystem/StepUp/SerialNumberMismatch",
            "During a step-up authentication, the optional unique serial number was provided in the request but did not match the one of the user’s mobile number.");

        public static readonly ResultMinorCode ServiceError = new ResultMinorCode("http://ais.swisscom.ch/1.1/resultminor/subsystem/StepUp/service",
            "A service error occurred during the step-up authentication. Error details are included in the error message.");

        public static readonly ResultMinorCode StepupInvalidStatus = new ResultMinorCode("http://ais.swisscom.ch/1.1/resultminor/subsystem/StepUp/status",
            "An unknown status code of the step-up subsystem was included in the fault response.");

        public static readonly ResultMinorCode StepupTimeout = new ResultMinorCode("http://ais.swisscom.ch/1.1/resultminor/subsystem/StepUp/timeout",
            "The transaction expired before the step-up authorization was completed.");

        public static readonly ResultMinorCode StepupCancel = new ResultMinorCode("http://ais.swisscom.ch/1.1/resultminor/subsystem/StepUp/cancel",
            "The user canceled the step-up authorization.");

        public static readonly ResultMinorCode TimestampError = new ResultMinorCode("http://ais.swisscom.ch/1.0/resultminor/TimestampError",
            "An error occurred, while creating a timestamp.");

        public static readonly ResultMinorCode UnexpectedData = new ResultMinorCode("http://ais.swisscom.ch/1.0/resultminor/UnexpectedData",
            "The request contains unexpected (wrong or misleading) data.");

        public static readonly ResultMinorCode UnknownCustomer = new ResultMinorCode("http://ais.swisscom.ch/1.0/resultminor/UnknownCustomer",
            "The customer is unknown.");

        public static readonly ResultMinorCode UnknownServiceEntity = new ResultMinorCode("http://ais.swisscom.ch/1.0/resultminor/UnknownServiceEntity",
            "The service entity (static key pair or the On Demand CA server) could not found. Maybe the customer does not have access to it.");

        public static readonly ResultMinorCode UnsupportedDigestAlgorithm = new ResultMinorCode("http://ais.swisscom.ch/1.0/resultminor/UnsupportedDigestAlgorithm",
            "The request contains a document hashed with unsupported or weak digest algorithms.");

        public static readonly ResultMinorCode UnsupportedProfile = new ResultMinorCode("http://ais.swisscom.ch/1.0/resultminor/UnsupportedProfile",
            "The request contained unknown profile URI.");

        public static readonly ResultMinorCode StepupTransportError = new ResultMinorCode("http://ais.swisscom.ch/1.1/resultminor:subsystem/StepUp/transport",
            "A subsystem transport error occurred.");

        public static readonly ResultMinorCode GeneralError = new ResultMinorCode("urn:oasis:names:tc:dss:1.0:resultminor:GeneralError",
            "A general internal error occurred.");


        public readonly string Uri;

        public readonly string Description;

        public ResultMinorCode(string uri, string description)
        {
            Uri = uri;
            Description = description;
        }

        public static ResultMinorCode GetByUri(string resultResultMinor)
        {
            throw new System.NotImplementedException();
        }
    }
}