namespace AIS.Rest.Model
{
    public class ResultMajorCode
    {
        public static readonly ResultMajorCode SubsystemError = new ResultMajorCode("http://ais.swisscom.ch/1.0/resultmajor/SubsystemError",
            "Some subsystem of the server produced an error. Details are included in the minor status code.");

        public static readonly ResultMajorCode Pending = new ResultMajorCode("urn:oasis:names:tc:dss:1.0:profiles:asynchronousprocessing:resultmajor:Pending",
            "Asynchronous request was accepted. It is pending now.");

        public static readonly ResultMajorCode RequesterError = new ResultMajorCode("urn:oasis:names:tc:dss:1.0:resultmajor:RequesterError",
            "It is assumed that the caller has made a mistake. Details are included in the minor status code.");

        public static readonly ResultMajorCode ResponderError = new ResultMajorCode("urn:oasis:names:tc:dss:1.0:resultmajor:ResponderError",
            "The server could not process the request. Details are included in the minor status code.");

        public static readonly ResultMajorCode Success = new ResultMajorCode("urn:oasis:names:tc:dss:1.0:resultmajor:Success",
            "Request was successfully executed");

        public readonly string Uri;

        public readonly string Description;

        public ResultMajorCode(string uri, string description)
        {
            Uri = uri;
            Description = description;
        }
    }
}