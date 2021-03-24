using System;
using AIS.Rest.Model.PendingRequest;
using AIS.Rest.Model.SignRequest;
using AIS.Rest.Model.SignResponse;
using AIS.Utils;

namespace AIS.Rest.Model
{
    public class RestClient : IRestClient
    {
        public AISSignResponse RequestSignature(AISSignRequest request, Trace trace)
        {
            throw new NotImplementedException();
        }

        public AISSignResponse PollForSignatureStatus(AISPendingRequest request, Trace trace)
        {
            throw new NotImplementedException();
        }
    }
}
