using System.Collections.Generic;
using Newtonsoft.Json;

namespace AIS.Rest.Model.SignResponse
{
    public class ScOCSPs
    {
        [JsonProperty("sc.OCSP")]
        public List<string> ScOCSP { get; set; }
    }
}