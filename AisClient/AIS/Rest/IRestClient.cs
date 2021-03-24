using AIS.Rest.Model.PendingRequest;
using AIS.Rest.Model.SignRequest;
using AIS.Rest.Model.SignResponse;
using AIS.Utils;

namespace AIS.Rest
{
    public interface IRestClient
    {
        AISSignResponse RequestSignature(AISSignRequest request, Trace trace);

        AISSignResponse PollForSignatureStatus(AISPendingRequest request, Trace trace);
    }
}
