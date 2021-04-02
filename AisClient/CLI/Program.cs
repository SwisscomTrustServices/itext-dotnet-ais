using AIS;
using AIS.Rest;
using AIS.Rest.Model.SignRequest;
using AIS.Utils;

namespace CLI
{
    class Cli
    {
        static void Main(string[] args)
        {
            IAisClient aisClient = new AisClient();
            RestClient r = new RestClient();
            r.RequestSignature(new AISSignRequest(), new Trace("dfdf"));
        }
    }
}
