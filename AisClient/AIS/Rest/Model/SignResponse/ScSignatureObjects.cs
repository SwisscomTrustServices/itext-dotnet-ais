using System.Collections.Generic;
using Newtonsoft.Json;

namespace AIS.Rest.Model.SignResponse
{
    public class ScSignatureObjects
    {
        [JsonProperty("sc.ExtendedSignatureObject")]
        public List<ScExtendedSignatureObject> ScExtendedSignatureObject { get; set; }
    }
}