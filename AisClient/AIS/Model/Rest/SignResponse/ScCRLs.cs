using System.Collections.Generic;
using AIS.Utils.Serialization;
using Newtonsoft.Json;

namespace AIS.Model.Rest.SignResponse
{
    public class ScCRLs
    {
        [JsonProperty("sc.CRL")]
        [JsonConverter(typeof(SingleOrArrayConverter<string>))]
        public List<string> ScCRL { get; set; }
    }
}

    
