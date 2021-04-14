using System.Collections.Generic;
using AIS.Utils.Serialization;
using Newtonsoft.Json;

namespace AIS.Model.Rest.SignResponse
{
    public class ScOCSPs
    {
        [JsonProperty("sc.OCSP")]
        [JsonConverter(typeof(SingleOrArrayConverter<string>))]
        public List<string> ScOCSP { get; set; }
    }
}