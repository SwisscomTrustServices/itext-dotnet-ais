using AIS.Model.Rest.PendingRequest;
using AIS.Model.Rest.SignRequest;
using AIS.Model.Rest.SignResponse;
using AIS.Utils;

namespace AIS.Rest
{
    public interface IRestClient
    {
        AISSignResponse RequestSignature(AISSignRequest request, Trace trace);

        AISSignResponse PollForSignatureStatus(AISPendingRequest request, Trace trace);
    }
}
