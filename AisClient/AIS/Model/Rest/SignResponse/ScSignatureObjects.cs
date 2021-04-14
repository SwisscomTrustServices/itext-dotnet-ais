using System.Collections.Generic;
using Newtonsoft.Json;

namespace AIS.Model.Rest.SignResponse
{
    public class ScSignatureObjects
    {
        [JsonProperty("sc.ExtendedSignatureObject")]
        public List<ScExtendedSignatureObject> ScExtendedSignatureObject { get; set; }
    }
}